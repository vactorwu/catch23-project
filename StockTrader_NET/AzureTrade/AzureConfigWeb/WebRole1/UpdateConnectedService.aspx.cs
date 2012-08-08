//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Form and logic to modify/add new Connected Service Definitions to the Configuration Database.
//============================================================================================

//====================Update History======================================================================================
// 10/17/2008:  V 2.0.2.1 -- Added BindingType checks for WEB_HTTP_BINDING; WS_DUAL_HTTPBINDING; WS_FEDERATED_HTTP_BINDING
//========================================================================================================================
using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class UpdateConnectedService : System.Web.UI.Page
    {
        bool processing = false;
        string hostNameIdentifier;
        string configName;
        string platform;
        string version;
        string hoster;
        string address;
        string binding;
        string userid;
        string csusername;
        string cspassword;
        ServiceUsers user;
        ServiceConfigurationClient configProxy;
        string actiontext;
        string action;
        int CSID = -1;
        ConnectedServices thisService;
        ConnectedServices thisOldService;
        ConnectedConfigServices thisServiceConfig;
        HostedServices hsSelected;
        List<ConnectedConfigServices> configServices;
        List<ServiceUsers> thisServiceUsers;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<BindingInformation> bindings;
        List<ClientInformation> clients;
        List<string> clientContracts;
        string generateClicked = "false";

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = RadioButtonListServiceType.ClientID;
            LabelIntroService.Text = "";
            LabelIntroService.ForeColor = System.Drawing.Color.Black;
            UpdateMessage.Text = "";
            UpdateMessage.ForeColor = System.Drawing.Color.Black;
            GenericUpdateMessage.Text = "";
            GenericUpdateMessage.ForeColor = System.Drawing.Color.Black;
            LabelGetServices.Text = "";
            LabelGetServices.ForeColor = System.Drawing.Color.Black;
            DropDownListServiceName.SelectedIndexChanged += new EventHandler(DropDownListServiceName_SelectedIndexChanged);
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (IsPostBack)
            {
                ViewState["processing"] = false;
                UpdateMessage.Text = "";
                generateClicked = (string)ViewState["generateClicked"];
                action = (string)ViewState["action"];
                thisService = (ConnectedServices)ViewState["thisService"];
                thisServiceConfig = (ConnectedConfigServices)ViewState["thisConfigService"];
                hostNameIdentifier = (string)ViewState["name"];
                configName = (string)ViewState["configname"];
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["CompositeServiceData"];
                bindings = compositeServiceData[0].BindingInformation;
                clients = compositeServiceData[0].ClientInformation;
                thisServiceUsers = (List<ServiceUsers>)ViewState["thisServiceUsers"];
                if (!action.Equals(ConfigUtility.ADD_CONNECTED_SERVICE))
                    CSID = (int)ViewState["ID"];
                if (generateClicked.Equals("true"))
                    ConnectedPanel.Visible = true;
            }
            else
            {
                DropDownListCsUser.Enabled = false;
                DropDownListGenericCredentialUsers.Enabled = false;
                RadioButtonListServiceType.Items.Add(new ListItem("Primary Connected Service", ConfigUtility.HOST_TYPE_CONNECTED_SERVICE.ToString()));
                RadioButtonListServiceType.Items.Add(new ListItem("Generic Connected Service", ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE.ToString()));
                action = Request["action"];
                ViewState["action"] = action;
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                thisServiceUsers = configProxy.getServiceUsers(hostNameIdentifier, configName, traversePath, user);
                ViewState["CompositeServiceData"] = compositeServiceData;
                ViewState["thisServiceUsers"] = thisServiceUsers;
                bindings = compositeServiceData[0].BindingInformation;
                clients = compositeServiceData[0].ClientInformation;
                clientContracts = compositeServiceData[0].ClientContracts;
                if (thisServiceUsers == null)
                    Response.Redirect(ConfigSettings.PAGE_NODES, true);
                for (int i = 0; i < thisServiceUsers.Count; i++)
                {
                    if (thisServiceUsers[i].Rights == ConfigUtility.CONFIG_SERVICE_OPERATION_RIGHTS)
                    {
                        DropDownListCsUser.Items.Add(new ListItem(thisServiceUsers[i].UserId, thisServiceUsers[i].UserKey.ToString()));
                        DropDownListGenericCredentialUsers.Items.Add(new ListItem(thisServiceUsers[i].UserId, thisServiceUsers[i].UserKey.ToString()));
                    }
                }
                for (int i = 0; i < clientContracts.Count; i++)
                {
                    DropDownListGenericContract.Items.Add(new ListItem(clientContracts[i], clientContracts[i]));
                    DropDownListPrimaryContract.Items.Add(new ListItem(clientContracts[i], clientContracts[i]));
                }
                if (!action.Equals(ConfigUtility.ADD_CONNECTED_SERVICE))
                {
                    buildBindingLists();
                    try
                    {
                        CSID = Convert.ToInt32(Request["ID"]);
                    }
                    catch
                    {
                        Response.Redirect(ConfigSettings.PAGE_CONNECTIONS,true);
                    }
                    ViewState["ID"] = CSID;
                    getData();
                    RadioButtonListServiceType.Enabled = false;
                }
                else
                {
                    buildBindingLists();
                    RadioButtonListServiceType.SelectedIndex = 0;
                    PanelConfig.Visible = true;
                    GenericPanel.Visible = false;
                    ConnectedPanel.Visible = false;
                    ViewState["generateClicked"] = generateClicked;
                }
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            TopNodeName.Text = compositeServiceData[0].ServiceHost;
            ServiceVersion.Text = compositeServiceData[0].ServiceVersion;
            ServiceHoster.Text = "" + compositeServiceData[0].ServiceHoster;
            ServicePlatform.Text = "" + compositeServiceData[0].RunTimePlatform;
            if (action.Equals(ConfigUtility.ADD_CONNECTED_SERVICE))
            {
                Update.Enabled = false;
                Delete.Enabled = false;
                ButtonUpdateGeneric.Enabled = false;
                ButtonDeleteGeneric.Enabled = false;

            }
            else
            {
                Add.Enabled = false;
                ButtonAddGeneric.Enabled = false;
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_CONNECTED_SERVICES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "\">Return to Connected Services List</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
        }

        private void buildBindingLists()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                BindingInformation theBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                if (clients[i].ElementName.ToLower().Contains("client_configsvc") && (theBinding.BindingType.Equals(ConfigUtility.BASIC_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_2007_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.NET_TCP_BINDING)))
                    DropDownListConfigClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                if (clients[i].ElementName.ToLower().StartsWith("client_") && !clients[i].ElementName.ToLower().StartsWith("client_configsvc") && !clients[i].ElementName.ToLower().StartsWith("client_nodesvc"))
                    DropDownListGenericClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
            }
            DropDownListConfigClient.SelectedIndex = 0;
            if (DropDownListConfigClient.Items.Count > 0)
            {
                DropDownListConfigClient.SelectedValue = DropDownListConfigClient.Items[0].Text;
                ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListConfigClient.SelectedValue); });
                BindingInformation startBinding = bindings.Find(delegate(BindingInformation bexist) { return bexist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
                if (startBinding != null)
                    BindingTypeAutoGenerate.Text = startBinding.BindingType;
            }
            DropDownListGenericClients.SelectedIndex = 0;
            if (DropDownListGenericClients.Items.Count > 0)
            {
                DropDownListGenericClients.SelectedValue = DropDownListGenericClients.Items[0].Text;
                ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListConfigClient.SelectedValue); });
                BindingInformation startBinding = bindings.Find(delegate(BindingInformation bexist) { return bexist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
                if (startBinding != null)
                    LabelGenericBindingType.Text = startBinding.BindingType;
            }
            for (int i = 0; i < clients.Count; i++)
            {
                if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                {
                    BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                    if (theClientBinding != null)
                    {
                        bool match = true;
                        if (match && !clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                        {
                            DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                            DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                        }
                    }
                }
            }
        }

        private void getData()
        {
            
            ServiceUsers thisCsUser = null;
            generateClicked = "true";
            ViewState["generateClicked"] = generateClicked;
            thisService = compositeServiceData[0].ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ConnectedServiceID == CSID; });
            ViewState["thisService"] = thisService;
            ViewState["thisOldService"] = thisService;
            if (thisService.ServiceType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
            {
                ConnectedPanel.Visible = true;
                GenericPanel.Visible = false;
                RadioButtonListServiceType.SelectedIndex = 0;
            }
            else
            {
                RadioButtonListServiceType.SelectedIndex = 1;
                ConnectedPanel.Visible = false;
                GenericPanel.Visible = true;
                PanelConfig.Visible = false;
            }
            RadioButtonListServiceType.Enabled = false;
            if (thisService == null)
                Response.Redirect(ConfigSettings.PAGE_CONNECTIONS,true);
            if (thisService.ServiceType == ConfigUtility.HOST_TYPE_CONNECTED_SERVICE)
            {
                RadioButtonListServiceType.SelectedIndex = 0;
                GenericPanel.Visible = false;
                ConnectedPanel.Visible = true;
                PanelConfig.Visible = false;
                panelConnectedService2.Visible = false;
                panelConnectedService1.Visible = true;
                thisServiceConfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                ViewState["thisConfigService"] = thisServiceConfig;
                thisCsUser = thisServiceUsers.Find(delegate(ServiceUsers csUserExist) { return csUserExist.UserKey == thisService.csUserKey; });
                if (thisService.UseDefaultClientCredentials)
                {
                    CheckBoxUserCredentials.Checked = true;
                    DropDownListCsUser.SelectedValue = thisService.DefaultCredentialUserKey.ToString();
                    DropDownListCsUser.Enabled = true;
                }
                else
                    DropDownListCsUser.Enabled = false;
                if (thisCsUser == null)
                    CSUserID.Text = "<span style=\"color:red;\">Invalid csUser! Please delete this entry and create new.</span";
                else
                    CSUserID.Text = "<span style=\"color:palegreen;\">" + thisCsUser.UserKey.ToString() + ": " + thisCsUser.UserId + "</span>";
                if (thisServiceConfig == null)
                    ConnectedConfigID.Text = "<span style=\"color:red;\">Invalid Connected Config Entry! Please delete this entry and create new.</span";
                else
                    ConnectedConfigID.Text = "<span style=\"color:palegreen;\">" + thisServiceConfig.ConnectedConfigServiceID.ToString() + "</span>";
                if (thisCsUser == null || thisServiceConfig == null)
                    return;
                HostNameID.Text = thisService.HostNameIdentifier;
                DropDownListServiceName.Items.Add(new ListItem(thisService.ServiceFriendlyName, thisService.ServiceFriendlyName));
                DropDownListServiceName.SelectedValue = thisService.ServiceFriendlyName;
                DropDownListServiceName.Enabled = false;
                LabelServiceFriendlyName.Text = thisService.ServiceFriendlyName;
                try
                {
                    DropDownListPrimaryContract.SelectedValue = thisService.ServiceContract;
                }
                catch
                {
                }
                try
                {
                    DropDownListPrimaryClients.SelectedValue = thisService.ClientConfiguration;
                }
                catch { }
                LabelServiceFriendlyName.Text = thisService.ServiceFriendlyName;
                LabelSvcBindingType.Text = thisService.BindingType;
                SecurityMode.Text = thisService.SecurityMode;
                SecurityMode2.Text = thisService.SecurityMode;
                LabelUseHttps.Text = thisService.UseHttps.ToString();
                LabelGenericBindingType.Text = thisService.BindingType;
                LabelContract2.Text = thisService.ServiceContract;
                LabelContract.Text = thisService.ServiceContract;
                LabelSvcBindingType2.Text = thisService.BindingType;
                LabelServiceFriendlyName2.Text = thisService.ServiceFriendlyName;
                LabelVPath.Text = "Determined Dynamically";
                LabelPort.Text = "Determined Dynamically";
                LabelSvcBindingConfig2.Text = thisService.ClientConfiguration;
                LabelIntroService.Text = "The Service friendly name as assigned by the Service Hoster.";
                LabelIntroService.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                LabelIntroPrimary.Text = "Primary Service Details";
                TextBoxOnlineMethod.Text = thisService.OnlineMethod;
                if (!thisService.OnlineParms.Equals(ConfigUtility.NO_ONLINE_PARMS))
                    TextBoxOnlineParms.Text = thisService.OnlineParms;
                thisServiceConfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                ViewState["thisConfigService"] = thisServiceConfig;
            }
            else
            {
                if (thisService.UseDefaultClientCredentials)
                {
                    CheckBoxGenericCredentials.Checked = true;
                    DropDownListGenericCredentialUsers.SelectedValue = thisService.DefaultCredentialUserKey.ToString();
                    DropDownListGenericCredentialUsers.Enabled = true;
                }
                else
                    DropDownListGenericCredentialUsers.Enabled = false;
                HostNameID.Text = thisService.HostNameIdentifier;
                RadioButtonListServiceType.SelectedIndex = 1;
                TextBoxHostNameGeneric.Text = thisService.HostNameIdentifier;
                TextBoxGenericName.Text = thisService.ServiceFriendlyName;
                try
                {
                    DropDownListGenericContract.SelectedValue = thisService.ServiceContract;
                }
                catch { }
                try
                {
                    DropDownListPrimaryClients.SelectedValue = thisService.ClientConfiguration;

                }
                catch { }
                TextBoxGenericOnlineMethod.Text = thisService.OnlineMethod;
                TextBoxGenericParms.Text = thisService.OnlineParms;
                TextBoxGenericAddress.Text = thisService.DefaultAddress;
                thisCsUser = thisServiceUsers.Find(delegate(ServiceUsers csUserExist) { return csUserExist.UserKey == thisService.csUserKey; });
                if (thisCsUser == null && user.Rights!=ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS)
                    CSUserID2.Text = "<span style=\"color:red;\">Invalid csUser! Please delete this entry and create new.</span";
                else
                    if (thisCsUser!=null)
                        CSUserID2.Text = "<span style=\"color:palegreen;\">" + thisCsUser.UserKey.ToString() + ": " + thisCsUser.UserId + "</span>";
                ConnectedConfigServices thisconfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices cse) { return cse.ConnectedConfigServiceID.Equals(thisService.ConnectedConfigServiceID); });
                if (thisconfig == null)
                    ConnectedConfigID2.Text = "<span style=\"color:red;\">Invalid Connected Config Entry! Please delete this entry and create new.</span";
                else
                    ConnectedConfigID2.Text = "<span style=\"color:palegreen;\">" + thisconfig.ConnectedConfigServiceID.ToString() + "</span>";
                if (thisCsUser == null || thisconfig == null)
                    return;
                TextBoxGenericUser.Text = thisCsUser.UserId;
                TextBoxGenericPassword.Text = thisCsUser.Password;
                TextBoxGenericPassword.Enabled = false;
                TextBoxGenericUser.Enabled = false;
                TextBoxGenericPassword.Attributes.Add("value", thisCsUser.Password);
            }
                DropDownListGenericClients.Items.Clear();
                DropDownListPrimaryClients.Items.Clear();
                for (int i = 0; i < clients.Count; i++)
                {
                    BindingInformation theBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                    if (theBinding != null)
                    {
                        string tsSecMode = null;
                        if (thisService == null)
                            tsSecMode = "unknown";
                        else
                            tsSecMode = thisService.SecurityMode;
                        if (clients[i].Contract.Equals(thisService.ServiceContract) && (theBinding.BindingType.Equals(thisService.BindingType) || thisService.BindingType.ToLower().Contains("custom")))
                        {
                            if ((clients[i].ElementName.ToLower().StartsWith("client_") && !clients[i].ElementName.ToLower().StartsWith("client_configsvc") && !clients[i].ElementName.ToLower().StartsWith("client_nodesvc")) && (theBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown")))
                            {
                                DropDownListGenericClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                if (clients[i].ElementName.Equals(thisService.ClientConfiguration))
                                {
                                    DropDownListGenericClients.SelectedValue = clients[i].ElementName;
                                    DropDownListGenericClients.Text = clients[i].ElementName;
                                    LabelGenericBindingType.Text = theBinding.BindingType;
                                    DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                    DropDownListPrimaryClients.Text = clients[i].ElementName;
                                    LabelBindingTypePrimary.Text = theBinding.BindingType;
                                }
                            }
                        }
                    }
                }
                thisServiceConfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                ViewState["thisConfigService"] = thisServiceConfig;
                ViewState["configServices"] = compositeServiceData[0].ConnectedConfigServices;
        }

        private void processUpdate()
        {
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            thisOldService = (ConnectedServices)ViewState["thisOldService"];
            configServices = (List<ConnectedConfigServices>)ViewState["configServices"];
            bindings = compositeServiceData[0].BindingInformation;
            clients = compositeServiceData[0].ClientInformation;
            BindingInformation thisBinding = null;
            ConnectedConfigServices configService = null;
            List<ConnectedConfigServices> addConfigs = new List<ConnectedConfigServices>();
            ClientInformation thisClient = compositeServiceData[0].ClientInformation.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListPrimaryClients.SelectedValue); });
            if (thisClient != null)
            {
                thisBinding = compositeServiceData[0].BindingInformation.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(thisClient.BindingConfiguration); });
                if (CheckBoxUserCredentials.Checked)
                {
                    thisService.UseDefaultClientCredentials = true;
                    if (DropDownListCsUser.SelectedValue != null && DropDownListCsUser.SelectedValue != "")
                        thisService.DefaultCredentialUserKey = Convert.ToInt32(DropDownListCsUser.SelectedValue);
                    else
                        return;
                }
                else
                {
                    thisService.DefaultCredentialUserKey = -1;
                    thisService.UseDefaultClientCredentials = false;
                }
                if (DropDownListPrimaryContract.Items.Count == 0 && action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
                {
                    UpdateMessage.Text = "You must provide a client contract definition in your startup procedure. There are no available client contracts thus supplied. Service contracts can be generated via svcutil.exe, or used directly based on existing projects/code.";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                if (DropDownListPrimaryClients.Items.Count == 0 && action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
                {
                    UpdateMessage.Text = "You must provide a client definition in your config file. There are no available client definitions thus supplied. Client definitions can be generated via svcutil.exe.";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                thisService.ServiceContract = DropDownListPrimaryContract.SelectedValue;
            }
            switch (action)
            {
                case ConfigUtility.ADD_CONNECTED_SERVICE:
                    {
                        string svcName = DropDownListServiceName.SelectedValue;
                        HostedServices hsConnect = configServices[0].PrimaryHostedServices.Find(delegate(HostedServices hsExist) { return hsExist.FriendlyName.Equals(svcName); });
                        thisService.HostedServiceID = hsConnect.HostedServiceID;
                        thisService.DefaultAddress = hsConnect.DefaultAddress;
                        thisService.ClientConfiguration = DropDownListPrimaryClients.SelectedValue;
                        thisService.BindingType = thisBinding.BindingType;
                        thisService.SecurityMode = thisBinding.SecurityMode;
                        string configAddress = TextBoxConfigLoginAddress.Text;
                        Uri configUri = new Uri(configAddress.ToLower());
                        string basePath = configUri.AbsolutePath;
                        int basePort = configUri.Port;
                        string configBinding = DropDownListConfigClient.SelectedValue;
                        ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListConfigClient.SelectedValue); });
                        BindingInformation theConfigBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
                        configService = configServices.Find(delegate(ConnectedConfigServices ccsExist)
                        {
                            string bindingCheck = ccsExist.BindingType;
                            switch (ccsExist.BindingType)
                            {
                                case ConfigUtility.CUSTOM_BINDING_HTTP:
                                    {
                                        bindingCheck = ConfigUtility.BASIC_HTTP_BINDING;
                                        break;
                                    }

                                case ConfigUtility.CUSTOM_BINDING_TCP:
                                    {
                                        bindingCheck = ConfigUtility.NET_TCP_BINDING;
                                        break;
                                    }
                                case ConfigUtility.CUSTOM_BINDING_WSHTTP:
                                    {
                                        bindingCheck = ConfigUtility.WS_HTTP_BINDING;
                                        break;
                                    }
                            }
                            Uri config2Uri = new Uri(ccsExist.Address);
                            string basePath2 = configUri.AbsolutePath;
                            int basePort2 = configUri.Port;
                            return (bindingCheck.Equals(theConfigBinding.BindingType) && basePort2 == basePort && basePath2.Equals(basePath));
                        });
                        if (configService == null)
                        {
                            UpdateMessage.Text = "An error ocurred finding a compatible Configuration Service Endpoint.  The remote service is listening at the attached endpoint, but reports this endpoint as inactive.  The error is likely on the host side.";
                            UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                            return;
                        }
                        configService.ClientConfiguration = configBinding;
                        configService.BindingType = theConfigBinding.BindingType;
                        thisService.OnlineMethod = TextBoxOnlineMethod.Text.Trim();
                        thisService.OnlineParms = TextBoxOnlineParms.Text.Trim();
                        if (thisService.OnlineMethod == null || thisService.OnlineMethod == "")
                            thisService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
                        if (thisService.OnlineParms == null || thisService.OnlineParms == "")
                            thisService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
                        thisService.ConnectedConfigServiceID = configService.ConnectedConfigServiceID;
                        thisService.HostNameIdentifier = configService.HostNameIdentifier;
                        
                        thisService.ServiceFriendlyName = hsConnect.FriendlyName;
                        thisService.LoadBalanceType = hsConnect.LoadBalanceType;
                        thisService.UseHttps = hsConnect.UseHttps;
                        thisService.ServiceImplementationClassName = hsConnect.ServiceImplementationClassName;
                        thisService.ServiceType = ConfigUtility.HOST_TYPE_CONNECTED_SERVICE;
                        if (checkcsUserDupe(configServices[0].InitialCSUserID,thisService.HostNameIdentifier))
                        {
                            UpdateMessage.Text = "The Connected Service user id '" + configServices[0].InitialCSUserID + " is already in use by another Connected Service definition to a different Virtual Service Host (Host Name Identifier). You need to supply a different csUser ID for this definition.";
                            UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                            return;
                        }
                        List<ConnectedConfigServices> otherConfigServices = configServices.FindAll(delegate(ConnectedConfigServices otherExist) { return otherExist.ConnectedConfigServiceID != configService.ConnectedConfigServiceID; });
                        addConfigs.Add(configService);
                        if (otherConfigServices != null)
                        {
                            for (int i = 0; i < otherConfigServices.Count; i++)
                            {
                                string bindingType = otherConfigServices[i].BindingType;
                                string bindingName = otherConfigServices[i].ClientConfiguration;
                                string bindingScheme = ConfigUtility.getBindingScheme(otherConfigServices[i].BindingType, false);
                                int sec = 0;
                                if (bindingName.ToLower().Contains("t_security"))
                                    sec = 1;
                                if (bindingName.ToLower().Contains("m_security"))
                                    sec = 2;
                                BindingInformation newBinding = null;
                                switch (sec)
                                {

                                    case 0:
                                        {
                                            newBinding = bindings.Find(delegate(BindingInformation biFind)
                                            {
                                                string thisBindingScheme = ConfigUtility.getBindingScheme(biFind.BindingType, otherConfigServices[i].UseHttps);
                                                return (thisBindingScheme.Equals(bindingScheme) && biFind.BindingConfigurationName.ToLower().Contains("client_configsvc") && !biFind.BindingConfigurationName.ToLower().Contains("t_security") && !biFind.BindingConfigurationName.ToLower().Contains("m_security"));
                                            }
                                            );
                                            break;
                                        }

                                    case 1:
                                        {
                                            newBinding = bindings.Find(delegate(BindingInformation biFind)
                                            {
                                                string thisBindingScheme = ConfigUtility.getBindingScheme(biFind.BindingType, otherConfigServices[i].UseHttps);
                                                return (thisBindingScheme.Equals(bindingScheme) && biFind.BindingConfigurationName.ToLower().Contains("client_configsvc") && biFind.BindingConfigurationName.ToLower().Contains("t_security"));
                                            }
                                            );
                                            break;
                                        }

                                    case 2:
                                        {
                                            newBinding = bindings.Find(delegate(BindingInformation biFind)
                                            {
                                                string thisBindingScheme = ConfigUtility.getBindingScheme(biFind.BindingType, otherConfigServices[i].UseHttps);
                                                return (thisBindingScheme.Equals(bindingScheme) && biFind.BindingConfigurationName.ToLower().Contains("client_configsvc") && biFind.BindingConfigurationName.ToLower().Contains("m_security"));
                                            }
                                            );
                                            break;
                                        }

                                }
                                if (newBinding != null)
                                {
                                    otherConfigServices[i].ClientConfiguration = newBinding.BindingConfigurationName;
                                    otherConfigServices[i].BindingType = newBinding.BindingType;
                                    otherConfigServices[i].OnlineMethod = configService.OnlineMethod;
                                    otherConfigServices[i].OnlineParms = configService.OnlineParms;
                                    addConfigs.Add(otherConfigServices[i]);
                                }
                            }
                        }
                        break;
                    }
                case ConfigUtility.UPDATE_CONNECTED_SERVICE:
                    {
                        configServices = compositeServiceData[0].ConnectedConfigServices;
                        configService = configServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                        addConfigs.Add(configService);
                        thisService.ClientConfiguration = DropDownListPrimaryClients.SelectedValue;
                        thisService.BindingType = thisBinding.BindingType;
                        thisService.SecurityMode = thisBinding.SecurityMode;
                        thisService.OnlineMethod = TextBoxOnlineMethod.Text.Trim();
                        thisService.OnlineParms = TextBoxOnlineParms.Text.Trim();
                        if (thisService.OnlineMethod == null || thisService.OnlineMethod == "")
                            thisService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
                        if (thisService.OnlineParms == null || thisService.OnlineParms == "")
                            thisService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
                        break;
                    }
                case ConfigUtility.REMOVE_CONNECTED_SERVICE:
                    {
                        configServices = compositeServiceData[0].ConnectedConfigServices;
                        configService = configServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                        addConfigs.Add(configService);
                        break;
                    }
            }
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            try
            {
                success = configProxy.receiveService(hostNameIdentifier, configName, null, null, thisOldService, thisService, addConfigs, null, null, true, action, traversePath, user);


                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    UpdateMessage.Text = "The Connected Service was successfully " + actiontext + ".";
                    UpdateMessage.ForeColor = System.Drawing.Color.PaleGreen;
                    if (action.Equals(ConfigUtility.ADD_CONNECTED_SERVICE))
                        UpdateMessage.Text = UpdateMessage.Text + "<br/>You can now use the Connections menu item to add an initial Connection Point to this Service!";
                    else
                        if (action.Equals(ConfigUtility.REMOVE_CONNECTED_SERVICE))
                        {
                            Delete.Enabled = false;
                            Update.Enabled = false;
                            Add.Enabled = false;
                        }
                    if (action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
                    {
                        compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                        ViewState["CompositeServiceData"] = compositeServiceData;
                        thisService = compositeServiceData[0].ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName.Equals(thisService.ServiceFriendlyName) && csExist.HostNameIdentifier.Equals(thisService.HostNameIdentifier) && csExist.ServiceImplementationClassName.Equals(thisService.ServiceImplementationClassName); });
                        thisServiceConfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                        ViewState["thisConfigService"] = thisServiceConfig;
                        ViewState["thisService"] = thisService;
                        ViewState["thisOldService"] = thisService;
                        ViewState["ID"] = thisService.ConnectedServiceID;
                        action = ConfigUtility.UPDATE_CONNECTED_SERVICE;
                        ViewState["action"] = action;
                    }
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
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                }
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    Add.Enabled = false;
                    if (action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
                    {
                        Update.Enabled = true;
                        Delete.Enabled = true;
                    }
                }
                PanelConfig.Visible = false;
                panelConnectedService1.Visible = false;
                panelConnectedService2.Visible = true;
                DropDownListServiceName.Enabled = false;
                LabelContract2.Text = thisService.ServiceContract;
                LabelSvcBindingType2.Text = thisService.BindingType;
                SecurityMode.Text = thisService.SecurityMode;
                SecurityMode2.Text = thisService.SecurityMode;
                LabelServiceFriendlyName2.Text = thisService.ServiceFriendlyName;
                LabelSvcBindingConfig2.Text = thisService.ClientConfiguration;
                LabelIntroService.Text = "The Service friendly name as assigned by the Service Hoster.";
                LabelIntroService.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                LabelIntroPrimary.Text = "Primary Service Details";
            }
            catch (Exception econfig)
            {
                ViewState["thisService"] = thisOldService;
                UpdateMessage.Text = "An exception was encountered.<br/> Exception is: " + econfig.ToString();
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
            }
        }

        private void processUpdateGeneric()
        {
            TextBoxGenericPassword.Attributes.Add("value", TextBoxGenericPassword.Text);
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            clients = compositeServiceData[0].ClientInformation;
            ClientInformation thisClient = compositeServiceData[0].ClientInformation.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListGenericClients.SelectedValue); });
            BindingInformation thisBinding = compositeServiceData[0].BindingInformation.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(thisClient.BindingConfiguration); });
            if (CheckBoxGenericCredentials.Checked)
            {
                thisService.UseDefaultClientCredentials = true;
                thisService.DefaultCredentialUserKey = Convert.ToInt32(DropDownListGenericCredentialUsers);
            }
            else
            {
                thisService.DefaultCredentialUserKey = -1;
                thisService.UseDefaultClientCredentials = false;
            }
            if (TextBoxGenericName.Text.Trim() == null || TextBoxGenericName.Text.Trim() == "")
            {
                GenericUpdateMessage.Text = "Please supply a Service Assigned Name!";
                GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                processing = false;
                return;
            }
            if (TextBoxHostNameGeneric.Text.Trim() == null || TextBoxHostNameGeneric.Text.Trim() == "")
            {
                GenericUpdateMessage.Text = "Please supply a Host Name Identifier!";
                GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            if (TextBoxGenericAddress.Text.Trim() == null || TextBoxGenericAddress.Text.Trim() == "")
            {
                GenericUpdateMessage.Text = "Please supply a default address!";
                GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            if (DropDownListGenericContract.Items.Count == 0 && action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
            {
                GenericUpdateMessage.Text = "You must provide a client contract definition in your startup procedure. There are no available client contracts thus supplied. Service contracts can be generated via svcutil.exe, or used directly based on existing projects/code.";
                GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            if (DropDownListGenericClients.Items.Count == 0 && action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
            {
                GenericUpdateMessage.Text = "You must provide a client definition in your config file. There are no available client definitions thus supplied. Client definitions can be generated via svcutil.exe.";
                GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            thisOldService = (ConnectedServices)ViewState["thisOldService"];
            configServices = (List<ConnectedConfigServices>)ViewState["configServices"];
            bindings = (List<BindingInformation>)ViewState["bindings"];
            ConnectedConfigServices configService = (ConnectedConfigServices)ViewState["thisConfigService"];
            thisService.HostedServiceID = -1;
            thisService.DefaultAddress = TextBoxGenericAddress.Text.Trim();
            thisService.ClientConfiguration = DropDownListGenericClients.SelectedValue;
            thisService.BindingType = thisBinding.BindingType;
            thisService.SecurityMode = thisBinding.SecurityMode;
            thisService.HostNameIdentifier = TextBoxHostNameGeneric.Text.Trim();
            thisService.ServiceContract = DropDownListGenericContract.SelectedValue;
            thisService.ServiceImplementationClassName = "Generic";
            if (configService != null)
                thisService.ConnectedConfigServiceID = configService.ConnectedConfigServiceID;
            else
            {
                configService = new ConnectedConfigServices();
                configService.InitialCSUserID = TextBoxGenericUser.Text.Trim();
                configService.InitialCSPassword = TextBoxGenericPassword.Text.Trim();
                if (configService.InitialCSUserID == "" || configService.InitialCSPassword == "")
                {
                    GenericUpdateMessage.Text = "You must provide a connected service username and password; this user will be created and associated with connections to this Generic service.";
                    GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                if (checkcsUserDupe(configService.InitialCSUserID,thisService.HostNameIdentifier))
                {
                    UpdateMessage.Text = "The Connected Service user id '" + configServices[0].InitialCSUserID + " is already in use by another Connected Service definition to a different Virtual Service Host (Host Name Identifier). You need to supply a different csUser ID for this definition.";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                configService.HostNameIdentifier = thisService.HostNameIdentifier;
                configService.ServiceImplementationClassName = thisService.ServiceContract;
                configService.ClientConfiguration = "none";
                configService.ServiceContract = thisService.ServiceContract;
                configService.ServiceType = ConfigUtility.HOST_TYPE_GENERIC;
                configService.BindingType = thisService.BindingType;
                configService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
                configService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
                configService.Address = "generic";
                configService.ServiceFriendlyName = "GenericConfig";
                configService.LoadBalanceType = 0;
                if (CheckBoxGenericUseHttps.Checked)
                    configService.UseHttps = true;
                else configService.UseHttps = false;
                thisService.ConnectedConfigServiceID = -1;
            }
            thisService.OnlineMethod = TextBoxGenericOnlineMethod.Text.Trim();
            thisService.OnlineParms = TextBoxGenericParms.Text.Trim();
            if (thisService.OnlineParms == null || thisService.OnlineParms == "")
                thisService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
            if (thisService.OnlineMethod == null || thisService.OnlineMethod == "")
                thisService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
            thisService.ServiceFriendlyName = TextBoxGenericName.Text.Trim();
            thisService.ServiceImplementationClassName = "none supplied";
            thisService.ServiceType = ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE;
            thisService.LoadBalanceType = ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS;
            List<ConnectedConfigServices> addConfigServices = new List<ConnectedConfigServices>();
            addConfigServices.Add(configService);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
            try
            {
                success = configProxy.receiveService(hostNameIdentifier, configName, null, null, thisOldService, thisService, addConfigServices, null, null, true, action, traversePath, user);
                if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
                {
                    GenericUpdateMessage.Text = "The Generic Connected Service was successfully " + actiontext + ".";
                    GenericUpdateMessage.ForeColor = System.Drawing.Color.PaleGreen;
                    if (action.Equals(ConfigUtility.ADD_CONNECTED_SERVICE))
                        GenericUpdateMessage.Text = UpdateMessage.Text + "<br/>You can now use the Connections menu item to add an initial Connection Point to this Service!";
                    else
                        if (action.Equals(ConfigUtility.REMOVE_CONNECTED_SERVICE))
                        {
                            Delete.Enabled = false;
                            Update.Enabled = false;
                            Add.Enabled = false;
                        }
                        else
                        {
                            Delete.Enabled = true;
                            Update.Enabled = true;
                            Add.Enabled = false;
                            Add.Visible = false;
                        }
                    if (action != ConfigUtility.REMOVE_CONNECTED_SERVICE)
                    {
                        compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                        ViewState["CompositeServiceData"] = compositeServiceData;
                        thisService = compositeServiceData[0].ConnectedServices.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName.Equals(thisService.ServiceFriendlyName) && csExist.HostNameIdentifier.Equals(thisService.HostNameIdentifier) && csExist.ServiceImplementationClassName.Equals(thisService.ServiceImplementationClassName); });
                        thisServiceConfig = compositeServiceData[0].ConnectedConfigServices.Find(delegate(ConnectedConfigServices ccsExist) { return ccsExist.ConnectedConfigServiceID == thisService.ConnectedConfigServiceID; });
                        ViewState["thisConfigService"] = thisServiceConfig;
                        ViewState["thisService"] = thisService;
                        ViewState["thisOldService"] = thisService;
                        ViewState["ID"] = thisService.ConnectedServiceID;
                        action = ConfigUtility.UPDATE_CONNECTED_SERVICE;
                        ViewState["action"] = action;
                    }
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
                    GenericUpdateMessage.Text = "<br/><span style=\"color:Maroon\">" + message + "</span>";
                    GenericUpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                }
                Add.Enabled = false;
                Update.Enabled = true;
                Delete.Enabled = true;
                PanelConfig.Visible = false;
                panelConnectedService1.Visible = false;
                panelConnectedService2.Visible = false;
                ConnectedPanel.Visible = false;
                GenericPanel.Visible = true;
            }
            catch (Exception econfig)
            {
                ViewState["thisService"] = thisOldService;
                UpdateMessage.Text = "An exception was encountered.<br/> Exception is: " + econfig.ToString();
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
            }
            TextBoxGenericPassword.Attributes.Add("value", TextBoxGenericPassword.Text);
        }

        protected void RadioButtonListServiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(RadioButtonListServiceType.SelectedValue))
            {
                case ConfigUtility.HOST_TYPE_CONNECTED_SERVICE:
                    {
                        GenericPanel.Visible = false;
                        PanelConfig.Visible = true;
                        if (generateClicked.Equals("true"))
                            ConnectedPanel.Visible = true;
                        else
                            ConnectedPanel.Visible = false;
                        break;
                    }
                case ConfigUtility.HOST_TYPE_GENERIC_CONNECTED_SERVICE:
                    {
                        PanelConfig.Visible = false;
                        ConnectedPanel.Visible = false;
                        GenericPanel.Visible = true;
                        break;
                    }
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.REMOVE_CONNECTED_SERVICE;
            actiontext = "deleted";
            processUpdate();
            ViewState["processing"] = false;
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.UPDATE_CONNECTED_SERVICE;
            actiontext = "updated";
            processUpdate();
            ViewState["processing"] = false;
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.ADD_CONNECTED_SERVICE;
            actiontext = "added";
            thisService = new ConnectedServices();
            processUpdate();
            ViewState["processing"] = false;
        }

        protected void DeleteGeneric_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.REMOVE_CONNECTED_SERVICE;
            thisService = (ConnectedServices)ViewState["thisService"];
            actiontext = "deleted";
            processUpdateGeneric();
            ViewState["processing"] = false;
        }

        protected void UpdateGeneric_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.UPDATE_CONNECTED_SERVICE;
            thisService = (ConnectedServices)ViewState["thisService"];
            actiontext = "updated";
            processUpdateGeneric();
            ViewState["processing"] = false;
        }

        protected void AddGeneric_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.ADD_CONNECTED_SERVICE;
            actiontext = "added";
            thisService = new ConnectedServices();
            processUpdateGeneric();
            ViewState["processing"] = false;
        }

        protected void AutoGenButton_Click(object sender, EventArgs e)
        {
            if (TextBoxConfigLoginAddress.Text == null || TextBoxConfigLoginAddress.Text == "")
            {
                LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                LabelGetServices.Text = "Please enter an address to the remote Configuration Service.";
                return;
            }
            try
            {
                Uri testConfigUri = new Uri(TextBoxConfigLoginAddress.Text);
            }
            catch (Exception)
            {
                LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                LabelGetServices.Text = "Please enter a valid address.  The address you entered is not a valid URI; no connection was attempted.";
                return;
            }
            string addressSes = null;
            string IDSes = null;
            ServiceUsers csUserSes = null;
            string confignameSes = null;
            string hosterSes = null;
            string versionSes = null;
            string platformSes = null;
            string clientSession = null;
            string nameSes = null;
            SessionInfo info = new SessionInfo();
            info.getSessionData(false, out addressSes, out csUserSes, out clientSession, out nameSes, out confignameSes, out hosterSes, out versionSes, out platformSes, out IDSes);
            address = addressSes;
            string currentbinding = clientSession;
            user = csUserSes;
            if (address == null || user == null)
                Response.Redirect(FormsAuthentication.LoginUrl,true);
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, currentbinding, user);
            DropDownListServiceName.Items.Clear();
            string addressConfig = TextBoxConfigLoginAddress.Text.Trim();
            ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListConfigClient.SelectedValue); });
            BindingInformation binding = bindings.Find(delegate(BindingInformation bindingItem) { return bindingItem.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
            string selectedBinding = theClient.ElementName;
            if (binding != null && binding.BindingType != null)
            {
                switch (binding.BindingType)
                {
                    case ConfigUtility.BASIC_HTTP_BINDING:
                        {
                            if (!addressConfig.ToLower().StartsWith("http"))
                            {

                                LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                                LabelGetServices.Text = "You have selected an <b>Http</b> binding: Please enter a valid http address.";
                                return;
                            }
                            break;
                        }
                    case ConfigUtility.WS_HTTP_BINDING:
                        {
                            goto case ConfigUtility.BASIC_HTTP_BINDING;
                        }

                    case ConfigUtility.WS_2007_HTTP_BINDING:
                        {
                            goto case ConfigUtility.BASIC_HTTP_BINDING;
                        }



                    case ConfigUtility.NET_TCP_BINDING:
                        {
                            if (!addressConfig.ToLower().StartsWith("net.tcp"))
                            {
                                LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                                LabelGetServices.Text = "You have selected a <b>Tcp</b> binding: Please enter a valid tcp address in form of net.tcp://";
                                return;
                            }
                            break;
                        }
                }
            }
            ServiceUsers configUser = new ServiceUsers();
            configUser = user;
            csusername= TextBoxConfigLoginUserId.Text.ToLower().Trim();
            cspassword = TextBoxConfigLoginPassword.Text.Trim();
            ServiceConfigurationClient newConfigProxy = null;
            try
            {
                newConfigProxy = new ServiceConfigurationClient(currentbinding, address, configUser);
                configServices = newConfigProxy.getMyConfigServiceDetails(hostNameIdentifier, configName, traversePath, addressConfig, selectedBinding,csusername,cspassword, configUser);
                if (configServices != null)
                {
                    if (configServices.Count == 0)
                    {
                        LabelGetServices.Text = "The remote host is currently not returning any Configuration Service Hosts to connect to.";
                        LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                        return;
                    }
                    LabelGetServices.Text = "<span style=\"font-size:1.2em;color:palegreen\">Connected!</span>";
                    ViewState["configServices"] = configServices;
                }
                else
                {
                    LabelGetServices.Text = "Service refused to supply information. Check userid/password and try again.";
                    LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                HostNameID.Text = configServices[0].HostNameIdentifier;
                TextBoxConfigLoginUserId.Text = configServices[0].InitialCSUserID;
                TextBoxConfigLoginPassword.Text = configServices[0].InitialCSPassword;
                TextBoxConfigLoginPassword.Attributes.Add("value", configServices[0].InitialCSPassword);
                string hsContractSelected = null;
                List<ConnectedServices> csList = compositeServiceData[0].ConnectedServices;
                for (int i = 0; i < configServices[0].PrimaryHostedServices.Count; i++)
                {
                    if (configServices[0].PrimaryHostedServices[i].ServiceType == ConfigUtility.HOST_TYPE_PRIMARY || configServices[0].PrimaryHostedServices[i].ServiceType == ConfigUtility.HOST_TYPE_GENERIC)
                    {
                        ConnectedServices csCheck = csList.Find(delegate(ConnectedServices csExist) { return csExist.ServiceFriendlyName.Equals(configServices[0].PrimaryHostedServices[i].FriendlyName); });
                        if (csCheck == null)
                        {
                            if (hsContractSelected == null)
                                hsContractSelected = configServices[0].PrimaryHostedServices[i].FriendlyName;
                            DropDownListServiceName.Items.Add(new ListItem(configServices[0].PrimaryHostedServices[i].FriendlyName, configServices[0].PrimaryHostedServices[i].FriendlyName));
                        }
                        else
                        {
                            UpdateMessage.Text = "There are services you already have established definitions for, and these have been removed from the available list!";
                            UpdateMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                        }
                    }
                }
                if (DropDownListServiceName.Items.Count > 0)
                {
                    DropDownListServiceName.SelectedIndex = 0;
                    string serviceName = DropDownListServiceName.SelectedValue;
                    hsSelected = configServices[0].PrimaryHostedServices.Find(delegate(HostedServices hsExist) { return hsExist.FriendlyName.Equals(DropDownListServiceName.Items[0].Text); });
                    TextBoxOnlineMethod.Text = hsSelected.OnlineMethod;
                    TextBoxOnlineParms.Text = hsSelected.OnlineParms;
                    DropDownListServiceName.SelectedValue = hsContractSelected;
                    DropDownListServiceName.Text = hsContractSelected;
                    DropDownListServiceName.Enabled = true;
                    Add.Enabled = true;
                    LabelContract.Text = hsSelected.ServiceContract;
                    LabelServiceFriendlyName.Text = hsSelected.FriendlyName;
                    try
                    {
                        DropDownListPrimaryContract.SelectedValue = hsSelected.ServiceContract;
                    }
                    catch { }
                    LabelSvcBindingType.Text = hsSelected.BindingType;
                    LabelPort.Text = hsSelected.Port;
                    LabelUseHttps.Text = hsSelected.UseHttps.ToString();
                    LabelVPath.Text = hsSelected.VirtualPath;
                    SecurityMode.Text = hsSelected.SecurityMode;
                    DropDownListPrimaryClients.Items.Clear();
                    string thisHssecurityMode = hsSelected.SecurityMode;
                    string thisHsBindingType = hsSelected.BindingType;
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                        {
                            BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                            if (theClientBinding != null)
                            {
                                string tsSecMode = null;
                                if (thisService == null)
                                    tsSecMode = "unknown";
                                else
                                    tsSecMode = thisService.SecurityMode;
                                if (theClientBinding.BindingType.Equals(thisHsBindingType) || thisHsBindingType.ToLower().Contains("custom"))
                                {
                                    if (theClientBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown"))
                                    {
                                        if (!clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                                        {
                                            DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                            DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListPrimaryClients.SelectedValue); });
                    if (theClient != null)
                    {
                        BindingInformation theSelectedBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
                        if (theSelectedBinding != null)
                        {
                            LabelBindingTypePrimary.Text = theSelectedBinding.BindingType;
                        }
                        LabelIntroService.Text = "<div style=\"font-size:.9em;color:#FFFF99;padding-bottom:10px\">These are the available service endpoints by friendly names as assigned by the Service Hoster. Each has its own binding requirements " +
                            "(network scheme, security, encoding, etc.). Please select the desired service from this list. Client configurations and bindings can be generated by svcutil.exe, " + 
                            "named to Configuration Service standards, and inserted into this application's configuration file to appear in this page as selectable. You will also need to supply the service contract " +
                            "your client uses in your startup call per the StockTrader example and the documentation.<br/>";
                        Add.Enabled = true;
                    }
                    else
                    {
                        LabelIntroService.Text = "Please change the selected Primary Hosted Service, or the Client Contract.  The current combination yields no valid client configuration to connect with. The check was performed against binding type, and security mode. You may need to add and appropriate client definition to your config file.";
                        Add.Enabled = false;
                        LabelIntroService.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                    }
                }
                else
                {
                    DropDownListServiceName.Enabled = false;
                    Add.Enabled = false;
                    LabelIntroService.Text = "Either this host does not expose services via the Config Service (try as a Generic Service); or you already have established definitions for all available services this virtual host exposes.";
                    LabelIntroService.ForeColor = System.Drawing.Color.Maroon;
                }
                LabelIntroPrimary.Text = "Please Select the Primary Service";
            }
            catch (Exception eConfig)
            {
                LabelGetServices.ForeColor = System.Drawing.Color.Maroon;
                LabelGetServices.Text = "Cannot Connect!<br/><br/>Please make sure the address to the remote configuration service endpoint is correct and you have chosen the appropriate client configuration/binding.  For example" +
                    " if specifying https, you must choose a client configuration using https/transport security.<br/>" +
                    "Full connect exception is: <br/>" + ConfigUtility.reMapExceptionForDisplay(eConfig.ToString());
                if (eConfig.InnerException != null)
                    LabelGetServices.Text = LabelGetServices.Text + "<br/> Inner Exception is:<br/>" + eConfig.InnerException.ToString();
                return;
            }
            generateClicked = "true";
            ViewState["generateClicked"] = generateClicked;
            ConnectedPanel.Visible = true;
            panelConnectedService1.Visible = true;
            panelConnectedService2.Visible = false;
        }

        protected void DropDownListBinding_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedConfig = DropDownListConfigClient.SelectedValue;
            DropDownListConfigClient.Text = selectedConfig;
            ClientInformation client = clients.Find(delegate(ClientInformation clientExist) { return clientExist.ElementName.Equals(selectedConfig); });
            if (client != null)
            {
                BindingInformation binding = bindings.Find(delegate(BindingInformation bindingItem) { return bindingItem.BindingConfigurationName.Equals(client.BindingConfiguration); });
                if (binding != null)
                    BindingTypeAutoGenerate.Text = binding.BindingType;
            }
        }
        
        protected void DropDownListBindingGeneric_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindings = compositeServiceData[0].BindingInformation;
            clients = compositeServiceData[0].ClientInformation;
            string selectedConfig = DropDownListGenericClients.SelectedValue;
            ClientInformation client = clients.Find(delegate(ClientInformation clientExist) { return clientExist.ElementName.Equals(selectedConfig); });
            if (client != null)
            {
                BindingInformation binding = bindings.Find(delegate(BindingInformation bindingItem) { return bindingItem.BindingConfigurationName.Equals(client.BindingConfiguration); });
                if (binding != null)
                {
                    LabelGenericBindingType.Text = binding.BindingType;
                    DropDownListGenericClients.Text = selectedConfig;
                }
            }
            ConnectedPanel.Visible = false;
            TextBoxGenericPassword.Attributes.Add("value", TextBoxGenericPassword.Text);
        }

        protected void DropDownListServiceName_SelectedIndexChanged(object sender, EventArgs e)
        {
            configServices = (List<ConnectedConfigServices>)ViewState["configServices"];
            thisServiceConfig = (ConnectedConfigServices)ViewState["thisServiceConfig"];
            bindings = compositeServiceData[0].BindingInformation;
            List<ClientInformation> clients = new List<ClientInformation>();
            clients.AddRange(compositeServiceData[0].ClientInformation);
            DropDownListPrimaryClients.Items.Clear();
            switch (action)
            {
                case ConfigUtility.ADD_CONNECTED_SERVICE:
                    {
                        HostedServices thisHS = configServices[0].PrimaryHostedServices.Find(delegate(HostedServices hsExists) { return hsExists.FriendlyName.Equals(DropDownListServiceName.SelectedValue); });
                        clients.RemoveAll(delegate(ClientInformation clientExist) { return !clientExist.Contract.Equals(thisHS.ServiceContract); });
                        LabelContract.Text = thisHS.ServiceContract;
                        LabelServiceFriendlyName.Text = thisHS.FriendlyName;
                        LabelSvcBindingType.Text = thisHS.BindingType;
                        LabelPort.Text = thisHS.Port;
                        LabelUseHttps.Text = thisHS.UseHttps.ToString();
                        LabelVPath.Text = thisHS.VirtualPath;
                        SecurityMode.Text = thisHS.SecurityMode;
                        TextBoxOnlineMethod.Text = thisHS.OnlineMethod;
                        TextBoxOnlineParms.Text = thisHS.OnlineParms;
                        string svcName = DropDownListServiceName.SelectedValue;
                        hsSelected = configServices[0].PrimaryHostedServices.Find(delegate(HostedServices hsExist) { return hsExist.FriendlyName.Equals(svcName); });
                        string thisHssecurityMode = hsSelected.SecurityMode;
                        string thisHsBindingType = hsSelected.BindingType;
                        for (int i = 0; i < clients.Count; i++)
                        {
                            if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                            {
                                BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                                if (theClientBinding != null)
                                {
                                    string tsSecMode = null;
                                    if (thisService == null)
                                        tsSecMode = "unknown";
                                    else
                                        tsSecMode = thisService.SecurityMode;
                                    if (theClientBinding.BindingType.Equals(thisHsBindingType) || thisHsBindingType.ToLower().Contains("custom"))
                                    {
                                        if (theClientBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown"))
                                        {
                                            if (!clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                                            {
                                                DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                                DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                case ConfigUtility.UPDATE_CONNECTED_SERVICE:
                    {
                        clients.RemoveAll(delegate(ClientInformation clientExist) { return !clientExist.Contract.Equals(thisService.ServiceContract); });
                        LabelContract.Text = thisService.ServiceContract;
                        LabelServiceFriendlyName.Text = thisService.ServiceFriendlyName;
                        LabelSvcBindingType.Text = thisService.BindingType;
                        LabelUseHttps.Text = thisService.UseHttps.ToString();
                        SecurityMode.Text = thisService.SecurityMode;
                        TextBoxOnlineMethod.Text = thisService.OnlineMethod;
                        TextBoxOnlineParms.Text = thisService.OnlineParms;
                        string svcName = DropDownListServiceName.SelectedValue;
                        string thisCssecurityMode = thisService.SecurityMode;
                        string thisCsBindingType = thisService.BindingType;
                        for (int i = 0; i < clients.Count; i++)
                        {
                            if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                            {
                                BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                                if (theClientBinding != null)
                                {
                                    string tsSecMode = null;
                                    if (thisService == null)
                                        tsSecMode = "unknown";
                                    else
                                        tsSecMode = thisService.SecurityMode;
                                    if (theClientBinding.BindingType.Equals(thisCsBindingType) || thisCsBindingType.ToLower().Contains("custom"))
                                    {
                                        if (theClientBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown"))
                                        {
                                            if (!clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                                            {
                                                DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                                DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            if (DropDownListPrimaryClients.Items.Count > 0)
            {
                string configName = DropDownListPrimaryClients.SelectedValue;
                ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(configName); });
                if (theClient != null)
                {
                    string theBindingName = theClient.BindingConfiguration;
                    BindingInformation theSelectedBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
                    if (theSelectedBinding != null)
                        LabelBindingTypePrimary.Text = theSelectedBinding.BindingType;
                }
                Add.Enabled = true;
            }
            else
            {
                Add.Enabled = false;
                LabelIntroService.Text = LabelIntroService.Text = "Please change the selected Primary Hosted Service, or the Client Contract.  The current combination yields no valid client configuration to connect with. The check was performed against binding type, and security mode. You may need to add and appropriate client definition to your config file.";
                LabelBindingTypePrimary.Text = "";
            }

        }
        
        protected void CheckBoxGenericCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxGenericCredentials.Checked)
                DropDownListGenericCredentialUsers.Enabled = true;
            else
                DropDownListGenericCredentialUsers.Enabled = false;
            ConnectedPanel.Visible = false;
            TextBoxGenericPassword.Attributes.Add("value", TextBoxGenericPassword.Text);
        }

        protected void CheckBoxUserCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxUserCredentials.Checked)
                DropDownListCsUser.Enabled = true;
            else
                DropDownListCsUser.Enabled = false;
        }

        protected void DropDownListPrimaryContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            thisService = (ConnectedServices)ViewState["thisService"];
            configServices = (List<ConnectedConfigServices>)ViewState["configServices"];
            string svcName = DropDownListServiceName.SelectedValue;
            switch (action)
            {
                case ConfigUtility.ADD_CONNECTED_SERVICE:
                    {
                        hsSelected = configServices[0].PrimaryHostedServices.Find(delegate(HostedServices hsExist) { return hsExist.FriendlyName.Equals(svcName); });
                        DropDownListPrimaryClients.Items.Clear();
                        if (hsSelected != null)
                        {
                            string thisHssecurityMode = hsSelected.SecurityMode;
                            string thisHsBindingType = hsSelected.BindingType;
                            for (int i = 0; i < clients.Count; i++)
                            {
                                if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                                {
                                    BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                                    if (theClientBinding != null)
                                    {
                                        string tsSecMode = null;
                                        if (thisService == null)
                                            tsSecMode = "unknown";
                                        else
                                            tsSecMode = thisService.SecurityMode;
                                        if (theClientBinding.BindingType.Equals(thisHsBindingType) || thisHsBindingType.ToLower().Contains("custom"))
                                        {
                                            if (theClientBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown"))
                                            {
                                                if (!clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                                                {
                                                    DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                                    DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                case ConfigUtility.UPDATE_CONNECTED_SERVICE:
                    {
                            DropDownListPrimaryClients.Items.Clear();
                            string thisCssecurityMode = thisService.SecurityMode;
                            string thisCsBindingType = thisService.BindingType;
                            for (int i = 0; i < clients.Count; i++)
                            {
                                if (DropDownListPrimaryContract.SelectedValue.Equals(clients[i].Contract))
                                {
                                    BindingInformation theClientBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                                    if (theClientBinding != null)
                                    {
                                        string tsSecMode = null;
                                        if (thisService == null)
                                            tsSecMode = "unknown";
                                        else
                                            tsSecMode = thisService.SecurityMode;
                                        if (theClientBinding.BindingType.Equals(thisCsBindingType) || thisCsBindingType.ToLower().Contains("custom"))
                                        {
                                            if (theClientBinding.SecurityMode.Equals(tsSecMode) || tsSecMode.Equals("unknown"))
                                            {
                                                if (!clients[i].ElementName.ToLower().Contains("host") && !clients[i].ElementName.ToLower().Contains("client_configsvc") && !clients[i].ElementName.ToLower().Contains("client_nodesvc"))
                                                {
                                                    DropDownListPrimaryClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                                    DropDownListPrimaryClients.SelectedValue = clients[i].ElementName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                    }
                    break;
            }
            if (DropDownListPrimaryClients.Items.Count == 0)
                LabelIntroService.Text = LabelIntroService.Text = "Please change the selected Primary Hosted Service, or the Client Contract.  The current combination yields no valid client configuration to connect with. The check was performed against binding type, and security mode. You may need to add and appropriate client definition to your config file.";
                            
        }

        protected void DropDownListGenericContract_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxGenericPassword.Attributes.Add("value", TextBoxGenericPassword.Text);
            DropDownListGenericClients.Items.Clear();
            for (int i = 0; i < clients.Count; i++)
            {
                BindingInformation theBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                if (theBinding != null)
                {
                    string contract = DropDownListGenericContract.SelectedValue;
                    string bindingType = null;
                    string clientConfig = null;
                    string tsSecMode = null;
                    if (thisService == null)
                    {
                        bindingType = theBinding.BindingType;
                        clientConfig = clients[i].ElementName;
                        tsSecMode = "unknown";
                    }
                    else
                    {
                        tsSecMode = thisService.SecurityMode;
                        bindingType = thisService.BindingType;
                        clientConfig = thisService.ClientConfiguration;
                    }
                    if (clients[i].Contract.Equals(contract) && clients[i].ElementName.ToLower().StartsWith("client_") && !clients[i].ElementName.ToLower().StartsWith("client_configsvc") && !clients[i].ElementName.ToLower().StartsWith("client_nodesvc"))
                        if ((theBinding.BindingType.Equals(bindingType) && (theBinding.SecurityMode.Equals(tsSecMode)) || tsSecMode.Equals("unknown")))
                        {
                            DropDownListGenericClients.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                            if (clients[i].ElementName.Equals(clientConfig))
                            {
                                DropDownListGenericClients.SelectedValue = clients[i].ElementName;
                                DropDownListGenericClients.Text = clients[i].ElementName;
                                LabelGenericBindingType.Text = theBinding.BindingType;
                            }
                    }
                }
            }
            ConnectedPanel.Visible = false;
        }

        protected void DropDownListPrimaryBindings_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this list will either have a valid entry for the contract and service host selected, or not value.  No check needed.
            ClientInformation theClient = clients.Find(delegate(ClientInformation ciExist) { return ciExist.ElementName.Equals(DropDownListPrimaryClients.SelectedValue); });
            BindingInformation theBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(theClient.BindingConfiguration); });
            LabelBindingTypePrimary.Text = theBinding.BindingType;
        }

        private bool checkcsUserDupe(string userName, string hostNameIdent)
        {
            bool dupe = false;
            
            thisServiceUsers = (List<ServiceUsers>)ViewState["thisServiceUsers"];
            switch(RadioButtonListServiceType.SelectedIndex)
            {
                case 0:
                    {
                        string configServiceImpName = "";
                        if (configServices.Count>0)
                            configServiceImpName = configServices[0].ServiceImplementationClassName;
                        for (int h = 0; h < compositeServiceData[0].ConnectedConfigServices.Count; h++)
                        {
                            int key = compositeServiceData[0].ConnectedConfigServices[h].csUserKey;
                            ServiceUsers foundUser = thisServiceUsers.Find(delegate (ServiceUsers userExist) {return (userExist.UserKey==key);});
                            if (foundUser != null)
                            {
                                if (foundUser.UserId.ToLower().Equals(userName))
                                {
                                    if (!(hostNameIdent.Equals(compositeServiceData[0].ConnectedConfigServices[h].HostNameIdentifier) && configServiceImpName.Equals(compositeServiceData[0].ConnectedConfigServices[h].ServiceImplementationClassName)))
                                    {
                                        dupe = true;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int h = 0; h < compositeServiceData[0].ConnectedConfigServices.Count; h++)
                        {
                            int key = compositeServiceData[0].ConnectedConfigServices[h].csUserKey;
                            ServiceUsers foundUser = thisServiceUsers.Find(delegate(ServiceUsers userExist) { return (userExist.UserKey == key); });
                            if (foundUser != null)
                            {
                                if (foundUser.UserId.ToLower().Equals(userName))
                                {
                                    if (!(hostNameIdent.Equals(compositeServiceData[0].ConnectedConfigServices[h].HostNameIdentifier)))
                                    {
                                        dupe = true;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
            }
            
            return dupe;
        }
    }
}