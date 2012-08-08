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
// The OrderProcessor ServiceContract, which is quite simple.
//======================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Threading;
using Trade.BusinessServiceDataContract;

namespace Trade.OrderProcessorContract
{
    /// <summary>
    /// This is the service contract for the Order Processor Service. It defines the service layer operations
    /// that are separately implemented in the implementation class.
    /// </summary> 
    [ServiceContract(Name = "OrderProcessorService", Namespace = "http://Trade.TraderOrderHost")]
    public interface IOrderProcessor
    {
        //IsOneWay Marks Methods as Asynchrounous. This is our transacted method for MSMQ transacted/durable
        //queue receive operations when OrderMode = Async_Msmq.
        [OperationContract(Action = "SubmitOrderTransactedQueue", IsOneWay = true)]
        void SubmitOrderTransactedQueue(OrderDataModel order);

        //IsOneWay Marks Methods as Asynchrounous.
        //This is our non-transacted method for async calls via http, tcp, and volatile (in-process) msmq.
        [OperationContract(Action = "SubmitOrder", IsOneWay = true)]
        void SubmitOrder(OrderDataModel order);

        /// <summary>
        /// Online check method.
        /// </summary>
        [OperationContract(Action = "isOnline", IsOneWay = true)]
        void isOnline();
    }
}

