using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceNodeCommunicationImplementation;
using ConfigService.ServiceConfigurationUtility;
using Trade.StockTraderWebApplicationSettings;
using Trade.StockTraderWebApplicationConfigurationImplementation;
using System.Diagnostics;
using Trade.Utility;


namespace Trade.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigUtility.setAzureRuntime(false);
            ConfigurationActions myConfigActions = new ConfigurationActions();
            Application["masterHost"] = ServiceConfigHelper.MasterServiceWebHost.MasterHost;
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_Start: Web App New TradeWeb Node Starting. Hello!\n", EventLogEntryType.Warning, true, new Trade.StockTraderWebApplicationSettings.Settings());
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
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_End: Web App TradeWeb Node Shutting Down!\n", EventLogEntryType.Warning, true, new Trade.StockTraderWebApplicationSettings.Settings());
            ServiceConfigHelper.MasterServiceWebHost masterHost = (ServiceConfigHelper.MasterServiceWebHost)Application["masterHost"];
            if (masterHost != null)
                masterHost.deActivateHosts();
            ConfigUtility.writeConsoleMessage("\nWeb Application Global Application_End: Web App TradeWeb Node ShutDown COMPLETE. Goodbye!\n", EventLogEntryType.Warning, true, new Trade.StockTraderWebApplicationSettings.Settings());
        }
    }
}