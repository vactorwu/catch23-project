//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to remove a node for a given Virtual Host.  This would be done for a node that crashed,
//and is not intended to be brought back online, for instance. Ordinarily, a node gracefully
//closed will remove itself from the cluster.  However, for nodes that have otherwise exited,
//if that node (machine as identified by dns name or IP address) is never intended to be
//brought back online, this operation will clear it from the configuration repository. However,
//if it is brought back online, even after removing it via this page, it will of course re-activate
//itself in the cluster as normal.
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
    public partial class RemoveNode : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        int purgeInt;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        List<TraverseNode> traversePath;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = Delete.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                purgeInt = (int)ViewState["purgeID"];
            }
            else
            {
                string purgeID =(string) Request["nodeid"];
                purgeInt = Convert.ToInt32(purgeID);
                ViewState["purgeID"] = purgeInt;
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            Delete.Enabled = false;
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            string eMessage = null;
            try
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                success = configProxy.receiveRemoveDownedNode(hostNameIdentifier, configName, true, purgeInt, traversePath, user);
            }
            catch (Exception ee)
            {
                try
                {
                    EventLog EventLog1 = new EventLog("Application");
                    EventLog1.Source = ConfigSettings.EVENT_LOG;
                    eMessage = "<br/>receiveRemoveDownedNode exception from remote service.<br/>Exception is: " + ee.ToString();
                    EventLog1.WriteEntry(eMessage, EventLogEntryType.Error);
                }
                catch
                {
                }
            }
            if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
            {
                Message.Text = "<br/><span style=\"color:PaleGreen\">The node was sucessfully removed.</span>";
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
                Delete.Enabled = true;
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigSettings.PAGE_NODES, true);
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