//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Form to modify/add Users from the Configuration Database.
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
    public partial class ManageUsers : System.Web.UI.Page
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
        string actiontext = "";
        string action = "";
        string identify;
        ServiceUsers thisUser;
        ServiceUsers oldUser;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<ServiceUsers> userList;

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = TextBoxUserName.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                identify = (string)ViewState["identify"];
                action = (string)ViewState["action"];
            }
            else
            {
                action = Request["action"];
                identify = Request["identify"];
                ViewState["action"] = action;
                ViewState["identify"] = identify;
            }
            if (!IsPostBack)
            {
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                ViewState["compositeServiceData"] = compositeServiceData;
                userList = configProxy.getServiceUsers(hostNameIdentifier, configName, traversePath, user);
                ViewState["userList"] = userList;
            }
            else
            {
                UpdateMessage.Text = "";
                oldUser = (ServiceUsers)ViewState["oldUser"];
                action = (string)ViewState["action"];
                userList = (List<ServiceUsers>)ViewState["userList"];
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["compositeServiceData"];
            }
            if (compositeServiceData == null)
            {
                Response.Redirect(ConfigSettings.PAGE_NODES,true);
            }
           
            if (action.Equals(ConfigUtility.ADD_USER))
            {
                Update.Enabled = false;
                Delete.Enabled = false;
            }
            else
            {
                Add.Enabled = false;
                thisUser = getUser(userList, identify);
                ViewState["thisUser"] = thisUser;
                if (!IsPostBack)
                    ViewState["oldUser"] = thisUser;
                if (thisUser == null)
                {
                    Response.Redirect(ConfigSettings.PAGE_NODES,true);
                }
            }
            if (!IsPostBack)
            {
                initRadioButtons();
                if (!action.Equals(ConfigUtility.ADD_USER))
                    getData();
            }
            if (action.Equals(ConfigUtility.REMOVE_USER))
            {
                RequiredFieldValidator1.Enabled = false;
                UpdateMessage.Text = "WARNING:  If you are removing a <em>Connected Service User</em> to a remote service , when you delete, you will also be removing <b>all</b> Connected Service Definitions to this host, as well as existing Connection Points for these Connected Service Definitions.";
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_USERS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to User List</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void initRadioButtons()
        {
            RadioButtonListLocalUser.Items.Add(new ListItem("true"));
            RadioButtonListLocalUser.Items.Add(new ListItem("false"));
            RadioButtonListLocalUser.SelectedIndex = 0;
            RadioButtonListRights.Items.Add(new ListItem(ConfigUtility.CONFIG_ADMIN_RIGHTS_STRING, ConfigUtility.CONFIG_ADMIN_RIGHTS.ToString()));
            RadioButtonListRights.Items.Add(new ListItem(ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS_STRING, ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS.ToString()));
            RadioButtonListRights.Items.Add(new ListItem(ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS_STRING, ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS.ToString()));
            RadioButtonListRights.Items.Add(new ListItem(ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS_STRING, ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS.ToString()));
            RadioButtonListRights.SelectedIndex = 1;
            RadioButtonListLocalUser.Enabled = true;
        }

        private void getData()
        {
            if (thisUser != null)
            {
                TextBoxUserName.Text = thisUser.UserId;
                switch (thisUser.Rights)
                {
                    case ConfigUtility.CONFIG_ADMIN_RIGHTS:
                        {
                            RadioButtonListRights.SelectedIndex = 0;
                            break;
                        }

                    case ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS:
                        {
                            RadioButtonListRights.SelectedIndex = 1;
                            break;
                        }


                    case ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS:
                        {
                            RadioButtonListRights.SelectedIndex = 2;
                            break;
                        }

                    case ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS:
                        {
                            RadioButtonListRights.SelectedIndex = 3;
                            break;
                        }
                }
                switch (thisUser.LocalUser)
                {
                    case true:
                        {
                            RadioButtonListLocalUser.SelectedIndex = 0;
                            RadioButtonListLocalUser.SelectedValue = "true";
                            break;
                        }

                    case false:
                        {
                            RadioButtonListLocalUser.SelectedIndex = 1;
                            RadioButtonListLocalUser.SelectedValue = "false";
                            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                            List<ConnectedConfigServices> connectedConfigServices = configProxy.getMyConnectedConfigServices(hostNameIdentifier, configName, traversePath, user);
                            ViewState["connectedConfigServices"] = connectedConfigServices;
                            ConnectedConfigServices thisUserConfigService = connectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisUser.ConnectedConfigServiceID; });
                            break;
                        }
                }
                TextBoxPassword.Attributes.Add("value", thisUser.Password);
                TextBoxPasswordConfirm.Attributes.Add("value", thisUser.Password);
            }
        }

        private ServiceUsers getUser(List<ServiceUsers> listToParse, string identity)
        {
            if (identity == null)
                return null;
            int key = Convert.ToInt32(identify);
            return listToParse.Find(delegate(ServiceUsers userExist) { return userExist.UserKey == key; });
        }

        private void processUpdate()
        {
            if (userList == null)
            {
                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION + "</span>";
                return;
            }

            if (action != ConfigUtility.REMOVE_USER)
                Page.Validate();
            else
                Page.Validate("User");
            ServiceUsers found = userList.Find(delegate(ServiceUsers userexist) { return userexist.UserId.ToLower().Equals(TextBoxUserName.Text.ToLower()); });
            if (found != null && action == ConfigUtility.ADD_USER)
            {
                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">User Id already exists!</span>";
                return;
            }
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            if (IsValid)
            {
                ServiceUsers updateUser = new ServiceUsers();
                updateUser.Rights = Convert.ToInt32(RadioButtonListRights.SelectedValue);
                updateUser.UserId = TextBoxUserName.Text;
                updateUser.Password = TextBoxPassword.Text;
                updateUser.UserKey = thisUser.UserKey;
                updateUser.ConnectedConfigServiceID = thisUser.ConnectedConfigServiceID;
                if (RadioButtonListLocalUser.SelectedIndex == 0)
                    updateUser.LocalUser = true;
                else
                    updateUser.LocalUser = false;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
                success = configProxy.receiveUser(hostNameIdentifier, configName, updateUser, oldUser, true, action, traversePath, user);
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    successPersisted(null, updateUser);
                }
                else
                {
                    string message = null;
                    switch (success)
                    {
                        case ConfigUtility.CLUSTER_UPDATE_FAIL_PERSISTED:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_PERSISTED;
                                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                                oldUser = (ServiceUsers)ViewState["oldUser"];
                                thisUser = oldUser;
                                ViewState["thisUser"] = oldUser;
                                break;
                            }

                        case ConfigUtility.CLUSTER_UPDATE_FAIL_VALIDATION:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_VALIDATION;
                                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                                oldUser = (ServiceUsers)ViewState["oldUser"];
                                thisUser = oldUser;
                                ViewState["thisUser"] = oldUser;
                                break;
                            }

                        case ConfigUtility.CLUSTER_UPDATE_FAIL_AUTHENTICATION:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_AUTHORIZATION;
                                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                                oldUser = (ServiceUsers)ViewState["oldUser"];
                                thisUser = oldUser;
                                ViewState["thisUser"] = oldUser;
                                break;
                            }

                        case ConfigUtility.CLUSTER_UPDATE_FAIL_REMOTE:
                            {
                                message = ConfigSettings.EXCEPTION_MESSAGE_FAIL_REMOTE_UPDATE;
                                break;
                            }

                        default:
                            {
                                successPersisted(ConfigSettings.EXCEPTION_MESSAGE_FAIL_REMOTE_PEER + success.ToString(), updateUser);
                                break;
                            }
                    }
                }
            }
        }

        private void successPersisted(string supplmentmessage, ServiceUsers updateUser)
        {
            if (supplmentmessage == null)
                supplmentmessage = "<span style=\"color:PaleGreen \">The user was sucessfully " + actiontext + ".</span>";
            else
                supplmentmessage = "<span style=\"color:Maroon\">" + supplmentmessage + ".</span>";
            SessionInfo info = new SessionInfo();
            string hostnameSession = null;
            string confignameSession = null;
            info.getHostNameConfigName(out hostnameSession, out confignameSession);
            if (oldUser != null && oldUser.UserKey == user.UserKey && (hostnameSession.Equals(hostNameIdentifier) && confignameSession.Equals(configName)))
            {
                string addressSes = null;
                string IDSes = null;
                ServiceUsers csUserSes = null;
                string confignameSes = null;
                string hosterSes = null;
                string versionSes = null;
                string platformSes = null;
                string clientSession = null;
                string nameSes = null;
                info.getSessionData(false, out addressSes, out csUserSes, out clientSession, out nameSes, out confignameSes, out hosterSes, out versionSes, out platformSes, out IDSes);
                HttpCookie cookie = info.setSessionData(addressSes, updateUser, clientSession, nameSes, confignameSes, hosterSes, versionSes, platformSes,IDSes);
                Response.Cookies.Remove(cookie.Name);
                Response.Cookies.Add(cookie);
                user = updateUser;
            }
            userList = configProxy.getServiceUsers(hostNameIdentifier, configName, traversePath, user);
            if (userList == null)
                Response.Redirect(ConfigSettings.PAGE_NODES, true);
            if (action.Equals(ConfigUtility.ADD_USER))
                thisUser = userList.Find(delegate(ServiceUsers userExist) { return userExist.UserId.ToLower().Equals(updateUser.UserId.ToLower()) && userExist.LocalUser == updateUser.LocalUser && userExist.Password.Equals(updateUser.Password); });
            else
                if (action.Equals(ConfigUtility.UPDATE_USER))
                    thisUser = userList.Find(delegate(ServiceUsers userExist) { return userExist.UserKey == thisUser.UserKey; });
            UpdateMessage.Text = supplmentmessage;
            if (action == ConfigUtility.REMOVE_USER && !oldUser.LocalUser)
                UpdateMessage.Text = UpdateMessage.Text + "<br>Note that all Connected Service Definitions and corresponding active Connection Point instances associated with the Connected Service user have also been removed from the configuration database.";
            ViewState["thisUser"] = thisUser;
            ViewState["oldUser"] = thisUser;
            ViewState["identify"] = thisUser.UserKey.ToString();
            if (action.Equals(ConfigUtility.ADD_USER))
            {
                Add.Enabled = false;
                Update.Enabled = true;
                Delete.Enabled = true;
            }
            if (action.Equals(ConfigUtility.REMOVE_USER))
            {
                Add.Enabled = false;
                Update.Enabled = false;
                Delete.Enabled = false;
            }
            ViewState["userList"] = userList;
            action = ConfigUtility.UPDATE_USER;
            ViewState["action"] = action;
            ViewState["CompositeServiceData"] = null;

        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            RequiredFieldValidator1.Enabled = false;
            action = ConfigUtility.REMOVE_USER;
            actiontext = "deleted";
            if (thisUser.UserId.Equals("localadmin"))
                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">You cannot delete the user account 'localadmin'. This is a mandatory Administrative account.</span>";
            else
                processUpdate();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            RequiredFieldValidator1.Enabled = true;
            action = ConfigUtility.UPDATE_USER;
            actiontext = "updated";
            processUpdate();
            
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            RequiredFieldValidator1.Enabled = true;
            action = ConfigUtility.ADD_USER;
            actiontext = "added";
            thisUser = new ServiceUsers();
            processUpdate();
        }

        protected void RadioButtonListRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (RadioButtonListRights.SelectedIndex)
            {
                case 1:
                    {
                        if (thisUser != null && thisUser.UserId.Equals("localadmin"))
                        {
                            UpdateMessage.Text = "<span style=\"color:Maroon\">The account 'localadmin' must always have Administrative Rights.</span>";
                            RadioButtonListRights.SelectedIndex = 0;
                        }
                        break;
                    }
                case 2:
                    {
                        goto case 1;
                    }

                case 3:
                    {
                        goto case 1;
                    }
            }
            TextBoxPassword.Attributes.Add("value", thisUser.Password);
            TextBoxPasswordConfirm.Attributes.Add("value", thisUser.Password);
        }

        protected void RadioButtonListLocalUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (RadioButtonListLocalUser.SelectedIndex)
            {
                case 1:
                    {
                        if (thisUser != null && thisUser.UserId.Equals("localadmin"))
                        {
                            UpdateMessage.Text = "<span style=\"color:Maroon\">The account 'localadmin' must always be a local user.</span>";
                            RadioButtonListLocalUser.SelectedIndex = 0;
                        }
                        break;
                    }
            }
            TextBoxPassword.Attributes.Add("value", thisUser.Password);
            TextBoxPasswordConfirm.Attributes.Add("value", thisUser.Password);
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
