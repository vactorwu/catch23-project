//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//This page contains the logic to add managed Connection Points to remote Connect Services.
//============================================================================================

//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.ServiceModel;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class AddConnection : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        string eMessage;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        ConnectionPoints connections;
        List<TraverseNode> traversePath;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = RadioButtonListHosts.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                connections = (ConnectionPoints)ViewState["connections"];
            }
            else
            {
                try
                {
                    traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                    connections = configProxy.getConnectionPoints(hostNameIdentifier, configName, new List<int>(new int[] { ConfigUtility.HOST_TYPE_CONNECTED_SERVICE, ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE }), false, traversePath, user);
                    ViewState["connections"] = connections;
                }
                catch (Exception ee)
                {
                    connections = null;
                    eMessage = "<span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">Error getting Connection Points from remote host @ " + address + "<br/>Exception is: " + ee.ToString() + "</span>";
                }
                
                if (connections.ConnectedServices != null)
                {
                    for (int i = 0; i < connections.ConnectedServices.Count; i++)
                    {
                        RadioButtonListHosts.Items.Add(new ListItem(connections.ConnectedServices[i].ServiceFriendlyName, connections.ConnectedServices[i].ServiceFriendlyName));
                    }
                    if (connections.ConnectedServices.Count == 0)
                    {
                        Add.Enabled = false;
                    }
                    else
                    {
                        RadioButtonListHosts.SelectedIndex = 0;
                        ConnectedServices thisCs = connections.ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName.Equals(RadioButtonListHosts.SelectedValue); });
                        ServiceHostAddress.Text = thisCs.DefaultAddress;
                    }
                }
            }
            if (connections.ConnectedServices == null || connections.ConnectedServices.Count == 0)
            {
                Add.Enabled = false;
                ServiceHostAddress.Enabled = false;
                if (eMessage == null)
                    Message.Text = "<span style=\"color:#cccccc;font-size:1.0em;font-weight:normal;\">This service does not have any Connected Services defined in it's configuration database.<br/> Use the Services tab to define valid services this service can connect to.</span>";
                else
                {
                    Message.Text = eMessage;
                }
            }
         
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONNECTIONS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Connections Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            string message = null;
            int success = ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED;
            HostListenEndPointInstance newConnection = new HostListenEndPointInstance();
            string serviceFriendlyName = RadioButtonListHosts.SelectedItem.Text;
            ConnectedServices thisCS = connections.ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName == serviceFriendlyName; });
            Uri testuri = null;
            if (thisCS.ServiceType == ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE)
            {

                try
                {
                    testuri = new Uri(ServiceHostAddress.Text);
                }
                catch
                {
                    testuri = null;
                    Message.Text = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">The connection point cannot be added becuase you have specified an invalid URI (endpoint address).  For Generic services, make sure you properly specify the full endpoint to the service; for Primary services, you just need to supply a server name/IP address, since the URI will be built from information already stored in the configuration database.</span>";
                }
            }
            else
                testuri = new Uri("http://localhost");
            if (testuri != null)
            {
                newConnection.ConnectedServiceID = thisCS.ConnectedServiceID;
                newConnection.ConnectedConfigServiceID = thisCS.ConnectedConfigServiceID;
                newConnection.VHostID = thisCS.VHostID;
                newConnection.RemoteAddress = ServiceHostAddress.Text.Trim().ToLower();
                if (thisCS.ServiceType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
                {
                    string[] splitString = newConnection.RemoteAddress.Split(new char[] { '/' });
                    if (splitString.Length > 1)
                    {
                        newConnection.RemoteAddress = splitString[2];
                    }
                    splitString = newConnection.RemoteAddress.Split(new char[] { ':' });
                    newConnection.RemoteAddress = splitString[0].ToLower();
                    ServiceHostAddress.Text = newConnection.RemoteAddress.ToLower();
                }
                newConnection.HostNameIdentifier = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].HostNameIdentifier;
                newConnection.Configuration = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].ClientConfiguration;
                newConnection.ServiceType = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].ServiceType;
                newConnection.ServiceImplementationClassName = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].ServiceImplementationClassName;
                newConnection.ServiceContract = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].ServiceContract;
                newConnection.ServiceFriendlyName = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].ServiceFriendlyName;
                newConnection.HostedServiceID = connections.ConnectedServices[RadioButtonListHosts.SelectedIndex].HostedServiceID;
                try
                {
                    traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                    List<ServiceConfigurationData> compositeConfigData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                    ConnectedConfigServices thisConfig = compositeConfigData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == newConnection.ConnectedConfigServiceID; });
                    if (thisConfig == null)
                        throw new Exception("Cannot find Configuration Service for this Connected Service Defininition in this Client's configuration database.  This entry is created when creating the Connected Service definition; and should never be manually deleted.");
                    newConnection.ConfigServiceImplementationClassName = thisConfig.ServiceImplementationClassName;
                    success = configProxy.receiveConnectionPoint(hostNameIdentifier, configName, newConnection, newConnection, true, null, ConfigUtility.ADD_CONNECTION, traversePath, user);
                }
                catch (Exception ee)
                {
                    message = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">receiveConnectionPoint exception. Exception is: " + ee.ToString();
                    Message.Text = Message.Text + "<br/>The Exception is: " + message + "</span>";
                }
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    Message.Text = "<br/><span style=\"color:#1B9427;font-size:13px;font-weight:normal;font-family:Verdana;\">The connection point was sucessfully added.</span>";
                }
                else
                {
                    switch (success)
                    {
                        case ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_PERSISTED + message + ". Make sure the remote service is running at the server/ip address specified.";
                                break;
                            }

                        case ConfigUtility.CLUSTER_UPDATE_FAIL_VALIDATION:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_VALIDATION + message;
                                break;
                            }

                        case ConfigUtility.CLUSTER_UPDATE_FAIL_AUTHENTICATION:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION + message;
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
                    Message.Text = "<br/><span style=\"color:Maroon;font-size:13px;font-weight:normal;font-family:Verdana;\">" + message + "</span>";
                }
            }
        }

        protected void RadioButtonListHosts_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConnectedServices thisCs = connections.ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName.Equals(RadioButtonListHosts.SelectedValue); });
            ServiceHostAddress.Text = thisCs.DefaultAddress;
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
