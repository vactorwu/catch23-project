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
//  Service-Oriented Applications

//======================================================================================================
// This is one of two optional host programs for the Order Processor Service .  In this case a Windows  
// application.  You can also optionally run the console host.  
//======================================================================================================



using System;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.ServiceModel;
using System.ServiceModel.Description;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceNodeCommunicationImplementation;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationUtility;
using Trade.OrderProcessorServiceConfigurationSettings;
using Trade.OrderProcessorHostConfigurationImplementation;
using Trade.OrderProcessorContract;
using Trade.OrderProcessorImplementation;

namespace Trade.OrderProcessorServiceHost
{
    /// <summary>
    /// The program entry class. 
    /// </summary>
    public class OrderProcessorServiceHost
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigUtility.setAzureRuntime(false);
            
            //Now the key call to create our list of runtime hosts to be initialized.
            List<ServiceHostInfo> startupList = new List<ServiceHostInfo>(new ServiceHostInfo[] { new ServiceHostInfo(false, null, new object[] { new Trade.OrderProcessorImplementation.ErrorBehaviorAttribute() }, new OrderProcessor()) });

            //Now we will create a list to hold our runtime-created endpoint behavior.  Note we could also simply
            //use config to define, and select via ConfigWeb on the endpoint (HostedService) definition, this is an
            //alternate mechanism to provide via code as an example.
            List<object> epBehaviorInstances = new List<object>();
            
            //Now we will construct (and add to our list) an endpoint behavior, in this case a TransactedBatchingBehavior.  This behavior
            //will be applied to the MSMQ endpoint, and allows .NET to process multiple entries from the queue as part of
            //a single distributed transaction---a performance benefit that must be balanced against potential database concurrency issues
            //if set too high.
            epBehaviorInstances.Add(new TransactedBatchingBehavior(5));
            EndPointBehaviors epBehaviors = new EndPointBehaviors(epBehaviorInstances, null);
            //Stock call to startup the Master Host.
            
            Application.Run(new OrderProcessorServiceConsole(new Settings(), new ConfigurationService(), new NodeCommunication(), null, new ConfigurationActions(), startupList,epBehaviors, new object[] { typeof(IOrderProcessor) }, "Trade.OrderProcessorServiceHost.OrderProcessorBanner.bmp"));
        }
  }
}
    