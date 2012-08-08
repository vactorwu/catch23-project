//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Form to modify/add Configuration Settings Keys.  The field name must already be incorporated
//into the app-specific Settings class as a static. Validition is performed.
//============================================================================================
//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Runtime.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ServiceModel;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfigurationUtility;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class ManageKeys : System.Web.UI.Page
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
        string levelString;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        string DisplayName;
        int ConfigLevel = 0;
        string SettingName;
        string DataType;
        string Description;
        string ValidValues;
        string ReadOnly;
        string Value;
        string identify;
        string action;
        string actiontext;
        int DisplayOrder = 0;
        ConfigurationKeyValues updatedKey;
        int level = 1;
        int index = 0;
        ConfigurationKeyValues thisKey;
        List<TraverseNode> traversePath;
        List<ConfigurationKeyValues> thisData;
        List<ServiceConfigurationData> compositeServiceData;
       

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = TextBoxDisplayName.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                levelString = (string)ViewState["level"];
                theTargetContract = (string)ViewState["targetContract"];
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["CompositeServiceData"];
                identify = (string)ViewState["identify"];
                action = (string)ViewState["action"];
            }
            else
            {
                theTargetContract = Request["contract"];
                ViewState["targetContract"] = theTargetContract;
            }
            configProxy = new ServiceConfigurationClient(binding, address, user);
            levelString = Request["level"];
            level = Convert.ToInt32(levelString);
            ViewState["level"] = levelString;
            if (!IsPostBack || compositeServiceData == null)
            {
                action = Request["action"];
                identify = Request["identify"];
                ViewState["identify"] = identify;
                ViewState["action"] = action;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, level, false, traversePath, user);
                ViewState["CompositeServiceData"] = compositeServiceData;
                initRadioButtons();
            }
            else
            {
                identify = (string)ViewState["identify"];
                action = (string)ViewState["action"];
            }
            thisData = findKeyValuesByContract(theTargetContract, 0);
            if (thisData == null)
            {
                home(false);
            }
            Add.PostBackUrl = ConfigSettings.PAGE_MANAGE_KEYS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.ADD_KEY  + "&level=" + levelString + "&identify=" + identify;
            Update.PostBackUrl = ConfigSettings.PAGE_MANAGE_KEYS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.EDIT_KEY + "&level=" + levelString + "&identify=" + identify;
            Delete.PostBackUrl = ConfigSettings.PAGE_MANAGE_KEYS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.REMOVE_KEY + "&level=" + levelString + "&identify=" + identify;
            ViewState["thisData"] = thisData;
            if (action.Equals(ConfigUtility.ADD_KEY))
            {
                Update.Enabled = false;
                Delete.Enabled = false;
            }
            else
            {
                Add.Enabled = false;
                thisKey = getKey(thisData, identify);
                ViewState["thisKey"] = thisKey;
                if (thisKey == null)
                {
                    Response.Redirect(ConfigSettings.PAGE_CONFIG,true);
                }
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONFIG_DEFINE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&contract=" + theTargetContract + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&level=" + level.ToString() + "\">Return to Configuration Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
            if (!IsPostBack && !action.Equals(ConfigUtility.ADD_KEY))
                getData();
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

        private ConfigurationKeyValues getKey(List<ConfigurationKeyValues> listToParse, string keyName)
        {
            for (int i = 0; i < listToParse.Count; i++)
            {
                if (listToParse[i].ConfigurationKeyFieldName == keyName)
                    return listToParse[i];
            }
            return null;
        }

        private void initRadioButtons()
        {
            RadioButtonReadOnly.Items.Add(new ListItem("true", "true"));
            RadioButtonReadOnly.Items.Add(new ListItem("false", "false"));
            RadioButtonReadOnly.SelectedIndex = 1;
            RadioButtonListLevel.Items.Add(new ListItem("Basic", ConfigUtility.CONFIG_LEVEL_BASIC.ToString()));
            RadioButtonListLevel.Items.Add(new ListItem("Detailed", ConfigUtility.CONFIG_LEVEL_DETAILED.ToString()));
            RadioButtonListLevel.Items.Add(new ListItem("Advanced", ConfigUtility.CONFIG_LEVEL_ADVANCED.ToString()));
            RadioButtonListLevel.SelectedIndex = 0;
            for (int i = 0; i < compositeServiceData.Count; i++)
            {
                List<ConfigurationKeyValues> group = compositeServiceData[i].ConfigurationData;
                if (group.Count > 0)
                {
                    DropDownListGroupName.Items.Add(new ListItem(group[0].GroupName, group[0].GroupName));
                }
            }
        }

        private void getData()
        {
            TextBoxDescription.Text = thisKey.ConfigurationKeyDescription;
            TextBoxDataType.Text = thisKey.ConfigurationKeyDataType;
            TextBoxDisplayName.Text = thisKey.ConfigurationKeyDisplayName;
            TextBoxDisplayOrder.Text = thisKey.DisplayOrder.ToString();
            TextBoxSettingName.Text = thisKey.ConfigurationKeyFieldName;
            TextBoxValidValues.Text = thisKey.ConfigurationKeyValidValues;
            if (!thisKey.ConfigurationKeyDataType.Equals("password"))
                TextBoxValue.Text = thisKey.ConfigurationKeyValue;
            DropDownListGroupName.SelectedValue = thisKey.GroupName;
            switch (thisKey.ConfigurationKeyLevel)
            {
                case ConfigUtility.CONFIG_LEVEL_BASIC:
                    {
                        RadioButtonListLevel.SelectedIndex = 0;
                        break;
                    }
                case ConfigUtility.CONFIG_LEVEL_DETAILED:
                    {
                        RadioButtonListLevel.SelectedIndex = 1;
                        break;
                    }

                case ConfigUtility.CONFIG_LEVEL_ADVANCED:
                    {
                        RadioButtonListLevel.SelectedIndex = 2;
                        break;
                    }
            }
            switch (thisKey.ConfigurationKeyReadOnly)
            {
                case true:
                    {
                        RadioButtonReadOnly.SelectedIndex = 0;
                        break;
                    }

                case false:
                    {
                        RadioButtonReadOnly.SelectedIndex = 1;
                        break;
                    }
            }
        }

        private void processUpdate()
        {
            Page.Validate();
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            if (IsValid)
            {
                ConfigurationKeyValues oldKey = null;
                if (action != ConfigUtility.ADD_KEY)
                {
                    thisKey = (ConfigurationKeyValues)ViewState["thisKey"];
                    identify = thisKey.ConfigurationKeyFieldName;
                    oldKey = getKey(thisData, identify);
                }
                DisplayName = TextBoxDisplayName.Text;
                SettingName = TextBoxSettingName.Text;
                DataType = TextBoxDataType.Text;
                Description = TextBoxDescription.Text;
                ValidValues = TextBoxValidValues.Text;
                ReadOnly = RadioButtonReadOnly.SelectedValue;
                ConfigLevel = Convert.ToInt32(RadioButtonListLevel.SelectedValue);
                Value = TextBoxValue.Text;
                DisplayOrder = Convert.ToInt32(TextBoxDisplayOrder.Text);
                updatedKey = new ConfigurationKeyValues(DisplayName, SettingName, Value, DataType, Description, Convert.ToBoolean(ReadOnly), ValidValues, System.DateTime.Now, ConfigLevel, compositeServiceData[0].ConfigurationData[0].OriginatingConfigServiceName, DisplayOrder, null, compositeServiceData[0].ServiceHost);
                if (CheckBoxGroupName.Checked)
                    updatedKey.GroupName = DropDownListGroupName.Text;
                else
                    updatedKey.GroupName = TextBoxGroupName.Text.Trim();
                if (action == ConfigUtility.ADD_KEY)
                    oldKey = updatedKey;
                try
                {
                    traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                    success = configProxy.receiveConfigurationKey(hostNameIdentifier, configName, oldKey, updatedKey, true, action, traversePath, user);
                    if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                    {
                        UpdateMessage.Text = "<br/><span style=\"color:PaleGreen \">The key was sucessfully " + actiontext + ".</span>";
                        ViewState["thisKey"] = updatedKey;
                        identify = updatedKey.ConfigurationKeyFieldName;
                        ViewState["identify"] = updatedKey.ConfigurationKeyFieldName;
                        identify = updatedKey.ConfigurationKeyFieldName;
                        if (action.Equals(ConfigUtility.ADD_KEY))
                        {
                            Add.Enabled = false;
                            Update.Enabled = true;
                            Delete.Enabled = true;
                        }
                        if (action.Equals(ConfigUtility.REMOVE_KEY))
                        {
                            Add.Enabled = false;
                            Update.Enabled = false;
                            Delete.Enabled = false;
                        }
                        action = ConfigUtility.EDIT_KEY;
                        ViewState["action"] = action;
                        compositeServiceData = null;
                        ViewState["CompositeServiceData"] = null;
                        oldKey = updatedKey;
                    }
                    else
                    {
                        string message = null;
                        switch (success)
                        {
                            case ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED:
                                {
                                    message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_PERSISTED;
                                    break;
                                }

                            case ConfigUtility.CLUSTER_UPDATE_FAIL_VALIDATION:
                                {
                                    message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_VALIDATION;
                                    break;
                                }

                            case ConfigUtility.CLUSTER_UPDATE_FAIL_AUTHENTICATION:
                                {
                                    message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
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
                        UpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                        if (action != ConfigUtility.ADD_KEY)
                        {
                            ViewState["thisKey"] = oldKey;
                            updatedKey = oldKey;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("not found"))
                        UpdateMessage.Text = "<br/><span style=\"color:Maroon\">The requested key is not yet defined in your target app/service Settings class.  You must define a static public field as a variable name matching the Settings Class Field Name, recompile the target app/service includings new Settings class, then add the requested key via ConfigWeb. The exception is: " + e.ToString() + "</span>";
                    else
                        UpdateMessage.Text = "<br/><span style=\"color:Maroon\">An exception was encountered during the request. The exception was: " + e.ToString() + "</span>";
                    if (action != ConfigUtility.ADD_KEY)
                        ViewState["identify"] = oldKey.ConfigurationKeyFieldName;
                }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.REMOVE_KEY;
            actiontext = "deleted";
            processUpdate();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.EDIT_KEY;
            actiontext = "updated";
            processUpdate();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.ADD_KEY;
            actiontext = "added";
            processUpdate();
        }
        protected void CheckBoxGroupName_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxGroupName.Checked)
            {
                TextBoxGroupName.Visible = false;
                DropDownListGroupName.Visible = true;
            }
            else
            {
                TextBoxGroupName.Visible = true;
                TextBoxGroupName.Text = DropDownListGroupName.SelectedValue;
                DropDownListGroupName.Visible = false;
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