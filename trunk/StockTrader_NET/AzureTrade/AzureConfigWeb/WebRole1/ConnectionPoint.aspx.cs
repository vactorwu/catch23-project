//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to display Connection Points to remote services, categorized by remote virtual host,
//sorted by bindingType.
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
    public partial class ConnectionPoint : System.Web.UI.Page
    {
        string hostedID;
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        string viewTheType;
        int viewType;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        int index = -1;
        string currentHost;
        string newHost;
        ConnectionPoints connections;
        List<TraverseNode> traversePath;
        Dictionary<string, string> onlineList = new Dictionary<string, string>();
        List<ConnectedServices> connectedServices;
        List<ConnectedConfigServices> connectedConfigServices;
        List<ServiceConfigurationData> compositeServiceData;
        List<HostListenEndPointInstance> connectionsClients;
        string postback;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = ViewTypeHosts.ClientID;
            SessionInfo info = new SessionInfo();
            info.getSessionData(IsPostBack,out address, out user, out binding, out hostNameIdentifier, out configName, out hoster, out version, out platform, out hostedID);
            if (!(Request["version"] == "drilldown"))
                Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            else
            {
                hostNameIdentifier = Request["name"];
                configName = Request["cfgSvc"];
            }
            MSMQ.Visible = false;
            ConnectionRepeater.ItemDataBound += new RepeaterItemEventHandler(ConnectionRepeater_ItemDataBound);
            viewTheType = (string)Request["viewType"];
            if (viewTheType == null || viewTheType == "")
                viewTheType = ConfigUtility.HOST_TYPE_CONNECTED_SERVICE.ToString();
            viewType = Convert.ToInt32(viewTheType);
            switch (viewType)
            {
                case ConfigUtility.HOST_TYPE_CONNECTED_SERVICE:
                    {
                        ViewTypeHosts.CssClass = "LoginButton";
                        ViewTypeClients.CssClass = "LoginButtonNonSelected";
                        AddConnection.Enabled = true;
                        break;
                    }
                case ConfigUtility.HOST_TYPE_CONNECTED_CLIENT_CONFIG:
                    {
                        ViewTypeClients.CssClass = "LoginButton";
                        ViewTypeHosts.CssClass = "LoginButtonNonSelected";
                        AddConnection.Enabled = false;
                        break;
                    }
            }
            getData();
            postback = "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster;
            ConnectionRepeater.DataBind();
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + postback + "\">Return to Home Page</a>";
            ViewTypeHosts.PostBackUrl = ConfigSettings.PAGE_CONNECTIONS + postback + "&viewType=" + ConfigUtility.HOST_TYPE_CONNECTED_SERVICE.ToString();
            ViewTypeClients.PostBackUrl = ConfigSettings.PAGE_CONNECTIONS + postback + "&viewType=" + ConfigUtility.HOST_TYPE_CONNECTED_CLIENT_CONFIG.ToString();
            AddConnection.PostBackUrl = ConfigSettings.PAGE_ADD_CONNECTION + postback;
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void getData()
        {
            List<int> connectionPointTypes = new List<int>();
            connectionPointTypes.Add(ConfigUtility.HOST_TYPE_CONNECTED_SERVICE);
            connectionPointTypes.Add(ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE);
            string eMessage = null;
            try
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                connectedServices = compositeServiceData[0].ConnectedServices;
                if (Request["version"] == "drilldown")
                {
                    version = compositeServiceData[0].ServiceVersion;
                    platform = compositeServiceData[0].RunTimePlatform;
                    hoster = compositeServiceData[0].ServiceHoster;
                    ViewState["name"] = hostNameIdentifier;
                    ViewState["configname"] = configName;
                    ViewState["version"] = version;
                    ViewState["platform"] = platform;
                    ViewState["hoster"] = hoster;
                }
                connectedConfigServices = compositeServiceData[0].ConnectedConfigServices;
                if (viewType == ConfigUtility.HOST_TYPE_CONNECTED_CLIENT_CONFIG)
                {
                    connectionsClients = configProxy.getActiveClients(hostNameIdentifier, configName, true, traversePath, user);
                    connections = new ConnectionPoints(hostNameIdentifier, configName);
                    connections.MyConnectionPoints = connectionsClients;
                }
                else
                    connections = configProxy.getConnectionPoints(hostNameIdentifier, configName, connectionPointTypes, true, traversePath, user);
            }
            catch (Exception e)
            {
                connections = null;
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
            if (connections == null || connections.MyConnectionPoints == null)
            {
                Message.Text = "<span style=\"font-size:15px;color:Maroon\">Cannot retrieve connection points for " + hostNameIdentifier + " becuase this service Host is not online, you do not have administrative privledges for this connected service, or there is a startup configuration issue." + eMessage + "</span>";
                AddConnection.Enabled = false;
            }
            else
            {
                if (connections.MyConnectionPoints.Count == 0)
                {
                    if (viewType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
                        Message2.Text = "<span style=\"font-size:15px\">This service currently has no defined connection points to other services.</span>";
                    else
                        Message2.Text = "<span style=\"font-size:15px\">This service currently has no clients to its Primary Service(s).</span>";
                }
                else
                    Message2.Visible = false;
                Message.Visible = false;
            }
            if (connections != null && connections.MyConnectionPoints != null)
            {
                for (int i = 0; i < connections.MyConnectionPoints.Count; i++)
                {
                    if (connections.MyConnectionPoints[i].Status.Equals(ConfigSettings.MESSAGE_ONLINE) || connections.MyConnectionPoints[i].Status.Equals(ConfigSettings.MESSAGE_UNKNOWN))
                    {
                        try
                        {
                            onlineList.Add(connections.MyConnectionPoints[i].HostNameIdentifier, connections.MyConnectionPoints[i].RemoteAddress);
                        }
                        catch
                        {

                        }
                    }
                }
                ConnectionRepeater.DataSource = connections.MyConnectionPoints;
                TopNodeName.Text = connections.HostNameIdentifier;
                if (compositeServiceData != null && compositeServiceData.Count > 0 && compositeServiceData[0] != null)
                {
                    ServiceVersion.Text = compositeServiceData[0].ServiceVersion;
                    ServiceHoster.Text = "" + compositeServiceData[0].ServiceHoster;
                    hoster = ServiceHoster.Text;
                    ViewState["hoster"] = hoster;
                    ServicePlatform.Text = "" + compositeServiceData[0].RunTimePlatform;
                }
               
            }
        }

        public void ConnectionRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string bindingSchemeBase = "";
            string status = "";
            string color = "Black";
            string address = null;
            string view = null;
            string purge = null;
            string delete = null;
            string tofrom = null;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                index++;
                HostListenEndPointInstance item = ((HostListenEndPointInstance)e.Item.DataItem);
                currentHost = item.HostNameIdentifier;
                if (item.InUse)
                    color = "#034022";
                bool isOnline = false;
                try
                {
                    string addressOnline = onlineList[item.HostNameIdentifier];
                    isOnline = true;
                }
                catch
                {

                }
                if (viewType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
                {
                    tofrom = "<span style=\"font-size:1.2em;font-weight:normal;\">Connections To Host: ";
                    purge = "<br/><span style=\"font-size:1.0em;background-color:#84837C;border:#c0c0c0 2px solid;\"> <a class=\"Config1\" href=\"" + ConfigSettings.PAGE_PURGE_CONNECTIONS + postback + "&purgename=" + item.HostNameIdentifier + "&purgecfgSvc=" + item.ConfigServiceImplementationClassName + "&client=false\">&nbsp;Purge All Connections to This Service Host&nbsp;</a></span><br/>";
                    delete = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><a class=\"Config2\" href=\"" + ConfigSettings.PAGE_DELETE_CONNECTION + postback + "&connectionID=" + item.InstanceID + "&action=" + ConfigUtility.DELETE_CONNECTION + "&client=false"  + "\">Delete</a></td></tr>";
                }
                else
                {
                    tofrom = "<span style=\"font-size:1.2em;font-weight:normal;\">Connections From Client: ";
                    purge = "<br/><span style=\"font-size:13px;background-color:#84837C;border:#c0c0c0 2px solid;\"> <a class=\"Config1\" href=\"" + ConfigSettings.PAGE_PURGE_CONNECTIONS +postback + "&purgename=" + item.HostNameIdentifier + "&purgecfgSvc=" + item.ConfigServiceImplementationClassName + "&client=true\">&nbsp;Purge All Clients From This Service Host&nbsp;</a></span><br/>";
                    delete = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><a class=\"Config2\" href=\"" + ConfigSettings.PAGE_DELETE_CONNECTION + postback + "&connectionID=" + item.InstanceID + "&action=" + ConfigUtility.DELETE_CONNECTION + "&client=true"  + "\">Delete</a></td></tr>";
                }
                if (item.ServiceType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE && isOnline)
                {
                    view = "<br/><span style=\"font-size:13px;background-color:#84837C;border:#c0c0c0 2px solid;\"> <a class=\"Config1\" href=\"" + ConfigSettings.PAGE_CONNECTIONS + "?name=" + item.HostNameIdentifier + "&cfgSvc=" + item.ConfigServiceImplementationClassName + "&version=drilldown" + "\">&nbsp;View/Configure This Service Host's Connection Points&nbsp;</a></span><br/>";
                }
                else
                {
                    if (item.ServiceType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE && !isOnline)
                        view = "<br/>The Service is Offline. Bring the Service Online to View/Configure its Connection Points<br/>";
                    else
                        if (item.ServiceType == ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE)
                            view = "<br/>This service does not implement the Configuration Management Service<br/>";
                }
                if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                {
                    if (item.LoadBalanceType!=1)
                        status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "Single Node: " + item.Exception + "' src='Images/singleserverup.png'/></td>";
                    else
                        status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "NAT Node: " + item.Exception + "' src='Images/lbnodesup.png'/></td>";
                }
                else
                    if (item.Status.Equals(ConfigSettings.MESSAGE_OFFLINE))
                    {
                        if (item.LoadBalanceType != 1)
                            status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "Single Node: " + item.Exception + "' src='Images/singleserverdown.png'/></td>";
                        else
                            status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "NAT Node: " + item.Exception + "' src='Images/lbnodesdown.png'/></td>";
                    }
                    else
                        if (item.Status.Equals(ConfigSettings.MESSAGE_UNKNOWN))
                        {
                            if (item.LoadBalanceType != 1)
                                status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "Single Node: " + item.Exception + "' src='Images/singleserverunknown.png'/></td>";
                            else
                                status = "<td class=\"TradeServiceTDStyle\" style=\"text-align:center;\"><img title='" + "NAT Node: " + item.Exception + "' src='Images/lbnodesunknown.png'/></td>";
                        }
                if (viewType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
                {
                    ConnectedServices thisCSItem = connectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ConnectedServiceID == item.ConnectedServiceID; });
                    bindingSchemeBase = thisCSItem.BindingType;
                    if (bindingSchemeBase.Equals(ConfigUtility.NET_MSMQ_BINDING))
                    {
                        MSMQ.Text = @"Note that MSMQ nodes will show up as online as long as the MSMQ messaging service is running
                                          on that node, even if the service host at that node is not active. This is by design, since
                                          these 'loosely-coupled' nodes will actively receive messages from clients as long as the MSMQ message service
                                          is running (and process these messages when the service host is started again).
                                          <br /><br />
                                          If the message service is stopped on a node, it will show up as offline, and StockTrader will
                                          automatically failover to the other nodes. You will only be able to view connection
                                          points for a remote service, however, if the host process itself is running, regardless
                                          of the status of the MSMQ messaging service.<br />";
                        MSMQ.Visible = true;
                    }
                }
                else
                {
                    ConnectedConfigServices ccs = connectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == item.ConnectedConfigServiceID; });
                    if (ccs == null)
                        Response.Redirect(ConfigSettings.PAGE_NODES, true);
                    bindingSchemeBase = ccs.BindingType;
                }
                if (currentHost != newHost)
                {
                    address = "<br/><table class=\"ConnectionPointTableStyle\" style=\"background-image: url('Images/connpointfill.png');\" align=\"center\" width=\"1100\">" +

                        "<tr><th class=\"ConnPointTopConfigTHStyle\" style=\"font-size:16px;\" colspan=\"4\">" + tofrom + item.HostNameIdentifier + "</span></th></tr>" +
                        "<tr><th colspan=\"5\" class=\"ConnPointTopConfigTHStyle\" style=\"text-align:center;font-size:11px;border-top: 0px\">" + view + purge + "<br/></th></tr>" +

                        "<tr>" +
                        "<th class=\"TradeConfigTHStyle3\" style=\"font-size:1.1em\">Remote Address</th>" +
                        "<th class=\"TradeConfigTHStyle3\" style=\"font-size:1.1em\">Client Details</th>" +
                        "<th class=\"TradeConfigTHStyle3\" style=\"font-size:1.1em\">Service Status</th>" +
                        "<th class=\"TradeConfigTHStyle3\" style=\"font-size:1.1em\">Remove</th>" +
                        "</tr>" + "<tr><td class=\"TradeServiceTDStyle\"><span style=\"text-align:center;font-size:1.0em;color:" + color + "\">" + item.RemoteAddress + "</span></td>";
                    if (index > 0)
                        address = "</table>" + address;
                }
                else
                {
                    address = "<tr><td class=\"TradeServiceTDStyle\"><span style=\"text-align:center;font-size:1.0em;color:" + color + "\">" + item.RemoteAddress + "</span></td>";
                }
                if (index == connections.MyConnectionPoints.Count - 1)
                    delete = delete + "</table>";

                string clientConfiguration = item.Configuration;
                string securityMode;
                string bindingConfig;
                string epBehavior;
                string contract;
                getInfo(clientConfiguration, out bindingConfig, out securityMode, out epBehavior, out contract);
                if (epBehavior == null || epBehavior == "")
                    epBehavior = "None Assigned";
                ((Controls_WebUserControl)e.Item.FindControl("Address")).Text = address;
                ((Controls_WebUserControl)e.Item.FindControl("Configuration")).Text = "<td><table align=\"center\" width=\"600px\" style=\"border:#5E6066 1px solid;table-layout:fixed;font-size:.9em;border-collapse:collapse\"><col width=\"150\" /><col width=\"450\" />" +
  "<tr><td colspan=\"2\" class=\"TradeServiceTDStyle\" style=\"background-color:#84837C;text-align:center;font-size:1.3em;border:#5E6066 1px solid\"><span style=\"font-weight:normal;color:" + color + "\">" + item.ServiceFriendlyName + "</span></td></tr>" +
  "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Client Configuration:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + clientConfiguration + "</td></tr>" +
   "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Service Contract:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + contract + "</td></tr>" +
  "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Binding Configuration:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + bindingConfig + "</td></tr>" +
  "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Binding Type:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + bindingSchemeBase + "</td></tr>" +
  "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Security Mode:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + securityMode + "</td></tr>" +
  "<tr><td style=\"text-align:right;padding-right:5px;border:#5E6066 1px solid;\">Endpoint Behavior:</td>" +
  "<td style=\"text-align:left;padding-left:5px;border:#5E6066 1px solid;\">" + epBehavior + "</td></tr>" +
  "</table></td>";



                ((Controls_WebUserControl)e.Item.FindControl("Online")).Text = status;
                ((Controls_WebUserControl)e.Item.FindControl("Delete")).Text = delete;
                newHost = item.HostNameIdentifier;
            }
        }

        private void getInfo(string clientConfig, out string bindingConfig, out string securityMode, out string endpointconfig, out string contract)
        {
            ClientInformation client =  compositeServiceData[0].ClientInformation.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(clientConfig); });
            BindingInformation binding = compositeServiceData[0].BindingInformation.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(client.BindingConfiguration); });
            securityMode = binding.SecurityMode;
            bindingConfig = binding.BindingConfigurationName;
            endpointconfig = client.EndpointConfiguration;
            contract = client.Contract;
        }
    }
}