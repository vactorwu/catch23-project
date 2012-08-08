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
// This is one of three optional host programs for the Business Service Layer.  In this case a Windows  
// application.  You can also optionally run the console host, as well as the IIS-hosted implementation.  
//======================================================================================================


using System;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceNodeCommunicationImplementation;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceConfigurationSettings;
using Trade.BusinessServiceHostConfigurationImplementation;
using Trade.BusinessServiceImplementation;
using Trade.OrderProcessorContract;



namespace Trade.BusinessServiceHost
{
    public class BusinessServiceHost
    {

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConfigUtility.setAzureRuntime(false);
            
            //This is the key call:  Note pass in your configuration implementation class and you primary service class Make sure
            //to add references (+ 'using' statements) as shown above.  You will also need to add referencess (but not using statements) to 
            //a couple ConfigService base classes, Visual Studio will tell you which ones, find them in the SharedLibraries folder.
            List<ServiceHostInfo> startupList = new List<ServiceHostInfo>(new ServiceHostInfo[] { new ServiceHostInfo(false, null, new object[] { new Trade.BusinessServiceImplementation.ErrorBehaviorAttribute() }, new TradeServiceBSL()) });
            Application.Run(new BusinessServiceConsole(new Settings(), new ConfigurationService(), new NodeCommunication(), null, new ConfigurationActions(true), startupList, null, new object[] { typeof(IOrderProcessor) }, "Trade.BusinessServiceHost.BusinessServiceBanner.bmp"));
        }
    }
}