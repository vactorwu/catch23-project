//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   


//============================================================================================
//Displays list of current Connected Service definitions for editing/deleting, or adding new 
//definitions.
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
    public partial class CSServices : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = AddConnectedService.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            ConnectedServiceRepeater.ItemDataBound += new RepeaterItemEventHandler(ConnectedService_ItemDataBound);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
            if (compositeServiceData != null && compositeServiceData[0] != null)
            {
                ConnectedServiceRepeater.DataSource = compositeServiceData[0].ConnectedServices;
                if (compositeServiceData[0].ConnectedServices != null)
                    ConnectedServiceRepeater.DataBind();
                if (compositeServiceData[0].ServiceType != ConfigUtility.HOST_TYPE_CONFIG)
                {
                    AddConnectedService.Enabled = false;
                }
            }
            else
            {
                Response.Redirect(ConfigSettings.PAGE_NODES,true);
            }
            AddConnectedService.PostBackUrl = ConfigSettings.PAGE_CONNECTED_SERVICE_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.ADD_CONNECTED_SERVICE;
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        public void ConnectedService_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string edit = "";
            string serviceName = "";
            string serviceContract = "";
            string bindingInfo = "";
            string serviceType = "";
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConnectedServices item = ((ConnectedServices)e.Item.DataItem);
                serviceContract = ChunkText.chunkDot(item.ServiceContract,'.');
                serviceName =item.ServiceFriendlyName;
                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_CONNECTED_SERVICE_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                         "&action=" + ConfigUtility.UPDATE_CONNECTED_SERVICE + "&ID=" + item.ConnectedServiceID + "\">Edit</a>";
                bindingInfo = ChunkText.chunkDot(item.ClientConfiguration, '_') + "<br/>Binding Type: " + item.BindingType + "<br/>SecurityMode: " + item.SecurityMode;
                switch (item.ServiceType)
                {
                    case ConfigUtility.HOST_TYPE_CONNECTED_SERVICE:
                        {
                            serviceType = "Primary Connected Service";
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE:
                        {
                            serviceType = "Generic Connected Service";
                            break;
                        }

                    default:
                        {
                            serviceType = "Unknown";
                            break;
                        }

                }
                ((Label)e.Item.FindControl("CSEdit")).Text = edit;
                ((Label)e.Item.FindControl("VHOSTID")).Text = item.VHostID.ToString();
                ((Label)e.Item.FindControl("CSNAME")).Text = serviceName; 
                ((Label)e.Item.FindControl("CSServiceContract")).Text = serviceContract;
                ((Label)e.Item.FindControl("CSBindingInfo")).Text = bindingInfo;
                ((Label)e.Item.FindControl("CSServiceType")).Text = serviceType;
            }
        }
    }
}
