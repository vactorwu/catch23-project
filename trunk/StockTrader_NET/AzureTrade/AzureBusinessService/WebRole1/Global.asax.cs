﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;
using System.Threading;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.AzureConfigUtility;
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceHostConfigurationImplementation;
using Trade.BusinessServiceConfigurationSettings;
using Trade.Utility;

namespace AzureBusinessServiceRole
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigUtility.setAzureRuntime(true);
            ConfigurationActions myConfigActions = new ConfigurationActions();
            Application["masterHost"] = ServiceConfigHelper.MasterServiceWebHost.MasterHost;
            if (Settings.ORDER_PROCESSING_MODE == StockTraderUtility.OPS_INPROCESS)
                InitConfigInProcessOrderService.initConfigOrderProcessService(ConfigUtility.masterServiceWebHostSyncObject, false);
            ConfigUtility.writeConsoleMessage("\nWeb Role Application_Start: New Node ID: " + AzureUtility.getRoleInstanceID() + " Has Started Successfully and Initialized Its Configuration From the Configuration Database. Welcome to Windows Azure!\n", EventLogEntryType.Warning, true, new Trade.BusinessServiceConfigurationSettings.Settings());
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
            ConfigUtility.writeConsoleMessage("\nWeb Role Application_End: Node ID: " + AzureUtility.getRoleInstanceID() + " Is Starting Shutdown Process...\n", EventLogEntryType.Warning, true, new Trade.BusinessServiceConfigurationSettings.Settings());
            ServiceConfigHelper.MasterServiceWebHost masterHost = (ServiceConfigHelper.MasterServiceWebHost)Application["masterHost"];
            if (masterHost != null)
                masterHost.deActivateHosts();
            ConfigUtility.writeConsoleMessage("\nWeb Role Application_End: Node ID: " + AzureUtility.getRoleInstanceID() + " Has Shut Down. Goodbye!\n", EventLogEntryType.Warning, true, new Trade.BusinessServiceConfigurationSettings.Settings());
        }
    }
}