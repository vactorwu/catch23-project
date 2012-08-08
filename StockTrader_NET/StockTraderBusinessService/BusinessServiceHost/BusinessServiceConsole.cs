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
// The Business Services Windows self-host program, which just inherits from the provided class, so is quite
// simple given the functionality it provides.
//======================================================================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceHostConsoleBase;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceHostConfigurationImplementation;
using Trade.BusinessServiceConfigurationSettings;
using Trade.Utility;


namespace Trade.BusinessServiceHost
{
       
    /// <summary>
    /// This class inherits from ServiceConsoleBase, a base class anyone can use as a Windows-based service host
    /// console. Simply inherit from the base class, which provides all the core functionality and UI! 
    /// Make sure to specify the single constructor below, deriving from and calling the base constructor.
    /// </summary>
    public class BusinessServiceConsole : ServiceConsoleBase
    {
        /// <summary>
        /// This constructor calls the base constructor, and starts up all services and initializes the Windows Form.
        /// </summary>
        /// <param name="theSettingsInstance">An instance of the Settings class for the app.</param>
        /// <param name="theConfigurationService">An instance of the configuration service implementation class for the app/service.</param>
        /// <param name="theNodeService">An instance of the Node Service/implementation class</param>
        /// <param name="theNodeDcService">An instance of the Node DC Service/implementation class.</param>
        /// <param name="theConfigurationActions">An instance of ConfigurationActions class for the app</param>
        /// <param name="startupList">List of hosts to startup.</param>
        /// <param name="endpointBehaviors">Any endpoint behaviors specified in code.</param>
        /// <param name="connectedServiceContracts">List of contracts for services this host will consume.</param>
        /// <param name="bannerImageResourceName">You can provide a custom banner, which should be defined as an embedded resource within visual studio</param>
        public BusinessServiceConsole(object theSettingsInstance, object theConfigurationService, object theNodeService, object theNodeDcService, object theConfigurationActions, List<ServiceHostInfo> startupList, EndPointBehaviors endpointBehaviors, object[] connectedServiceContracts, string bannerImageResourceName)
            : base(theSettingsInstance, theConfigurationService, theNodeService, theNodeDcService, theConfigurationActions, startupList, endpointBehaviors, connectedServiceContracts, bannerImageResourceName)
        {
           
        }

        //This will be added by visual studio automatically, so do not add manually in code.
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainConsole
            // 
            this.MainConsole.Location = new System.Drawing.Point(8, 196);
            this.MainConsole.Margin = new System.Windows.Forms.Padding(5);
            this.MainConsole.Size = new System.Drawing.Size(992, 357);
            // 
            // Initialize
            // 
            this.Initialize.Location = new System.Drawing.Point(11, 169);
            // 
            // OrderProcessorServiceConsole
            // 
            this.ClientSize = new System.Drawing.Size(1023, 775);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BusinessServiceConsole";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        /// <summary>
        /// Performs steps (if needed) to be performed after service host startup.  The base implementation
        /// will work, we are overriding here simply becuase for BSL, we  want
        /// to init OPS in-process as a special step, and display a custom message after startup with the Order Processing Mode
        /// for the BSL.
        /// </summary>
        /// <param name="message">Message to display on main tab of service host console.</param>
        public override void PostInitProcedure(string message)
        {
            if (message.StartsWith("EXCEPTION") || Settings.ORDER_PROCESSING_MODE ==null)
            {
                base.PostInitProcedure(message);
                return;
            }
            if (Initialize.InvokeRequired == false)
            {
                Initialize.Text = "Order Mode is: " + Settings.ORDER_PROCESSING_MODE;
                ConfigUtility.writeConsoleMessage("\nMaster Host Initialization is Now Complete!\n", EventLogEntryType.Information,true,settingsInstance);
            }
            else
            {
                // Show message asynchronously
                PostInitProcedureDelegate showdoneinit = new PostInitProcedureDelegate(PostInitProcedure);
                this.BeginInvoke(showdoneinit, new object[] { message });
            }
        }

        delegate void ShowOrderModeDelegate(string message, object settingsInstance);
        /// <summary>
        /// The only unique/new method added on top of our base class for Business Services self-host Console.
        /// </summary>
        /// <param name="message">String message to display in main tab of base console</param>
        /// <param name="settingsInstance">Instance of the Settings class for this app.</param>
        public void ShowOrderMode(string message, object settingsInstance)
        {
            if (Initialize.InvokeRequired == false)
            {
                Initialize.Text = "Order Mode is: " + Settings.ORDER_PROCESSING_MODE;
            }
            else
            {
                // Show message asynchronously
                ShowOrderModeDelegate showordermode = new ShowOrderModeDelegate(ShowOrderMode);
                this.BeginInvoke(showordermode, new object[] { message, settingsInstance });
            }
        }
    }
}
