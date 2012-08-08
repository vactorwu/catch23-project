//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Form and logic to modify/add new Hosted Service (Endpoint) definitions to the Confguration
//Database.
//============================================================================================


//====================Update History====================================================================
// 10/17/2008:  V 2.0.2.1 -- Added BindingType WS_FEDERATED_HTTP_BINDING
// 3/31/2011:   V 5.0.0.0 -- A brand new release, updated for private/public clouds, other major changes.
//======================================================================================================

using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.RuntimeHostData;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;

namespace ConfigService.ServiceConfiguration.Web
{
    public partial class UpdateHostedService : System.Web.UI.Page
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
        HostedServices thisService;
        HostedServices thisOldService;
        int SHID = -1;
        int HSID = -1;
        bool processing = false;
        List<TraverseNode> traversePath;
        List<ServiceConfigurationData> compositeServiceData;
        List<MasterServiceHostInstance> serviceHosts;
        MasterServiceHostInstance thisServiceHost;
        List<ServiceUsers> serviceUsers;

        protected override void OnLoad(EventArgs e)
        {
            Page.Form.DefaultFocus = DropDownListContracts.ClientID;
            Input.getHostData(IsPostBack, ViewState, out userid, out address, out user, out binding, out hostNameIdentifier, out configName, out version, out platform, out hoster, false);
            if (!IsPostBack)
            {
                ViewState["processing"] = false;
                action = Request["action"];
                string idString = Request["HSID"];
                string vidString = Request["SHID"];
                SHID = Convert.ToInt32(vidString);
                if (vidString == null || vidString == "")
                    Response.Redirect(ConfigSettings.PAGE_NODES,true);
                if (idString != null)
                    HSID = Convert.ToInt32(idString);
                traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);
                compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                serviceHosts = configProxy.getVirtualServiceHosts(hostNameIdentifier, configName, traversePath, user);
                serviceUsers = configProxy.getServiceUsers(hostNameIdentifier, configName, traversePath, user);
                serviceUsers.RemoveAll(delegate(ServiceUsers userExist) { return !userExist.LocalUser || userExist.Rights != ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS; });
                if (compositeServiceData == null)
                {
                    Response.Redirect(ConfigSettings.PAGE_NODES,true);
                }
                thisService = compositeServiceData[0].HostedServices.Find(delegate(HostedServices hsExist) { return hsExist.HostedServiceID == HSID; });
                thisServiceHost = serviceHosts.Find(delegate(MasterServiceHostInstance mshExist) { return mshExist.ServiceHostID == SHID; });
                if (thisServiceHost.HostEnvironment == ConfigUtility.HOSTING_ENVIRONMENT_IIS)
                {
                    LabelVDirWarning.Text = "This is an IIS-Hosted Service.  If you mark this endpoint as a BASE address (below), you must specify the exact virtual directory + filename for your .svc file, as this is what WCF will expect as a base address. Relative addresses for an IIS-hosted service, become " +
                    "the virtual directory + /[.svcfilename]/path.  For example, you can add a relative address to /myVdir/myservice.svc as /myVDir/myservice.svc/mynewendpoint.";
                    LabelPortWarning.Text = "As an IIS-hosted service endpoint, typically this will be 80 for non-SSL services, and 443 for SSL services, unless you have otherwise configured your IIS site that hosts your service.";
                    LabelPortWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                    LabelVDirWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                }
                if (thisServiceHost.ServiceType.Equals(ConfigUtility.HOST_TYPE_NODE) || thisServiceHost.ServiceType.Equals(ConfigUtility.HOST_TYPE_NODE_DC))
                    RadioButtonListLoadBalanceType.Enabled=false;
                DropDownListEPB.Items.Clear();
                for (int i = 0; i < compositeServiceData[0].EndpointBehaviorNames.Count; i++)
                {
                    DropDownListEPB.Items.Add(new ListItem(compositeServiceData[0].EndpointBehaviorNames[i], compositeServiceData[0].EndpointBehaviorNames[i]));
                }
                if (DropDownListEPB.Items.Count == 0)
                {
                    DropDownListEPB.Enabled = false;
                    LabelEPB.Text = "No Endpoint Behaviors were initialized as part of your service startup procedure.  Hence, you cannot select any endpoint behaviors at this time.";
                    CheckBoxEPB.Enabled = false;
                }
                else
                {
                    DropDownListEPB.Enabled = true;
                    CheckBoxEPB.Enabled = true;
                }
                DropDownListIdentity.Items.Clear();
                for (int i = 0; i < compositeServiceData[0].EndpointIdentityNames.Count; i++)
                {
                    DropDownListIdentity.Items.Add(new ListItem(compositeServiceData[0].EndpointIdentityNames[i], compositeServiceData[0].EndpointIdentityNames[i]));
                }
                if (DropDownListIdentity.Items.Count == 0)
                {
                    DropDownListEPB.Enabled = false;
                    LabelIdentity.Text = "No Endpoint identities have been configured in the config file section <endpointIdentities>. Hence, you cannot select any endpoint identities at this time, default values will be used.";
                    CheckBoxIdentity.Enabled = false;
                }
                ViewState["CompositeServiceData"] = compositeServiceData;
                ViewState["SHID"] = SHID;
                ViewState["HSID"] = HSID;
                ViewState["thisOldService"] = thisService;
                ViewState["thisService"] = thisService;
                ViewState["action"] = action;
                ViewState["serviceHosts"] = serviceHosts;
                ViewState["serviceUsers"] = serviceUsers;
                initRadioButtons();
                narrowSelections();
                if (!action.Equals(ConfigUtility.ADD_HOSTED_SERVICE))
                    getData();
                else
                    initData();
                buildBindingLists(false);
            }
            else
            {
                UpdateMessageAzureLB.Text = "";
                compositeServiceData = (List<ServiceConfigurationData>)ViewState["CompositeServiceData"];
                SHID = (int)ViewState["SHID"];
                HSID = (int)ViewState["HSID"];
                serviceHosts = (List<MasterServiceHostInstance>)ViewState["serviceHosts"];
                serviceUsers = (List<ServiceUsers>)ViewState["serviceUsers"];
                thisOldService = (HostedServices)ViewState["thisOldService"];
                thisService = (HostedServices)ViewState["thisService"];
                action = (string)ViewState["action"];
                thisServiceHost = serviceHosts.Find(delegate(MasterServiceHostInstance mshExist) { return mshExist.ServiceHostID == SHID; });
            }
            TopNode.PostBackUrl = ConfigSettings.PAGE_NODES;
            ServiceVersion.Text = version;
            ServicePlatform.Text = platform;
            ServiceHoster.Text = hoster;
            TopNodeName.Text = hostNameIdentifier;
            ReturnLabel.Text = "<a class=\"Return\" href=\"" + ConfigSettings.PAGE_HOSTED_SERVICES + "?name=" + hostNameIdentifier + "&cfgSvc=" + configName + "&version=" + version + "&platform=" + platform + "&hoster=" + hoster + "&ID=" + SHID + "&hostname=" + thisServiceHost.HostName + "\">Return to Hosted Services for this Virtual Host</a>";
            GetImageButton.runtimePoweredBy(platform, RuntimePlatform);
            if (action.Equals(ConfigUtility.ADD_HOSTED_SERVICE))
            {
                Update.Enabled = false;
                Delete.Enabled = false;
            }
            else
            {
                Add.Enabled = false;
            }
            thisServiceHost = serviceHosts.Find(delegate(MasterServiceHostInstance mshExist) { return mshExist.ServiceHostID == SHID; });
            LabelClassName.Text = thisServiceHost.ServiceImplementationClass;
            LabelVHostName.Text = thisServiceHost.HostName;
        }

        private void narrowSelections()
        {
            List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
            validateBaseAddresses(thisServiceHostServices, action);
            if (thisServiceHostServices == null || thisServiceHostServices.Count == 0)
            {
                CheckBoxBaseAddress.Checked = true;
                CheckBoxBaseAddress.Enabled = false;
                LabelBaseAddressWarning.Text = "This is the only endpoint so far defined, so it will be a base address.";
            }
            if (thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY || thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_CONFIG)
            {
                RadioButtonListLoadBalanceType.Enabled = true;
                TextBoxLoadBalanceAddress.Enabled = true;
            }
            else
            {
                RadioButtonListLoadBalanceType.Enabled = false;
                TextBoxLoadBalanceAddress.Enabled = false;
            }
            if (thisServiceHost.ServiceType != ConfigUtility.HOST_TYPE_PRIMARY)
            {
                TextBoxOnlineMethod.Enabled = false;
                CheckBoxOnlineParms.Enabled = false;
                TextBoxOnlineParms.Enabled = false;
                TextBoxOnlineMethod.Text = "isOnline";
            }
            if (thisServiceHost.ServiceType != ConfigUtility.HOST_TYPE_NODE)
            {
                CheckBoxFailover.Enabled = false;
            }
            if (thisServiceHost.ServiceType != ConfigUtility.HOST_TYPE_CONFIG)
            {
                CheckBoxAutoSupplyUser.Checked = false;
                CheckBoxAutoSupplyUser.Enabled = false;
                DropDownListCsUser.Enabled = false;
            }
        }

        private void buildBindingLists(bool removeNonHttpsTemplates)
        {
            DropDownListBindingConfig.Items.Clear();
            List<BindingInformation> bindings = new List<BindingInformation>();
            List<ClientInformation> clients = new List<ClientInformation>();
            for (int i = 0; i < compositeServiceData[0].BindingInformation.Count; i++)
            {
                bindings.Add(compositeServiceData[0].BindingInformation[i]);
            }
            for (int i = 0; i < compositeServiceData[0].ClientInformation.Count; i++)
            {
                clients.Add(compositeServiceData[0].ClientInformation[i]);
            }
            switch (thisServiceHost.ServiceType)
            {
                case ConfigUtility.HOST_TYPE_CONFIG:
                    {
                        bindings.RemoveAll(delegate(BindingInformation biExist) { return !(biExist.BindingConfigurationName.ToLower().StartsWith("host_configsvc") || biExist.BindingConfigurationName.ToLower().StartsWith("client_configsvc")); });
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE:
                    {
                        bindings.RemoveAll(delegate(BindingInformation biExist) { return !(biExist.BindingConfigurationName.ToLower().StartsWith("host_nodesvc") || biExist.BindingConfigurationName.ToLower().StartsWith("client_nodesvc")); });
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE_DC:
                    {
                        goto case ConfigUtility.HOST_TYPE_NODE;
                    }

                case ConfigUtility.HOST_TYPE_PRIMARY:
                    {
                        bindings.RemoveAll(delegate(BindingInformation biExist) { return (biExist.BindingConfigurationName.ToLower().StartsWith("host_nodesvc") || biExist.BindingConfigurationName.ToLower().StartsWith("client_nodesvc") || biExist.BindingConfigurationName.ToLower().StartsWith("host_configsvc") || biExist.BindingConfigurationName.ToLower().StartsWith("client_configsvc")); });
                        break;
                    }
            }
            if (removeNonHttpsTemplates)
            {
                bindings.RemoveAll(delegate(BindingInformation biExist) { return !biExist.BindingConfigurationName.ToLower().Contains("t_security"); });
            }
            string bindingType = RadioButtonListBindingType.SelectedValue;
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].BindingConfigurationName.ToLower().StartsWith("host") && bindingType.Equals(bindings[i].BindingType))
                    DropDownListBindingConfig.Items.Add(new ListItem(bindings[i].BindingConfigurationName, bindings[i].BindingConfigurationName));
            }
            if (action != ConfigUtility.ADD_HOSTED_SERVICE)
            {
                try
                {
                    DropDownListBindingConfig.SelectedValue = thisService.ServiceBinding;
                }
                catch
                {
                }
            }
            buildInternalConfigNames(bindings, clients);
        }

        private void buildInternalConfigNames()
        {
            compositeServiceData = (List<ServiceConfigurationData>)ViewState["CompositeServiceData"];
            List<BindingInformation> bindings = new List<BindingInformation>();
            List<ClientInformation> clients = new List<ClientInformation>();
            for (int i = 0; i < compositeServiceData[0].BindingInformation.Count; i++)
            {
                bindings.Add(compositeServiceData[0].BindingInformation[i]);
            }
            for (int i = 0; i < compositeServiceData[0].ClientInformation.Count; i++)
            {
                clients.Add(compositeServiceData[0].ClientInformation[i]);
            }
            buildInternalConfigNames(bindings, clients);
        }

        private void buildInternalConfigNames(List<BindingInformation> bindings, List<ClientInformation> clients)
        {
            string bindingType = RadioButtonListBindingType.SelectedValue;
            string scheme = ConfigUtility.getBindingScheme(bindingType, CheckBoxHttps.Checked);
            DropDownListInternalClient.Items.Clear();
            DropDownListInternalClient.Items.Add(new ListItem(ConfigUtility.NO_CLIENT_CONFIGURATION, ConfigUtility.NO_CLIENT_CONFIGURATION));
            for (int i = 0; i < clients.Count; i++)
            {
                BindingInformation theBinding = bindings.Find(delegate(BindingInformation biExist) { return biExist.BindingConfigurationName.Equals(clients[i].BindingConfiguration); });
                if (theBinding != null)
                {

                    bool transportSec = DropDownListBindingConfig.SelectedValue.ToLower().Contains("t_security");
                    bool myBindingTransportSec = clients[i].ElementName.ToLower().Contains("t_security");
                    bool messageSec = DropDownListBindingConfig.SelectedValue.ToLower().Contains("m_security");
                    bool myBindingMessageSec = clients[i].ElementName.ToLower().Contains("m_security");
                    if (theBinding.BindingConfigurationName.ToLower().StartsWith("client"))
                    {
                        bool check = (transportSec == myBindingTransportSec && messageSec == myBindingMessageSec && clients[i].Contract.Equals(DropDownListContracts.SelectedValue));
                        switch (bindingType)
                        {
                            case ConfigUtility.WEB_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.WEB_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.WS_DUAL_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_DUAL_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }


                            case ConfigUtility.WS_FEDERATED_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_FEDERATED_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.WS_2007FEDERATED_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_2007FEDERATED_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }


                            case ConfigUtility.BASIC_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.BASIC_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.BASIC_HTTP_CONTEXT_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }
                            case ConfigUtility.WS_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.WS_2007_HTTP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_2007_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.WS_HTTP_CONTEXT_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.WS_HTTP_CONTEXT_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.CUSTOM_BINDING_HTTP:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_HTTP) || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || theBinding.BindingType.Equals(ConfigUtility.WEB_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_2007FEDERATED_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_FEDERATED_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_DUAL_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING) || theBinding.BindingType.Equals(ConfigUtility.BASIC_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_HTTP_CONTEXT_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_HTTP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.WS_2007_HTTP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }
                            case ConfigUtility.CUSTOM_BINDING_WSHTTP:
                                {
                                    goto case ConfigUtility.CUSTOM_BINDING_HTTP;
                                }

                            case ConfigUtility.CUSTOM_BINDING_TCP:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_TCP) || theBinding.BindingType.Equals(ConfigUtility.NET_TCP_BINDING) || theBinding.BindingType.Equals(ConfigUtility.NET_TCP_CONTEXT_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }
                            case ConfigUtility.CUSTOM_BINDING_NET_MSMQ:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_NET_MSMQ) || theBinding.BindingType.Equals(ConfigUtility.NET_MSMQ_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.NET_MSMQ_BINDING:
                                {
                                    goto case ConfigUtility.CUSTOM_BINDING_NET_MSMQ;
                                }

                            case ConfigUtility.NET_TCP_BINDING:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_TCP) || theBinding.BindingType.Equals(ConfigUtility.NET_TCP_BINDING)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.NET_TCP_CONTEXT_BINDING:
                                {
                                    if (check && (theBinding.BindingType == ConfigUtility.NET_TCP_CONTEXT_BINDING || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_TCP)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE:
                                {
                                    if (check && (theBinding.BindingType.Equals(ConfigUtility.NET_NAME_PIPE_BINDING) || theBinding.BindingType.Equals(ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE)))
                                        DropDownListInternalClient.Items.Add(new ListItem(clients[i].ElementName, clients[i].ElementName));
                                    break;
                                }

                            case ConfigUtility.NET_NAME_PIPE_BINDING:
                                {
                                    goto case ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE;
                                }

                            default:
                                {
                                    goto case ConfigUtility.CUSTOM_BINDING_HTTP;
                                }
                        }
                    }
                }
            }
            if (action != ConfigUtility.ADD_HOSTED_SERVICE)
            {
                try
                {
                    DropDownListInternalClient.SelectedValue = thisService.InternalClientConfiguration;
                }
                catch
                {
                    DropDownListInternalClient.SelectedIndex = 0;
                }
            }
        }

        private void initRadioButtons()
        {
            RadioButtonListServiceType.Items.Add(new ListItem("Primary Service", ConfigUtility.HOST_TYPE_PRIMARY.ToString()));
            RadioButtonListServiceType.Items.Add(new ListItem("Configuration Service", ConfigUtility.HOST_TYPE_CONFIG.ToString()));
            RadioButtonListServiceType.Items.Add(new ListItem("Node Service", ConfigUtility.HOST_TYPE_NODE.ToString()));
            RadioButtonListServiceType.Items.Add(new ListItem("Node Distributed Cache Service", ConfigUtility.HOST_TYPE_NODE_DC.ToString()));
            RadioButtonListServiceType.SelectedValue = thisServiceHost.ServiceType.ToString();
            if (thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY)
            {
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.CUSTOM_BINDING_HTTP, ConfigUtility.CUSTOM_BINDING_HTTP));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.CUSTOM_BINDING_WSHTTP, ConfigUtility.CUSTOM_BINDING_WSHTTP));
            }
            if (!thisServiceHost.IsWorkFlowServiceHost)
            {
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.CUSTOM_BINDING_TCP, ConfigUtility.CUSTOM_BINDING_TCP));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.BASIC_HTTP_BINDING, ConfigUtility.BASIC_HTTP_BINDING));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_HTTP_BINDING, ConfigUtility.WS_HTTP_BINDING));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_2007_HTTP_BINDING, ConfigUtility.WS_2007_HTTP_BINDING));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.NET_TCP_BINDING, ConfigUtility.NET_TCP_BINDING));
                if (thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY)
                {
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING, ConfigUtility.BASIC_HTTP_CONTEXT_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WEB_HTTP_BINDING, ConfigUtility.WEB_HTTP_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_DUAL_HTTP_BINDING, ConfigUtility.WS_DUAL_HTTP_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_FEDERATED_HTTP_BINDING, ConfigUtility.WS_FEDERATED_HTTP_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_2007FEDERATED_HTTP_BINDING, ConfigUtility.WS_2007FEDERATED_HTTP_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.NET_TCP_CONTEXT_BINDING, ConfigUtility.NET_TCP_CONTEXT_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.NET_MSMQ_BINDING, ConfigUtility.NET_MSMQ_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.NET_NAME_PIPE_BINDING, ConfigUtility.NET_NAME_PIPE_BINDING));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.CUSTOM_BINDING_NET_MSMQ, ConfigUtility.CUSTOM_BINDING_NET_MSMQ));
                    RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE, ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE));
                }
                RadioButtonListBindingType.SelectedValue = ConfigUtility.BASIC_HTTP_BINDING;
            }
            else
            {
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING, ConfigUtility.BASIC_HTTP_CONTEXT_BINDING));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.WS_HTTP_CONTEXT_BINDING, ConfigUtility.WS_HTTP_CONTEXT_BINDING));
                RadioButtonListBindingType.Items.Add(new ListItem(ConfigUtility.NET_TCP_CONTEXT_BINDING, ConfigUtility.NET_TCP_CONTEXT_BINDING));
                RadioButtonListBindingType.SelectedValue = ConfigUtility.BASIC_HTTP_CONTEXT_BINDING;
            }
            RadioButtonListLoadBalanceType.Items.Add(new ListItem("Normal: Host Name/IP Address", ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS.ToString()));
            RadioButtonListLoadBalanceType.Items.Add(new ListItem("External Load Balancer/Virtual Address", ConfigUtility.LOAD_BALANCE_TYPE_VIRTUAL_ADDRESS.ToString()));
            for (int i = 0; i < serviceUsers.Count; i++)
            {
                if (serviceUsers[i].Rights == ConfigUtility.CONFIG_CONNECTED_SERVICE_RIGHTS)
                    DropDownListCsUser.Items.Add(new ListItem(serviceUsers[i].UserId, serviceUsers[i].UserKey.ToString()));
            }
            if (action != ConfigUtility.ADD_HOSTED_SERVICE)
            {
                for (int i = 0; i < thisService.ServiceContracts.Count; i++)
                {
                    DropDownListContracts.Items.Add(new ListItem(thisService.ServiceContracts[i], thisService.ServiceContracts[i]));
                }
            }
            if (action == ConfigUtility.UPDATE_HOSTED_SERVICE)
                DropDownListContracts.Enabled = false;
        }

        private void initData()
        {
            CheckBoxActivate.Checked = true;
            TextBoxOnlineMethod.Text = "isOnline";
            CheckBoxAutoSupplyUser.Checked = true;
            DropDownListIdentity.Enabled = false;
            DropDownListEPB.Enabled = false;
            DropDownListEPB.Items.Clear();
            DropDownListIdentity.Items.Clear();
            ServiceHostInfo thisHostInfo = compositeServiceData[0].ServiceHostInfoList.Find(delegate(ServiceHostInfo shiExist) { return shiExist.ServiceHostID == SHID; });
            if (thisHostInfo != null)
            {
                for (int i = 0; i < thisHostInfo.ServiceContractNames.Count; i++)
                {
                    DropDownListContracts.Items.Add(new ListItem(thisHostInfo.ServiceContractNames[i], thisHostInfo.ServiceContractNames[i]));
                }
                for (int i = 0; i < thisHostInfo.ServiceBehaviorNames.Count; i++)
                {
                    DropDownListEPB.Items.Add(new ListItem(thisHostInfo.ServiceBehaviorNames[i], thisHostInfo.ServiceBehaviorNames[i]));
                }
                if (DropDownListEPB.Items.Count == 0)
                {
                    DropDownListEPB.Enabled = false;
                    LabelEPB.Text = "No Endpoint Behaviors were initialized as part of your service startup procedure.  Hence, you cannot select any endpoint behaviors at this time.";
                    CheckBoxEPB.Enabled = false;
                }
                for (int i = 0; i < compositeServiceData[0].EndpointIdentityNames.Count; i++)
                {
                    DropDownListIdentity.Items.Add(new ListItem(compositeServiceData[0].EndpointIdentityNames[i], compositeServiceData[0].EndpointIdentityNames[i]));
                }
                if (DropDownListIdentity.Items.Count == 0)
                {
                    DropDownListEPB.Enabled = false;
                    LabelIdentity.Text = "No Endpoint identities have been configured in the config file section <endpointIdentities>. Hence, you cannot select any endpoint identities at this time, default values will be used.";
                    CheckBoxIdentity.Enabled = false;
                }
                RadioButtonListLoadBalanceType.SelectedIndex = 0;
                TextBoxLoadBalanceAddress.Enabled = false;
                if (thisServiceHost.IsWorkFlowServiceHost)
                    WorkFlow.Text = "Yes";
                else
                    WorkFlow.Text = "False";
            }
            else
            {
                UpdateMessage.Text = "<br/><span style=\"color:Maroon\">Cannot Find a Valid Initialized ServiceHost for this Defined ServiceHost.  Have you properly included this ServiceHost in your startup procedure?</span>";
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                Add.Enabled = false;
                Update.Enabled = false;
                Delete.Enabled = false;
            }
        }

        private void getData()
        {
            RadioButtonListBindingType.SelectedValue = thisService.BindingType;
            if (thisService.EndPointBehavior != ConfigUtility.NO_ENDPOINT_BEHAVIOR)
            {
                CheckBoxEPB.Checked = true;
                DropDownListEPB.Enabled = true;
            }
            else
            {
                DropDownListEPB.Enabled = false;
            }
            if (thisService.IdentityConfigName != ConfigUtility.NO_ENDPOINT_IDENTITY)
                CheckBoxIdentity.Checked = true;
            else
                DropDownListIdentity.Enabled = false;
            switch (thisService.ServiceType)
            {
                case ConfigUtility.HOST_TYPE_PRIMARY:
                    {
                        RadioButtonListServiceType.SelectedIndex = 0;
                        CheckBoxFailover.Enabled = false;
                        CheckBoxAutoSupplyUser.Enabled = false;
                        break;
                    }
                case ConfigUtility.HOST_TYPE_CONFIG:
                    {
                        RadioButtonListServiceType.SelectedIndex = 1;
                        CheckBoxFailover.Enabled = false;
                        CheckBoxAutoSupplyUser.Enabled = true;
                        break;
                    }
                case ConfigUtility.HOST_TYPE_NODE:
                    {
                        RadioButtonListServiceType.SelectedIndex = 2;
                        CheckBoxAutoSupplyUser.Enabled = false;
                        break;
                    }
            }
            switch (thisService.LoadBalanceType)
            {
                case ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS: { RadioButtonListLoadBalanceType.SelectedIndex = 0; TextBoxLoadBalanceAddress.Enabled = false; break; }
                case ConfigUtility.LOAD_BALANCE_TYPE_VIRTUAL_ADDRESS: { RadioButtonListLoadBalanceType.SelectedIndex = 1; TextBoxLoadBalanceAddress.Enabled = true; TextBoxLoadBalanceAddress.Text = thisService.LoadBalanceAddress; break; }
            }
            if (thisServiceHost.IsWorkFlowServiceHost)
                WorkFlow.Text = "Yes";
            else
                WorkFlow.Text = "False";
            switch (RadioButtonListBindingType.SelectedValue)
            {
                case ConfigUtility.BASIC_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WEB_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_2007_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_DUAL_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_FEDERATED_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_2007FEDERATED_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.NET_TCP_BINDING: { CheckBoxHttps.Enabled = false; break; }
                case ConfigUtility.NET_MSMQ_BINDING: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.NET_NAME_PIPE_BINDING: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_HTTP: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.CUSTOM_BINDING_WSHTTP: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.CUSTOM_BINDING_TCP: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_NET_MSMQ: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.BASIC_HTTP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.WS_HTTP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.NET_TCP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = false; break; };
            }
            TextBoxHostedServiceName.Text = thisService.FriendlyName;
            TextBoxVirtualPath.Text = thisService.VirtualPath;
            TextBoxPort.Text = thisService.Port;
            if (thisService.IsBaseAddress)
                CheckBoxBaseAddress.Checked = true;
            if (thisService.mex)
                CheckBoxMex.Checked = true;
            if (thisService.UseHttps)
                CheckBoxHttps.Checked = true;
            if (thisService.MarkedForFailover)
                CheckBoxFailover.Checked = true;
            if (thisService.csUserKey > 0)
            {
                CheckBoxAutoSupplyUser.Checked = true;
                try
                {
                    DropDownListCsUser.SelectedValue = thisService.csUserKey.ToString();
                }
                catch { }
            }
            else
                DropDownListCsUser.Enabled = false;
            if (thisService.EndPointBehavior != ConfigUtility.NO_ENDPOINT_BEHAVIOR)
            {
                try
                {
                    DropDownListEPB.SelectedValue = thisService.EndPointBehavior;
                }
                catch { }
            }
            if (thisService.IdentityConfigName != ConfigUtility.NO_ENDPOINT_IDENTITY)
            {
                try
                {
                    DropDownListIdentity.SelectedValue = thisService.IdentityConfigName;
                }
                catch { }
            }
            if (thisService.Activated)
                CheckBoxActivate.Checked = true;
            TextBoxOnlineMethod.Text = thisService.OnlineMethod;
            TextBoxOnlineParms.Text = thisService.OnlineParms;
            if (thisService.OnlineParms == ConfigUtility.NO_ONLINE_PARMS)
            {
                CheckBoxOnlineParms.Checked = false;
                TextBoxOnlineParms.Enabled = false;
            }
            else
            {
                CheckBoxOnlineParms.Checked = true;
                TextBoxOnlineParms.Enabled = true;
            }
            if (DropDownListContracts.Items.Count > 0)
                DropDownListContracts.SelectedValue = thisService.ServiceContract;
        }

        private bool validateBaseAddresses(List<HostedServices> thisServiceHostServices, string action)
        {
            LabelBaseAddressWarning.Text = "";
            UpdateMessage.Text = "";
            string scheme = ConfigUtility.getBindingScheme(RadioButtonListBindingType.SelectedValue, CheckBoxHttps.Checked);
            if (scheme != "https" && scheme != "http")
                CheckBoxHttps.Enabled = false;
            if (scheme == "net.msmq")
            {
                CheckBoxMex.Enabled = false;
                TextBoxPort.Text = "0";
                TextBoxPort.Enabled = false;
            }
            bool isBase = CheckBoxBaseAddress.Checked;
            List<HostedServices> checkList = thisServiceHostServices.FindAll(delegate(HostedServices hsExist) { return hsExist.HostedServiceID != HSID; });
            if (isBase)
            {
                for (int i = 0; i < checkList.Count; i++)
                {
                    string checkScheme = ConfigUtility.getBindingScheme(checkList[i].BindingType, checkList[i].UseHttps);
                    switch (action)
                    {
                        case ConfigUtility.REMOVE_HOSTED_SERVICE:
                            {
                                scheme = ConfigUtility.getBindingScheme(thisService.BindingType, CheckBoxHttps.Checked);
                                if (scheme.Equals(checkScheme) && checkList[i].Activated && CheckBoxActivate.Checked)
                                {
                                    LabelBaseAddressWarning.Text = "You have one or more relative addresses defined for this scheme, and this endpointe is a base address.  You must delete your relative address for scheme '" + scheme + "' before you can delete this base address.";
                                    LabelBaseAddressWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                                    UpdateMessage.Text = "You have one or more relative addresses defined for this scheme, and this endpointe is a base address.  You must delete your relative address for scheme '" + scheme + "' before you can delete this base address.";
                                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                    return false;
                                }
                                break;
                            }
                        default:
                            {
                                scheme = ConfigUtility.getBindingScheme(RadioButtonListBindingType.SelectedValue, CheckBoxHttps.Checked);
                                if (scheme.Equals(checkScheme))
                                {
                                    if (checkList[i].IsBaseAddress && checkList[i].Activated && CheckBoxActivate.Checked)
                                    {
                                        LabelBaseAddressWarning.Text = "You already have defined a base address for this scheme! To make this endpoint a base address, delete/modify your existing base address for this binding scheme first, or change the scheme by changing the selected host endpoint binding type. Otherwise, corrections have already been made and you can continue!";
                                        LabelBaseAddressWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                                        UpdateMessage.Text = "You already have defined a base address for this scheme! To make this endpoint a base address, delete/modify your existing base address for this binding scheme first, or change the scheme by changing the selected host endpoint binding type. Otherwise, corrections have already been made and you can continue!";
                                        UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                        CheckBoxBaseAddress.Checked = false;
                                        TextBoxPort.Text = checkList[i].Port;
                                        TextBoxPort.Enabled = false;
                                        return false;
                                    }
                                }
                                if (action != ConfigUtility.ADD_HOSTED_SERVICE)
                                {
                                    string originalScheme = ConfigUtility.getBindingScheme(thisService.BindingType, thisService.UseHttps);
                                    if (!originalScheme.Equals(scheme))
                                    {
                                        if (thisService.IsBaseAddress && checkScheme.Equals(originalScheme) && thisService.Activated)
                                        {
                                            LabelBaseAddressWarning.Text = "You are attempting to change the scheme for an active endpoint that is a base address, with other relative addresses for this scheme defined.  Please delete all relative addresses for the scheme '" + originalScheme + "' first.";
                                            LabelBaseAddressWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                                            UpdateMessage.Text = "You are attempting to change the scheme for an endpoint that is a base address, with other relative addresses for this scheme defined.  Please delete all relative addresses for the scheme '" + originalScheme + "' first.";
                                            UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                            return false;
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                if (scheme != "net.msmq")
                    TextBoxPort.Enabled = true;
                return true;
            }
            else
            {
                bool foundBase = false;
                for (int i = 0; i < checkList.Count; i++)
                {
                    string checkScheme = ConfigUtility.getBindingScheme(checkList[i].BindingType, checkList[i].UseHttps);
                    if (checkScheme.Equals(scheme))
                    {
                        if (checkList[i].IsBaseAddress)
                        {
                            foundBase = true;
                            TextBoxPort.Text = checkList[i].Port;
                            TextBoxPort.Enabled = true;
                        }
                    }
                }
                if (!foundBase)
                {
                    LabelBaseAddressWarning.Text = "You must define a base address for this scheme, and this is the first endpoint for this scheme. Thus, this must be a base address.  Remember, if hosting in IIS, the VirtualPath for the base address must exactly match the virtual directory name in IIS + the .svc filename.";
                    LabelBaseAddressWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    CheckBoxBaseAddress.Checked = true;
                    if (scheme != "net.msmq")
                        TextBoxPort.Enabled = true;
                    return true;
                }
                else
                    return true;
            }
        }

        //Checks for warning on port sharing if two different service hosts try to use the same port with different network schemes (protocols).
        private bool checkPorts()
        {
            string scheme = ConfigUtility.getBindingScheme(RadioButtonListBindingType.SelectedValue, CheckBoxHttps.Checked);
            string port = TextBoxPort.Text;
            int thisSHID = thisService.ServiceHostID;
            for (int i = 0; i < compositeServiceData[0].HostedServices.Count; i++)
            {
                if (port == compositeServiceData[0].HostedServices[i].Port)
                {
                    if (action.Equals(ConfigUtility.ADD_HOSTED_SERVICE))
                    {
                        if ( compositeServiceData[0].HostedServices[i].Activated && scheme != ConfigUtility.getBindingScheme(compositeServiceData[0].HostedServices[i].BindingType, compositeServiceData[0].HostedServices[i].UseHttps))
                            return false;
                    }
                    else
                        if (action.Equals(ConfigUtility.UPDATE_HOSTED_SERVICE))
                            if (compositeServiceData[0].HostedServices[i].HostedServiceID != thisService.HostedServiceID && scheme != ConfigUtility.getBindingScheme(compositeServiceData[0].HostedServices[i].BindingType, CheckBoxHttps.Checked) && compositeServiceData[0].HostedServices[i].Activated)
                                return false;
                }
            }
            return true;
        }

        private void processUpdate()
        {
            if (!checkPorts())
            {
                UpdateMessage.Text = UpdateMessage.Text + "<br/><span style=\"color:Yellow \">You are already using the same port for another endpoint with a different" +
                " network scheme (protocol) within this service host. You must choose a different port.</span>";
                return;
            }
            int success = ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS;
            UpdateMessage.Text = "";
            LabelBaseAddressWarning.Text = "";
            List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
            if (!validateBaseAddresses(thisServiceHostServices, action))
                return;
            Page.Validate();
            if (!Page.IsValid)
                return;
            if (DropDownListBindingConfig.Items.Count == 0 || DropDownListInternalClient.Items.Count == 0)
            {
                UpdateMessage.Text = "You must ensure this host has valid Binding Configurations for both the host binding and the internal client binding defined in it's config file.  Currently, for the selected binding type, this is not the case. Please see tutorial: Binding Configurations names must be prefaced with 'Host_' and 'Client_' to appear properly in this page.";
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            HostedServices updateHostedService = new HostedServices();
            if (TextBoxOnlineMethod.Text.Trim() == null || TextBoxOnlineMethod.Text.Trim() == "")
                updateHostedService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
            else
                updateHostedService.OnlineMethod = TextBoxOnlineMethod.Text;
            if (CheckBoxOnlineParms.Checked)
                updateHostedService.OnlineParms = TextBoxOnlineParms.Text;
            else
                updateHostedService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
            switch (thisServiceHost.ServiceType)
            {
                case ConfigUtility.HOST_TYPE_CONFIG:
                    {
                        if (action == ConfigUtility.REMOVE_HOSTED_SERVICE || action == ConfigUtility.UPDATE_HOSTED_SERVICE)
                        {
                            List<HostedServices> configList = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceType == ConfigUtility.HOST_TYPE_CONFIG; });
                            if (configList.Count < 2 && action == ConfigUtility.REMOVE_HOSTED_SERVICE)
                            {
                                if (action == ConfigUtility.REMOVE_HOSTED_SERVICE || !CheckBoxActivate.Checked)
                                    UpdateMessage.Text = "You must have at least one active Configuration Service Endpoint!";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                CheckBoxActivate.Checked = true;
                                return;
                            }
                        }
                        break;
                    }
                case ConfigUtility.HOST_TYPE_NODE:
                    {
                        ConfigurationKeyValues activeNodeID = compositeServiceData[0].ConfigurationData.Find(delegate(ConfigurationKeyValues keyExist) { return keyExist.ConfigurationKeyFieldName == "NODE_ACTIVE_SERVICE_ID"; });
                        if (activeNodeID.ConfigurationKeyValue.Equals(thisService.HostedServiceID.ToString()))
                        {
                            if (!CheckBoxActivate.Checked)
                            {
                                UpdateMessage.Text = "You cannot deactivate the configured Active Node Endpoint!";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                CheckBoxActivate.Checked = true;
                                return;
                            }
                            if (action == ConfigUtility.REMOVE_HOSTED_SERVICE)
                            {
                                UpdateMessage.Text = "You cannot delete the configured Active Node Endpoint!";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                return;
                            }
                            if (DropDownListInternalClient.SelectedValue.Equals(ConfigUtility.NO_CLIENT_CONFIGURATION))
                            {

                                UpdateMessage.Text = "You cannot define an Active Node Service Endpoint without selecting a valid Internal Client Configuration from your existing definitions in the applications 'client' section of config.  Please either select an existing client configuration above, or add a new client definition to your configuration if needed.";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                return;
                            }
                        }
                        break;
                    }

                case ConfigUtility.HOST_TYPE_NODE_DC:
                    {
                        ConfigurationKeyValues activeNodeDCID = compositeServiceData[0].ConfigurationData.Find(delegate(ConfigurationKeyValues keyExist) { return keyExist.ConfigurationKeyFieldName == "NODE_ACTIVE_DC_SERVICE_ID"; });
                        if (activeNodeDCID.ConfigurationKeyValue.Equals(thisService.HostedServiceID.ToString()))
                        {
                            if (!CheckBoxActivate.Checked)
                            {
                                UpdateMessage.Text = "You cannot deactivate the configured Active Node Distributed Cache Endpoint!";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                CheckBoxActivate.Checked = true;
                                return;
                            }
                            if (action == ConfigUtility.REMOVE_HOSTED_SERVICE)
                            {
                                UpdateMessage.Text = "You cannot delete the configured Active Node Distributed Cache Endpoint!";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                return;
                            }
                            if (DropDownListInternalClient.SelectedValue.Equals(ConfigUtility.NO_CLIENT_CONFIGURATION))
                            {

                                UpdateMessage.Text = "You cannot define an Active Node DC Service Endpoint without selecting a valid Internal Client Configuration from your existing definitions in the applications 'client' section of config.  Please either select an existing client configuration above, or add a new client definition to your configuration if needed.";
                                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                                return;
                            }
                        }
                        break;
                    }
            }
            if (CheckBoxActivate.Checked)
                updateHostedService.Activated = true;
            else
                updateHostedService.Activated = false;
            updateHostedService.BindingType = RadioButtonListBindingType.SelectedValue;
            updateHostedService.UseHttps = CheckBoxHttps.Checked;
            updateHostedService.VirtualPath = TextBoxVirtualPath.Text.ToLower().Trim().Trim(new char[] { '/' });
            TextBoxVirtualPath.Text = updateHostedService.VirtualPath;
            int port = -1;
            try
            {
                port = Convert.ToInt32(TextBoxPort.Text.Trim().Trim(new char[] { ':' }));
            }
            catch
            {
                UpdateMessage.Text = "You must specify a port in the form of an integer!";
                UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                return;
            }
            if (updateHostedService.BindingType != ConfigUtility.NET_MSMQ_BINDING && updateHostedService.BindingType != ConfigUtility.CUSTOM_BINDING_NET_MSMQ)
            {
                if (port < 80 || port > 65534)
                {
                    UpdateMessage.Text = "You must specify a port between 80 and 65534!";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
            }
            if (!CheckBoxIdentity.Checked)
                updateHostedService.IdentityConfigName = ConfigUtility.NO_ENDPOINT_IDENTITY;
            else
                updateHostedService.IdentityConfigName = DropDownListIdentity.SelectedValue;
            updateHostedService.Port = TextBoxPort.Text.Trim().Trim(new char[] { ':' });
            TextBoxPort.Text = updateHostedService.Port;
            if (thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY || thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_CONFIG)
            {
                int userKey = -1;
                if (DropDownListCsUser.Items.Count > 0)
                    userKey = Convert.ToInt32(DropDownListCsUser.SelectedValue);
                updateHostedService.csUserKey = userKey;
            }
            else
                updateHostedService.csUserKey = -1;
            string scheme = ConfigUtility.getBindingScheme(updateHostedService.BindingType, updateHostedService.UseHttps);
            string defaultAddress = null;
            if (DropDownListEPB.Items.Count == 0 || !CheckBoxEPB.Checked)
                updateHostedService.EndPointBehavior = ConfigUtility.NO_ENDPOINT_BEHAVIOR;
            else
                if (DropDownListEPB.Items.Count == 0)
                    updateHostedService.EndPointBehavior = ConfigUtility.NO_ENDPOINT_BEHAVIOR;
                else
                    updateHostedService.EndPointBehavior = DropDownListEPB.SelectedValue;
            updateHostedService.FriendlyName = TextBoxHostedServiceName.Text.Trim();
            if (action != ConfigUtility.ADD_HOSTED_SERVICE)
                updateHostedService.HostedServiceID = thisService.HostedServiceID;
            else
                updateHostedService.HostedServiceID = -1;
            updateHostedService.HostName = LabelVHostName.Text;
            updateHostedService.InternalClientConfiguration = DropDownListInternalClient.SelectedValue;
            updateHostedService.IsBaseAddress = CheckBoxBaseAddress.Checked;
            if (thisServiceHost.ServiceType != ConfigUtility.HOST_TYPE_NODE && thisServiceHost.ServiceType != ConfigUtility.HOST_TYPE_NODE_DC)
                updateHostedService.LoadBalanceType = Convert.ToInt32(RadioButtonListLoadBalanceType.SelectedValue);
            else
                updateHostedService.LoadBalanceType = ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS;
            if (updateHostedService.LoadBalanceType == ConfigUtility.LOAD_BALANCE_TYPE_VIRTUAL_ADDRESS)
            {
                string lba = TextBoxLoadBalanceAddress.Text.ToLower().Trim().Trim(new char[] { '/' });
                if (lba == null || lba.Length < 1)
                {
                    UpdateMessage.Text = "If using external load balancing, you must supply the load balanced address!";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                updateHostedService.LoadBalanceAddress = lba;
            }
            else
                updateHostedService.LoadBalanceAddress = "Not Implemented";
            if (thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY)
                if (updateHostedService.LoadBalanceType.Equals(ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS))
                    defaultAddress = updateHostedService.LoadBalanceAddress;
                else
                    defaultAddress = "[enter servername here]";
            else
                defaultAddress = "none";
            updateHostedService.DefaultAddress = defaultAddress;
            updateHostedService.MarkedForFailover = CheckBoxFailover.Checked;
            updateHostedService.mex = CheckBoxMex.Checked;
            updateHostedService.OnlineMethod = TextBoxOnlineMethod.Text.Trim();
            if (updateHostedService.OnlineMethod == null || updateHostedService.OnlineMethod == "")
                updateHostedService.OnlineMethod = ConfigUtility.NO_ONLINE_METHOD;
            if (CheckBoxOnlineParms.Checked)
            {
                string parms = TextBoxOnlineParms.Text.Trim();
                if (parms == null || parms.Length < 1)
                {
                    UpdateMessage.Text = "If your online method expects parameters as you indicate, you must supply here in form of delimited list like 'string=strvalue;int=6;bool=true' or combination thereof.";
                    UpdateMessage.ForeColor = System.Drawing.Color.Maroon;
                    return;
                }
                updateHostedService.OnlineParms = parms;
            }
            else
                updateHostedService.OnlineParms = ConfigUtility.NO_ONLINE_PARMS;
            updateHostedService.ServiceBinding = DropDownListBindingConfig.SelectedValue;
            updateHostedService.ServiceHostID = SHID;
            updateHostedService.ServiceImplementationClassName = thisServiceHost.ServiceImplementationClass;
            updateHostedService.ServiceType = thisServiceHost.ServiceType;
            updateHostedService.ServiceContract = DropDownListContracts.SelectedValue;
            traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy,  address, binding, user);

            //OK, enough checking for valid settings---lets do it! Make the call to update or add or delete this endpoint!!
            success = configProxy.receiveService(hostNameIdentifier, configName, thisOldService, updateHostedService, null, null, null, null, null, true, action, traversePath, user);
            if (success == ConfigUtility.CLUSTER_UPDATE_FULL_SUCCESS)
            {
                Thread.Sleep(5000);
                if (compositeServiceData[0].RunTimePlatform.ToLower().Contains("azure"))
                {
                    if (!updateHostedService.LoadBalanceType.Equals(ConfigUtility.LOAD_BALANCE_TYPE_VIRTUAL_ADDRESS) && updateHostedService.ServiceType != ConfigUtility.HOST_TYPE_NODE)
                    {
                        UpdateMessageAzureLB.Text = "<br/><span style=\"color:#FFFF99;\">WARNING: You are hosting on Windows Azure so typically, you should mark your Configuration Service and Primary endpoints as externally load balanced on this page; and supply " +
                            "the Azure hosted service DNS base address + port for this endpoint.  Windows Azure will <strong>always</strong> externally load balance input endpoints.  By not specifiying the external load balance address and port" +
                            " (for example azurestocktraderbsl.cloudapp.net:443) on this page, Configuration Service may not work properly. It is very important to get the address and port exactly right.  The update has gone through, " +
                            "but after considering this message, you can make any adjustments and update the endpoint again with the correct Azure public-facing address + port.  If this is an internal endpoint, you can ignore this message.</span>";
                    }
                }
                try
                {
                    configProxy.Channel = null;
                    traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                    compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                }
                catch (Exception)
                {
                    //Try again for good measure.
                    Thread.Sleep(3000);
                    try
                    {
                        traversePath = DynamicTraversePath.getTraversePath(hostNameIdentifier, configName, ref configProxy, address, binding, user);
                        compositeServiceData = null;
                        compositeServiceData = configProxy.getServiceConfiguration(hostNameIdentifier, configName, ConfigUtility.CONFIG_LEVEL_BASIC, false, traversePath, user);
                        if (compositeServiceData == null || compositeServiceData[0] == null)
                        {
                            UpdateMessage.Text = "<br/><span style=\"color:Maroon\">Error! The Configuration Service was not able to restart with your new definition.  Please check the event log.  You will" +
                                "not be able to continue until this is fixed, since ConfigWeb has no way to establish a connection to the Configuration Service, since it is no longer running at the remote host(s).</span>";
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        UpdateMessage.Text = "<br/><span style=\"color:Maroon\">Error! The Configuration Service was not able to restart with your new definition.  Please check the event log.  You will" +
                               "not be able to continue until you login in again.  If there is an errror starting the Configuration Service Host, you will not be able to login until this issue is fixed.</span>";
                        SessionInfo info = new SessionInfo();
                        info.setSessionData(null, null, null, null, null, null, null, null, null);
                        return;
                    }
                }
                switch (action)
                {
                    case ConfigUtility.ADD_HOSTED_SERVICE:
                        {
                            if (compositeServiceData != null)
                            {
                                thisService = compositeServiceData[0].HostedServices.Find(delegate(HostedServices hsExist) { return hsExist.FriendlyName == updateHostedService.FriendlyName && hsExist.ServiceHostID == SHID; });
                                //thisService = updateHostedService;
                                if (thisService != null)
                                {
                                    thisOldService = thisService;
                                    SHID = thisService.ServiceHostID;
                                    ViewState["SHID"] = SHID;
                                    HSID = thisService.HostedServiceID;
                                    ViewState["HSID"] = HSID;
                                    ViewState["CompositeServiceData"] = compositeServiceData;
                                    ViewState["thisOldService"] = thisService;
                                    ViewState["thisService"] = thisService;
                                    action = ConfigUtility.UPDATE_HOSTED_SERVICE;
                                    ViewState["action"] = action;
                                    Add.Enabled = false;
                                    Update.Enabled = true;
                                    Delete.Enabled = true;
                                }
                            }
                            break;
                        }
                    case ConfigUtility.REMOVE_HOSTED_SERVICE:
                        {
                            Add.Enabled = false;
                            Update.Enabled = false;
                            Delete.Enabled = false;
                            break;
                        }
                }
                if (compositeServiceData!=null)
                    UpdateMessage.Text = "<br/><span style=\"color:PaleGreen \">The Hosted Service definition was sucessfully " + actiontext + ".</span>";
                else
                    UpdateMessage.Text = "<br/><span style=\"color:Maroon \">The Hosted Service definition was sucessfully sent.  However, there was an error retrieving updated results.</span>";
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
                thisOldService = (HostedServices)ViewState["thisOldService"];
                thisService = thisOldService;
                ViewState["thisService"] = thisService;
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            processing = (bool)ViewState["processing"];
            if (processing)
                return;
            ViewState["processing"] = true;
            action = ConfigUtility.REMOVE_HOSTED_SERVICE;
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
            action = ConfigUtility.UPDATE_HOSTED_SERVICE;
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
            action = ConfigUtility.ADD_HOSTED_SERVICE;
            actiontext = "added";
            thisService = new HostedServices();
            processUpdate();
            ViewState["processing"] = false;
        }

        protected void CheckBoxBaseAddress_CheckedChanged(object sender, EventArgs e)
        {
            List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
            validateBaseAddresses(thisServiceHostServices, action);
        }

        protected void CheckBoxHttps_CheckedChanged(object sender, EventArgs e)
        {
            LabelBaseAddressWarning.Text = "";
            UpdateMessage.Text = "";
            LabelHttpsWarning.Text = "";
            List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
            if (!validateBaseAddresses(thisServiceHostServices, action))
            {
                LabelHttpsWarning.Text = "Please ensure you have exactly one base address defined for the scheme!";
                LabelHttpsWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                if (!validateBaseAddresses(thisServiceHostServices, action))
                {
                    if (CheckBoxHttps.Checked)
                        CheckBoxHttps.Checked = false;
                    else
                        CheckBoxHttps.Checked = true;
                    return;
                }
            }
            bool removenonHttpsBindings = false;
            if (CheckBoxHttps.Checked && (RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.BASIC_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_2007_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_HTTP_CONTEXT_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.CUSTOM_BINDING_HTTP)))
            {
                LabelHttpsWarning.Text = "Http-based endpoints that use Https must select a binding that uses transport or transport+message security, per WCF behavior. Select appropriate bindings in the binding selections below, the template-provided bindings in the dropdowns below have already been reduced to just those that will work with Https!";
                removenonHttpsBindings = true;
            }
            else
                LabelHttpsWarning.Text = "";
            buildBindingLists(removenonHttpsBindings);
        }

        protected void CheckBoxFailover_CheckedChanged(object sender, EventArgs e)
        {
            LabelFailoverWarning.Text = "";
            if (CheckBoxFailover.Checked)
            {
                if (!action.Equals(ConfigUtility.ADD_HOSTED_SERVICE))
                {
                    ConfigurationKeyValues nodeActiveKey = compositeServiceData[0].ConfigurationData.Find(delegate(ConfigurationKeyValues configKeyExist) { return configKeyExist.ConfigurationKeyFieldName.Equals("NODE_ACTIVE_SERVICE_ID"); });
                    int NODE_ACTIVE_SERVICE_ID = Convert.ToInt32(nodeActiveKey.ConfigurationKeyValue);
                    if (NODE_ACTIVE_SERVICE_ID == thisService.HostedServiceID)
                    {
                        LabelFailoverWarning.Text = "You cannot mark the active Node Service as a failover endpoint.";
                        LabelFailoverWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                        CheckBoxFailover.Checked = false;
                        return;
                    }
                }
                bool failover = false;
                List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
                List<HostedServices> checkList = thisServiceHostServices.FindAll(delegate(HostedServices hsExist) { return hsExist.HostedServiceID != HSID; });
                for (int i = 0; i < checkList.Count; i++)
                {
                    if (checkList[i].MarkedForFailover)
                        failover = true;
                }
                if (failover)
                {
                    LabelFailoverWarning.Text = "You already have a node service marked for failover.  Only one endpoint can be marked as the failover endpoint for this release.";
                    LabelFailoverWarning.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                    CheckBoxFailover.Checked = false;
                }
            }
        }

        protected void RadioButtonListBindingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<HostedServices> thisServiceHostServices = compositeServiceData[0].HostedServices.FindAll(delegate(HostedServices hsExist) { return hsExist.ServiceHostID == SHID; });
            if (!validateBaseAddresses(thisServiceHostServices, action))
            {
                LabelBindingWarning.Text = "Please examine the 'Base Address' selection.  Automatic corrections have been made based on your selected binding scheme, as each scheme needs exactly one base address. You can continue, but should read display messages for explanations of auto-corrections made.";
            }
            bool removenonHttpsBindings = false;
            if (CheckBoxHttps.Checked && (RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.BASIC_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WEB_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_2007FEDERATED_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_FEDERATED_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_DUAL_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_2007_HTTP_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.WS_HTTP_CONTEXT_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.BASIC_HTTP_CONTEXT_BINDING) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.CUSTOM_BINDING_WSHTTP) || RadioButtonListBindingType.SelectedValue.Equals(ConfigUtility.CUSTOM_BINDING_HTTP)))
            {
                LabelHttpsWarning.Text = "Http-based endpoints that use Https must select a binding that uses transport or transport+message security, per WCF behavior. Select appropriate bindings in the binding selections below, the template-provided bindings in the dropdowns below have already been reduced to just those that will work with Https!";
                removenonHttpsBindings = true;
            }
            else
                LabelHttpsWarning.Text = "";
            buildBindingLists(removenonHttpsBindings);
            if (RadioButtonListBindingType.SelectedValue == ConfigUtility.NET_NAME_PIPE_BINDING)
                LabelBindingWarning.Text = "Named Pipes is a local-only binding. All requests will be made to the local server from requesting clients.";
            else
                LabelBindingWarning.Text = "";
            switch (RadioButtonListBindingType.SelectedValue)
            {
                case ConfigUtility.WEB_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_DUAL_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_FEDERATED_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_2007FEDERATED_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.BASIC_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.WS_2007_HTTP_BINDING: { CheckBoxHttps.Enabled = true; break; }
                case ConfigUtility.NET_TCP_BINDING: { CheckBoxHttps.Enabled = false; break; }
                case ConfigUtility.NET_MSMQ_BINDING: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.NET_NAME_PIPE_BINDING: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_HTTP: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.CUSTOM_BINDING_WSHTTP: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.CUSTOM_BINDING_TCP: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_NET_MSMQ: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.CUSTOM_BINDING_NET_NAME_PIPE: { CheckBoxHttps.Enabled = false; break; };
                case ConfigUtility.BASIC_HTTP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.WS_HTTP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = true; break; };
                case ConfigUtility.NET_TCP_CONTEXT_BINDING: { CheckBoxHttps.Enabled = false; break; };
            }
        }

        protected void RadioButtonListLoadBalanceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(RadioButtonListLoadBalanceType.SelectedValue))
            {
                case ConfigUtility.LOAD_BALANCE_TYPE_HOST_ADDRESS:
                    {
                        TextBoxLoadBalanceAddress.Text = "";
                        TextBoxLoadBalanceAddress.Enabled = false;
                        break;
                    }
                case ConfigUtility.LOAD_BALANCE_TYPE_VIRTUAL_ADDRESS:
                    {
                        TextBoxLoadBalanceAddress.Enabled = true;
                        if (thisService != null)
                            TextBoxLoadBalanceAddress.Text = thisService.LoadBalanceAddress;
                        break;
                    }
            }
        }

        protected void CheckBoxOnlineParms_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxOnlineParms.Checked && thisServiceHost.ServiceType == ConfigUtility.HOST_TYPE_PRIMARY)
            {
                TextBoxOnlineParms.Enabled = true;
                TextBoxOnlineParms.Text = null;
            }
            else
            {
                TextBoxOnlineParms.Text = ConfigUtility.NO_ONLINE_PARMS;
                TextBoxOnlineParms.Enabled = false;
                CheckBoxOnlineParms.Checked = false;
            }
        }
        protected void CheckBoxEPB_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxEPB.Checked)
                DropDownListEPB.Enabled = true;
            else
                DropDownListEPB.Enabled = false;
        }
        protected void CheckBoxIdentity_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxIdentity.Checked)
                DropDownListIdentity.Enabled = true;
            else
                DropDownListIdentity.Enabled = false;
        }
        protected void DropDownListContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            buildInternalConfigNames();
        }

        protected void DropDownListBindingConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            buildInternalConfigNames();
        }
        protected void CheckBoxAutoSupplyUser_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxAutoSupplyUser.Checked)
                DropDownListCsUser.Enabled = true;
            else
                DropDownListCsUser.Enabled = false;
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

