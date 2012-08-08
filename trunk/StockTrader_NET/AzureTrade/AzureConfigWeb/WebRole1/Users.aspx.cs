//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Displays list of current registered users from the Configuration Database.
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
    public partial class Users : System.Web.UI.Page
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
        List<ServiceConfigurationData> compositeServiceData;
        List<TraverseNode> traversePath;
        List<ServiceUsers> userList;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = AddUser.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            AddUser.PostBackUrl = ConfigSettings.PAGE_MANAGE_USERS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&hoster=" + hoster + "&platform=" + platform +  "&action=" + ConfigUtility.ADD_USER + "&identify=0";
            UserRepeater.ItemDataBound += new RepeaterItemEventHandler(User_ItemDataBound);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
            if (compositeServiceData != null && compositeServiceData[0] != null)
            {
                userList = configProxy.getServiceUsers(hostNameIdentifier, configName, traversePath, user);
                UserRepeater.DataSource = userList;
                if (userList != null)
                    UserRepeater.DataBind();
                if (compositeServiceData[0].ServiceType != ConfigUtility.HOST_TYPE_CONFIG)
                {
                    AddUser.Enabled = false;
                }
            }
            else
            {
                home(false);
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_NODES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Home Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        public void User_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string edit = "";
            string userName = "";
            string rights = "";
            string userKey = "";
            string localUser = "";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ServiceUsers item = ((ServiceUsers)e.Item.DataItem);
                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_MANAGE_USERS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                         "&action=" + ConfigUtility.UPDATE_USER + "&identify=" + item.UserKey + "\">Edit</a>";
                userName = item.UserId;
                userKey = item.UserKey.ToString();
                if (item.LocalUser)
                    localUser = "true";
                else
                    localUser = "false";
                switch (item.Rights)
                {
                    case ConfigUtility.CONFIG_ADMIN_RIGHTS:
                        {
                            rights = ConfigUtility.CONFIG_ADMIN_RIGHTS_STRING;
                            break;
                        }

                    case ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS:
                        {
                            rights = ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS_STRING;
                            break;
                        }

                    case ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS:
                        {
                            rights = ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS_STRING;
                            break;
                        }

                    case ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS:
                        {
                            rights = ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS_STRING;
                            break;
                        }


                    default:
                        {
                            rights = "Unknown";
                            break;
                        }

                }
                ((Label)e.Item.FindControl("Edit")).Text = edit;
                ((Label)e.Item.FindControl("UserName")).Text = userName;
                ((Label)e.Item.FindControl("Rights")).Text = rights;
                ((Label)e.Item.FindControl("UserKey")).Text = userKey;
                ((Label)e.Item.FindControl("Local")).Text = localUser;
            }
        }

        protected void AddUser_Click(object sender, EventArgs e)
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
