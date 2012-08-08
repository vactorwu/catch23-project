using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceHostConfigurationImplementation;
using Trade.BusinessServiceConfigurationSettings;
using Trade.Utility;

namespace TradeWebBSL
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigUtility.setAzureRuntime(false);
            ConfigurationActions myConfigActions = new ConfigurationActions();
            Application["masterHost"] = ServiceConfigHelper.MasterServiceWebHost.MasterHost;
            if (Settings.ORDER_PROCESSING_MODE == StockTraderUtility.OPS_INPROCESS)
                InitConfigInProcessOrderService.initConfigOrderProcessService(ConfigUtility.masterServiceWebHostSyncObject, false);
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_Start: Web App New BSL Node Starting. Hello!\n", EventLogEntryType.Information, true, new Trade.BusinessServiceConfigurationSettings.Settings());
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_End: Web App BSL Node Shutting Down\n", EventLogEntryType.Warning, true, new Trade.BusinessServiceConfigurationSettings.Settings());
            ServiceConfigHelper.MasterServiceWebHost masterHost = (ServiceConfigHelper.MasterServiceWebHost)Application["masterHost"];
            if (masterHost != null)
                masterHost.deActivateHosts();
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_End: Web App BSL Node ShutDown COMPLETE. Goodbye!\n", EventLogEntryType.Warning, true, new Trade.BusinessServiceConfigurationSettings.Settings());
        }
    }
}