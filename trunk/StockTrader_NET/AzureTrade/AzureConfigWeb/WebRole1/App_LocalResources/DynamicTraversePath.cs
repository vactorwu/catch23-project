//  .NET StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azurestocktrader.cloudapp.net
//                                   
//
//  Configuration Service 5 Notes:
//                      The application implements Configuration Service 5.0, for which the source code also ships in the sample. However, the .NET StockTrader 5
//                      sample is a general-purpose performance sample application for Windows Server and Windows Azure even if you are not implementing the Configuration Service. 
//                      
//


//============================================================================================
//Logic to dynamically retrieve a network traverse path through connected service layers to 
//the target of a Config Service operation.  Direct network connectivity or awareness of 
//services beyond n+1 levels is never assumed; in essence a traversepath acts like a router.
//This file also contains some commonly used utility methods for the ConfigWeb app.
//============================================================================================

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Caching;
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
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace ConfigService.ServiceConfiguration.Web
{

    public static class Input
    {
        public const string EXPRESSION_VALIDATE = @"^[A-Za-z0-9''.'':''-'-\d'_'' ''*''#''/''('')'','\s]{1,200}$";
        public const string EXPRESSION_NAVIGATE = @"^[A-Za-z0-9''.'':''-'-\d'_'' ''*''#''/''('')'','\s]{1,200}$";
        public const string EXPRESSION_USERID = @"^[A-Za-z0-9''_''\s]{1,50}$";
        public const string EXPRESSION_PWORD = @"^[A-Za-z0-9''.'':''-'-\d'_'' ''*''#''/''('')'','\s]{1,50}$";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string InputText(string inputString, string expression)
        {
            // check incoming parameters for null or blank string
            if ((inputString != null) && (inputString != String.Empty))
            {
                if (Regex.IsMatch(inputString, expression))
                    return inputString;
            }
            return null;
        }

        public static string LoginValidate(string inputString, string expression)
        {
            if (inputString==null)
                return null;
            inputString = inputString.Trim();
            if (inputString.Length > 50)
                return null;
            if (!Regex.IsMatch(inputString, expression))
                return null;
            return inputString;
        }

        public static bool ValidateHost(ref string hostNameIdentifier, ref string configName, ref string version, ref string platform, ref string hoster)
        {
            string hostNameIdentifierRequest  = System.Web.HttpContext.Current.Request["name"];
            string configNameRequest = System.Web.HttpContext.Current.Request["cfgSvc"];
            string versionRequest = System.Web.HttpContext.Current.Request["version"];
            string platformRequest = System.Web.HttpContext.Current.Request["platform"];
            string hosterRequest = System.Web.HttpContext.Current.Request["hoster"];
            if (hostNameIdentifierRequest != null && configNameRequest != null && versionRequest != null && hosterRequest != null && platformRequest != null)
            {
                hostNameIdentifier = hostNameIdentifierRequest;
                configName = configNameRequest;
                hoster = hosterRequest;
                version = versionRequest;
                platform = platformRequest;
            }
            if (InputText(hostNameIdentifier, EXPRESSION_VALIDATE)==null ||
                InputText(configName, EXPRESSION_VALIDATE)==null ||
                InputText(version, EXPRESSION_VALIDATE) == null ||
                InputText(platform, EXPRESSION_VALIDATE) == null ||
                InputText(hoster, EXPRESSION_VALIDATE) == null)
                return false;
            else
                return true;
        }

        public static void getHostData(bool isPostBack, StateBag viewstate, out string userid, out string address, out ServiceUsers csUser,out string client, out string hostNameIdentifier, out string configName, out string version, out string platform, out string hoster, bool logout)
        {
            SessionInfo info = new SessionInfo();
            address = null;
            csUser = null;
            client = null;
            configName = null;
            hoster = null;
            version = null;
            platform = null;
            client = null;
            hostNameIdentifier = null;
            string hostedID = null;
            userid = null;
            if (!info.getSessionData(isPostBack, out address,out csUser,out client,out hostNameIdentifier, out configName, out hoster,out version, out platform, out hostedID))
                home(true);
            if (isPostBack)
            {
                hostNameIdentifier = (string)viewstate["name"];
                configName = (string)viewstate["configname"];
                version = (string)viewstate["version"];
                platform = (string)viewstate["platform"];
                hoster = (string)viewstate["hoster"];
            }
            else
            {
                if (!Input.ValidateHost(ref hostNameIdentifier, ref configName, ref version, ref platform, ref hoster))
                    home(logout);
                viewstate["name"] = hostNameIdentifier;
                viewstate["configname"] = configName;
                viewstate["version"] = version;
                viewstate["platform"] = platform;
                viewstate["hoster"] = hoster;
            }
        }

        
        public static void home(bool logout)
        {
            if (logout)
                System.Web.HttpContext.Current.Response.Redirect(ConfigSettings.PAGE_LOGOUT,true);
            else
                System.Web.HttpContext.Current.Response.Redirect(ConfigSettings.PAGE_NODES, true);
        }
    }

    public class SessionInfo
    {
        public SessionInfo() { }

        public bool getSessionData(bool isPostBack, out string address, out ServiceUsers csUser, out string client, out string name, out string configname, out string hoster, out string version, out string platform, out string HostedID)
        {
            address = null;
            HostedID = null;
            csUser = null;
            client = null;
            configname = null;
            hoster = null;
            version = null;
            platform = null;
            client = null;
            name = null;
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            if (id == null)
                return false;
            FormsAuthenticationTicket ticket = id.Ticket;
            string userData = ticket.UserData;
            string[] userDataAuth = userData.Split(new char[] { (char)182 });
            if (userDataAuth.Length != 13)
                return false;
            address = userDataAuth[0];
            csUser = new ServiceUsers(Convert.ToInt32(userDataAuth[5]), userDataAuth[4], userDataAuth[2], Convert.ToInt32(userDataAuth[3]), Convert.ToBoolean(userDataAuth[1]), 0);
            client = userDataAuth[6];
            configname = userDataAuth[8];
            name = userDataAuth[7];
            hoster = userDataAuth[9];
            version = userDataAuth[10];
            platform = userDataAuth[11];
            HostedID = userDataAuth[12];
            if (address == null || client == null || csUser.UserId == null || csUser.Password==null)
                return false;
            else
                return true;
        }

        public HttpCookie setSessionData(string address, ServiceUsers csUser, string client, string name, string configname, string hoster, string version, string platform, string ID)
        {
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            if (id == null)
                return null;
            string sessionInfo = address + (char)182 + csUser.LocalUser.ToString() + (char)182 + csUser.Password + (char)182 +
                            csUser.Rights.ToString() + (char)182 + csUser.UserId + (char)182 + csUser.UserKey.ToString() + (char)182 +client +
                                (char)182 + name + (char)182 + configname + (char)182 + hoster + (char)182 +
                                version + (char)182 +platform + (char)182 + ID;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, csUser.UserId, DateTime.Now, DateTime.Now.AddMinutes(ConfigSettings.CONFIGWEB_FORMSAUTH_TIMEOUT_MINUTES), false, sessionInfo);
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);
            return cookie;
        }

        public void getHostNameConfigName(out string name, out string configname)
        {
            configname = null;
            name = null;
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            if (id == null)
                return;
            FormsAuthenticationTicket ticket = id.Ticket;
            string userData = ticket.UserData;
            string[] userDataAuth = userData.Split(new char[] { (char)182 });
            if (userDataAuth.Length != 13)
                return;
            configname = userDataAuth[8];
            name = userDataAuth[7];
            return;
        }

        public void getConfigHostedID(out string hostedserviceID)
        {
            hostedserviceID = null;
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            if (id == null)
                return;
            FormsAuthenticationTicket ticket = id.Ticket;
            string userData = ticket.UserData;
            string[] userDataAuth = userData.Split(new char[] { (char)182 });
            if (userDataAuth.Length != 13)
                return;
            hostedserviceID = userDataAuth[12];
            return;
        }

    }

    public static class CertCheck
    {
        public static bool EasyCertCheck(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error) { return true; }
    }

    public static class GetImageButton
    {
        public const string WIN7URL = "~/Images/win7.png";
        public const string WIN7PUB = "http://msdn.microsoft.com/en-us/windows/dd535817"; 
        public const string WIN2008URL = "~/Images/win2008.png";
        public const string WIN2008PUB = "http://www.microsoft.com/windowsserver2008/en/us/news-reviews.aspx";
        public const string WIN2008R2URL = "~/Images/win2008r2.png";
        public const string WIN2008R2PUB = "http://www.microsoft.com/windowsserver2008/en/us/news-reviews.aspx";
        public const string WINVISTAURL = "~/Images/winvista.png";
        public const string WINVISTPUB = "http://msdn.microsoft.com/en-us/windows/dd535817";
        public const string WINURL ="~/Images/winbrand.png";
        public const string WINPUB = "http://msdn.microsoft.com/en-us/windows";
        public const string AZUREURL = "~/Images/WinAzureLogo.png";
        public const string AZUREPUB = "http://msdn.microsoft.com/library/gg432998.aspx";
        public const string VMM2008URL = "~/Images/hypervwin2008.png";
        public const string VMM2008PUB = "http://www.microsoft.com/systemcenter/en/us/virtual-machine-manager/vm-vnext-beta.aspx";
        public const string VMM2008R2URL = "~/Images/hypervwin2008R2.png";
        public const string VMM2008R2PUB = "http://www.microsoft.com/systemcenter/en/us/virtual-machine-manager/vm-vnext-beta.aspx";
        public const string VMMWINDOWSURL = "~/Images/hypervwindows.png";
        public const string VMMWINDOWSPUB = "http://www.microsoft.com/systemcenter/en/us/virtual-machine-manager/vm-vnext-beta.aspx";


        public static void runtimePoweredBy(string hostPlatform, System.Web.UI.WebControls.ImageButton image)
        {
            if (hostPlatform == null)
            {
                image.ImageUrl = WINURL;
                image.PostBackUrl = WINPUB;
                return;
            }
            if (hostPlatform.ToLower().Contains("hyper-v"))  //Hyper-V
            {
                if (hostPlatform.ToLower().Contains("windows server 2008 r2"))
                {
                    image.ImageUrl = VMM2008R2URL;
                    image.PostBackUrl = VMM2008R2PUB;
                }
                else
                    if (hostPlatform.ToLower().Contains("windows server 2008"))
                    {
                        image.ImageUrl = VMM2008R2URL;
                        image.PostBackUrl = VMM2008R2PUB;
                    }
                    else
                    {
                        image.ImageUrl = VMMWINDOWSURL;
                        image.PostBackUrl = VMMWINDOWSPUB;
                    }
                return;
            }
            if (hostPlatform.ToLower().Contains("azure"))
            {
                image.ImageUrl = AZUREURL;   //Azure cloud!
                image.PostBackUrl = AZUREPUB;
                return;
            }
            if (hostPlatform.ToLower().Contains("windows server 2008 r2"))
            {
                image.ImageUrl = WIN2008R2URL;
                image.PostBackUrl = WIN2008R2PUB;
                return;
            }
            if (hostPlatform.ToLower().Contains("windows server 2008"))
            {
                image.ImageUrl = WIN2008URL;
                image.PostBackUrl = WIN2008PUB;
                return;
            }
            if (hostPlatform.ToLower().Contains("windows 7"))
            {
                image.ImageUrl = WIN7URL;
                image.PostBackUrl = WIN7PUB;
                return;
            }

            if (hostPlatform.ToLower().Contains("vista"))
            {
                image.ImageUrl = WINVISTAURL;
                image.PostBackUrl = WINVISTPUB;
                return;
            }
            
            image.ImageUrl = WINURL;
            image.PostBackUrl = WINPUB;
            return;
        }
    }

    /* Helps with display of potentially long fields in fixed width tables. */
    public static class ChunkText
    {
        
        public static string chunk(int charCount, string textToChunk)
        {
            if (textToChunk == null)
                return " ";
            else 
                if (textToChunk.Length <= 10)
                    return textToChunk;
            string chunked = "";
            string[] splitString = textToChunk.Split(new char[] { ' ' });
            for (int i = 0; i < splitString.Length; i++)
            {
                if (splitString[i].Length > charCount)
                {
                    int count = 0;
                    for (int j = charCount; j < splitString[i].Length; j += charCount)
                    {
                        if ((j +count)< splitString[i].Length)
                            splitString[i] = splitString[i].Insert(j + count, "<br/>");
                        count++;
                    }
                    chunked = chunked + " " + splitString[i];
                }
                else
                    chunked = chunked + " " + splitString[i];
            }
            if (chunked.Length == 0)
                chunked = textToChunk;
            return chunked;
        }

        public static string chunkDot(string textToChunk, char chunkChar)
        {
            string[] splitString = textToChunk.Split(new char[] { chunkChar });
            string returnString = splitString[0];
            for (int i = 1; i < splitString.Length; i++)
            {
                returnString = returnString + chunkChar + " " + splitString[i];
            }
            return returnString;
        }
    }
    /// <summary>
    /// This class is used for Config Web to automatically find a new online config host in the root
    /// Virtual host cluster, should the existing root node go down.  It calls the *fundamental* service
    /// operation 'getTraversePath' to dynamically find the target service virtual host and then specific online physical
    /// instance of this service host via the fastest path available to that service, which may be several service-layers down.
    /// The fastest path may be ever changing, the Configuration Service dynamically adjusts.  
    /// </summary>
    public static class DynamicTraversePath
    {
        /// <summary>
        /// Returns a valid network traverse path to the target host, uniquely identified by hostNameIdentifier + configName.
        /// </summary>
        /// <param name="hostNameIdentifier"></param>
        /// <param name="configName"></param>
        /// <param name="configProxy"></param>
        /// <param name="attemptedeps"></param>
        /// <param name="address"></param>
        /// <param name="bindingConfig"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<TraverseNode> getTraversePath(string hostNameIdentifier, string configName, ref ServiceConfigurationClient configProxy, string address, string bindingConfig, ServiceUsers user)
        {
            List<TraverseNode> returnPath=null;
            try
            {
                configProxy = new ServiceConfigurationClient(bindingConfig, address, user);
                returnPath = configProxy.getTraversePath(null, hostNameIdentifier, configName, user);
            }
            catch (CommunicationException)
            {
                    return null;
            }
            return returnPath;
        }
    }
}
