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
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
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
using Trade.OrderProcessorServiceConfigurationSettings;
using Trade.OrderProcessorHostConfigurationImplementation;
using Trade.OrderProcessorContract;
using Trade.OrderProcessorImplementation;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using ConfigService.AzureConfigUtility;
using System.Reflection;
using System.Configuration;

namespace AzureOPSRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private static MyHost myHost;
        private static EndPointBehaviors endpointBehaviorList;
        public static List<ServiceHostInfo> startupList;

        /// <summary>
        /// The program entry class. Note how this simply inherits from the provided base class.
        /// </summary>
        class MyHost : ShellServiceConsoleBase
        {
            /// <summary>
            /// This is the key call where you will define parameters for the Master host startup, and call
            /// the base 'startService' method.
            /// </summary>
            public void startUp()
            {
                ConfigUtility.setAzureRuntime(true);
                //First we will create a list to hold our runtime-created endpoint behavior.  Note we could also simply
                //use config to define, and select via ConfigWeb on the endpoint (HostedService) definition, this is an
                //alternate mechanism to provide via code as an example.
                List<object> endpointBehaviors = new List<object>();
                //Now we will construct (and add to our list) an endpoint behavior, in this case a TransactedBatchingBehavior.  This behavior
                //will be applied to the MSMQ endpoint, and allows .NET to process multiple entries from the queue as part of
                //a single distributed transaction---a performance benefit that must be balanced against potential database concurrency issues
                //if set too high.
                endpointBehaviors.Add(new TransactedBatchingBehavior(5));
                endpointBehaviorList = new EndPointBehaviors(endpointBehaviors, null);
                //Now the key call to create our list of runtime hosts to be initialized.
                startupList = new List<ServiceHostInfo>(new ServiceHostInfo[] { new ServiceHostInfo(false, null, new object[] { new Trade.OrderProcessorImplementation.ErrorBehaviorAttribute() }, new OrderProcessor()) });
                //Stock call to startup the Master Host.
                base.startNTService(new Settings(), new ConfigurationService(), new NodeCommunication(), null, new ConfigurationActions(), startupList, endpointBehaviorList, new object[] { typeof(IOrderProcessor) });
            }
        }

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            //Trace.WriteLine("Azure Order Processor entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
            }
        }

        //Event Handler
        public void RoleInstanceShutdown(object sender, RoleEnvironmentStoppingEventArgs e)
        {

            myHost.deActivateHosts();
            ConfigUtility.writeConsoleMessage("\nWorker Role RoleInstanceShutdown: Node ID: " + AzureUtility.getRoleInstanceID() + " Has Shut Down. Goodbye!\n", EventLogEntryType.Warning, true, new Settings());

        }

        //Event Handler
        public void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            /*var changes = from ch in e.Changes.OfType<RoleEnvironmentTopologyChange>()
                          select ch;
            if (changes.Any())
            {
                myHost.deActivateHosts();
                ConfigUtility.writeConsoleMessage("\nWorker Role OnStop: Node ID: " + AzureUtility.getRoleInstanceID() + " Has Shut Down. Goodbye!\n", EventLogEntryType.Warning, true, new Sample.HelloServiceSettings.Settings()); 
            }*/
        }


        public override bool OnStart()
        {

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 64;

            //Wire up our key event handlers, to ensure proper lifecycle management.
            RoleEnvironment.Stopping += RoleInstanceShutdown;
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            myHost = new MyHost();
            myHost.startUp();
            return base.OnStart();
        }

        public override void OnStop()
        {
            Thread.Sleep(22000);
            base.OnStop();
        }
    }
}
