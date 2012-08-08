//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to perform an update to an existing settings key value.
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
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.ServiceModel;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfigurationUtility;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class ConfigUpdate : System.Web.UI.Page
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
        string originalValue;
        string updatedValue;
        int level = 1;
        int index = 0;
        ConfigurationKeyValues thisKey;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<ConfigurationKeyValues> thisData;
      
        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = NewValue.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            string postback = "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster; 
            if (IsPostBack)
            {
                theTargetContract = (string)ViewState["contract"];
                level = (Int32)ViewState["level"];
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["CompositeServiceData"];
                traversePath = (List<TraverseNode>)ViewState["traversePath"];
            }
            else
            {
                theTargetContract = Request["contract"];
                ViewState["contract"] = theTargetContract;
                string levelString = Request["level"];
                if (!Int32.TryParse(levelString, out level))
                    level = ConfigUtility.CONFIG_LEVEL_BASIC;
                ViewState["level"] = level;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, level, true, traversePath, user);
                ViewState["CompositeServiceData"] = compositeServiceData;
                ViewState["traversePath"] = traversePath;
                
            }
            string keyName = null;
            thisData = findKeyValuesByContract(theTargetContract, 0);
            if (thisData == null)
            {
                Response.Redirect(ConfigSettings.PAGE_CONFIG + postback + "&contract=" + theTargetContract + "&level=1",true);
            }
            if (IsPostBack)
            {
                keyName = (string)ViewState["keyname"];
                thisKey = (ConfigurationKeyValues)ViewState["thisKey"];
                if (thisKey.ConfigurationKeyDataType.ToLower().Equals("radiobutton"))
                {
                    NewValue.Text = RadioButtonListValidValues.SelectedValue;
                }
                if (thisKey.ConfigurationKeyDataType.ToLower().Equals("password") && NewValue.Text.Equals("NOT VIEWABLE"))
                    updatedValue = thisKey.ConfigurationKeyValue;
                else
                    updatedValue = NewValue.Text;
            }
            else
            {
                keyName = Request["keyname"];
                ViewState["keyname"] = keyName;
                thisKey = getKey(thisData, keyName);
                ViewState["thisKey"] = thisKey;
            }
            if (thisKey == null)
            {
                Response.Redirect(ConfigSettings.PAGE_CONFIG + postback + "&contract=" + theTargetContract + "&level=1",true);
            }
            ViewState["keyName"] = thisKey.ConfigurationKeyFieldName;
            KeyName.Text =thisKey.ConfigurationKeyDisplayName;
            if (thisKey.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "nodisplay" && thisKey.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "password")
                Value.Text =  ChunkText.chunkDot(thisKey.ConfigurationKeyValue, '.');
            else
                Value.Text = "NOT VIEWABLE";
            if (!Value.Text.Contains(" "))
            {
                if (Value.Text.Length > 32)
                    Value.Text = ChunkText.chunkDot(Value.Text, '_');
                if (Value.Text.Length > 32)
                    Value.Text = ChunkText.chunk(32, Value.Text);
            }
            string[] splitIt = Value.Text.Split(' ');
            for (int i = 0; i < splitIt.Length; i++)
            {
                if (splitIt[i].Length > 40)
                    Value.Text = ChunkText.chunk(40, thisKey.ConfigurationKeyValue);
            }
            Description.Text = thisKey.ConfigurationKeyDescription;
            string[] format = thisKey.ConfigurationKeyValidValues.Split(new char[]{' '});
            bool needChunk = false;
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i].Length > 48)
                    needChunk = true;
            }
            if (needChunk)
                ValidValues.Text = ChunkText.chunk(48,thisKey.ConfigurationKeyValidValues);
            else
                ValidValues.Text = thisKey.ConfigurationKeyValidValues;
            ValidValues.Text =  thisKey.ConfigurationKeyValidValues;
            LastUpdated.Text = thisKey.ConfigurationKeyLastUpdate.ToString();
            originalValue = thisKey.ConfigurationKeyValue;
            string keyreadonly = thisKey.ConfigurationKeyReadOnly.ToString();
            if (keyreadonly.ToLower() == "false")
            {
                NewValue.Enabled = true;
            }
          
            if (thisKey.ConfigurationKeyDataType.ToLower().Equals("password"))
                NewValue.Text = "NOT VIEWABLE";
            else
                NewValue.Text = thisKey.ConfigurationKeyValue;
            if (thisKey.ConfigurationKeyDataType.ToLower().Equals("radiobutton"))
            {
                NewValue.Visible = false;
                RadioButtonListValidValues.Visible = true;
                if (!IsPostBack)
                    initradiobutton();
            }
            else
            {
                NewValue.Visible = true;
                RadioButtonListValidValues.Visible = false;
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONFIG + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&contract=" + theTargetContract + "&version=" + version + "&level=" + level.ToString() + "&platform=" + platform + "&hoster=" + hoster  + "\">Return to Configuration Page</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
            if (IsPostBack)
                processUpdate(thisKey);
        }

        private void initradiobutton()
        {
            string[] values = thisKey.ConfigurationKeyValidValues.Split(';');
            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i].TrimEnd(' ');
                string displayvalue = value.TrimStart(' ');
                RadioButtonListValidValues.Items.Add(new ListItem(displayvalue, displayvalue));
                if (thisKey.ConfigurationKeyValue.Equals(displayvalue))
                    RadioButtonListValidValues.SelectedIndex = i;
            }
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

        private void processUpdate(ConfigurationKeyValues keyToUpdate)
        {
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            ConfigurationKeyValues oldKey = getKey(thisData, keyToUpdate.ConfigurationKeyFieldName);
            if (oldKey.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "nodisplay" && oldKey.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "password")
                Value.Text = updatedValue;
            else
                Value.Text = "NOT VIEWABLE";
            keyToUpdate.ConfigurationKeyValue = updatedValue;
            if (oldKey.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) == "password")
                NewValue.Text = "NOT VIEWABLE";
            else
                NewValue.Text = keyToUpdate.ConfigurationKeyValue;
            try
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                success = configProxy.receiveConfigurationKey(hostNameIdentifier, configName, oldKey, keyToUpdate, true, ConfigUtility.UPDATE_KEY_VALUE, traversePath, user);
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    UpdateMessage.Text = "<br/><span style=\"color:PaleGreen \">The configuration key was sucessfully updated.</span>";
                    if (keyToUpdate.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "nodisplay" && keyToUpdate.ConfigurationKeyDataType.ToLower().Trim(new char[] { ' ' }) != "password")
                        Value.Text = ChunkText.chunkDot(keyToUpdate.ConfigurationKeyValue, '.');
                    else
                        Value.Text = "NOT VIEWABLE";
                    if (!Value.Text.Contains(" "))
                    {
                        if (Value.Text.Length > 32)
                            Value.Text = ChunkText.chunkDot(Value.Text, '_');
                        if (Value.Text.Length > 32)
                            Value.Text = ChunkText.chunk(32, Value.Text);
                    }
                    string[] splitIt = Value.Text.Split(' ');
                    for (int i = 0; i < splitIt.Length; i++)
                    {
                        if (splitIt[i].Length > 40)
                            Value.Text = ChunkText.chunk(40, thisKey.ConfigurationKeyValue);
                    }
                    ViewState["thisKey"] = keyToUpdate;
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
                    keyToUpdate.ConfigurationKeyValue = originalValue;
                }
            }
            catch (Exception e)
            {
                keyToUpdate.ConfigurationKeyValue = originalValue;
                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">An exception was encountered while updating the configuration key. The exception was: " + e.ToString() + "</span>";
            }
        }

        protected void Update_Click(object sender, EventArgs e)
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

