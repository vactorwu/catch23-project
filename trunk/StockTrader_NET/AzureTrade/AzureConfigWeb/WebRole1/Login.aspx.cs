//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to perform a login to a Configuration Service endpoint.  Note you can change the
//'Known List' by modifying ConfigWeb web.config <appSettings>, to display your
//own list to known Config Service endpoints.
//============================================================================================

//====================Update History===========================================================
// 3/31/2011: V5.0.0.0: A brand new release, updated for private/public clouds, other major changes.
//=============================================================================================
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Administration;
using System.Runtime.Serialization;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceConfiguration.DataContract;
using ConfigService.ServiceConfigurationRemote;
using ConfigService.ServiceNodeCommunication.DataContract;
using ConfigService.ServiceConfigurationContract;
using ConfigService.ServiceConfiguration.IDAL;
using ConfigService.ServiceConfiguration.DALFactory;
using ConfigService.ServiceConfigurationUtility;

namespace ConfigService.ServiceConfiguration.Web
{
    /// <summary>
    /// Validates user against a service configuration database, logging into the configuration system. 
    /// This remotely attaches to any service implementing the Configuration Service contract, over whatever
    /// binding the target configuration service supports and you have added in ConfigWeb as a client binding.
    /// The bindings must be defined in ConfigWeb's web.config in the <system.serviceModel> section--if you add any
    /// new WCF bindings to your Config Service host, you can simply add them into ConfigWeb's web.config to get them
    /// as options--for example, bindings with different security modes.
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {

        List<string> names;
        List<string> clientDefinitions;
        List<string> knownAddresses;
        List<ClientInformation> allClients;
        List<BindingInformation> bindings;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultFocus = uid.ClientID;
            Page.Form.DefaultButton = LoginButton1.UniqueID;
            if (!IsPostBack)
            {
                Configuration config = ServiceConfigHelper.getConfig(ConfigUtility.HOSTING_ENVIRONMENT_IIS, null);
                ServiceConfigHelper.getConfigWebAddresses(config, out names, out clientDefinitions, out knownAddresses);
                bindings = ServiceConfigHelper.getBindingInformation(config);
                allClients = ServiceConfigHelper.getClientInformation(config);
                ViewState["allClients"] = allClients;
                ViewState["knownAddresses"] = knownAddresses;
                ViewState["names"] = names;
                ViewState["clientDefinitions"] = clientDefinitions;
                ViewState["bindings"] = bindings;
                for (int i = 0; i < knownAddresses.Count; i++)
                {
                   DropDownListNames.Items.Add(names[i]);
                }
                if (DropDownListNames.Items.Count > 0)
                {
                    CheckBoxAddress.Checked = true;
                    ServiceAddress.Visible = true;
                    textboxaddress.Visible = false;
                    ClientConfiguration.Visible = true;
                    clients.Visible = false;
                    DropDownListNames.SelectedValue = DropDownListNames.Items[0].Text;
                    if (knownAddresses.Count>0)
                        ServiceAddress.Text = knownAddresses[0];
                    if (clientDefinitions.Count > 0)
                        ClientConfiguration.Text = clientDefinitions[0];
                }
                else
                {
                    ServiceAddress.Visible = false;
                    textboxaddress.Visible = true;
                    ClientConfiguration.Visible = false;
                    clients.Visible = true;
                }
                for (int i=0; i<allClients.Count;i++)
                {
                    clients.Items.Add(allClients[i].ElementName);
                    clients.SelectedIndex=0;
                }
                ViewState["allClients"] = allClients;
            }
            else
                InValid.Text = "";
            if (ConfigUtility.onAzure)
                LocalRunning.Text = "";
            else
                LocalRunning.Text = "For the local sample installed services, use userid: 'localadmin'; password: 'admin' after initial install.<br/>";
        }

        protected void processLogin()
        {
           HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
           if (authcookie != null)
           {
               FormsAuthenticationTicket tryTicket = (FormsAuthenticationTicket)FormsAuthentication.Decrypt(authcookie.Value);
               if (User.Identity.IsAuthenticated)
               {
                   string userid = User.Identity.Name;
                   Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
               }
           }
            InValid.Text = null;
            Page.Validate();
            allClients = (List<ClientInformation>)ViewState["allClients"];
            knownAddresses = (List<string>)ViewState["knownAddresses"];
            names = (List<string>)ViewState["names"];
            clientDefinitions = (List<string>)ViewState["clientDefinitions"];
            bindings = (List<BindingInformation>)ViewState["bindings"];
            ClientInformation client = null;
            BindingInformation binding = bindings.Find(delegate(BindingInformation bindingItem)
            {
                client = allClients.Find(delegate(ClientInformation clientExist) { return clientExist.ElementName.Equals(clients.SelectedValue); });
                return bindingItem.BindingConfigurationName.Equals(client.BindingConfiguration);
            });
            if (Page.IsValid)
            {
                //Notes on security: 
                //We are using ASP.NET Forms authentication from ConfigWeb, which automates the authentication process against
                //either a simple list of valid users, or a backend database of registered users (ConfigWeb uses this mechanism),
                //Windows Active Directory, or any pluggable mechanism based on the extensibility of Forms Authentication in ASP.NET 2.0.
                //Via Forms Authentication, ASP.NET provides automatic authentication for restricted pages, and automates redirects to login forms 
                //such as this one. In addition, security can be set with WCF bindings, which support transport, message, and transport with
                //message credentials.  You can add additional WCF binding configurations and client configurations to ConfigWeb web.config, and they will 
                //automatically show up as drop-down options on the login page.  The Configuration Service itself authenticates every
                //request against its registered users database; with service operations restricted based on the user rights for that registered user.

                //The remote Configuration Service you are logging into must support the binding option you choose (via the selected Client Configuration) on login; with WCF
                //a service host can simultaneously support many different bindings (perhaps with different security settings), many different
                //listen endpoints (URIs).  And, clients can also support these different binding options, as illustrated with this login
                //page that allows netTcp (binary encoding); or basicHttp (SOAP/text-XML encoding) to be chosen as an option for login and
                //all subsequent requests for Configuration Service operations. Again, you could add different bindings to your Configuration 
                //Service host, and simply add the appropriate client bindings (can generate via svcutil) to ConfigWeb web.config to 
                //restrict access based on transport and/or message level security set in your binding configurations on client and host--
                //for example, adding wsHttp support over https.  See:
                //
                //                          http://msdn2.microsoft.com/en-us/library/ms735093.aspx 
                //
                //
                //Finally, ASP.NET Forms authentication defaults to use SHA1 for HMAC Generation and AES for
                //Encryption, which is recommended. An application such as this in production on the public Internet
                //(vs. internal private net) would be run over SSL. An excellent security Patterns and Practices resource 
                //on how to secure Internet applications can be found at:
                ///
                //                http://msdn2.microsoft.com/en-us/library/aa302415.aspx
                //
                //Information on Forms Authentication, encryption and using Forms Authentication with SSL is available at:
                //
                //               http://msdn2.microsoft.com/en-us/library/ms998310.aspx

                string userID = uid.Text;
                string password = pwd.Text;
                password = Input.LoginValidate(password, Input.EXPRESSION_PWORD);
                userID = Input.LoginValidate(userID, Input.EXPRESSION_USERID);
                if (userID==null || password == null)
                {
                    InValid.Text = "Please enter a valid userid and/or password!";
                    return;
                }
                string address = null;
                if (!CheckBoxAddress.Checked)
                    address = textboxaddress.Text.ToLower().Trim();
                else
                    address = ServiceAddress.Text.ToLower().Trim();
                if (address == null || address == "")
                {
                    InValid.Text = "Please enter a valid address!";
                    return;
                }
                Uri theUri = null;
                try
                {
                    theUri = new Uri(address);
                }
                catch (Exception)
                {
                    InValid.Text = "Please enter a valid address!";
                    return;
                }
                bool remap = false;
                ServiceConfigurationClient configProxy = null;
                ServiceUsers user = new ServiceUsers(0, userID, password, ConfigUtility.CONFIG_NO_RIGHTS, true, 0);
                try
                {
                    user.UserId.Trim();
                    if (!CheckBoxAddress.Checked)
                    {
                        if (binding.BindingType != null)
                        {
                            switch (binding.BindingType)
                            {
                                case ConfigUtility.BASIC_HTTP_BINDING:
                                    {
                                        if (!client.ElementName.ToLower().Contains("t_security"))
                                        {
                                            if (!address.ToLower().StartsWith("https") && address.ToLower().StartsWith("http"))
                                            {
                                                break;
                                            }
                                            else if (address.ToLower().StartsWith("https"))
                                            {
                                                address = address.Remove(4, 1);
                                                textboxaddress.Text = address;
                                                theUri = new Uri(address);
                                                remap = true;
                                                break;
                                            }
                                            else
                                            {
                                                InValid.Text = "You have selected an <b>Http</b> configuration: Please enter a valid <strong>http</strong> address.";
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            if (!address.ToLower().StartsWith("https") && address.ToLower().StartsWith("http"))
                                            {
                                                address = address.Insert(4, "s");
                                                theUri = new Uri(address);
                                                textboxaddress.Text = address;
                                                remap = true;
                                                break;
                                            }
                                            else if (!address.ToLower().StartsWith("https"))
                                            {
                                                InValid.Text = "You have selected an <b>Https</b> configuration: Please enter a valid <strong>https</strong> address.";
                                                return;
                                            }
                                            break;
                                        }
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
                                        if (!address.ToLower().StartsWith("net.tcp"))
                                        {
                                            InValid.Text = "You have selected a <b>Tcp</b> configuration: Please enter a valid tcp address in form of net.tcp://";
                                            return;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    string theClientConfig = null;
                    ServiceUsers returnUser=null;
                    if (CheckBoxAddress.Checked)
                        theClientConfig = ClientConfiguration.Text;
                    else
                        theClientConfig = clients.SelectedValue;
                    configProxy = new ServiceConfigurationClient(theClientConfig, address, user);

                    //Ok, request a login from the remote service
                    LoginInfo returnInfo = configProxy.login(user);
                    if (returnInfo!=null)
                        returnUser = returnInfo.CsUser;

                    //By default, the service will reject admin ops on a per request basis, but we will also reject login here 
                    //in ConfigWeb for non-admins.
                    if (returnUser != null && returnUser.Rights >= ConfigUtility.CONFIG_DEMO_ADMIN_RIGHTS)
                    {
                        //OK--we are logged in!  To allow scale out of ConfigWeb itself, we are going to jam some basic information into a cookie
                        //as client-side session state.  In secure scenarios, like on Azure, ConfigWeb is always run over https.  This is an alternative
                        //to distributed caching of session state, but only appropriate when the state per session is very, very small (a few strings below).
                        string sessionInfo = address + (char)182 + returnUser.LocalUser.ToString() + (char)182 + returnUser.Password + (char)182 +
                            returnUser.Rights.ToString() + (char)182 + returnUser.UserId + (char)182 + returnUser.UserKey.ToString() + (char)182 + theClientConfig +
                                (char)182 + returnInfo.HostNameIdentifier + (char)182 + returnInfo.ConfigServiceName + (char)182 + returnInfo.ServiceHoster + (char)182 +
                                returnInfo.ServiceVersion + (char)182 + returnInfo.RuntimePlatform + (char)182 + returnInfo.HostedServiceID;
                        //FormsAuthentication.SetAuthCookie(returnUser.UserId, false);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, returnUser.UserId,DateTime.Now, DateTime.Now.AddMinutes(ConfigSettings.CONFIGWEB_FORMSAUTH_TIMEOUT_MINUTES),false, sessionInfo);
                        string hash = FormsAuthentication.Encrypt(ticket);
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
                        Response.Cookies.Add(cookie);
                        Response.Redirect(FormsAuthentication.DefaultUrl, true);
                    }
                    else
                    {
                        InValid.Text = ConfigSettings.EXCEPTION_MESSAGE_INVALID_LOGIN;
                    }
                }
                catch (Exception e)
                {
                    if (remap)
                        InValid.Text = "<b>You entered an https or http address that did not match the selected client configuration binding, and the address was remapped to/from https to/from http automatically before attempting login. </b><br/><br/>";
                    else
                        InValid.Text = "";
                    //Note if a service is misconfigured and will not start, the exception will likely be a web page; which has to be handled differently since its encoding is not text/xml, its text/html 
                    //with lots of characters that will not display if simply printed out on a label control.
                    string displayMessage=null;
                    string innerMessage = "";
                    if (e.Message.ToLower().Contains("<html>"))
                        displayMessage = ConfigUtility.reMapExceptionForDisplay(e.Message);
                    else
                        displayMessage = e.Message;
                    if (e.InnerException != null)
                        innerMessage = "<br/>Inner Exception is: " + e.InnerException.Message;
                    InValid.Text = InValid.Text + "Error Logging In. Exception is: <br/>" + displayMessage + innerMessage;
                }
            }
        }

        protected void CheckBoxAddress_CheckedChanged(object sender, EventArgs e)
        {
            allClients = (List<ClientInformation>)ViewState["allClients"];
            knownAddresses = (List<string>)ViewState["knownAddresses"];
            names = (List<string>)ViewState["names"];
            clientDefinitions = (List<string>)ViewState["clientDefinitions"];
            if (CheckBoxAddress.Checked)
            {
                ServiceAddress.Visible = true;
                textboxaddress.Visible = false;
                ClientConfiguration.Visible = true;
                clients.Visible = false;
                int i = DropDownListNames.SelectedIndex;
                ClientConfiguration.Text = clientDefinitions[i];
                ServiceAddress.Text = knownAddresses[i];
                DropDownListNames.Visible = true;
            }
            else
            {
                DropDownListNames.Visible = false;
                ServiceAddress.Visible = false;
                textboxaddress.Visible = true;
                ClientConfiguration.Visible = false;
                clients.Visible = true;
                clients.SelectedValue=ClientConfiguration.Text;
                textboxaddress.Text = ServiceAddress.Text;
            }
            pwd.Attributes.Add("value", pwd.Text);
        }

        protected void LoginButton1_Click(object sender, EventArgs e)
        {
            processLogin();
        }

        protected void DropDownListNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            allClients= (List<ClientInformation>)ViewState["allClients"];
            knownAddresses = (List<string>)ViewState["knownAddresses"];
            names = (List<string>)ViewState["names"];
            clientDefinitions = (List<string>)ViewState["clientDefinitions"];
            int i = DropDownListNames.SelectedIndex;
            if (clientDefinitions.Count>=i)
                ClientConfiguration.Text = clientDefinitions[i];
            if (knownAddresses.Count>=i)
                ServiceAddress.Text = knownAddresses[i];
            pwd.Attributes.Add("value", pwd.Text);
        }

        protected void clients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
