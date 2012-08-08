//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Displays a list of Hosted Services (Service Endpoints) from the Config DB, for modifying,
//also allows Add operation to add new Endpoints to services.
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
    public partial class HSServices : System.Web.UI.Page
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
        List<HostedServices> serviceList;
        int HOSTID;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = AddHostedService.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            HostedServiceRepeater.ItemDataBound += new RepeaterItemEventHandler(HostedService_ItemDataBound);
            string idString = Request["ID"];
            string vHostName = Request["hostname"];
            HOSTID = Convert.ToInt32(idString);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
            if (compositeServiceData != null && compositeServiceData[0] != null)
            {
                if (compositeServiceData[0].HostedServices != null)
                {
                    serviceList = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == HOSTID; });
                    HostedServiceRepeater.DataSource = serviceList;
                    if (serviceList != null)
                        HostedServiceRepeater.DataBind();
                }
                if (compositeServiceData[0].ServiceType != ConfigUtility.HOST_TYPE_CONFIG)
                {
                    AddHostedService.Enabled = false;
                }
            }
            else
            {
                home(false);
            }
            VHostName.Text = vHostName;
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_VHOSTS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Virtual Host List</a>";
            AddHostedService.PostBackUrl = ConfigSettings.PAGE_HOSTED_SERVICE_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&SHID=" + HOSTID.ToString() +  "&action=" + ConfigUtility.ADD_HOSTED_SERVICE;
        }

        public void HostedService_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string edit = "";
            string serviceName = "";
            string serviceContract = "";
            string bindingInfo = "";
            string baseAddress = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HostedServices item = ((HostedServices)e.Item.DataItem);
                serviceContract = ChunkText.chunkDot(item.ServiceContract,'.');
                if (item.IsBaseAddress)
                    baseAddress = "yes";
                else
                    baseAddress = "no";
                serviceName =item.FriendlyName;
                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_HOSTED_SERVICE_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                         "&action=" + ConfigUtility.UPDATE_HOSTED_SERVICE + "&HSID=" + item.HostedServiceID.ToString() + "&SHID=" + HOSTID.ToString() + "\">Edit</a>";
                bindingInfo = ChunkText.chunkDot(item.ServiceBinding,'_') + "<br/>Binding Type: " + item.BindingType + "<br/>Use Https: " + item.UseHttps;
                ((Label)e.Item.FindControl("Edit")).Text = edit;
                ((Label)e.Item.FindControl("ServiceName")).Text = ChunkText.chunk(27,serviceName);
                ((Label)e.Item.FindControl("ServiceContract")).Text = serviceContract;
                ((Label)e.Item.FindControl("BindingInfo")).Text = bindingInfo;
                ((Label)e.Item.FindControl("HSID")).Text = item.HostedServiceID.ToString();
                ((Label)e.Item.FindControl("BaseAddress")).Text = baseAddress;
                string lb="";
                if (item.LoadBalanceType.Equals(1))
                {
                    lb="<br/><span style=\"color:#FFFF99\">NAT Address:<br/>" + item.LoadBalanceAddress + "</span>";
                }
                else
                {
                    lb = "<br/>NAT Address:<br/>N/A";
                }
                ((Label)e.Item.FindControl("ServiceVirtualPath")).Text = "Port: " + item.Port + "<br/>Path: " + item.VirtualPath + lb;
                string color=null;
                if (item.Activated)
                    color = "Palegreen";
                else
                    color = "Maroon";
                ((Label)e.Item.FindControl("HSActive")).Text = "<span style=\"color:" + color + "\">" + item.Activated.ToString() + "</span>";
            }
        }

        protected void AddHostedService_Click(object sender, EventArgs e)
        {

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