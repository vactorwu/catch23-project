//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to view and choose configuration keys to edit, or add new keys.
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
    public partial class ConfigDefine : System.Web.UI.Page
    {
        string userid;
        string hostNameIdentifier;
        string configName;
        string address;
        string binding;
        string platform;
        string version;
        string hoster;
        string theTargetContract;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        List<ServiceConfigurationData> compositeServiceData = null;
        int theLevel = 1;
        List<TraverseNode> traversePath = null;
        int indexCount = 0;
        int index = 0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = TopNode.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                theTargetContract = (string)ViewState["contract"];
            }
            else
            {
                theTargetContract = Request["contract"];
                ViewState["contract"] = theTargetContract;
            }
            string levelString = Request["level"];
            indexCount = 0;
            ConfigRepeater.ItemDataBound += new RepeaterItemEventHandler(ConfigRepeater_ItemDataBound);
            theLevel = Convert.ToInt32(levelString);
            switch (theLevel)
            {
                case ConfigUtility.CONFIG_LEVEL_BASIC:
                    {
                        Basic.CssClass = "ConfigButton";
                        Detailed.CssClass = "ConfigButtonNonSelected";
                        Advanced.CssClass = "ConfigButtonNonSelected";
                        Level.Text = "Basic";
                        break;
                    }
                case ConfigUtility.CONFIG_LEVEL_DETAILED:
                    {
                        Basic.CssClass = "ConfigButtonNonSelected";
                        Detailed.CssClass = "ConfigButton";
                        Advanced.CssClass = "ConfigButtonNonSelected";
                        Level.Text = "Detailed";
                        break;
                    }
                case ConfigUtility.CONFIG_LEVEL_ADVANCED:
                    {
                        Basic.CssClass = "ConfigButtonNonSelected";
                        Detailed.CssClass = "ConfigButtonNonSelected";
                        Advanced.CssClass = "ConfigButton";
                        Level.Text = "Advanced";
                        break;
                    }
            }
            Contract.Text = theTargetContract;
            if (theTargetContract.ToLower().Contains("host."))
                Scope.Text="Configuration Service Inherited Settings";
            else
                Scope.Text="Custom Application Settings";
            Basic.PostBackUrl = ConfigSettings.PAGE_CONFIG_DEFINE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&contract=" + theTargetContract + "&level=1";
            Detailed.PostBackUrl = ConfigSettings.PAGE_CONFIG_DEFINE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&contract=" + theTargetContract + "&level=0";
            Advanced.PostBackUrl = ConfigSettings.PAGE_CONFIG_DEFINE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&contract=" + theTargetContract + "&level=-1";
            Add.PostBackUrl = ConfigSettings.PAGE_MANAGE_KEYS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&contract=" + theTargetContract + "&level=" + theLevel.ToString() +
                       "&action=" + ConfigUtility.ADD_KEY;
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, theLevel, false, traversePath, user);
            if (compositeServiceData != null && compositeServiceData[0] != null)
            {
                ConfigRepeater.DataSource = (findKeyValuesByContract(theTargetContract, 0));
                ConfigRepeater.DataBind();
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONFIG + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&contract=" + theTargetContract + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&level=" + levelString + "\">Return to Configuration Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private List<ConfigurationKeyValues> findKeyValuesByContract(string contract, int startIndex)
        {
            for (int i = startIndex; i < compositeServiceData.Count; i++)
            {
                if (compositeServiceData[i].ServiceContract.Equals(contract))
                {
                    index = i;
                    return compositeServiceData[i].ConfigurationData;
                }
            }
            return new List<ConfigurationKeyValues>();
        }

        public void ConfigRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            string edit = "";
            string DisplayNameString = "";
            string SettingNameString = "";
            string DataTypeString = "";
            string DescriptionString = "";

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConfigurationKeyValues keyItem = ((ConfigurationKeyValues)e.Item.DataItem);
                edit = "<a class=\"Config2\" href=\"" + ConfigSettings.PAGE_MANAGE_KEYS  +"?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster +
                        "&action=" + ConfigUtility.EDIT_KEY + "&keyname=" + keyItem.ConfigurationKeyDisplayName +
                        "&identify=" + keyItem.ConfigurationKeyFieldName + "&contract=" + theTargetContract + "&level=" + theLevel.ToString()
                        + "\">Edit</a>";
                ((Label)e.Item.FindControl("Edit")).Text = edit;
                DisplayNameString = keyItem.ConfigurationKeyDisplayName;
                ((Label)e.Item.FindControl("DisplayName")).Text = DisplayNameString;
                SettingNameString = keyItem.ConfigurationKeyFieldName;
                ((Label)e.Item.FindControl("SettingName")).Text = SettingNameString;
                DataTypeString = keyItem.ConfigurationKeyDataType;
                ((Label)e.Item.FindControl("DataType")).Text = DataTypeString;
                DescriptionString = keyItem.ConfigurationKeyDescription;
                ((Label)e.Item.FindControl("Description")).Text =  DescriptionString;
                ((Label)e.Item.FindControl("DisplayOrder")).Text = keyItem.DisplayOrder.ToString();
                indexCount++;
            }
        }

        protected void AddKey_Click(object sender, EventArgs e)
        {
           
        }
        protected void Basic_Click(object sender, EventArgs e)
        {

        }
        protected void Detailed_Click(object sender, EventArgs e)
        {

        }
        protected void Advanced_Click(object sender, EventArgs e)
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