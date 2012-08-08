//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to retrieve and display the Connected SOA Map for a root service, showing Virtual
//Host to Virtual Host connections, and node information down to endpoint level.  The ASP.NET
//Treeview is used for display, with recursive logic to traverse and build the nodes as 
//returned from the Configuration Service call getSOANodeMap.
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
using System.Diagnostics;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{

    public partial class SOAMap : System.Web.UI.Page
    {
        string hostNameIdentifier;
        string configName;
        string address;
        string userid;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        SOA mySOA;
        List<string> imageList;
        public static string metaRefresh;
        
            
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = ButtonClose.ClientID;
            imageList = (List<string>)ViewState["ImagesList"];
            if (CheckBoxNoDetail.Checked)
                Legend.Visible = false;
            else
                Legend.Visible = true;
            if (imageList == null)
            {
                //Lots of icons for lots of displayed info :-).  Its important to get the order just right here,
                //as the Windows Forms ServiceHostConsoleBase (BSL and OPS use) base class uses same images in a Windows App
                //with the .NET Windows Forms Treeview Control to display a Windows-Forms implementation of this same page.  Here
                //we will use the ASP.NET Treeview.  Amazingly, everything Config Services does is based on a network tree
                //view of a composite app.  So, everything here will simply load into one simple ASP.NET control and show an
                //entire live view of the deployment topology, across cloud, on-premise and hybrid environments.  Thanks to Azure,
                //now all viewable on a Windows Phone 7, Apple IPhone, or Google Android device from anywhere.  Now back to
                //adding icons for the page.
                imageList = new List<string>();
                imageList.Add("~/Images/SOA_up.png");  //0
                imageList.Add("~/Images/SOA_down.png"); //1
                imageList.Add("~/Images/SOA_yellow.png"); //2
                imageList.Add("~/Images/SOA_recursed.png"); //3 this bears special mention.  Config Service needs to track recursed nodes, to prevent 
                                                             //race conditions.  Imagine a linked list whose last node references the first node.  This icon
                                                             //shows when a service has a circular reference to another remote service in the network tree.  This is
                                                             //perfectly legal, of course, but we do  not want to try to recurse such node since it will be
                                                             //an endless loop/race condition.  The user will still see all the info on the first reference in the Treeview.  The second 
                                                             //will display this icon, since ConfigService has detected this and not recursed the node again.
                imageList.Add("~/Images/vhostup.png"); //4
                imageList.Add("~/Images/azure_vhostup.png"); //5
                imageList.Add("~/Images/hyperv_vhostup.png"); //6
                imageList.Add("~/Images/vhostdown.png"); //7
                imageList.Add("~/Images/azure_vhostdown.png"); //8
                imageList.Add("~/Images/hyperv_vhostdown.png"); //9
                imageList.Add("~/Images/vhostyellow.png"); //10
                imageList.Add("~/Images/azure_vhostyellow.png"); //11
                imageList.Add("~/Images/hyperv_vhostyellow.png"); //12
                imageList.Add("~/Images/serverup.png"); //13
                imageList.Add("~/Images/azure_serverup.png"); //14
                imageList.Add("~/Images/hypervup.png"); //15
                imageList.Add("~/Images/serverdown.png"); //16
                imageList.Add("~/Images/azure_serverdown.png"); //17
                imageList.Add("~/Images/hypervdown.png"); //18
                imageList.Add("~/Images/servergeneric.png"); //19
                imageList.Add("~/Images/azureservergeneric.png"); //20
                imageList.Add("~/Images/hypervgeneric.png"); //21
                imageList.Add("~/Images/config.png"); //22
                imageList.Add("~/Images/primary.png"); //23
                imageList.Add("~/Images/cache.png"); //24
                imageList.Add("~/Images/endpointup.bmp"); //25
                imageList.Add("~/Images/endpointdown.bmp"); //26
                imageList.Add("~/Images/endpointunknown.bmp"); //27
                imageList.Add("~/Images/sql_server_up.png"); //28
                imageList.Add("~/Images/sql_azure_up.png"); //29
                imageList.Add("~/Images/sql_server_down.png"); //30
                imageList.Add("~/Images/sql_azure_down.png"); //31
                imageList.Add("~/Images/sql_server_yellow.png"); //32
                imageList.Add("~/Images/sql_azure_yellow.png"); //33
                imageList.Add("~/Images/sql_server_db_up.png"); //34
                imageList.Add("~/Images/sql_azure_db_up.png"); //35
                imageList.Add("~/Images/sql_server_db_down.png"); //36
                imageList.Add("~/Images/sql_azure_db_down.png"); //37
                imageList.Add("~/Images/dbserverroot.png"); //38
                imageList.Add("~/Images/stats.png"); //39
                imageList.Add("~/Images/SQL.png"); //40
                imageList.Add("~/Images/sql2.png"); //41
                imageList.Add("~/Images/clock.png"); //42
                imageList.Add("~/Images/wincacheup.png"); //43
                imageList.Add("~/Images/azurecacheup.png"); //44
                imageList.Add("~/Images/wincachedown.png"); //45
                imageList.Add("~/Images/azurecachedown.png"); //46
                imageList.Add("~/Images/wincacheyellow.png"); //47
                imageList.Add("~/Images/azurecacheyellow.png"); //48
                imageList.Add("~/Images/cachenode.png"); //49
                imageList.Add("~/Images/serveryellow.png"); //50
                imageList.Add("~/Images/azure_serveryellow.png"); //51
                imageList.Add("~/Images/hyperv_serveryellow.png"); //52
                imageList.Add("~/Images/cachenodeyellow.png"); //53
                imageList.Add("~/Images/dbserverrootyellow.png"); //54
                ViewState["ImageList"] = imageList;
            }
            if (IsPostBack)
            {
                if (!AutoRefresh.Checked)
                    TimerJS.Text = null;
                else
                    getData();
            }
            else 
                getData();
        }

        private void getData()
        {
            string version = null;
            string platform = null;
            string hoster = null;
            string binding = null;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            UTC.Text = "<span style=\"color:silver;font-size:1.1em;padding-left:20px;\">" + DateTime.Now.ToUniversalTime().ToString("f") + " (UTC)";
            try
            {
                configProxy = new ServiceConfigurationClient(binding, address, user);
                mySOA = configProxy.getSOAMap(hostNameIdentifier, configName, null, user);  //this one call gets the entire network deployment across the world, complete with perf stats.
                soaTreeView.Nodes.Clear();
                if (mySOA != null && mySOA.MyVirtualHost != null)
                {
                    fillMap(soaTreeView);  //now display it.
                }
            }
            catch (Exception eSoa)
                {
                    InValid.Text = "Error Getting SOA Map (try again).  Exception is: <br/>" + eSoa.ToString();
                }
        }

        private void fillMap(TreeView soaTreeView)
        {
            string rootName = "<span><Font color='#84BDEC'>" + mySOA.SOAName + "</font></span>";
            TreeNode root = new TreeNode(rootName, "0",imageList[0]);
            root.ToolTip = "Service Domain";
            root.Expand();
            int imageIndex = 0;
            int offset=0;
            string prefix="";
            
            if (mySOA.MyVirtualHost.ServiceNodes != null && mySOA.MyVirtualHost.ServiceNodes.Count > 0 && mySOA.MyVirtualHost.ServiceNodes[0] != null)
            {
                if (mySOA.MyVirtualHost.ServiceNodes[0].RuntimePlatform != null && mySOA.MyVirtualHost.ServiceNodes[0].RuntimePlatform.ToLower().Contains("azure"))
                {
                    offset = 1;
                    prefix = "Azure Platform Cloud Deployed ";
                }
                else
                    if (mySOA.MyVirtualHost.ServiceNodes[0].RuntimePlatform != null && mySOA.MyVirtualHost.ServiceNodes[0].RuntimePlatform.ToLower().Contains("hyper"))
                    {
                        offset = 2;
                        prefix = "On-premise Deployed (Hyper-V) ";
                    }
                    else
                    {
                        offset = 0;
                        prefix = "On-premise Deployed ";
                    }
            }
            else
                offset = 0;
            if (mySOA.MyVirtualHost.Status == ConfigSettings.MESSAGE_ONLINE)
                imageIndex =4 + offset;
            else
                if (mySOA.MyVirtualHost.Status == ConfigSettings.MESSAGE_OFFLINE)
                    imageIndex = 7+offset;
                else
                    if (mySOA.MyVirtualHost.Status == ConfigSettings.MESSAGE_SOME_NODES_DOWN)
                        imageIndex = 10+offset;
            TreeNode myVHost = new TreeNode(prefix + mySOA.MyVirtualHost.VHostName, imageIndex.ToString(), imageList[imageIndex]);
            myVHost.Expand();
            soaTreeView.ShowLines = true;
            soaTreeView.ShowExpandCollapse = true;
            TreeNode[] clusterNodes = new TreeNode[mySOA.MyVirtualHost.ServiceNodes.Count];
            string strASPNET;
            string strWCF;
            string strCPU;
            string deployment="";
            string display="";
            for (int i = 0; i < mySOA.MyVirtualHost.ServiceNodes.Count; i++)
            {
                TreeNode primaryEPs = new TreeNode("", "23", imageList[23]);
                primaryEPs.ToolTip = "Primary Endpoints are those defined by the business application developer to service business requests via their custom business logic.";
                primaryEPs.Collapse();
                TreeNode configEPs = new TreeNode("", "22", imageList[22]);
                configEPs.ToolTip = "Configuration Service Endpoints are infrastructure endpoints available for the Configuration Service itself, for example an endpoint to which ConfigWeb connects.";
                configEPs.Collapse();
               // TreeNode dcEPs = new TreeNode("", "24", imageList[24]);
               // dcEPs.Collapse();
                TreeNode[] endPointNodesPrimary = new TreeNode[mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints.Count];
                TreeNode[] endPointNodesConfig = new TreeNode[mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints.Count];
                //TreeNode[] endPointNodesDC = new TreeNode[mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints.Count];
                for (int j = 0; j < mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints.Count; j++)
                {
                    int endPointImageIndex = 0;
                    string NAT = "";
                    string NAT2 = "";
                    switch (mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].Status)
                    {
                        case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                        case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                        case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex =27; break;}
                    }
                    TreeNode endpointnode = null;
                    if (mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].LoadBalanceType.Equals(1))
                    {
                        NAT = "<span style=\"color:#FFFFFF\"> --> {" + mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].LoadBalanceAddress + "}</span>";
                        NAT2 = "<span style=\"color:#FFFFFF\"> --> {NAT Load-Balanced}</span>";
                    }
                    if (CheckBoxEndpointDetail.Checked)
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].RemoteAddress + "</span>" + NAT, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    else
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].ServiceFriendlyName + "</span>" + NAT2, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    endpointnode.ToolTip = mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints[j].Exception;
                    endPointNodesPrimary[j] = endpointnode;
                }

                for (int j = 0; j < mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints.Count; j++)
                {
                    string NAT = "";
                    string NAT2 = "";
                    int endPointImageIndex = 0;
                    switch (mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].Status)
                    {
                        case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                        case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                        case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex = 27; break; }
                    }
                    TreeNode endpointnode = null;
                    if (mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].LoadBalanceType.Equals(1))
                    {
                        NAT = "<span style=\"color:#FFFFFF\"> --> {" + mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].LoadBalanceAddress + "}</span>";
                        NAT2 = "<span style=\"color:#FFFFFF\"> --> {NAT Load-Balanced}</span>";
                    }
                    if (CheckBoxEndpointDetail.Checked)
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].RemoteAddress + "</span>" + NAT, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    else
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].ServiceFriendlyName + "</span>" + NAT2, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    endpointnode.ToolTip = mySOA.MyVirtualHost.ServiceNodes[i].ConfigServiceListenEndpoints[j].Exception;
                    endPointNodesConfig[j] = endpointnode;
                }
                /*
                for (int j = 0; j < mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints.Count; j++)
                {
                    int endPointImageIndex = 0;
                    switch (mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints[j].Status)
                    {
                        case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                        case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                        case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex = 27; break; }
                    }
                    TreeNode endpointnode = null;
                    if (CheckBoxEndpointDetail.Checked)
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints[j].RemoteAddress + "</span>", endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    else
                    {
                        endpointnode = new TreeNode("<span style=\"color:#848483;\">" + mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints[j].ServiceFriendlyName + "</span>", endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                    }
                    endpointnode.ToolTip = mySOA.MyVirtualHost.ServiceNodes[i].DCServiceListenEndpoints[j].Exception;
                    endPointNodesDC[j] = endpointnode;
                }
                for (int k = 0; k < endPointNodesDC.Length; k++)
                {
                    dcEPs.ChildNodes.Add(endPointNodesDC[k]);
                }
                 * */
                for (int k = 0; k < endPointNodesConfig.Length; k++)
                {
                    configEPs.ChildNodes.Add(endPointNodesConfig[k]);
                }
                for (int k = 0; k < endPointNodesPrimary.Length; k++)
                {
                    primaryEPs.ChildNodes.Add(endPointNodesPrimary[k]);
                }
                int nodeimageIndex = 0;
                offset = 0;
                if (mySOA.MyVirtualHost.ServiceNodes[i].RuntimePlatform!=null && mySOA.MyVirtualHost.ServiceNodes[i].RuntimePlatform.ToLower().Contains("azure"))
                {
                    offset = 1;
                    deployment = "Windows Azure";
                }
                else
                    if (mySOA.MyVirtualHost.ServiceNodes[i].RuntimePlatform != null && mySOA.MyVirtualHost.ServiceNodes[i].RuntimePlatform.ToLower().Contains("hyper"))
                    {
                        offset = 2;
                        deployment = "On-Premise/Private Cloud (Hyper-V)";
                    }
                    else
                    {
                        offset = 0;
                        deployment = "On-Premise/Private Cloud";
                    }
                switch (mySOA.MyVirtualHost.ServiceNodes[i].Status)
                {
                    case ConfigSettings.MESSAGE_ONLINE: { nodeimageIndex = 13+offset; break; }
                    case ConfigSettings.MESSAGE_OFFLINE: { nodeimageIndex = 16+offset; break; }
                    case ConfigSettings.MESSAGE_UNKNOWN: { nodeimageIndex = 19+offset; break; }
                    case ConfigSettings.MESSAGE_SOME_NODES_DOWN: { nodeimageIndex = 50 + offset; break; }
                }
                TreeNode node = null;
                string vhostname = null;
                string azureRoleInstanceID = "";
                if (mySOA.MyVirtualHost.ServiceNodes[i].AzureRoleInstanceID != null && mySOA.MyVirtualHost.ServiceNodes[i].AzureRoleInstanceID != "")
                    azureRoleInstanceID = "<span style=\"font-size:.85em;\">&nbsp;&nbsp;&nbsp;{" + mySOA.MyVirtualHost.ServiceNodes[i].AzureRoleInstanceID + "}</span>";
                if (CheckBoxEndpointDetail.Checked)
                {
                    vhostname = mySOA.MyVirtualHost.ServiceNodes[i].Address + azureRoleInstanceID;
                }
                else
                {
                    vhostname = mySOA.MyVirtualHost.ServiceNodes[i].NodeServiceName + azureRoleInstanceID;
                }
                if (mySOA.MyVirtualHost.ServiceNodes[i].CPU != -1f)
                    strCPU = System.Math.Round(mySOA.MyVirtualHost.ServiceNodes[i].CPU).ToString();
                else
                    strCPU = "*";
                if (mySOA.MyVirtualHost.ServiceNodes[i].ASPNETReqPerSec != -1f)
                    strASPNET = System.Math.Round(mySOA.MyVirtualHost.ServiceNodes[i].ASPNETReqPerSec).ToString();
                else
                    strASPNET = "*";
                if (mySOA.MyVirtualHost.ServiceNodes[i].WCFReqPerSec != -1f)
                    strWCF = System.Math.Round(mySOA.MyVirtualHost.ServiceNodes[i].WCFReqPerSec).ToString();
                else
                    strWCF = "*";
                string runtime2;
                if (mySOA.MyVirtualHost.ServiceNodes[i].OSVersionString == null)
                    runtime2 = "";
                else
                    runtime2 = mySOA.MyVirtualHost.ServiceNodes[i].RuntimePlatform + ": OS v " + mySOA.MyVirtualHost.ServiceNodes[i].OSVersionString;
                string requests = "";
                string requestsPerDay = "";
                if (mySOA.MyVirtualHost.ServiceNodes[i].EndPointData != null && mySOA.MyVirtualHost.ServiceNodes[i].EndPointData.Count > 0)
                {
                    requests = mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].TotalRequests.ToString();
                    requestsPerDay = mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].RequestsPerDay.ToString();
                }
                else
                {
                    requests = "*";
                    requestsPerDay = "*";
                }
                if (CheckBoxNoDetail.Checked)
                {
                    display = "<span style=\"padding-left:10px;color:#848483;\">" + vhostname + "</span>";
                }
                else
                {
                    display = "<table align=\"right\" width='1675px' style='color:#848483;border:0px;table-layout:fixed;border-collpase:collapse'>" +
                                            "<col width=\"775\"/><col width=\"200\"/><col width=\"200\"/><col width=\"200\"/><col width=\"300\"/> <tr>" +
                                            "<td style=\"width:770;padding-left:10px;\">" + vhostname + "</td>" +
                                            "<td style=\"width:200;font-size:.9em\">ASP.NET Request/Sec: <span style=\"Color:#6B8EAA;\">" + strASPNET + "</td></span>" +
                                            "<td style=\"width:200;font-size:.9em\">WCF Requests/Sec: <span style=\"Color:#6B8EAA;\">" + strWCF + "</td></span>" +
                                            "<td style=\"width:200;font-size:.9em\">Current CPU  : <span style=\"Color:#6B8EAA;\">" + strCPU + "%</span></td>" +
                                            "<td style=\"width:300;font-size:.9em\">Runtime Platform: " + runtime2 + "</td></tr></table>";
                }
                node = new TreeNode(display, nodeimageIndex.ToString(), imageList[nodeimageIndex]);
                if (mySOA.MyVirtualHost.ServiceNodes[i].Exception == "Online" && mySOA.MyVirtualHost.ServiceNodes[i].EndPointData != null && mySOA.MyVirtualHost.ServiceNodes[i].EndPointData.Count > 0)
                    if (mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints.Count > 0 || mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].RequestsPerDay > 0)
                    {
                        if (mySOA.MyVirtualHost.ServiceNodes[i].PrimaryListenEndpoints.Count >0)
                            node.ToolTip = "Node Active Since: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].ActiveSince.ToString() + "\nTotal Primary Business Service Requests Since Active: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].TotalRequests.ToString() + "\nPrimary Business Service Requests per Day: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].RequestsPerDay.ToString();
                        else
                            node.ToolTip = "Node Active Since: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].ActiveSince.ToString() + "\nTotal Measured Page Requests Since Active: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].TotalRequests.ToString() + "\nMeasured Page Requests per Day: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].RequestsPerDay.ToString();
                    }
                    else
                        node.ToolTip = "Node Active Since: " + mySOA.MyVirtualHost.ServiceNodes[i].EndPointData[0].ActiveSince.ToString();
                else
                    node.ToolTip = "Node Service Exception: " + mySOA.MyVirtualHost.ServiceNodes[i].Exception;
                node.Collapse();
                //node.ChildNodes.Add(dcEPs);
                node.ChildNodes.Add(configEPs);
                node.ChildNodes.Add(primaryEPs);
                clusterNodes[i] = node;
            }
            string vName = display;
            if (mySOA.MyVirtualHost.AvgCPU != -1f)
                strCPU = System.Math.Round(mySOA.MyVirtualHost.AvgCPU).ToString();
            else
                strCPU = "*";
            if (mySOA.MyVirtualHost.TotalASPNETRecPerSec != -1f)
                strASPNET = System.Math.Round(mySOA.MyVirtualHost.TotalASPNETRecPerSec).ToString();
            else
                strASPNET = "*";
            if (mySOA.MyVirtualHost.TotalWCFRecPerSec != -1f)
                strWCF = System.Math.Round(mySOA.MyVirtualHost.TotalWCFRecPerSec).ToString();
            else
                strWCF = "*";
            string display2 = "";
            if (CheckBoxNoDetail.Checked)
            {
                display2 = "<span style=\"color:#FFFFFF;padding-left:10px\">" + mySOA.MyVirtualHost.VHostName + "  (Node Count=" + mySOA.MyVirtualHost.ServiceNodes.Count.ToString() + ")</span>"; 
            }
            else
            {
                 display2 = "<table align=\"right\" width=\"1700px\" style=\"color:#FFFFFF;border:0px;table-layout:fixed;border-collapse:collapse\">" +
                  "<col width=\"800\"/><col width=\"200\"/><col width=\"200\"/><col width=\"200\"/><col width=\"300\"/><tr>" +
                                          "<td style=\"width:800;padding-left:10px;border:0px\">" + mySOA.MyVirtualHost.VHostName + "  (Node Count=" + mySOA.MyVirtualHost.ServiceNodes.Count.ToString() + ")</td>" +
                                          "<td style=\"width:200;font-size:.9em\">ASP.NET Request/Sec: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strASPNET + "</span></td>" +
                                          "<td style=\"width:200;font-size:.9em\">WCF Requests/Sec: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strWCF + "</td></span>" +
                                          "<td style=\"width:200;font-size:.9em\">Avg. Node CPU: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strCPU + "%</td></span>" +
                                          "<td style=\"width:300;font-size:.9em\">Deployment: " + deployment + "</td></tr></table>";
            }
            myVHost.Text = display2;
            soaTreeView.Nodes.Add(root);
            TreeNode myDatabases = getMyDatabases(mySOA);
            TreeNode myDistributedCaches = getMyDistributedCaches(mySOA);
            soaTreeView.Nodes[0].ChildNodes.Add(myDatabases);
            soaTreeView.Nodes[0].ChildNodes.Add(myDistributedCaches);
            string prefix2 = "Service Domain ";
            if (deployment.ToLower().Contains("azure"))
                prefix2 = "Windows Azure Service Domain ";
            else
                if (deployment.ToLower().Contains("hyper-v"))
                    prefix2 = "Windows Server Hyper-V Service Domain ";
            if (mySOA.MyVirtualHost.ServiceNodes != null && mySOA.MyVirtualHost.ServiceNodes.Count > 0)
            {
                if (mySOA.MyVirtualHost.ServiceNodes[0].PrimaryListenEndpoints.Count > 0 || mySOA.MyVirtualHost.RequestsPerDay>0)
                {
                    if (mySOA.MyVirtualHost.ServiceNodes[0].PrimaryListenEndpoints.Count > 0)
                        myVHost.ToolTip = prefix2 + "\nTotal Primary Business Service Requests: " + mySOA.MyVirtualHost.TotalRequests.ToString() + "\nPrimary Business Service Requests per Day: " + mySOA.MyVirtualHost.RequestsPerDay.ToString();
                    else
                        myVHost.ToolTip = prefix2 + "\nTotal Measured Page Requests: " + mySOA.MyVirtualHost.TotalRequests.ToString() + "\nMeasured Page Requests per Day: " + mySOA.MyVirtualHost.RequestsPerDay.ToString();
                }
                else
                    myVHost.ToolTip = prefix2;
            }
            soaTreeView.Nodes[0].ChildNodes.Add(myVHost);
            for (int z = 0; z < clusterNodes.Length; z++)
            {
                soaTreeView.Nodes[0].ChildNodes[2].ChildNodes.Add(clusterNodes[z]);
            }
            TreeNode[] connectedServices = recurseSOATree(mySOA);
            if (connectedServices != null)
            {
                for (int z = 0; z < connectedServices.Length; z++)
                {
                    soaTreeView.Nodes[0].ChildNodes.Add(connectedServices[z]);
                }
            }
        }

        private TreeNode getMyDistributedCaches(SOA mySOA)
        {
            bool rootYellow = false;
            if (mySOA.MyDistributedCaches != null)
            {
                for (int i = 0; i < mySOA.MyDistributedCaches.Count; i++)
                {
                    if (mySOA.MyDistributedCaches[i].Status != ConfigUtility.DISTRIBUTED_CACHE_ONLINE)
                        rootYellow = true;
                }
            }
            int imageIndex = 0;
            if (rootYellow)
                imageIndex = 53;
            else
                imageIndex = 49;
            TreeNode root = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px;\">Distributed Caches</span>", imageIndex.ToString(), imageList[imageIndex]);
            if (rootYellow)
                root.ToolTip = "Distributed Caches - Some Offline!";
            else
                root.ToolTip = "Distributed Caches";
            if (mySOA.MyDistributedCaches == null || mySOA.MyDistributedCaches.Count == 0)
                return root;
            root.Collapse();
            imageIndex=0;
            int offset = 0;
            TreeNode[] clusterNodes = new TreeNode[mySOA.MyDistributedCaches.Count];
            string prefix = "";
            for (int i = 0; i < mySOA.MyDistributedCaches.Count; i++)
            {
                if (mySOA.MyDistributedCaches[i].Status == null)
                {
                    mySOA.MyDistributedCaches[i].Status = "Unknown";
                }
                else
                    if (mySOA.MyDistributedCaches[i].CacheServers!=null && mySOA.MyDistributedCaches[i].CacheServers.Count != 0)
                    {
                        if (mySOA.MyDistributedCaches[i].CacheServers[0].ServerName.ToLower().Contains("windows.net"))
                        {
                            offset = 1;
                            prefix = "Windows Azure AppFabric ";
                        }
                        else
                        {
                            offset = 0;
                            prefix = "Windows Server AppFabric ";
                        }
                    }
                if (mySOA.MyDistributedCaches[i].Status.Equals(ConfigUtility.DISTRIBUTED_CACHE_ONLINE))
                    imageIndex = 43 + offset;
                else
                    if (mySOA.MyDistributedCaches[i].Status.Equals(ConfigUtility.DISTRIBUTED_CACHE_OFFLINE))
                    {
                        imageIndex = 45 + offset;
                    }
                    else
                    {
                        imageIndex = 47 + offset;
                    }
                TreeNode cacheServer;
                if (CheckBoxNoDetail.Checked)
                {
                    cacheServer = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px\">" + prefix + mySOA.MyDistributedCaches[i].Name + "</span>", imageIndex.ToString(), imageList[imageIndex]);
                }
                else
                {
                    cacheServer = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px\">" + prefix + mySOA.MyDistributedCaches[i].Name + "</span>", imageIndex.ToString(), imageList[imageIndex]);
                }
                cacheServer.ToolTip = mySOA.MyDistributedCaches[i].Exception;
                cacheServer.Collapse();
                TreeNode[] cacheservers = new TreeNode[mySOA.MyDistributedCaches[i].CacheServers.Count];
                TreeNode latencyLocal = new TreeNode("<span style=\"color:#63553D;\">Latency for Client Local Cache Get ms: " + mySOA.MyDistributedCaches[i].LocalCacheLatency.ToString() + "</span>", "42", imageList[42]);
                TreeNode latencyDistributed = new TreeNode("<span style=\"color:#63553D;\">Latency for Client Distributed Cache Get ms: " + mySOA.MyDistributedCaches[i].DistributedCacheLatency.ToString() + "</span>", "42", imageList[42]);
                latencyLocal.ToolTip = "The amount of time in ms to pull from the in-memory local cache a 3K custom object that exists in the local cache, inclusive of deserialization time. Since the test object exists in the local cache, a network request to the distributed cache cluster is not necessary and is not performed by the AppFabric cache client.";
                latencyDistributed.ToolTip = "The amount of time in ms to pull from the distributed cache cluster a 3K custom object. This is inclusive of the remote network call, and deserialization time.  The cache client is gauranteed to make a distributed network call on this request, as the local cache will not contain the test object.";
                if (offset == 1)
                    imageIndex = 5;
                else
                    imageIndex = 13;
                for (int j = 0; j < mySOA.MyDistributedCaches[i].CacheServers.Count; j++)
                {
                    if (mySOA.MyDistributedCaches[i].CacheServers[j].ServerName == null || mySOA.MyDistributedCaches[i].CacheServers[j].ServerName == "")
                        break;
                    cacheservers[j] = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px;\">" + mySOA.MyDistributedCaches[i].CacheServers[j].ServerName + ":" + mySOA.MyDistributedCaches[i].CacheServers[j].Port.ToString(), imageIndex.ToString(), imageList[imageIndex]);
                    if (imageIndex == 5)
                        cacheservers[j].ToolTip = "Windows Azure AppFabric Cache Namespace: Multiple Servers Service This Cache Behind Azure AppFabric Load Balancer";
                    else
                        cacheservers[j].ToolTip = "Windows Server AppFabric Cache Host";
                }
                if (mySOA.MyDistributedCaches[i].Status == ConfigUtility.DISTRIBUTED_CACHE_ONLINE)
                {
                    cacheServer.ChildNodes.Add(latencyDistributed);
                    cacheServer.ChildNodes.Add(latencyLocal);
                }
                if (cacheservers != null)
                {
                    for (int nodecount = 0; nodecount < cacheservers.Length; nodecount++)
                    {
                        cacheServer.ChildNodes.Add(cacheservers[nodecount]);
                    }
                }
                clusterNodes[i] = cacheServer;
            }
            if (clusterNodes != null && clusterNodes.Length > 0)
            {
                for (int i = 0; i < clusterNodes.Length; i++)
                {
                    root.ChildNodes.Add(clusterNodes[i]);
                }
            }
            return root;
        }

        private TreeNode getMyDatabases(SOA mySOA)
        {
            int imageIndex = 0;
            bool rootYellow=false;
            if (mySOA.MyDatabaseServers!=null)
            {
                for (int i = 0; i < mySOA.MyDatabaseServers.Count; i++)
                {
                    if (mySOA.MyDatabaseServers[i].Status != ConfigUtility.DATABASE_ONLINE)
                        rootYellow = true;
                }
            }
            if (rootYellow)
                imageIndex = 54;
            else
                imageIndex = 38;
            TreeNode root = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px;\">Database Servers</span>", imageIndex.ToString(), imageList[imageIndex]);
            if (rootYellow)
                root.ToolTip = "Connected Databases - Some Offline!";
            else
                root.ToolTip = "Connected Databases";
            if (mySOA.MyDatabaseServers == null || mySOA.MyDatabaseServers.Count == 0)
                return root;
            root.Collapse();
            imageIndex = 0;
            int offset = 0;
            TreeNode[] clusterNodes = new TreeNode[mySOA.MyDatabaseServers.Count];
            string prefix = "";
            for (int i = 0; i < mySOA.MyDatabaseServers.Count; i++)
            {
                if (mySOA.MyDatabaseServers[i].Edition == null)
                {
                    mySOA.MyDatabaseServers[i].Edition = "Unknown";
                    mySOA.MyDatabaseServers[i].Version = "Unknown";
                }
                else
                    if (mySOA.MyDatabaseServers[i].Edition.ToLower().Contains("azure"))
                    {
                        offset = 1;
                        prefix = "SQL Azure Cloud RDBMS ";
                    }
                    else
                    {
                        offset = 0;
                        prefix = "On-Premise SQL Server ";
                    }
                if (mySOA.MyDatabaseServers[i].Status.Equals(ConfigUtility.DATABASE_ONLINE))
                    imageIndex = 28 + offset;
                else
                    if (mySOA.MyDatabaseServers[i].Status.Equals(ConfigUtility.DATABASE_OFFLINE))
                        imageIndex = 30 + offset;
                    else
                        imageIndex = 32 + offset;
                TreeNode dbServer;
                if (CheckBoxNoDetail.Checked)
                {
                    dbServer = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px\">" + prefix + mySOA.MyDatabaseServers[i].ServerName + "</span>", imageIndex.ToString(), imageList[imageIndex]);
                }
                else
                {
                    dbServer = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px\">" + prefix + mySOA.MyDatabaseServers[i].ServerName + " : " + mySOA.MyDatabaseServers[i].Edition + " : " + mySOA.MyDatabaseServers[i].Version + "</span>", imageIndex.ToString(), imageList[imageIndex]);
                }
                dbServer.ToolTip = mySOA.MyDatabaseServers[i].Exception;
                dbServer.Collapse();
                TreeNode[] databases = new TreeNode[mySOA.MyDatabaseServers[i].MyDatabases.Count];
                for (int j = 0; j < mySOA.MyDatabaseServers[i].MyDatabases.Count; j++)
                {
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].Edition == null)
                    {
                        mySOA.MyDatabaseServers[i].MyDatabases[j].Edition = "Unknown";
                    }
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].Edition.ToLower().Contains("azure"))
                        offset = 1;
                    else
                        offset = 0;
                    switch (mySOA.MyDatabaseServers[i].MyDatabases[j].Status)
                    {
                        case ConfigUtility.DATABASE_ONLINE: { imageIndex = 34 + offset; break; }
                        case ConfigUtility.DATABASE_OFFLINE: { imageIndex = 36 + offset; break; }
                        default: { imageIndex = 36 + offset; break; }
                    }
                    //the most likely cause of not having data here is that the user does not permissions for stats on SQL Server.  For the detailed query stats, you need the login user to be in an admin role or some sort.
                    //For ConfigWeb, the config administrator needs to be granted these permissions on a per database case; if desired.  If not desired,
                    //you  just will get no additional info.
               
                    databases[j] = new TreeNode("<span style=\"color:#8D7A59;padding-left:10px;\">" + mySOA.MyDatabaseServers[i].MyDatabases[j].DatabaseName +  " : Size=  </span><span style=\"Color:#6B8EAA;\">" + (mySOA.MyDatabaseServers[i].MyDatabases[j].DatabaseSize*1000f).ToString() + " (MB)</span>", imageIndex.ToString(), imageList[imageIndex]);
                    databases[j].ToolTip = mySOA.MyDatabaseServers[i].MyDatabases[j].Exception;
                    TreeNode[] queries = new TreeNode[5];
                    TreeNode latency = new TreeNode("<span style=\"color:#63553D;\">Latency to SQL Database ms: " + mySOA.MyDatabaseServers[i].MyDatabases[j].Latency.ToString() + "</span>", "42", imageList[42]);
                    latency.ToolTip = "The amount of time in ms to execute a simple query and retrieve the single row-result set from the remote SQL database. Note that service-domains co-located with their SQL Azure databases in the same Azure region will typically have lower latency on database calls.";
                    queries[0] = latency;
                    databases[j].ChildNodes.Add(queries[0]);
                    //top slowest first.
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByExecTime != null)
                    {
                        TreeNode topSlowSQL = new TreeNode("<span style=\"color:#63553D;\">Top " + mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByExecTime.Count.ToString() + " Slowest SQL Statements</span>", "39", imageList[39]);
                        topSlowSQL.ToolTip = "The slowest n queries as returned by SQL Server Dynamic Management View statistics.  Pay attention to the number of times executed, and the last vs. Avg completion time. A database beginning to approach capacity (CPU and/or IO) will show an increasing last-exec time deviation from avg-exec time. You can easily adjust the number of queries shown using ConfigWeb.";
                        TreeNode[] sqlData0;
                        for (int queryslowestcount = 0; queryslowestcount < mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByExecTime.Count; queryslowestcount++)
                        {
                            Queries query0 = mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByExecTime[queryslowestcount];
                            sqlData0 = new TreeNode[5];
                            sqlData0[0] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query0.AvgExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData0[1] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query0.LastExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData0[2] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query0.AvgCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData0[3] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query0.LastCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData0[4] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Total Executions: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query0.TotalExecutions).ToString() + "</span></span>", "41", imageList[41]);
                            TreeNode queryNode0 = new TreeNode("<span style=\"color:#848483;font-size:.8em\">" + query0.SQLStatement + "</span>", "40", imageList[40]);
                            for (int childcount = 0; childcount < sqlData0.Length; childcount++)
                            {
                                queryNode0.ChildNodes.Add(sqlData0[childcount]);
                            }
                            queries[1] = queryNode0;
                            topSlowSQL.ChildNodes.Add(queryNode0);
                        }
                        databases[j].ChildNodes.Add(topSlowSQL);
                    }

                    //top worst CPU use next
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByCPUTime != null)
                    {
                        TreeNode topExpensiveSQL = new TreeNode("<span style=\"color:#63553D;\">Top " + mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByCPUTime.Count.ToString() + " Most CPU Intense SQL Statements</span>", "39", imageList[39]);
                        topExpensiveSQL.ToolTip = "The SQL queries that consume the most CPU time as returned by SQL Server Dynamic Management View statistics.  Pay attention to the number of times executed--focus on queries executed frequently within the application, as you will also see here one-off admin queries as executed over time. You can easily adjust the number of queries shown using ConfigWeb.";
                        TreeNode[] sqlData1;
                        for (int queryworstexpensivecount = 0; queryworstexpensivecount < mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByCPUTime.Count; queryworstexpensivecount++)
                        {
                            Queries query1 = mySOA.MyDatabaseServers[i].MyDatabases[j].TopWorstByCPUTime[queryworstexpensivecount];
                            sqlData1 = new TreeNode[5];
                            sqlData1[0] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query1.AvgExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData1[1] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query1.LastExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData1[2] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query1.AvgCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData1[3] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query1.LastCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData1[4] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Total Executions: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query1.TotalExecutions).ToString() + "</span></span>", "41", imageList[41]);
                            TreeNode queryNode1 = new TreeNode("<span style=\"color:#848483;font-size:.8em\">" + query1.SQLStatement + "</span>", "40", imageList[40]);
                            for (int childcount = 0; childcount < sqlData1.Length; childcount++)
                            {
                                queryNode1.ChildNodes.Add(sqlData1[childcount]);
                            }
                            queries[2] = queryNode1;
                            topExpensiveSQL.ChildNodes.Add(queryNode1);
                        }
                        databases[j].ChildNodes.Add(topExpensiveSQL);
                    }

                    //Now the best.  Save for last!
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByExecTime != null)
                    {
                        TreeNode topFastestSQL = new TreeNode("<span style=\"color:#63553D;\">Top " + mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByExecTime.Count.ToString() + " Fastest SQL Statements</span>", "39", imageList[39]);
                        TreeNode[] sqlData2;
                        topFastestSQL.ToolTip = "The fastest n queries as returned by SQL Server Dynamic Management View statistics. You can easily adjust the number of queries shown using ConfigWeb.";
                        for (int queryfastestcount = 0; queryfastestcount < mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByExecTime.Count; queryfastestcount++)
                        {
                            Queries query2 = mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByExecTime[queryfastestcount];
                            sqlData2 = new TreeNode[5];
                            sqlData2[0] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query2.AvgExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData2[1] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query2.LastExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData2[2] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query2.AvgCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData2[3] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query2.LastCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData2[4] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Total Executions: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query2.TotalExecutions).ToString() + "</span></span>", "41", imageList[41]);
                            TreeNode queryNode2 = new TreeNode("<span style=\"color:#848483;font-size:.8em\">" + query2.SQLStatement + "</span>", "40", imageList[40]);
                            for (int childcount = 0; childcount < sqlData2.Length; childcount++)
                            {
                                queryNode2.ChildNodes.Add(sqlData2[childcount]);
                            }
                            queries[3] = queryNode2;
                            topFastestSQL.ChildNodes.Add(queryNode2);
                        }
                        databases[j].ChildNodes.Add(topFastestSQL);
                    }

                    //Now best by CPU use
                    if (mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByCPUTime != null)
                    {
                        TreeNode topFreeSQL = new TreeNode("<span style=\"color:#63553D;\">Top " + mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByCPUTime.Count.ToString() + " Least CPU Intense SQL Statements</span>", "39", imageList[39]);
                        topFreeSQL.ToolTip = "The SQL queries that consume the least CPU time as returned by SQL Server Dynamic Management View statistics. You can easily adjust the number of queries shown using ConfigWeb.";
                        TreeNode[] sqlData3;
                        for (int queryleastexpcount = 0; queryleastexpcount < mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByCPUTime.Count; queryleastexpcount++)
                        {
                            Queries query3 = mySOA.MyDatabaseServers[i].MyDatabases[j].TopBestByCPUTime[queryleastexpcount];
                            sqlData3 = new TreeNode[5];
                            sqlData3[0] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query3.AvgExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData3[1] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last Completion Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query3.LastExecutionTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData3[2] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Avg CPU Time ms: " + "<span span style=\"color:ffffff;font-size:.9em\">" + (query3.AvgCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData3[3] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Last CPU Time ms: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query3.LastCPUTime / 1000).ToString() + "</span></span>", "41", imageList[41]);
                            sqlData3[4] = new TreeNode("<span style=\"color:#848483;font-size:.9em\">Total Executions: " + "<span span style=\"color:#ffffff;font-size:.9em\">" + (query3.TotalExecutions).ToString() + "</span></span>", "41", imageList[41]);
                            TreeNode queryNode3 = new TreeNode("<span style=\"color:#848483;font-size:.8em\">" + query3.SQLStatement + "</span>", "40", imageList[40]);
                            for (int childcount = 0; childcount < sqlData3.Length; childcount++)
                            {
                                queryNode3.ChildNodes.Add(sqlData3[childcount]);
                            }
                            queries[4] = queryNode3;
                            topFreeSQL.ChildNodes.Add(queryNode3);
                        }
                        databases[j].ChildNodes.Add(topFreeSQL);
                    }
                }
                for (int nodecount = 0; nodecount < databases.Length; nodecount++)
                {
                    dbServer.ChildNodes.Add(databases[nodecount]);
                }
                clusterNodes[i] = dbServer;
            }
            if (clusterNodes != null && clusterNodes.Length > 0)
            {
                for (int i = 0; i < clusterNodes.Length; i++)
                {
                    root.ChildNodes.Add(clusterNodes[i]);
                }
            }
            return root;
        }

        private TreeNode[] recurseSOATree(SOA soa)
        {
            string strCPU = null;
            string strWCF = null;
            string strASPNET = null;
            if (soa.ConnectedSOAs == null || soa.ConnectedSOAs.Count == 0)
                return null;
            TreeNode[] returnNodes = new TreeNode[soa.ConnectedSOAs.Count];
            for (int i = 0; i < soa.ConnectedSOAs.Count; i++)
            {
                int imageIndex = 0;
                if (soa.ConnectedSOAs[i].Status.Equals(ConfigSettings.MESSAGE_CIRCULAR_REF_TERMINAL))
                    imageIndex = 3;
                else
                    if (soa.ConnectedSOAs[i].Status.Equals(ConfigSettings.MESSAGE_OFFLINE))
                        imageIndex = 1;
                string rootName = "<span><Font color='#84BDEC'>" + soa.ConnectedSOAs[i].SOAName + "</font></span>";
                TreeNode root = new TreeNode(rootName, imageIndex.ToString(), imageList[imageIndex]);
                root.ToolTip = "Connected Service Domain";
                root.Expand();
                imageIndex = 0;
                int offset = 0;
                string prefix = "";
                string deployment = "";
                string display="";
                if (soa.ConnectedSOAs[i].MyVirtualHost != null)
                {
                    if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes != null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count > 0 && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0] != null)
                    {
                        if (soa.ConnectedSOAs[i].MyVirtualHost.Status == ConfigSettings.MESSAGE_OFFLINE)
                            offset = 0;
                        else
                        {
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].RuntimePlatform!=null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].RuntimePlatform.ToLower().Contains("azure"))
                            {
                                offset = 1;
                                prefix = "Azure Platform Cloud Deployed ";
                            }
                            else
                                if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].RuntimePlatform != null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].RuntimePlatform.ToLower().Contains("hyper"))
                                {
                                    offset = 2;
                                    prefix = "On-premise Deployed (Hyper-V) ";
                                }
                                else
                                {
                                    offset = 0;
                                    prefix = "On-premise Deployed ";
                                }
                        }
                    }
                    if (soa.ConnectedSOAs[i].MyVirtualHost.Status == ConfigSettings.MESSAGE_ONLINE)
                        imageIndex = 4 + offset;
                    else
                        if (soa.ConnectedSOAs[i].MyVirtualHost.Status == ConfigSettings.MESSAGE_OFFLINE)
                            imageIndex = 7 + offset;
                        else
                            if (soa.ConnectedSOAs[i].MyVirtualHost.Status == ConfigSettings.MESSAGE_SOME_NODES_DOWN)
                                imageIndex = 10 + offset;
                    TreeNode myVHost = new TreeNode(prefix + soa.ConnectedSOAs[i].MyVirtualHost.VHostName, imageIndex.ToString(), imageList[imageIndex]);
                    myVHost.Expand();
                    TreeNode[] clusterNodes = new TreeNode[soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count];
                    
                    for (int h = 0; h < soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count; h++)
                    {
                        TreeNode primaryEPs = new TreeNode("", "23", imageList[23]);
                        primaryEPs.ToolTip = "Primary Endpoints are those defined by the business application developer to service business requests via their custom business logic.";
                        primaryEPs.Collapse();
                        TreeNode configEPs = new TreeNode("", "22", imageList[22]);
                        configEPs.ToolTip = "Configuration Service Endpoints are infrastructure endpoints available for the Configuration Service itself, for example an endpoint to which ConfigWeb connects.";
                        configEPs.Collapse();
                     //   TreeNode dcEPs = new TreeNode("", "24", imageList[24]);
                     //   dcEPs.Collapse();
                        TreeNode[] endPointNodesPrimary = new TreeNode[soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints.Count];
                        TreeNode[] endPointNodesConfig = new TreeNode[soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints.Count];
                     //   TreeNode[] endPointNodesDC = new TreeNode[soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints.Count];
                        for (int j = 0; j < soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints.Count; j++)
                        {
                            string NAT = "";
                            string NAT2 = "";
                            int endPointImageIndex = 0;
                            switch (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].Status)
                            {
                                case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                                case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                                case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex = 27; break; }
                            }
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].LoadBalanceType.Equals(1))
                            {
                                NAT = "<span style=\"color:#FFFFFF\"> --> {" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].LoadBalanceAddress + "}</span>";
                                NAT2 = "<span style=\"color:#FFFFFF\"> --> {NAT Load-Balanced}</span>";
                            }
                            TreeNode endpointnode = null;
                            if (CheckBoxEndpointDetail.Checked)
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].RemoteAddress + "</span>" + NAT, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            else
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">" +soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].ServiceFriendlyName + "</span>" + NAT2, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            endpointnode.ToolTip = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints[j].Exception;
                            endPointNodesPrimary[j] = endpointnode;
                        }
                        for (int j = 0; j < soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints.Count; j++)
                        {
                            string NAT = "";
                            string NAT2 = "";
                            int endPointImageIndex = 0;
                            switch (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].Status)
                            {
                                case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                                case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                                case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex = 27; break; }
                            }
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].LoadBalanceType.Equals(1))
                            {
                                NAT = "<span style=\"color:#FFFFFF\"> --> {" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].LoadBalanceAddress + "}</span>";
                                NAT2 = "<span style=\"color:#FFFFFF\"> --> {NAT Load-Balanced}</span>";
                            }
                            TreeNode endpointnode = null;
                            if (CheckBoxEndpointDetail.Checked)
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].RemoteAddress + "</span>" +  NAT, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            else
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].ServiceFriendlyName + "</span>" + NAT2, endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            endpointnode.ToolTip = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ConfigServiceListenEndpoints[j].Exception;
                            endPointNodesConfig[j] = endpointnode;
                        }
                        /*
                        for (int j = 0; j < soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints.Count; j++)
                        {
                            int endPointImageIndex = 0;
                            switch (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints[j].Status)
                            {
                                case ConfigSettings.MESSAGE_ONLINE: { endPointImageIndex = 25; break; }
                                case ConfigSettings.MESSAGE_OFFLINE: { endPointImageIndex = 26; break; }
                                case ConfigSettings.MESSAGE_UNKNOWN: { endPointImageIndex = 27; break; }
                            }
                            TreeNode endpointnode = null;
                            if (CheckBoxEndpointDetail.Checked)
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">"+ soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints[j].RemoteAddress+ "</span>", endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            else
                            {
                                endpointnode = new TreeNode("<span style=\"color:#848483;\">" +soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints[j].ServiceFriendlyName + "</span>", endPointImageIndex.ToString(), imageList[endPointImageIndex]);
                            }
                            endpointnode.ToolTip = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].DCServiceListenEndpoints[j].Exception;
                            endPointNodesDC[j] = endpointnode;
                        }
                        for (int k = 0; k < endPointNodesDC.Length; k++)
                        {
                            dcEPs.ChildNodes.Add(endPointNodesDC[k]);
                        }
                         * */
                        for (int k = 0; k < endPointNodesConfig.Length; k++)
                        {
                            configEPs.ChildNodes.Add(endPointNodesConfig[k]);
                        }
                        for (int k = 0; k < endPointNodesPrimary.Length; k++)
                        {
                            primaryEPs.ChildNodes.Add(endPointNodesPrimary[k]);
                        }
                        int nodeimageIndex = 0;
                        offset = 0;
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].RuntimePlatform != null)
                        {
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].RuntimePlatform.ToLower().Contains("azure"))
                            {
                                offset = 1;
                                deployment = "Windows Azure";
                            }
                            else
                                if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].RuntimePlatform.ToLower().Contains("hyper"))
                                {
                                    offset = 2;
                                    deployment = "On-Premise/Private Cloud (Hyper-V)";
                                }
                                else
                                {
                                    offset = 0;
                                    deployment = "On-Premise/Private Cloud";
                                }
                        }
                        else
                        {
                            offset = 0;
                        }
                            switch (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].Status)
                            {
                                case ConfigSettings.MESSAGE_ONLINE: { nodeimageIndex = 13 + offset; break; }
                                case ConfigSettings.MESSAGE_OFFLINE: { nodeimageIndex = 16 + offset; break; }
                                case ConfigSettings.MESSAGE_UNKNOWN: { nodeimageIndex = 19 + offset; break; }
                                case ConfigSettings.MESSAGE_SOME_NODES_DOWN: { nodeimageIndex = 50 + offset; break; }
                            }
                        TreeNode node = null;
                        string vhostname = null;
                        string azureRoleInstanceID = "";
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].AzureRoleInstanceID!=null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].AzureRoleInstanceID!="")
                            azureRoleInstanceID = "<span style=\"font-size:.85em;\">&nbsp;&nbsp;&nbsp;{" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].AzureRoleInstanceID + "}</span>";
                        if (CheckBoxEndpointDetail.Checked)
                        {
                            vhostname = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].Address + azureRoleInstanceID;
                        }
                        else
                        {
                            vhostname = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].NodeServiceName + azureRoleInstanceID;

                        }
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].CPU != -1f)
                            strCPU = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].CPU).ToString();
                        else
                            strCPU = "*";
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ASPNETReqPerSec != -1f)
                            strASPNET = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].ASPNETReqPerSec).ToString();
                        else
                            strASPNET = "*";
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].WCFReqPerSec != -1f)
                            strWCF = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].WCFReqPerSec).ToString();
                        else
                            strWCF = "*";
                        string runtime;
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].OSVersionString == null)
                            runtime = "";
                        else
                            runtime = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].RuntimePlatform + ": OS v " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].OSVersionString;
                        if (CheckBoxNoDetail.Checked)
                        {
                            display = "<span style=\"padding-left:10px;color:#848483;\">" + vhostname + "</span>";
                        }
                        else
                        {
                            display = "<table align=\"right\" width='1675px' style='color:#848483;border:0px;table-layout:fixed;border-collpase:collapse'>" +
                                                    "<col width=\"775\"/><col width=\"200\"/><col width=\"200\"/><col width=\"200\"/><col width=\"300\"/> <tr>" +
                                                    "<td style=\"width:770;padding-left:10px;\">" + vhostname + "</td>" +
                                                    "<td style=\"width:200;font-size:.9em\">ASP.NET Request/Sec: <span style=\"Color:#6B8EAA;\">" + strASPNET + "</td></span>" +
                                                    "<td style=\"width:200;font-size:.9em\">WCF Requests/Sec: <span style=\"Color:#6B8EAA;\">" + strWCF + "</td></span>" +
                                                    "<td style=\"width:200;font-size:.9em\">Current CPU  : <span style=\"Color:#6B8EAA;\">" + strCPU + "%</span></td>" +
                                                    "<td style=\"width:300;font-size:.9em\">Runtime Platform: " + runtime + "</td></tr></table>";
                        }
                        node = new TreeNode(display, nodeimageIndex.ToString(), imageList[nodeimageIndex]);

                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].Exception == "Online" && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData != null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData.Count > 0)
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints.Count > 0 || soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].RequestsPerDay > 0)
                            {
                                if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].PrimaryListenEndpoints.Count > 0)
                                    node.ToolTip = "Active Since: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].ActiveSince.ToString() + "\nTotal Primary Business Service Requests Since Active: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].TotalRequests.ToString() + "\nPrimary Business Service Requests per Day: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].RequestsPerDay.ToString();
                                else
                                    node.ToolTip = "Active Since: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].ActiveSince.ToString() + "\nTotal Measured Page Requests Since Active: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].TotalRequests.ToString() + "\nMeasured Page Requests per Day: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].RequestsPerDay.ToString();
                            }
                            else
                                node.ToolTip = "Active Since: " + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].EndPointData[0].ActiveSince.ToString();
                        else
                            node.ToolTip = soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[h].Exception;
                        node.Collapse();
                       // node.ChildNodes.Add(dcEPs);
                        node.ChildNodes.Add(configEPs);
                        node.ChildNodes.Add(primaryEPs);
                        clusterNodes[h] = node;
                    }
                    if (soa.ConnectedSOAs[i].MyVirtualHost.AvgCPU != -1f)
                        strCPU = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.AvgCPU).ToString();
                    else
                        strCPU = "*";
                    if (soa.ConnectedSOAs[i].MyVirtualHost.TotalASPNETRecPerSec != -1f)
                        strASPNET = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.TotalASPNETRecPerSec).ToString();
                    else
                        strASPNET = "*";
                    if (soa.ConnectedSOAs[i].MyVirtualHost.TotalWCFRecPerSec != -1f)
                        strWCF = System.Math.Round(soa.ConnectedSOAs[i].MyVirtualHost.TotalWCFRecPerSec).ToString();
                    else
                        strWCF = "*";
                    string display2 = "";
                    if (CheckBoxNoDetail.Checked)
                    {

                        display2 = "<span style=\"color:#FFFFFF;padding-left:10px\">" + soa.ConnectedSOAs[i].MyVirtualHost.VHostName + "  (Node Count=" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count.ToString() + ")</span>";
                    }
                    else
                    {
                          display2 = "<table align=\"right\" width=\"1700px\" style=\"color:#FFFFFF;border:0px;table-layout:fixed;border-collapse:collapse\">" +
                          "<col width=\"800\"/><col width=\"200\"/><col width=\"200\"/><col width=\"200\"/><col width=\"300\"/><tr>" +
                                                  "<td style=\"width:800;padding-left:10px;border:0px\">" + soa.ConnectedSOAs[i].MyVirtualHost.VHostName + "  (Node Count=" + soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count.ToString() + ")</td>" +
                                                  "<td style=\"width:200;font-size:.9em\">ASP.NET Request/Sec: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strASPNET + "</span></td>" +
                                                  "<td style=\"width:200;font-size:.9em\">WCF Requests/Sec: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strWCF + "</td></span>" +
                                                  "<td style=\"width:200;font-size:.9em\">Avg. Node CPU: <span style=\"Color:#6B8EAA;font-weight:bold;\">" + strCPU + "%</td></span>" +
                                                  "<td style=\"width:300;font-size:.9em\">Deployment: " + deployment + "</td></tr></table>";
                    }
                    myVHost.Text = display2;
                    for (int c = 0; c < clusterNodes.Length; c++)
                    {
                        myVHost.ChildNodes.Add(clusterNodes[c]);
                    }
                    TreeNode myDatabases = getMyDatabases(soa.ConnectedSOAs[i]);
                    TreeNode myDistributedCaches = getMyDistributedCaches(soa.ConnectedSOAs[i]);
                    root.ChildNodes.Add(myDatabases);
                    root.ChildNodes.Add(myDistributedCaches);
                    string prefix2 = "Service Domain ";
                    if (deployment.ToLower().Contains("azure"))
                        prefix2 = "Windows Azure Service Domain ";
                    else
                        if (deployment.ToLower().Contains("hyper-v"))
                            prefix2 = "Windows Server Hyper-V Service Domain ";
                    if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes != null && soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes.Count > 0)
                    {
                        if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].PrimaryListenEndpoints.Count > 0 || soa.ConnectedSOAs[i].MyVirtualHost.RequestsPerDay > 0)
                        {
                            if (soa.ConnectedSOAs[i].MyVirtualHost.ServiceNodes[0].PrimaryListenEndpoints.Count>0)
                                myVHost.ToolTip = prefix2 + "\nTotal Primary Business Service Requests Since Active: " + soa.ConnectedSOAs[i].MyVirtualHost.TotalRequests.ToString() + "\nPrimary Business Service Requests per Day: " + soa.ConnectedSOAs[i].MyVirtualHost.RequestsPerDay.ToString();
                            else
                                myVHost.ToolTip = prefix2 + "\nTotal Measured Page Requests Since Active: " + soa.ConnectedSOAs[i].MyVirtualHost.TotalRequests.ToString() + "\nPrimary Measured Page Requests per Day: " + soa.ConnectedSOAs[i].MyVirtualHost.RequestsPerDay.ToString();
                        }
                        else
                            myVHost.ToolTip = prefix2;
                    }
                    root.ChildNodes.Add(myVHost);
                    if (soa.ConnectedSOAs[i].ConnectedSOAs != null && soa.ConnectedSOAs[i].ConnectedSOAs.Count > 0)
                    {
                        TreeNode[] csnodes = recurseSOATree(soa.ConnectedSOAs[i]);
                        for (int k = 0; k < csnodes.Length; k++)
                        {
                            root.ChildNodes[2].ChildNodes.Add(csnodes[k]);
                        }
                    }

                }
                returnNodes[i] = root;
            }
            return returnNodes;
        }
            

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            getData();
        }

        protected void ButtonClose_Click(object sender, EventArgs e)
        {
            bool iswp7 = false;
            bool isSafari = false;
            for (int i = 0; i < Request.Headers.Count; i++)
            {
                if (Request.Headers[i].ToString().Contains("XBLWP7"))
                    iswp7 = true;
                if (Request.Headers[i].ToString().ToLower().Contains("safari"))
                    isSafari = true;
            }
            if (isSafari)
                Response.Redirect(ConfigSettings.PAGE_NODES, true);
            if (!iswp7)
                AlittleJS.Text = "<script language='javascript'>window.close();</script>";
            else
                Response.Redirect(ConfigSettings.PAGE_NODES, true);
        }

        protected void AutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (AutoRefresh.Checked)
                TimerJS.Text = "<script language='javascript'>InitializeTimer();</script>";
            else
                TimerJS.Text = null;
        }
        private void home(bool logout)
        {
            if (logout)
                Response.Redirect(ConfigSettings.PAGE_LOGOUT,true);
            else
                Response.Redirect(ConfigSettings.PAGE_NODES,true);
        }

        protected void CheckBoxNoDetail_CheckedChanged(object sender, EventArgs e)
        {

        }
}
}

