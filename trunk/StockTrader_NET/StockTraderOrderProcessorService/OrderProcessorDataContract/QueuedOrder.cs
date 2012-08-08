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

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Trade.OrderProcessorDataContract
{

    // Define  Data Contract for the Order that will be passed asynchronously. Note we could also
    // simply use the existing StockTrader/Trade 6.1 OrderDataModel class directly, which is already
    // exposed via a DataContract for the Web Service operations.  This code could be eliminated, but
    // also could be useful for allowing mappings/transformations between order types in two separate
    // systems.
    [DataContract(Namespace = "http://stocktrader.samples.microsoft.com")]  //Attribute for WCF System.Runtime.Serialization
    public class QueuedOrder
    {
        [DataMember]
        public int OrderID;

        [DataMember]
        public int HoldingID;

        [DataMember]
        public string OrderType;

        [DataMember]
        public string OrderStatus;

        [DataMember]
        public DateTime OpenDate;

        [DataMember]
        public DateTime CompletionDate;

        [DataMember]
        public double Quantity;

        [DataMember]
        public decimal Price;

        [DataMember]
        public decimal OrderFee;

        [DataMember]
        public string Symbol;

        [DataMember]
        public int AccountID;
    }
}
