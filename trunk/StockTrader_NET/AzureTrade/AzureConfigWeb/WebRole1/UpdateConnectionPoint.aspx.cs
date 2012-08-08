//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Delete action to delete an existing Connection Point.
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
    public partial class UpdateConnectionPoint : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        ServiceUsers user;
        int connectionID;
        ServiceConfigurationClient configProxy;
        string action;
        ConnectionPoints thisConnection;
        ConnectionPoints oldConnection;
        List<TraverseNode> traversePath;
        string client = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            userid = HttpContext.Current.User.Identity.Name;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONNECTIONS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Connections Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
            action = ConfigUtility.DELETE_CONNECTION;
            client = Request["client"];
            AddressLabel.Visible = true;
            getData();
        }

        private void getData()
        {
            string eMessage = null;
            ClientService.Text = hostNameIdentifier;
            if (IsPostBack)
            {
                thisConnection = (ConnectionPoints)ViewState["connection"];
                oldConnection = (ConnectionPoints)ViewState["oldConnection"];
                traversePath = (List<TraverseNode>)ViewState["traversePath"];
                connectionID = (int)ViewState["connectionID"];
            }
            else
            {
                connectionID = Convert.ToInt32(Request["connectionID"]);
                try
                {
                    traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                    if (client != "true")
                        thisConnection = configProxy.getConnectionPoint(hostNameIdentifier, configName, connectionID, false, traversePath, user);
                    else
                    {
                        List<HostListenEndPointInstance> clients = configProxy.getActiveClients(hostNameIdentifier, configName, false, traversePath, user);
                        thisConnection = new ConnectionPoints(hostNameIdentifier, configName);
                        thisConnection.MyConnectionPoints = clients;
                        thisConnection.MyConnectionPoints.RemoveAll(delegate(HostListenEndPointInstance epExist) { return epExist.InstanceID != connectionID; });
                    }
                }
                catch (Exception e)
                {
                    eMessage = "Error getting Connection Points from remote host @ " + address + "<br/>Exception is: " + e.ToString();
                    try
                    {
                        EventLog EventLog1 = new EventLog("Application");
                        EventLog1.Source = ConfigSettings.EVENT_LOG;
                        EventLog1.WriteEntry(eMessage, EventLogEntryType.Error);
                    }
                    catch
                    {
                    }
                }
                ViewState["oldConnection"] = thisConnection;
                ViewState["connection"] = thisConnection;
                ViewState["traversePath"] = traversePath;
                ViewState["connectionID"] = connectionID;
            }
            if (thisConnection == null)
                ReturnLabel.Text = eMessage;
            else
            {
                ServiceHost.Text = thisConnection.MyConnectionPoints[0].HostNameIdentifier;
                AddressLabel.Text = thisConnection.MyConnectionPoints[0].RemoteAddress;

                ReturnLabel.Text = "<br/><a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONNECTIONS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "\">Return to Connection Page</a><br/><br/>";

            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            string actiontext = "";
            action = ConfigUtility.DELETE_CONNECTION;
            oldConnection = (ConnectionPoints)ViewState["oldConnection"];
            string userid = HttpContext.Current.User.Identity.Name;
            if (userid == null)
                Response.Redirect(FormsAuthentication.LoginUrl,true);
            actiontext = "deleted";
            Delete.Enabled = false;
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            string eMessage = null;
            try
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                success = configProxy.receiveConnectionPoint(hostNameIdentifier, configName, oldConnection.MyConnectionPoints[0], thisConnection.MyConnectionPoints[0], true, null, action, traversePath, user);
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
                Message.Text = "<br/><span style=\"color:PaleGreen \">The connection point was sucessfully " + actiontext + ".</span>";
                AddressLabel.Text = thisConnection.MyConnectionPoints[0].RemoteAddress;
                ViewState["connection"] = thisConnection;
                ViewState["oldConnection"] = thisConnection;
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
                thisConnection = oldConnection;
            }
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