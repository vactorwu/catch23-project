//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Displays the active nodes running for a given Virtual Host; and their Primary Service Endpoints.
//============================================================================================
//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class NodeMap : System.Web.UI.Page
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
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        List<TraverseNode> traversePath;
        float totalReqs = 0f;
        float totalReqsPerDay = 0f;
        string singleUp = "Images/singleserverup.png";
        string singleDown = "Images/singleserverdown.png";
        string singleUnknown = "Images/singleserverunknown.png";
        string natUp = "Images/lbnodesup.png";
        string natDown = "Images/lbnodesdown.png";
        string natUnknown = "Images/lbnodesunknown.png";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                SessionInfo info = new SessionInfo();
                if (!info.getSessionData(IsPostBack, out address, out user, out binding, out hostNameIdentifier, out configName, out hoster, out version, out platform, out hostedID))
                    home(false);
                hostNameIdentifier = (string)ViewState["name"];
                configName = (string)ViewState["configname"];
                version = (string)ViewState["version"];
                platform = (string)ViewState["platform"];
                hoster = (string)ViewState["hoster"];
            }
            else
            {
                Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
                ViewState["name"] = hostNameIdentifier;
                ViewState["configname"] = configName;
                ViewState["version"] = version;
                ViewState["platform"] = platform;
                ViewState["hoster"] = hoster;
            }
            LabelReq.Text = "Measured Page Requests";
            LabelReqDay.Text = "Measured Page Requests Per Day";
            UTC.Text = DateTime.Now.ToUniversalTime().ToString("f") + " (UTC)";
            NodeRepeater.ItemDataBound += new RepeaterItemEventHandler(NodeRepeater_ItemDataBound);
            List<ServiceNode> myNodeMap = null;
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            if (traversePath != null && traversePath.Count > 0)
            {
                if (traversePath[traversePath.Count - 1].MyNode.Status == ConfigSettings.MESSAGE_OFFLINE)
                {
                    myNodeMap = new List<ServiceNode>();
                    myNodeMap.Add(traversePath[traversePath.Count - 1].MyNode);
                    if (traversePath[traversePath.Count - 1].PeerNodes != null)
                        myNodeMap.AddRange(traversePath[traversePath.Count - 1].PeerNodes);
                }
            }
            VirtualHost myVhost=null;
            if (myNodeMap == null)
            {
                myVhost = configProxy.getServiceNodeMap(hostNameIdentifier, configName, traversePath, user);
                if (myVhost == null)
                    return;
                myNodeMap = myVhost.ServiceNodes;
            }
            VNODETotalReqs.Text = " (" + myVhost.TotalRequests.ToString() + ")";
            VNODETotalReqsPerDay.Text = " (" + myVhost.RequestsPerDay.ToString() + ")";
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            NodeRepeater.DataSource = myNodeMap;
            NodeRepeater.DataBind();
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        public void NodeRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string status = "";
            string address = "";
            string activeSince = "";
            string totalRequests = "*";
            string requestsPerDay = "*";
            string image = "";
            bool nat = false;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ServiceNode item = ((ServiceNode)e.Item.DataItem);
                if (item.ConfigServiceListenEndpoints != null && item.ConfigServiceListenEndpoints.Count > 0)
                {
                    foreach (HostListenEndPointInstance ep in item.ConfigServiceListenEndpoints)
                    {
                        if (ep.LoadBalanceType.Equals(1))
                        {
                            nat = true;
                            break;
                        }
                    }
                    if (!nat)
                        foreach (HostListenEndPointInstance ep in item.PrimaryListenEndpoints)
                        {
                            if (ep.LoadBalanceType.Equals(1))
                            {
                                nat = true;
                                break;
                            }
                        }
                }
                if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                {
                    if (nat)
                        image = natUp;
                    else
                        image = singleUp;
                    status = "<img title=\"" + item.Exception + "\" src=\"" + image+ "\"/>";
                    if (item.EndPointData != null && item.EndPointData.Count > 0 && item.EndPointData[0] != null)
                    {
                        totalReqs = totalReqs + item.EndPointData[0].TotalRequests;
                        totalReqsPerDay = totalReqsPerDay + item.EndPointData[0].RequestsPerDay;
                        totalRequests = item.EndPointData[0].TotalRequests.ToString();
                        requestsPerDay = item.EndPointData[0].RequestsPerDay.ToString();
                    }
                }
                else
                    if (item.Status.Equals(ConfigSettings.MESSAGE_OFFLINE))
                    {
                        if (nat)
                            image = natDown;
                        else
                            image = singleDown;
                        if (item.ConfigServiceListenEndpoints != null && item.ConfigServiceListenEndpoints.Count > 0)
                            status = "<img title=\"" + item.Exception + "\" src=\"" + image + "\"/><br/><a href='RemoveNode.aspx?nodeid=" + item.ConfigServiceListenEndpoints[0].RemoteInstanceID + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + ServiceVersion.Text + "&hoster=" + ServiceHoster.Text + "&platform=" + ServicePlatform.Text + "'>Remove Node From Cluster</a>";
                        else
                            status = "<img title=\"" + item.Exception + "\" src=\"" + image + "\"/>";
                        totalRequests = "*";
                        requestsPerDay = "*";
                    }
                    else
                        if (item.Status.Equals(ConfigSettings.MESSAGE_UNKNOWN) || item.Status.Equals(ConfigSettings.MESSAGE_SOME_NODES_DOWN))
                        {
                            if (nat)
                                image = natUnknown;
                            else
                                image = singleUnknown;
                            if (item.ConfigServiceListenEndpoints != null && item.ConfigServiceListenEndpoints.Count > 0)
                                status = "<img title=\"" + item.Exception + "\" src=\"" + image + "\"/><br/><a href='RemoveNode.aspx?nodeid=" + item.ConfigServiceListenEndpoints[0].RemoteInstanceID + "&name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + ServiceVersion.Text + "&hoster=" + ServiceHoster.Text + "&platform=" + ServicePlatform.Text + "'>Remove Node From Cluster</a>";
                            else
                                status = "<img title=\"" + item.Exception + "\" src=\"" + image + "\" />";
                            totalRequests = "*";
                            requestsPerDay = "*";
                        }
                if (item.ActiveSince == DateTime.MinValue)
                    activeSince = "Not Online";
                else
                    activeSince = item.ActiveSince.ToString("f");
                string[] splitstring = item.Address.Split(new char[] { '/' });
                address = splitstring[2];
                splitstring = address.Split(new char[] { ':' });
                address = splitstring[0];
                ((Label)e.Item.FindControl("Status")).Text = status;
                ((Label)e.Item.FindControl("ActiveSince")).Text = activeSince;
                ((Label)e.Item.FindControl("Address")).Text = address;
                if (item.PrimaryListenEndpoints.Count == 0)
                {
                    ((Label)e.Item.FindControl("LabelTotalRequests")).Text = totalRequests;
                    ((Label)e.Item.FindControl("LabelRequestsPerDay")).Text = requestsPerDay;
                }
                else
                {
                    LabelReq.Text = "Primary Business Service Requests";
                    LabelReqDay.Text = "Primary Business Service Requests Per Day";
                    ((Label)e.Item.FindControl("LabelTotalRequests")).Text = totalRequests;
                    ((Label)e.Item.FindControl("LabelRequestsPerDay")).Text = requestsPerDay;
                }
                Repeater endpoints = ((Repeater)e.Item.FindControl("EndPointRepeater"));
                endpoints.ItemDataBound += new RepeaterItemEventHandler(EndPointRepeater_ItemDataBound);
                endpoints.DataSource = item.PrimaryListenEndpoints;
                endpoints.DataBind();
            }
        }

        public void EndPointRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string address = null;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                HostListenEndPointInstance item = ((HostListenEndPointInstance)e.Item.DataItem);
                string NAT = "";
                if (item.LoadBalanceType.Equals(1))
                    NAT = "   [" + item.LoadBalanceAddress + "]";
                if (item.Status == null)
                    item.Status = ConfigSettings.MESSAGE_UNKNOWN;
                if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                    address = "<span style=\"color:palegreen;\">" + item.RemoteAddress + "<span style=\"color:darkgreen;\">  " + NAT + "</span>";
                else
                    if (item.Status.Equals(ConfigSettings.MESSAGE_OFFLINE))
                        address = "<span style=\"color:Maroon;\">" + item.RemoteAddress + "<span style=\"color:#38010A;\">  " + NAT + "</span>";
                    else
                        address = "<span style=\"color:#45474A;\">" + item.RemoteAddress + "<span style=\"color:black;\"> " + NAT + "</span>"; 
                ((Label)e.Item.FindControl("Address")).Text = address;
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

