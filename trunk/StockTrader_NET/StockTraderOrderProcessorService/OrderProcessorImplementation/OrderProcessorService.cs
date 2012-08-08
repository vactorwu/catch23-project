//  .NET StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azurestocktrader.cloudapp.net
//                                   
//
//  Configuration Service 5 Notes:
//                      The application implements Configuration Service 5.0, for which the source code also ships in the sample. However, the .NET StockTrader 5
//                      sample is a general-purpose performance sample application for Windows Server and Windows Azure even if you are not implementing the Configuration Service. 
//                      
//

//======================================================================================================
// The Order Processor Service implementation class/business logic.
//======================================================================================================


using System;
using System.Configuration;
using System.Diagnostics;
using System.Transactions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Threading;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;
using Trade.BusinessServiceDataContract;
using Trade.OrderProcessorContract;
using Trade.OrderProcessorServiceConfigurationSettings;
using Trade.Utility;
using Trade.IDAL;
using Trade.DALFactory;
using Trade.OrderProcessorAsyncClient;
using Trade.OrderProcessorPoisonMessageHandler;


namespace Trade.OrderProcessorImplementation
{
    
    /// <summary>
    /// Service class which implements the Order Processor service contract.
    /// ReleaseServiceInstanceOnTransactionComplete is marked as false  to support transacted batches from the 
    /// transacted (persisted) durable MSMQ message queue. This is required!
    /// </summary>
    [ServiceBehavior(ReleaseServiceInstanceOnTransactionComplete = false, TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [PoisonErrorBehavior]
    public class OrderProcessor : IOrderProcessor
    {
        public OrderProcessor()
        {
            Interlocked.Increment(ref  Settings.invokeCount);
        }

        private IOrder dalOrder;

        /// <summary>
        /// Interesting in that this service contract is a one-way contract. However, it
        //  turns out this online check works fine in that we are loosely couple when working with MSMQ
        //  and still tightly coupled on one-way TCP or HTTP bindings.  The behavior is such
        //  that this method will fail from calling client only on TCP/HTTP one-way (Async)
        //  bindings, which happens regardless on any method call; but will not fail on MSMQ bindings 
        //  if the messaging service is running.  If MSMQ messaging service is not running at the endpoint, 
        //  we will have endpoint not found detection as desired.
        /// </summary>
        public void isOnline()
        {
            //We will not count this online check from Remote client as an activation, since its not a real operation.  Ultimately,
            //there is a TODO here, which is apply a WCF endpoint behavior, such that we count actual requests at the service op level.
            //Also note while this will be called from ConfigWeb for admin procedures (and discounted here); it will not be called
            //when using an external load balancer; or if the known node count=1.  There is no point in this check, if there is no
            //node with a different address to failover to.  So, for on-premise, this kicks in with 2 or more nodes, with the
            //benefit of service-op level failover.  With external load balancers, its up to them to decide at what level
            //they check online status.  It might be at the (virtual) machine level; or at the endpoint level.
            Interlocked.Decrement(ref Settings.invokeCount);
            return;
        }
        
        

        /// <summary>
        /// SubmitOrderTransactedQueue() service operation is a transacted service operation that will gaurantee
        /// order messages are either processed to the database, or left in the durable queue 
        /// (never lost). This utilizes a two-phase tx across the DB and the MSMQ transacted queue; as such
        /// this operation will always involve the Distributed Transaction Coordinator (DTC). The transaction
        /// must be scoped as part of the WCF operaton behavior, and marked as AutoComplete.  Should order message processing
        /// to the database fail, the following will happen:
        ///
        /// 1.  WCF will automatically retry the processing operation based on the retry and cycle values
        ///     set in the netmsmq binding for the service (configured in the .config file). 
        ///
        /// 2.  If the message still cannot be handled properly, WCF will automatically throw a poison message exception
        ///     which will be handled by our WCF poison message error handler (set up via a WCF
        ///     behavior). WCF will then also automatically fault the service, since the message is poison and is 
        ///     corrupting normal order processing operations as we have the service setup to always process orders
        ///    out of the queue in the order they arrive. Hence:
        ///
        /// 3.  The poison message error handler will re-read the poison message from the mainline queue
        ///     also as part of a distributed transaction, and move it to the poison message
        ///     queue for later recovery, so the message is never lost.  Once out of the mainline queue,
        ///     The OrderProcessor Service will be automatically restarted within the service host program as 
        ///    step 4 below:
        ///
        /// 4.  A simple event handler within the service host program handles the ServiceFaulted event, 
        ///     and will then re-start the service automatically within the existing CLR process, so normal 
        ///     processing is not halted.
        ///
        /// All of this can be visually seen via the Console display---simply buy the stock "ACIDBUY" and watch
        /// the console!  You can also buy and then try to sell the stock "ACIDSELL" to see this on the sell-side
        /// as part of our AcidTests included with the sample.  You should never be able to buy symbol 'AcidBuy':
        /// It should never show up in your portfolio; conversely, you should never be able to sell symbol 'AcidSell':
        /// Once you bought shares in it, it's yours forever based on our intential exceptions generated to simulate
        /// system failure on the sell of this stock symbol! So you will never be able to get 'ACIDSELL' out 
        /// of your portfolio, by design, until you re-set the database.
        /// </summary>
        /// <param name="order"></param>
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void SubmitOrderTransactedQueue(OrderDataModel order)
        {
            processOrder(order);
            return;
        }

        /// <summary>
        /// SubmitOrder service operation is a service operation that processes order messages from the client
        /// coming in via TCP, HTTP, or a non-transacted (volatile) MSMQ WCF binding from either the BSL or another remote instance
        /// </summary>
        /// <param name="order">Order to submit.</param>
        public void SubmitOrder(OrderDataModel order)
        {
           System.Transactions.TransactionOptions txOps = new TransactionOptions();
           txOps.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
           txOps.Timeout = TimeSpan.FromSeconds(Settings.SYSTEMDOTTRANSACTION_TIMEOUT);
           try
           {
               dalOrder = Trade.DALFactory.Order.Create(Settings.DAL);
               dalOrder.Open(Settings.TRADEDB_SQL_CONN_STRING);
               dalOrder.getSQLContextInfo();
           }
           catch { }
           finally { dalOrder.Close(); }
           //  Start our System.Transactions tx with the options set above.
           using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required, txOps))
            {
                try
                {
                    processOrder(order);

                    //Complete transaction scope!
                    tx.Complete();
                }
                catch (Exception e)
                {
                    string innerException = null;
                    if (e.InnerException != null)
                         innerException = e.InnerException.ToString();
                    ConfigUtility.writeErrorConsoleMessage("Error Completing Order! Exception: " + e.ToString() + "\nInner Exception: " + innerException,EventLogEntryType.Error,true, new Settings());
                    throw;
                }
            }
        return;
        }

        /// <summary>
        /// Processes the order and creates holding, or sells stock.
        /// </summary>
        /// <param name="queuedOrder">Order to process.</param>
        private void processOrder(OrderDataModel queuedOrder)
        {
            //You could extend this application by transforming the order message
            //as it comes in to another format, if required.  In this case, 
            //no transformation actually takes place since the types are the same.  
            //You can imagine different systems bridged might have different schema, 
            //however, and transformations could be rather easily accomplished here. 

            if (queuedOrder.orderID % Settings.DISPLAYNUMBERORDERITERATIONS == 0)
            {
                string message = string.Format("Processing Order {0}, symbol {1}, type {2}.", queuedOrder.orderID.ToString(), queuedOrder.symbol, queuedOrder.orderType);
                ConfigUtility.writeConsoleMessage(message + "\n",EventLogEntryType.Information,false, new Settings());
            }
            ProcessOrder orderService = new ProcessOrder();
            orderService.ProcessAndCompleteOrder(queuedOrder);
            return;
        }
    }
}
