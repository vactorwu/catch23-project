//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Form and logic to modify/add new Virtual Host Definitions to the Configuration Database.
//============================================================================================

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
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class VHostUpdate : System.Web.UI.Page
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
        int SHID;
        ServiceConfigurationClient configProxy;
        string actiontext = "";
        string action = "";
        MasterServiceHostInstance thisServiceHost;
        MasterServiceHostInstance oldServiceHost;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<MasterServiceHostInstance> serviceHostList;
        List<ServiceHostInfo> startupServiceHostList;

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = RadioButtonListServiceType.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                SHID = (int)ViewState["SHID"];
            }
            else
            {
                try
                {
                    SHID = Convert.ToInt32(Request["SHID"]);
                    ViewState["SHID"] = SHID;
                }
                catch
                {
                    Response.Redirect(ConfigSettings.PAGE_NODES,true);
                }
            }
            Add.PostBackUrl = ConfigSettings.PAGE_VHOST_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.ADD_VHOST + "&SHID=" + SHID;
            Update.PostBackUrl = ConfigSettings.PAGE_VHOST_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.UPDATE_VHOST + "&SHID=" + SHID;
            Delete.PostBackUrl = ConfigSettings.PAGE_VHOST_UPDATE + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&action=" + ConfigUtility.REMOVE_VHOST + "&SHID=" + SHID;
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            action = Request["action"];
           
            if (!IsPostBack)
            {
                if (action == null)
                    action = Request["action"];
                ViewState["action"] = action;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                ViewState["compositeServiceData"] = compositeServiceData;
                serviceHostList = configProxy.getVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
                startupServiceHostList = configProxy.getStartupVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
                ViewState["serviceHostList"] = serviceHostList;
                RadioButtonListServiceType.SelectedIndex = 0;
            }
            else
            {
                UpdateMessage.Text = "";
                oldServiceHost = (MasterServiceHostInstance)ViewState["oldServiceHost"];
                action = Request["action"];
                serviceHostList = (List<MasterServiceHostInstance>)ViewState["serviceHostList"];
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["compositeServiceData"];
            }
            if (action.Equals(ConfigUtility.ADD_VHOST))
                RadioButtonListServiceType.Enabled = true;
            if (compositeServiceData == null)
            {
                Response.Redirect(ConfigSettings.PAGE_VHOSTS,true);
            }
            if (action.Equals(ConfigUtility.ADD_VHOST))
            {
                Update.Enabled = false;
                Delete.Enabled = false;
            }
            else
            {
                Add.Enabled = false;
                thisServiceHost = serviceHostList.Find(delegate(MasterServiceHostInstance shExist) { return shExist.ServiceHostID == SHID; });
                ViewState["thisServiceHost"] = thisServiceHost;
                if (!IsPostBack)
                    ViewState["oldServiceHost"] = thisServiceHost;
                if (thisServiceHost == null)
                {
                    home(false);
                }
            }
            if (!IsPostBack)
            {
                if (action == ConfigUtility.ADD_VHOST)
                    initRadioButtons(-1);
                else
                    initRadioButtons(thisServiceHost.ServiceType);
                if (action.Equals(ConfigUtility.ADD_VHOST))
                {
                    RadioButtonListServiceType.SelectedIndex = 0;
                }
                DropDownListConfig.Items.Add(new ListItem(ConfigUtility.NO_SERVICE_BEHAVIOR_CONFIGURATION, ConfigUtility.NO_SERVICE_BEHAVIOR_CONFIGURATION));
                for (int i = 0; i < compositeServiceData[0].ServiceBehaviorNames.Count; i++)
                {
                    DropDownListConfig.Items.Add(new ListItem(compositeServiceData[0].ServiceBehaviorNames[i], compositeServiceData[0].ServiceBehaviorNames[i]));
                }
                if (!action.Equals(ConfigUtility.ADD_VHOST))
                    getData();
                else
                    getDataAdd(ConfigUtility.HOST_TYPE_PRIMARY);
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_VHOSTS + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Virtual Host List</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void initRadioButtons(int serviceType)
        {
            switch (serviceType)
            {
                case -1:
                    {
                        RadioButtonListServiceType.Items.Add(new ListItem("Primary Business Service", ConfigUtility.HOST_TYPE_PRIMARY.ToString()));
                        RadioButtonListServiceType.Items.Add(new ListItem("Node Custom Cache Service", ConfigUtility.HOST_TYPE_NODE_DC.ToString()));
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE:
                    {
                        RadioButtonListServiceType.Items.Add(new ListItem("Node Service", ConfigUtility.HOST_TYPE_NODE.ToString()));
                        break;
                    }
                case ConfigUtility.HOST_TYPE_CONFIG:
                    {
                        RadioButtonListServiceType.Items.Add(new ListItem("Configuration Service", ConfigUtility.HOST_TYPE_CONFIG.ToString()));
                        break;
                    }

                case ConfigUtility.HOST_TYPE_PRIMARY:
                    {
                        goto case -1;
                    }
                case ConfigUtility.HOST_TYPE_NODE_DC:
                    {
                        goto case -1;
                    }
            }
            RadioButtonListHostEnvironment.SelectedIndex = 0;
            RadioButtonListHostEnvironment.Items.Add(new ListItem(ConfigUtility.HOST_IIS_STRING, ConfigUtility.HOSTING_ENVIRONMENT_IIS.ToString()));
            RadioButtonListHostEnvironment.Items.Add(new ListItem(ConfigUtility.HOST_NTSERVICE_STRING, ConfigUtility.HOSTING_ENVIRONMENT_WINSERVICE.ToString()));
            RadioButtonListHostEnvironment.Items.Add(new ListItem(ConfigUtility.HOST_WINDOWS_STRING, ConfigUtility.HOSTING_ENVIRONMENT_WINDOWS.ToString()));
            RadioButtonListHostEnvironment.Items.Add(new ListItem(ConfigUtility.HOST_CONSOLE_STRING, ConfigUtility.HOSTING_ENVIRONMENT_CONSOLE.ToString()));
            RadioButtonListHostEnvironment.SelectedIndex = 0;
        }

        private void getDataAdd(int serviceType)
        {
            switch (serviceType)
            {
                case ConfigUtility.HOST_TYPE_PRIMARY:
                    {
                        RadioButtonListServiceType.SelectedIndex = 0;
                        if (startupServiceHostList == null)
                            startupServiceHostList = new List<ServiceHostInfo>();

                        foreach (ServiceHostInfo primaryHostInfo in startupServiceHostList)
                        {
                            DropDownListClass.Items.Add(new ListItem(primaryHostInfo.HostName, primaryHostInfo.HostName));
                        }
                        if (startupServiceHostList.Count == 0)
                        {
                            ServiceClassNameLabel.Text = "You must supply service implementation names in your startup logic before you can select them here and add a virtual service host.";
                            ServiceClassNameLabel.ForeColor = System.Drawing.Color.DarkRed;
                        }
                        else
                        {
                            ServiceClassNameLabel.Text = "Please select one of these available implementation classes that you provided in your startup logic.";
                            ServiceClassNameLabel.ForeColor = System.Drawing.Color.PaleGreen;
                        }
                        DropDownListClass.SelectedIndex = 0;
                        RadioButtonListServiceType.Enabled = true;
                        Update.Enabled = false;
                        Delete.Enabled = false;
                        Add.Enabled = true;
                        CheckBoxWorkFlow.Enabled = true;
                        break;
                    }

                case ConfigUtility.HOST_TYPE_CONFIG:
                    {
                        RadioButtonListServiceType.SelectedIndex = 0;
                        MasterServiceHostInstance ConfigHostInfo = serviceHostList.Find(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceType.Equals(ConfigUtility.HOST_TYPE_CONFIG); });
                        DropDownListClass.Items.Add(new ListItem(ConfigHostInfo.ServiceImplementationClass, ConfigHostInfo.ServiceImplementationClass));
                        DropDownListClass.SelectedIndex = 0;
                        Delete.Enabled = false;
                        Add.Enabled = true;
                        Update.Enabled = false;
                        CheckBoxWorkFlow.Enabled = false;
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE:
                    {

                        RadioButtonListServiceType.SelectedIndex = 0;
                        MasterServiceHostInstance NodeHostInfo = serviceHostList.Find(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceType.Equals(ConfigUtility.HOST_TYPE_NODE); });
                        DropDownListClass.Items.Add(new ListItem(NodeHostInfo.ServiceImplementationClass, NodeHostInfo.ServiceImplementationClass));
                        DropDownListClass.SelectedIndex = 0;
                        RadioButtonListServiceType.SelectedIndex = 0;
                        Delete.Enabled = false;
                        Add.Enabled = false;
                        Update.Enabled = false;
                        CheckBoxWorkFlow.Enabled = false;
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE_DC:
                    {
                        RadioButtonListServiceType.SelectedIndex = 0;
                        foreach (ServiceHostInfo nodeDcInfo in startupServiceHostList)
                        {
                            DropDownListClass.Items.Add(new ListItem(nodeDcInfo.HostName, nodeDcInfo.HostName));
                        }
                        if (startupServiceHostList.Count == 0)
                        {
                            ServiceClassNameLabel.Text = "You must supply service implementation names in your startup logic before you can select them here and add a virtual service host.";
                            ServiceClassNameLabel.ForeColor = System.Drawing.Color.DarkRed;
                        }
                        else
                        {
                            ServiceClassNameLabel.Text = "Please select one of these available implementation classes that you provided in your startup logic.";
                            ServiceClassNameLabel.ForeColor = System.Drawing.Color.PaleGreen;
                        }
                        DropDownListClass.SelectedIndex = 0;
                        RadioButtonListServiceType.SelectedIndex = 1;
                        RadioButtonListServiceType.Enabled = true;
                        Update.Enabled = false;
                        Delete.Enabled = false;
                        Add.Enabled = true;
                        CheckBoxWorkFlow.Enabled = false;
                        break;
                    }
            }
        }

        private void getData()
        {
            if (thisServiceHost != null)
            {
                switch (thisServiceHost.ServiceType)
                {
                    case ConfigUtility.HOST_TYPE_PRIMARY:
                        {
                            RadioButtonListServiceType.SelectedIndex = 0;
                            List<MasterServiceHostInstance> PrimaryHostInfo = serviceHostList.FindAll(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceType.Equals(ConfigUtility.HOST_TYPE_PRIMARY); });
                            foreach (MasterServiceHostInstance primaryHostInfo in PrimaryHostInfo)
                            {
                                DropDownListClass.Items.Add(new ListItem(primaryHostInfo.ServiceImplementationClass, primaryHostInfo.ServiceImplementationClass));
                            }
                            foreach (ServiceHostInfo primaryHostInfo in startupServiceHostList)
                            {
                                if (DropDownListClass.Items.FindByText(primaryHostInfo.HostName) == null)
                                    DropDownListClass.Items.Add(new ListItem(primaryHostInfo.HostName, primaryHostInfo.HostName));
                            }
                            DropDownListClass.SelectedValue = thisServiceHost.ServiceImplementationClass;
                            if (!thisServiceHost.ServiceImplementationClass.Equals(DropDownListClass.SelectedValue))
                            {
                                //Handles case where user has changed implementation class but not yet updated repository with ne wvalue
                                DropDownListClass.Items.Add(new ListItem(thisServiceHost.ServiceImplementationClass, thisServiceHost.ServiceImplementationClass));
                                DropDownListClass.SelectedValue = thisServiceHost.ServiceImplementationClass;
                            }
                            RadioButtonListServiceType.Enabled = true;
                            Update.Enabled = true;
                            Delete.Enabled = true;
                            Add.Enabled = false;
                            CheckBoxWorkFlow.Enabled = true;
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_CONFIG:
                        {
                            RadioButtonListServiceType.SelectedIndex = 0;
                            MasterServiceHostInstance ConfigHostInfo = serviceHostList.Find(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceType.Equals(ConfigUtility.HOST_TYPE_CONFIG); });
                            DropDownListClass.Items.Add(new ListItem(ConfigHostInfo.ServiceImplementationClass, ConfigHostInfo.ServiceImplementationClass));
                            DropDownListClass.SelectedIndex = 0;
                            Delete.Enabled = false;
                            Add.Enabled = false;
                            Update.Enabled = true;
                            CheckBoxWorkFlow.Enabled = false;
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_NODE:
                        {
                            RadioButtonListServiceType.SelectedIndex = 0;
                            MasterServiceHostInstance NodeHostInfo = serviceHostList.Find(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceType.Equals(ConfigUtility.HOST_TYPE_NODE); });
                            DropDownListClass.Items.Add(new ListItem(NodeHostInfo.ServiceImplementationClass, NodeHostInfo.ServiceImplementationClass));
                            DropDownListClass.SelectedIndex = 0;
                            RadioButtonListServiceType.SelectedIndex = 0;
                            Delete.Enabled = false;
                            Add.Enabled = false;
                            Update.Enabled = true;
                            CheckBoxWorkFlow.Enabled = false;
                            break;
                        }

                    case ConfigUtility.HOST_TYPE_NODE_DC:
                        {
                            RadioButtonListServiceType.SelectedIndex = 0;
                            List<MasterServiceHostInstance> NodeDCHostInfo = serviceHostList.FindAll(delegate(MasterServiceHostInstance SHInfoExist) { return SHInfoExist.ServiceHostID == SHID; });
                            foreach (MasterServiceHostInstance nodeDcInfo in NodeDCHostInfo)
                            {
                                DropDownListClass.Items.Add(new ListItem(nodeDcInfo.ServiceImplementationClass, nodeDcInfo.ServiceImplementationClass));
                            }
                            foreach (ServiceHostInfo primaryHostInfo in startupServiceHostList)
                            {
                                if (DropDownListClass.Items.FindByText(primaryHostInfo.HostName) == null)
                                    DropDownListClass.Items.Add(new ListItem(primaryHostInfo.HostName, primaryHostInfo.HostName));
                            }
                            RadioButtonListServiceType.SelectedIndex = 1;
                            RadioButtonListServiceType.Enabled = true;
                            DropDownListClass.SelectedValue = thisServiceHost.ServiceImplementationClass;
                            Update.Enabled = true;
                            Delete.Enabled = true;
                            Add.Enabled = false;
                            CheckBoxWorkFlow.Enabled = false;
                            break;
                        }
                }
                switch (thisServiceHost.HostEnvironment)
                {
                    case ConfigUtility.HOSTING_ENVIRONMENT_IIS:
                        {
                            RadioButtonListHostEnvironment.SelectedIndex = 0;
                            break;
                        }

                    case ConfigUtility.HOSTING_ENVIRONMENT_WINSERVICE:
                        {
                            RadioButtonListHostEnvironment.SelectedIndex = 1;
                            break;
                        }

                    case ConfigUtility.HOSTING_ENVIRONMENT_WINDOWS:
                        {
                            RadioButtonListHostEnvironment.SelectedIndex = 2;
                            break;
                        }

                    case ConfigUtility.HOSTING_ENVIRONMENT_CONSOLE:
                        {
                            RadioButtonListHostEnvironment.SelectedIndex = 3;
                            break;
                        }
                }
                try
                {
                    DropDownListConfig.SelectedValue = thisServiceHost.ServiceBehaviorConfiguration;
                }
                catch
                {
                    DropDownListConfig.SelectedValue = ConfigUtility.NO_SERVICE_BEHAVIOR_CONFIGURATION;
                }
                TextBoxHttpHeader.Text = thisServiceHost.HttpHostHeaderName;
                TextBoxTcpHeader.Text = thisServiceHost.TcpHostHeaderName;
                if (thisServiceHost.IsWorkFlowServiceHost)
                    CheckBoxWorkFlow.Checked = true;
                else
                    CheckBoxWorkFlow.Checked = false;
                if (thisServiceHost.HttpHostHeaderName == ConfigUtility.NO_HOST_HEADER_NAME)
                {
                    CheckBoxHttpHeader.Checked = false;
                    TextBoxHttpHeader.Enabled = false;
                }
                else
                {
                    CheckBoxHttpHeader.Checked = true;
                    TextBoxHttpHeader.Enabled = true;
                }
                if (thisServiceHost.TcpHostHeaderName == ConfigUtility.NO_HOST_HEADER_NAME)
                {
                    CheckBoxTcpHeader.Checked = false;
                    TextBoxTcpHeader.Enabled = false;
                }
                else
                {
                    CheckBoxTcpHeader.Checked = true;
                    TextBoxTcpHeader.Enabled = true;
                }
            }
        }

        private void processUpdate()
        {
            Page.Validate();
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            if (IsValid)
            {
                oldServiceHost = (MasterServiceHostInstance)ViewState["oldServiceHost"];
                MasterServiceHostInstance updateServiceHost = new MasterServiceHostInstance();
                updateServiceHost.ServiceType = Convert.ToInt32(RadioButtonListServiceType.SelectedValue);
                updateServiceHost.HostEnvironment = Convert.ToInt32(RadioButtonListHostEnvironment.SelectedValue);
                updateServiceHost.HostName = DropDownListClass.SelectedValue;
                updateServiceHost.ServiceBehaviorConfiguration = DropDownListConfig.SelectedValue;
                if (CheckBoxWorkFlow.Checked)
                    updateServiceHost.IsWorkFlowServiceHost = true;
                else
                    updateServiceHost.IsWorkFlowServiceHost = false;
                updateServiceHost.ServiceImplementationClass = DropDownListClass.SelectedValue;
                if (CheckBoxHttpHeader.Checked)
                    updateServiceHost.HttpHostHeaderName = TextBoxHttpHeader.Text;
                else
                    updateServiceHost.HttpHostHeaderName = ConfigUtility.NO_HOST_HEADER_NAME;
                if (CheckBoxTcpHeader.Checked)
                    updateServiceHost.TcpHostHeaderName = TextBoxTcpHeader.Text;
                else
                    updateServiceHost.TcpHostHeaderName = ConfigUtility.NO_HOST_HEADER_NAME;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                success = configProxy.receiveService(hostNameIdentifier, configName, null, null, null, null, null, oldServiceHost, updateServiceHost, true, action, traversePath, user);
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    thisServiceHost = updateServiceHost;
                    UpdateMessage.Text = "<br/><span style=\"color:PaleGreen \">The Virtual Host definition was sucessfully " + actiontext + ".</span>";
                    if (action == ConfigUtility.REMOVE_VHOST)
                        UpdateMessage.Text = UpdateMessage.Text + "<br>Note that all Service Definitions for this Virtual Host have also been removed from the configuration database.";
                    ViewState["thisServiceHost"] = thisServiceHost;
                    ViewState["oldServiceHost"] = thisServiceHost;
                    ViewState["SHID"] = thisServiceHost.ServiceHostID;
                    if (action.Equals(ConfigUtility.ADD_VHOST))
                    {
                        Add.Enabled = false;
                        Update.Enabled = true;
                        Delete.Enabled = true;
                    }
                    if (action.Equals(ConfigUtility.REMOVE_VHOST))
                    {
                        Add.Enabled = false;
                        Update.Enabled = false;
                        Delete.Enabled = false;
                    }
                    serviceHostList = configProxy.getVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
                    if (action != ConfigUtility.REMOVE_VHOST)
                    {
                        thisServiceHost = serviceHostList.Find(delegate(MasterServiceHostInstance sh) { return sh.HostName.Equals(thisServiceHost.HostName); });
                        ViewState["thisServiceHost"] = thisServiceHost;
                        ViewState["oldServiceHost"] = thisServiceHost;
                    }
                    ViewState["serviceHostList"] = serviceHostList;
                    action = ConfigUtility.UPDATE_VHOST;
                    ViewState["action"] = action;
                    ViewState["CompositeServiceData"] = null;
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
                    oldServiceHost = (MasterServiceHostInstance)ViewState["oldServiceHost"];
                    thisServiceHost = oldServiceHost;
                    ViewState["thisServiceHost"] = oldServiceHost;
                }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.REMOVE_VHOST;
            actiontext = "deleted";
            processUpdate();
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.UPDATE_VHOST;
            actiontext = "updated";
            processUpdate();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            action = ConfigUtility.ADD_VHOST;
            actiontext = "added";
            thisServiceHost = new MasterServiceHostInstance();
            processUpdate();
        }

        protected void CheckBoxHttpHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxHttpHeader.Checked)
            {
                TextBoxHttpHeader.Enabled = true;
                TextBoxHttpHeader.Text = null;
            }
            else
            {
                TextBoxHttpHeader.Text = ConfigUtility.NO_HOST_HEADER_NAME;
                TextBoxHttpHeader.Enabled = false;
            }

        }
        protected void CheckBoxTcpHeader_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxTcpHeader.Checked)
            {
                TextBoxTcpHeader.Enabled = true;
                TextBoxTcpHeader.Text = null;
            }
            else
            {
                TextBoxTcpHeader.Text = ConfigUtility.NO_HOST_HEADER_NAME;
                TextBoxTcpHeader.Enabled = false;
            }
        }

        protected void TopNode_Click(object sender, EventArgs e)
        {
            Response.Redirect(ConfigSettings.PAGE_NODES);
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