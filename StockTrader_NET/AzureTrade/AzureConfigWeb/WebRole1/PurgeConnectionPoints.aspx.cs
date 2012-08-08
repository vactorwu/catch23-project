//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to purge connections for a given Virtual Host.  Purge removes all connections to all
//services,  but does not remove Connected Service Definitions to these services.  
//============================================================================================
//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================
using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class PurgeConnectionPoints : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        string purgeHostName;
        string purgeConfigName;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        string action;
        List<TraverseNode> traversePath;
        string client;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = Purge.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                purgeHostName = (string)ViewState["purgename"];
                purgeConfigName = (string)ViewState["purgeconfigname"];
            }
            else
            {
                purgeHostName = (string)Request["purgename"];
                purgeConfigName = (string)Request["purgecfgSvc"];
                ViewState["purgename"] = purgeHostName;
                ViewState["purgeconfigname"] = purgeConfigName;
            }
            action = ConfigUtility.PURGE_CONNECTIONS;
            client = Request["client"];
            RemoteHost.Text = "<span style=\"color:Navy;font-weight:normal\">" + purgeHostName + "</span>";
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONNECTIONS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Connections Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        protected void Purge_Click(object sender, EventArgs e)
        {
            string actiontext = "";
            actiontext = "purged";
            Purge.Enabled = false;
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            string eMessage = null;
            HostListenEndPointInstance connPurge = new HostListenEndPointInstance();
            connPurge.HostNameIdentifier = purgeHostName;
            connPurge.ConfigServiceImplementationClassName = purgeConfigName;
            if (client != "true")
                connPurge.ServiceType = ConfigUtility.HOST_TYPE_CONNECTED_SERVICE;
            else
                connPurge.ServiceType = ConfigUtility.HOST_TYPE_CONNECTED_CLIENT_CONFIG;
            try
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                success = configProxy.receiveConnectionPoint(hostNameIdentifier, configName, connPurge, connPurge, true, null, action, traversePath, user);
            }
            catch (Exception ee)
            {
                try
                {
                    EventLog EventLog1 = new EventLog("Application");
                    EventLog1.Source = ConfigSettings.EVENT_LOG;
                    eMessage = "<br/>receiveConnectionPoint exception from remote service.<br/>Exception is: " + ee.ToString();
                    EventLog1.WriteEntry(eMessage, EventLogEntryType.Error);
                }
                catch
                {
                }
            }
            if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
            {
                Message.Text = "<br/><span style=\"color:PaleGreen \">The connection points were sucessfully " + actiontext + ".</span>";
            }
            else
            {
                string message = null;
                switch (success)
                {
                    case ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED:
                        {
                            message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_PERSISTED;
                            break;
                        }

                    case ConfigUtility.CLUSTER_UPDATE_FAIL_VALIDATION:
                        {
                            message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_VALIDATION;
                            break;
                        }

                    case ConfigUtility.CLUSTER_UPDATE_FAIL_AUTHENTICATION:
                        {
                            message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
                            break;
                        }
                    case ConfigUtility.CLUSTER_UPDATE_FAIL_REMOTE:
                        {
                            message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_REMOTE_UPDATE;
                            break;
                        }
                    default:
                        {
                            message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_REMOTE_PEER + success.ToString();
                            break;
                        }
                }
                Message.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                Purge.Enabled = true;
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigSettings.PAGE_CONNECTIONS + "?name=" + hostNameIdentifier + "&cfgsvc=" + configName + "&version=" + version + "&hoster=" + hoster + "&platform=" + platform, true);
        }
        private void home(bool logout)
        {
            if (logout)
                Response.Redirect(ConfigSettings.PAGE_LOGOUT,true);
            else
                Response.Redirect(ConfigSettings.PAGE_NODES,true);
        }
    }
}