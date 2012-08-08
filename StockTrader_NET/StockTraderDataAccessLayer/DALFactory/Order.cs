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
// The Factory class that returns an instance of the Order Data Access Class, part of the Data Access 
// Layer (DAL).  This will return either a SQL Server DAL, or an Oracle DAL, depending on the configuration
// the .NET StockTrader application is running in, as set in the repository via ConfigWeb. To run against
// Oracle 10G or 11G, you will need to download Oracle's .NET ODP and Oracle client for Windows, and install
// on your nodes.
//======================================================================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Security.Policy;
using Trade.IDAL;

namespace Trade.DALFactory
{
    public sealed class Order
    {

        public static Trade.IDAL.IOrder Create(string path)
        {
           

            string className = path + ".Order";

            // Using the evidence given in the config file load the appropriate assembly and class
            return (Trade.IDAL.IOrder)Assembly.Load(path).CreateInstance(className);
        }
    }
}
