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
// This is the client for the OrderProcessorService.
//======================================================================================================


using System;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Globalization;
using System.Reflection;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.LoadBalancingClient;
using Trade.BusinessServiceDataContract;
using Trade.OrderProcessorContract;
using Trade.Utility;

namespace Trade.OrderProcessorAsyncClient
{
    /// <summary>
    /// This is the WCF client class for the remote Order Processor Services. This class implements channel initialization and
    /// load balancing/failover logic across cached channel instances specific to the Configuration Management/Node services
    /// implemented in StockTrader via the LoadBalancingClient.Client class, now re-used across all clients 
    /// implementing the configuration service. 
    /// </summary>
    public class TradeOrderServiceAsyncClient : IOrderProcessor
    {
        public Client opsclient;
        private object _settingsInstance;
        private string _orderMode = "In-Process Activation";

        /// <summary>
        /// This will initialize the correct client/endpoint based on the OrderMode setting the user has set
        /// in the repository.
        /// </summary>
        /// <param name="orderMode">The order mode, determines what type of binding/remote interface is used for communication.</param>
        /// <param name="settingsInstance">instance of this host's Settings class.</param>
        public TradeOrderServiceAsyncClient(string orderMode, object settingsInstance)
        {
            _settingsInstance = settingsInstance;
            _orderMode = orderMode;
            opsclient = new Client(orderMode, settingsInstance);
        }

        /// <summary>
        /// This returns the base channel type, cast to the specific contract type.
        /// </summary>
        public IOrderProcessor Channel
        {
            get
            {
                return (IOrderProcessor)opsclient.Channel;
            }
            set
            {
                opsclient.Channel = (IChannel)value;
            }
        }

        /// <summary>
        /// The online check method.
        /// </summary>
        public void isOnline()
        {
            try
            {
                this.Channel.isOnline();
                return;
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Submits an order via transacted MSMQ, over WCF service binding.
        /// </summary>
        /// <param name="order"></param>
        public void SubmitOrderTransactedQueue(OrderDataModel order)
        {
            try
            {
                this.Channel.SubmitOrderTransactedQueue(order);
                return;
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Submits an order to a non-transacted queue.
        /// </summary>
        /// <param name="order"></param>
        public void SubmitOrder(OrderDataModel order)
        {
            try
            {
                this.Channel.SubmitOrder(order);
                return;
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// This method sends an order off to the order processor service, in an async fashion depending on OrderMode setting.
        /// </summary>
        /// <param name="order">Order to submit.</param>
        public void processOrderASync(Trade.BusinessServiceDataContract.OrderDataModel order)
        {
            try
            {
                if (_orderMode == StockTraderUtility.OPS_SELF_HOST_MSMQ)

                    //SubmitOrderTransactedQueue() has an operation contract definition as follows: 
                    
                    // ----OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
                    //-----public void SubmitOrderTransactedQueue(Trade.TraderOrderHost.QueuedOrder order)
 
                    //and is part of a service definition that is Async by way of
                    //the <oneWay> definition in the service contract and MSMQ binding.

                    //The SubmitOrderTransactedQueue() operation is a write to this durable queue, through the magic of
                    //WCF as a developer you just interact with the service according to the service contract.
                    //You never actually talk to an MSMQ directly, or even import the System.Messaging namespace.
                    //Rather, this is a service-oriented model; and WCF is *automatically*, via the binding,
                    //send the order to a queue (local or remote), and not directly to the service itself.
                    //Hence, it's loosely coupled.  This service call is wrapped in a transaction becuase it is
                    //a service operation that is bound to a *transacted* queue. Transacted queues are durable 
                    //(persisted) queues that are used to ensure messages are never lost once delivered. 
                    //The read operation from the queue by the remote service we are submitting to is triggered based on 
                    //messages being present in the (same) queue the service host is bound to.  

                    //So we have two distinct two-phase, DTC-coordinated transaction going on:
                    //One here, on the client send side (if you cannot send the message to the service, then rollback
                    //the order header from the database!). The other on the service-side, where the read and
                    //order processing happen. Its important to understand these are two distinct
                    //distributed transactions. The first tx here on the client gaurantees the order message is either
                    //sent to the service, or the order header is rolledback from the database and the user notified
                    //the order could not be sent (in our case, by an exception we purposefully let flow back to the browser). 
                    //The SECOND tx on the service side gaurantees that once the message is received in the queue, it will not be lost during
                    //processing. The read operation is part of a distributed transaction. See the OrderProcesserImplementation project in the
                    // Order Processor Solution project to see the service implementation of SubmitOrderTransactedQueue().
                        
                    //send the order to our service!
                    SubmitOrderTransactedQueue(order);

                else

                    //SubmitOrder is not defined in the service contract as a transacted operation
                    //and is used when the order processing binding is other than a transacted queue (e.g. Async_Tcp;
                    //Async_Http, Async_Msmq_Volatile). 
                    
                    //Note the SEND (below) is still part of a tx operation from the BSL (client); however, on the
                    //service side since the source of the message is non-durable, it is a one-phase transaction on the processing side
                    //against our database since it will not be doing a read from a transacted queue as part of the processing operation.
                         SubmitOrder(order);
            }
            catch (Exception e)
            {
                string innerException = null;
                if (e.InnerException != null)
                    innerException = e.InnerException.ToString();
                ConfigUtility.writeErrorConsoleMessage(e.ToString() + "\nInner Exception: " + innerException,EventLogEntryType.Error,true,_settingsInstance);
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// The OPS service will use this when running in OrderProcessingBehavior="Forward".  It fowards the order to a another
        /// remote instance of the Order Processing Service, perhaps over a different binding even, and does not actually
        /// process the order.  This makes this instance an intermediary.
        /// </summary>
        /// <param name="order">Order to submit.</param>
        public void forwardOrderTransactedQueue(OrderDataModel order)
        {
            try
            {
                SubmitOrderTransactedQueue(order);
                return;
            }
            catch (Exception e)
            {
                string innerException = null;
                if (e.InnerException != null)
                    innerException = e.InnerException.InnerException.ToString();
                ConfigUtility.writeErrorConsoleMessage(e.ToString() + "\nInner Exception: " + innerException, EventLogEntryType.Error, true, _settingsInstance);
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// The OPS service will use this when running in OrderProcessingBehavior="Forward".  It fowards the order to a another
        /// remote instance of the Order Processing Service, perhaps over a different binding even, and does not actually
        /// process the order.  This makes this instance an intermediary.
        /// </summary>
        /// <param name="order">Order to submit.</param>
        public void forwardOrder(OrderDataModel order)
        {
            try
            {
                SubmitOrder(order);
                return;
            }
            catch (Exception e)
            {
                string innerException = null;
                if (e.InnerException != null)
                    innerException = e.InnerException.InnerException.ToString();
                ConfigUtility.writeErrorConsoleMessage(e.ToString() + "\nInner Exception: " + innerException, EventLogEntryType.Error, true, _settingsInstance);
                this.Channel = null;
                throw;
            }
        }
    }
}