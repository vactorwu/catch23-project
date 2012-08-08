//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Displays list of currently defined Virtual Hosts (service implementations).
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
using ConfigService.RuntimeHostData;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class VHosts : System.Web.UI.Page
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
        ServiceConfigurationClient configProxy;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<MasterServiceHostInstance> serviceHosts;
        List<ServiceHostInfo> startupServiceHostList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = AddVirtualHost.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            string action = (string)Request["action"];
            if (action!=null && action.Equals(ConfigUtility.REMOVE_VHOST))
            {
                removeVHost();
            }
            VHostRepeater.ItemDataBound += new RepeaterItemEventHandler(VHostRepeater_ItemDataBound);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
            compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
            startupServiceHostList = configProxy.getStartupVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
            if (compositeServiceData == null || compositeServiceData.Count == 0)
                home(false);
            serviceHosts = configProxy.getVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
            if (serviceHosts != null)
            {
                VHostRepeater.DataSource = serviceHosts;
                VHostRepeater.DataBind();
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void removeVHost()
        {
            string IDStr = (string)Request["ID"];
            int ID=-1;
            if (!Int32.TryParse(IDStr,out ID))
                return;
            MasterServiceHostInstance oldVHost = new MasterServiceHostInstance();
            oldVHost.ServiceHostID = ID;
            MasterServiceHostInstance newVHost = new MasterServiceHostInstance();
            newVHost.ServiceHostID = ID;
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            try
            {
                int success = configProxy.receiveService(hostNameIdentifier, configName, null, null, null, null, null, oldVHost, newVHost, true, ConfigUtility.REMOVE_VHOST, traversePath, user);
            }
            catch
            {
                return;
            }
            return;
        }

        public void VHostRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string edit = "";
            string view = "";
            string name = "";
            string workflow = "";
            string serviceType = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MasterServiceHostInstance item = ((MasterServiceHostInstance)e.Item.DataItem);
                name = "<a class=\"Config3\" href=\"" + ConfigSettings.PAGE_HOSTED_SERVICES + "?name=" +
                        hostNameIdentifier + "&cfgSvc=" + configName +
                        "&action=edit&ID="
                        + item.ServiceHostID + "&hostname=" + item.HostName
                        + "\">" + item.HostName + "</a>";
                
                List<HostedServices> thisServiceList = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == item.ServiceHostID; });
                
                if (item.IsWorkFlowServiceHost)
                    workflow = "Yes";
                else
                    workflow = "No";
                view = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_HOSTED_SERVICES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                      "&action=" + ConfigUtility.UPDATE_HOSTED_SERVICE + "&ID=" + item.ServiceHostID + "\">Select</a>";
                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_VHOST_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                      "&action=" + ConfigUtility.UPDATE_VHOST + "&SHID=" + item.ServiceHostID + "\">Edit</a>";
                switch (item.ServiceType)
                {
                    case ConfigUtility.HOST_TYPE_PRIMARY:
                        {
                            serviceType = "Primary Service";
                            if (startupServiceHostList ==null)
                                startupServiceHostList = new List<ServiceHostInfo>();
                            ServiceHostInfo thehost = startupServiceHostList.Find(delegate (ServiceHostInfo shiExist) {return shiExist.HostName.Equals(item.HostName);});
                            if (thehost == null)
                            {
                                view = "<span style=\"color:Maroon\">Not Included in Startup Logic!</span>";
                                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_VHOSTS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                                  "&action=" + ConfigUtility.REMOVE_VHOST + "&ID=" + item.ServiceHostID + "\">Delete</a>";
                                name = item.HostName;
                            }
                            else
                            {

                            }
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_CONFIG:
                        {
                            serviceType = "Configuration Service";
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_NODE:
                        {
                            serviceType = "Node Service";
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_NODE_DC:
                        {
                            serviceType = "Node Custom Cache Service";
                            break;
                        }
                }
                
                
                ((Label)e.Item.FindControl("Edit")).Text = edit;
                ((Label)e.Item.FindControl("LabelWorkflow")).Text = workflow;
                ((Label)e.Item.FindControl("ViewServices")).Text = view;
                ((Label)e.Item.FindControl("ServiceHostName")).Text =name;
                ((Label)e.Item.FindControl("ServiceHostType")).Text = serviceType;
                ((Label)e.Item.FindControl("NumEndpoints")).Text = thisServiceList.Count.ToString();
                ((Label)e.Item.FindControl("VID")).Text = item.ServiceHostID.ToString();
            }
        }

        protected void AddVirtualHost_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigSettings.PAGE_VHOST_UPDATE + "?name=" +
                        hostNameIdentifier + "&cfgSvc=" + configName +
                        "&action=" + ConfigUtility.ADD_VHOST,true);
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
