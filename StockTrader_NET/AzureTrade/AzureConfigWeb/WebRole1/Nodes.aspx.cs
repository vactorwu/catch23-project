//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//This is the 'Home Page' for ConfigWeb.  It displays on the left the current selected 'root'
//node, with top level menu items always operating against this root node.  The right hand
//side displays connected services n+1 levels deep, and these can be selected to become the 
//'root' node, and hence perform remote configuration actions given authentication credentials.
//Acts as if exploring a folder structure.
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
    public partial class Nodes : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string thePlatform;
        string theVersion;
        string theHoster;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        List<ServiceConfigurationData> compositeServiceData;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> thisHostServices;
        List<ServiceConfigurationData> thisHostServicesAndInProcessServices;
        int index = -1;
        string currentHost;
        string newhost;
        int remoteIndex = -1;
        int index2 = -1;
        string azureupurl = "<img src=\"Images/azureup.png\" title=\"Windows Azure Data Center: Application Online\" width='130' height='130'/>";
        string azuredownurl = "<img src=\"Images/azuredown.png\" title=\"Windows Azure Data Center: Connectivity Issue with Application\" width='130' height='130'/>";
        string azureunknownurl = "<img src=\"Images/azureunknown.png\" title=\"Windows Azure Data Center: Connectivity Timeout to Application\" width='130' height='130'/>";
        string rackupurl = "<img src=\"Images/rackup.png\" title=\"On-Premise Data Center: Application Online\" width='130' height='130'/>";
        string rackdownurl = "<img src=\"Images/rackdown.png\" title=\"On-Premise Data Center: Connectivity Issue with Application\" width='130' height='130'/>";
        string rackunknownurl = "<img src=\"Images/rackunknown.png\" title=\"On-Premise Data Center: Connectivity Timeout to Application\" width='130' height='130'/>";
        string desktopupurl = "<img src=\"Images/winup.png\" title=\"Desktop: Application Online\" width='130' height='130'/>";
        string desktopdownurl = "<img src=\"Images/windown.png\" title=\"Desktop: Connectivity Issue with Application\" width='130' height='130'/>";
        string desktopunknownurl = "<img src=\"Images/winunknown.png\" title=\"Desktop: Connectivity Timeout to Application\" width='130' height='130'/>";
          

        protected void Page_Load(object sender, EventArgs e)
        {
            string hostedID = null;
            Page.Form.DefaultFocus = SoaMapButton.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding,out hostNameIdentifier, out configName, out theVersion, out thePlatform, out theHoster, true);
            if (IsPostBack)
            {
                SessionInfo info = new SessionInfo();
                info.getSessionData(false, out address, out user, out binding, out hostNameIdentifier, out configName, out theHoster, out theVersion, out thePlatform, out hostedID);
            }
            InProcessRepeater.ItemDataBound += new RepeaterItemEventHandler(InProcessRepeater_ItemDataBound);
            CompositeServicesRepeater.ItemDataBound += new RepeaterItemEventHandler(CompositeServicesRepeater_ItemDataBound);
            List<ServiceConfigurationData> blankData = new List<ServiceConfigurationData>();
            ServiceConfigurationData blankItem = new ServiceConfigurationData();
            blankItem.ServiceHost = "-";
            blankItem.ServiceContract = "-";
            blankItem.Status = "-";
            blankData.Add(blankItem);
            int level;
            string levelString = (string)Request["level"];
            if (levelString == null)
                level = ConfigUtility.CONFIG_LEVEL_BASIC;
            else
            {
                level = Convert.ToInt32(levelString);
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            string action = Request["action"];
            if (action != "navigate")
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, level, true, traversePath, user);
                if (compositeServiceData != null)
                {
                    if (compositeServiceData.Count > 0 && compositeServiceData[0] != null)
                    {
                        string postback = "?name=" + compositeServiceData[0].ServiceHost + "&cfgSvc=" + compositeServiceData[0].ConfigServiceImplementationClassName + "&version=" + compositeServiceData[0].ServiceVersion + "&platform=" + compositeServiceData[0].RunTimePlatform + "&hoster=" + compositeServiceData[0].ServiceHoster;
                        Hosted.PostBackUrl = ConfigSettings.PAGE_VHOSTS + postback;
                        Connected.PostBackUrl = ConfigSettings.PAGE_CONNECTED_SERVICES + postback;
                        Connections.PostBackUrl = ConfigSettings.PAGE_CONNECTIONS + postback;
                        Logs.PostBackUrl = ConfigSettings.PAGE_AUDIT + postback;
                        Users.PostBackUrl = ConfigSettings.PAGE_USERS + postback;
                        InProcessRepeater.DataSource = compositeServiceData;
                        InProcessRepeater.DataBind();
                        TopNodeName.Text = hostNameIdentifier;
                        ServiceVersion.Text = theVersion;
                        ServiceHoster.Text = "" + theHoster;
                        ServicePlatform.Text = "" + thePlatform;
                        GetImageButton.runtimePoweredBy(thePlatform, RuntimePlatform);
                        if (compositeServiceData[0].ConnectedServiceConfigurationData != null && compositeServiceData[0].ConnectedServiceConfigurationData.Count > 0)
                        {
                            CompositeServicesRepeater.DataSource = compositeServiceData[0].ConnectedServiceConfigurationData;
                        }
                        else
                        {
                            CompositeServicesRepeater.DataSource = blankData;
                        }
                        CompositeServicesRepeater.DataBind();
                    }
                    else
                        Response.Redirect(ConfigSettings.PAGE_NODES,true);
                }
                else
                {
                    Response.Redirect(ConfigSettings.PAGE_LOGOUT,true);
                }
            }
        }


        public void InProcessRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string view = "";
            string status = "";
            string configure = "";
            string tdHeader = "";
            string tdHeader1 = "";
            string tdHeadertop = "";
            string image = "";
            
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                index++;
                ServiceConfigurationData item = ((ServiceConfigurationData)e.Item.DataItem);
                string noendpoints = "";
                string serviceContract = ChunkText.chunkDot(item.ServiceContract, '.');
                if (index > 0)
                {
                    //We will check for a special condition---running services in-process within another service host and using them as classes
                    //as opposed to actual remote services; in this case we need to check to see if these services are actually exposed in their
                    //master host program.  For example, in StockTrader, this would be the case for the Web App running Business Services 
                    //in-process based on AccessMode; and/or Business Services running the Order Processor in-process based on it's OrderMode setting.
                    if (compositeServiceData[0].ServiceHost != item.ServiceHost)
                    {
                        noendpoints = "<span style=\"color:Maroon\">* (in-process)</span>";
                    }
                }
                tdHeadertop = "<tr><td class=\"TradeServiceTDStyle\">";
                if (item.RunTimePlatform == null)
                    item.RunTimePlatform = "unknown";
                if (index == 0)
                {
                    tdHeader = "<td class=\"TradeServiceTDStyle\">";
                    tdHeader1 = "<td colspan=\"2\" class=\"TradeServiceTDStyle\">";
                    if (item.RunTimePlatform.ToLower().Contains("azure"))
                        status = "<td>" + azureupurl + "</td>";
                    else
                        if (item.RunTimePlatform.ToLower().Contains("server"))
                        status = "<td>" + rackupurl + "</td>";
                        else
                            status = "<td>" + desktopupurl + "</td>";
                    view = tdHeader + "<a class=\"toplinks\" href=\"" + ConfigSettings.PAGE_NODE_MAP + "?name=" + item.ServiceHost + "&cfgSvc=" + item.ConfigServiceImplementationClassName + "&displayName=" + item.ServiceHost + "&version=" + item.ServiceVersion + "&platform=" + item.RunTimePlatform + "&hoster=" + item.ServiceHoster + "\">View Nodes</a></td>";
                    configure = tdHeader + "<a class=\"toplinks\" href=\"" + ConfigSettings.PAGE_CONFIG + "?name=" + item.ServiceHost + "&cfgSvc=" + item.ConfigServiceImplementationClassName + "&contract=" + item.ServiceContract + "&version=" + item.ServiceVersion + "&platform=" + item.RunTimePlatform + "&hoster=" + item.ServiceHoster + "&level=1 \">Edit Inherited Settings</a></td></tr>";
                    configure = configure + "<tr><th class=\"TradeConfigTHStyle2\" style=\"border-left:#000000 1px solid;border-right:#000000 1px solid;\" colspan=\"4\">Setting Groups</th></tr>";
                    if (compositeServiceData.Count == 1)
                        configure = configure + "<tr><td class=\"TradeServiceTDStyle\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td></tr>";
                }
                else
                {
                    view = "";
                    tdHeader1 = "<td colspan=\"2\" class=\"TradeServiceTDStyle\">";
                    tdHeader = "<td class=\"TradeServiceTDStyle\">";
                    if (item.ServiceHost == "-")
                    {
                        status = tdHeader1 + "-</td>";
                        configure = tdHeader + "-</td></tr>";
                    }
                    else
                    {
                        image = "<img height=\"35px\" width=\"35px\" src='Images/cogs.gif' /></td>";
                        status = tdHeader1 + image;
                        string settingsName = "Edit Inherited Settings";
                        if (!serviceContract.StartsWith("Host."))
                            settingsName = "Edit Custom Settings";
                        configure = "<td class=\"TradeServiceTDStyle\"><a class=\"toplinks\" href=\"" + ConfigSettings.PAGE_CONFIG + "?name=" + item.ServiceHost + "&cfgSvc=" + item.ConfigServiceImplementationClassName + "&contract=" + item.ServiceContract + "&version=" + item.ServiceVersion + "&platform=" + item.RunTimePlatform + "&hoster=" + item.ServiceHoster + "&level=1 \">" + settingsName + "</a></td></tr>";
                    }
                }
                ((Controls_WebUserControl)e.Item.FindControl("CurrentName")).Text = tdHeadertop + item.ServiceHost + " : " + serviceContract + noendpoints + "</td>";
                ((Controls_WebUserControl)e.Item.FindControl("Status1")).Text = status;
                ((Controls_WebUserControl)e.Item.FindControl("Deployment1")).Text = view;
                ((Controls_WebUserControl)e.Item.FindControl("Configure1")).Text = configure;
            }
        }

        public void CompositeServicesRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string view = "";
            string status = "";
            string configure = "";
            string tdHeader = "";
            string tdHeader2 = "";
            string tdHeader3 = "";
            string image = "";
            string noendpoints = null;
            string remoteHoster = "";
            string remoteVersion = "";
            string remotePlatform = "";
          
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                remoteIndex++;
                index2++;
                ServiceConfigurationData item = ((ServiceConfigurationData)e.Item.DataItem);
                if (item.InProcessHost != null)
                    currentHost = item.InProcessHost;
                else
                    currentHost = item.ServiceHost;
                if (currentHost != newhost)
                    index2 = 0;
                
                if (item.ServiceHost != "-")
                {
                    if (index2 == 0)
                    {
                        thisHostServices = compositeServiceData[0].ConnectedServiceConfigurationData.FindAll(delegate(ServiceConfigurationData sc) { return (sc.ServiceHost.Equals(compositeServiceData[0].ConnectedServiceConfigurationData[remoteIndex].ServiceHost)); });
                        thisHostServicesAndInProcessServices = compositeServiceData[0].ConnectedServiceConfigurationData.FindAll(delegate(ServiceConfigurationData sc) { return (sc.ServiceHost.Equals(compositeServiceData[0].ConnectedServiceConfigurationData[remoteIndex].ServiceHost) || (sc.InProcessHost != null && sc.InProcessHost.Equals(compositeServiceData[0].ConnectedServiceConfigurationData[remoteIndex].ServiceHost))); });
                    }
                    noendpoints = "";
                    if (index2 > 0 && compositeServiceData[0].ConnectedServiceConfigurationData.Count > 0)
                    {
                        //We will check for a special condition---running services in-process within another service host and using them as classes
                        //as opposed to actual remote services; in this case we need to check to see if these services are actually exposed in their
                        //master host program.  This would be the case for the Web App running Business Services in-process based on AccessMode; and/or 
                        //Business Services running the Order Processor in-process based on it's OrderMode setting.
                        ServiceConfigurationData activelyHosted = thisHostServices.Find(delegate(ServiceConfigurationData sc) { return (sc.ServiceContract.Equals(item.ServiceContract) && sc.ServiceHost.Equals(item.ServiceHost)); });
                        if (activelyHosted == null)
                        {
                            noendpoints = "<span style=\"color:Maroon\">*</span>";
                        }
                    }
                
                }
                if (item.Status != ConfigSettings.MESSAGE_ONLINE)
                {
                    remoteHoster = "Uknown";
                    remoteVersion = "Uknown";
                    remotePlatform = "Unknown";
                    
                }
                else
                {
                    remoteHoster = item.ServiceHoster;
                    remotePlatform = item.RunTimePlatform;
                    remoteVersion = item.ServiceVersion;
                }
                string serviceContract = ChunkText.chunkDot(item.ServiceContract, '.');
                if (item.RunTimePlatform == null)
                    item.RunTimePlatform = "unknown";
                if (index2 == 0)
                {
                    if (item.ServiceHost != "-")
                    {
                        tdHeader = "<td class=\"TradeServiceTDStyle\">";
                        tdHeader2 = "<td class=\"TradeServiceTDStyle\">";

                        if (compositeServiceData[0].ConnectedServiceConfigurationData[remoteIndex].Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                        {
                            if (item.RunTimePlatform.ToLower().Contains("azure"))
                                image = azureupurl + "</td>";
                            else
                                if (item.RunTimePlatform.ToLower().Contains("server") || item.RunTimePlatform.ToLower().Contains("unknown"))
                                    image = rackupurl + "</td>";
                                else
                                    image = desktopupurl + "</td>";
                        }
                        else
                            if (compositeServiceData[0].ConnectedServiceConfigurationData[remoteIndex].Status.Equals(ConfigSettings.MESSAGE_UNKNOWN))
                            {
                                if (item.RunTimePlatform.ToLower().Contains("azure"))
                                    image = azureunknownurl + "</td>";
                                else
                                    if (item.RunTimePlatform.ToLower().Contains("server") || item.RunTimePlatform.ToLower().Contains("unknown"))
                                        image = rackunknownurl + "</td>";
                                    else
                                        image = desktopunknownurl + "</td>";
                            }
                            else
                            {
                                if (item.RunTimePlatform.ToLower().Contains("azure"))
                                    image = azuredownurl + "</td>";
                                else
                                    if (item.RunTimePlatform.ToLower().Contains("server") || item.RunTimePlatform.ToLower().Contains("unknown"))
                                        image = rackdownurl + "</td>";
                                    else
                                        image = desktopdownurl + "</td>";
                            }
                    }
                    else
                    {
                        tdHeader = "<td class=\"TradeServiceTDStyle3\">";
                        tdHeader2 = "<td class=\"TradeServiceTDStyle3\">";
                    }
                }
                else
                {
                    tdHeader = "<td class=\"TradeServiceTDStyle3\">";
                    tdHeader2 = "<td class=\"TradeServiceTDStyle3\">";
                    tdHeader3 = "<td colspan = \"3\" class=\"TradeServiceTDStyle3\">";
                    if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                        image = tdHeader3 + "<img height=\"35px\" width=\"35px\" src='Images/cogs.gif' /></td></tr>";
                    else
                        image = tdHeader3 + "<img height=\"35px\" width=\"35px\" src='Images/cogs2.gif' /></td></tr>";
                }
                string nextHostHeader = "";
                if (index2 == 0 && remoteIndex > 0)
                {
                    nextHostHeader = "<tr><th colspan=\"4\" class=\"TradeConfigTHStyle2\" style=\"background-color:#616161;border-bottom:#c1c1c1 2px solid;border-top:#c1c1c1 2px solid;border-right:1px #c1c1c1 solid\">&nbsp;</th></tr>" +
                                                "<tr><th class=\"TradeConfigTHStyle2\" style=\"border-right:1px #c1c1c1 solid\">Name</th>" +
                                                "<th class=\"TradeConfigTHStyle2\" style=\"border-right:1px #c1c1c1 solid\">Hosted Service Domain</th>" +
                                                "<th class=\"TradeConfigTHStyle2\" style=\"border-right:1px #c1c1c1 solid\">Nodes</th>" +
                                                "<th class=\"TradeConfigTHStyle2\" style=\"border-right:1px #c1c1c1  solid\">Select</th></tr>";
                }
                ((Controls_WebUserControl)e.Item.FindControl("RemoteServiceName")).Text = nextHostHeader + "<tr>" + tdHeader2 + item.ServiceHost + " : " + serviceContract + noendpoints + "</td>";

                if (item.ServiceHost == "-")
                {
                    view = tdHeader + "-</td>";
                    status = tdHeader + "-</td>";
                    configure = tdHeader + "-</td>";
                    image = tdHeader + "-</td>";
                }
                else
                {
                    if (index2 == 0)
                    {
                        if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                            view = tdHeader + "<a class=\"toplinks\" href=\"" + ConfigSettings.PAGE_NODE_MAP + "?name=" + item.ServiceHost + "&cfgSvc=" + item.ConfigServiceImplementationClassName + "&version=" + remoteVersion + "&platform=" + remotePlatform + "&hoster=" + remoteHoster + "\">View Nodes</a></td>";
                        else
                            view = tdHeader + "Offline";
                    }
                    if (item.Status.Equals(ConfigSettings.MESSAGE_ONLINE))
                    {
                        if (index2 > 0)
                        {
                            status = image;
                            tdHeader = tdHeader3;
                        }
                        else
                        {
                            string linkStr = "";
                            status = tdHeader + image;
                            string[] allowedList = compositeServiceData[0].AllowedRemoteList.Split(new char[] { ';' });
                            bool okToTraverse = false;
                            for (int i = 0; i < allowedList.Length; i++)
                            {
                                string checkName = allowedList[i].Trim().ToLower();
                                if (item.ServiceHost.Trim().ToLower().Equals(checkName))
                                    okToTraverse = true;
                            }
                            if (!okToTraverse)
                                linkStr ="Not Selectable";
                            else
                                linkStr = "<a class=\"toplinks\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + item.ServiceHost + "&cfgSvc=" + item.ConfigServiceImplementationClassName +  "&version=" + remoteVersion + "&platform=" + remotePlatform + "&hoster=" + remoteHoster + "\">Select</a>";
                            configure = tdHeader + linkStr;
                        }
                    }
                    else
                    {
                        if (index2 > 0)
                        {
                            status = image;
                            tdHeader = tdHeader3;
                        }
                        else
                        {
                            status = tdHeader + image;
                            configure = tdHeader + "Offline";
                        }
                    }
                }
                if (index2 == 0)
                {
                    configure = configure + "</td></tr><tr><th class=\"TradeConfigTHStyle2\" style=\"border-left:#000000 1px solid;border-right:#000000 1px solid;\" colspan=\"4\">Setting Groups</th></tr>";
                    if (thisHostServices == null || thisHostServices.Count == 1)
                        configure = configure + "</td></tr><tr><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td><td class=\"TradeServiceTDStyle3\" style=\"text-align:center\">-</td></tr>";
                }
                ((Controls_WebUserControl)e.Item.FindControl("Deployment2")).Text = view;
                ((Controls_WebUserControl)e.Item.FindControl("Status2")).Text = status;
                ((Controls_WebUserControl)e.Item.FindControl("Configure2")).Text = configure;
                newhost = currentHost;
            }
        }

        protected void SoaMapButton_Click(object sender, ImageClickEventArgs e)
        {
            bool iswp7 = false;
            bool isSafari=false;
            for (int i = 0; i < Request.Headers.Count; i++)
            {
                //This header will be in win phone 7 requests.  If win phone 7, instead of popup window, which does not work, just display as normal page.
                if (Request.Headers[i].ToString().Contains("XBLWP7"))
                    iswp7 = true;

                //Similar to win phone 7, based on way safari handles popups, it does not like SOAMap as a new window, so display as std page.
                if (Request.Headers[i].ToString().ToLower().Contains("safari"))
                    isSafari = true;
            }
            if (isSafari)
                Response.Redirect(ConfigSettings.PAGE_SOA_MAP + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + compositeServiceData[0].ServiceVersion + "&platform=" + compositeServiceData[0].RunTimePlatform + "&hoster=" + compositeServiceData[0].ServiceHoster, true);
            if (!iswp7)
                ALittleJS.Text = "<script language='javascript'>window.open('" + ConfigSettings.PAGE_SOA_MAP + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + compositeServiceData[0].ServiceVersion + "&platform=" + compositeServiceData[0].RunTimePlatform + "&hoster=" + compositeServiceData[0].ServiceHoster + "');</script>";
            else
                ALittleJS.Text = "<script language='javascript'>window.navigate('" + ConfigSettings.PAGE_SOA_MAP + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + compositeServiceData[0].ServiceVersion + "&platform=" + compositeServiceData[0].RunTimePlatform + "&hoster=" + compositeServiceData[0].ServiceHoster + "');</script>";
        }

        protected void Users_Click(object sender, EventArgs e)
        {
            Response.Redirect("Users.aspx" + "?name=" + compositeServiceData[0].ServiceHost + "&cfgSvc=" + compositeServiceData[0].ConfigServiceImplementationClassName + "&version=" + compositeServiceData[0].ServiceVersion + "&platform=" + compositeServiceData[0].RunTimePlatform + "&hoster=" + compositeServiceData[0].ServiceHoster,true);
        }
    }
}