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
// This is one of three optional host programs for the Business Service .  In this case a Windows  
// NT Service.  You can also optionally run the console host, or the Windows .NET Forms host.  
//======================================================================================================


using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationContract;
using ConfigService.ServiceNodeCommunicationContract;
using ConfigService.IConfigActions;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceNodeCommunicationImplementation;
using ConfigService.ServiceHostShellConsoleBase;
using Trade.BusinessServiceConfigurationSettings;
using Trade.BusinessServiceHostConfigurationImplementation;
using Trade.BusinessServiceContract;
using Trade.BusinessServiceImplementation;
using Trade.OrderProcessorContract;
using System.ServiceProcess;
using System.Reflection;
using System.ComponentModel;


namespace Trade.BusinessServiceNTServiceHost
{

    public partial class BSLService : ServiceBase
    {
        MyHost myHost;
        public BSLService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            myHost = new MyHost();
            myHost.startUp();
        }

        protected override void OnStop()
        {
            if (myHost!=null)
                try
                {
                    myHost.deActivateHosts();
                }
                catch { }
        }
    }


        /// <summary>
        /// The program entry class. Note how this simply inherits from the provided base class.
        /// </summary>
        class MyHost : ShellServiceConsoleBase
        {
            /// <summary>
            /// This is the key call where you will define parameters for the Master host startup, and call
            /// the base 'startService' method.
            /// </summary>
            /// 

            //private static EndPointBehaviors endpointBehaviorList;
            //public static List<ServiceHostInfo> startupList;

            public void startUp()
            {
                ConfigUtility.setAzureRuntime(false);
                //The key call to create our list of runtime hosts to be initialized.
                List<ServiceHostInfo> startupList = new List<ServiceHostInfo>(new ServiceHostInfo[] { new ServiceHostInfo(false, null, new object[] { new Trade.BusinessServiceImplementation.ErrorBehaviorAttribute() }, new TradeServiceBSL()) });
                //Stock call to startup the Master Host.
                base.startNTService(new Trade.BusinessServiceConfigurationSettings.Settings(), new ConfigurationService(), new NodeCommunication(), null, new ConfigurationActions(true), startupList, null, new object[] { typeof(IOrderProcessor) });
            }
        }
    }


    
