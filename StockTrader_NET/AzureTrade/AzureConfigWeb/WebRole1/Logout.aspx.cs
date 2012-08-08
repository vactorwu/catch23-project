//  Configuration Service 5 Sample Code. Can implement and re-distribute in custom applications per contained End User License Agreement.
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azureconfigweb.cloudapp.net
//                                   

//============================================================================================
//Logic to destroy session and Auth ticket to perform a clean logout from ConfigWeb.
//============================================================================================

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConfigService.ServiceConfigurationUtility;

namespace ConfigService.ServiceConfiguration.Web
{
    /// <summary>
    /// Logs user out via Forms Authentication, destroying session data.
    /// </summary>
    public partial class Logout : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
         HttpCookie authcookie = Request.Cookies[FormsAuthentication.FormsCookieName];
         FormsAuthenticationTicket ticket = (FormsAuthenticationTicket)FormsAuthentication.Decrypt(authcookie.Value);
         if (User.Identity.IsAuthenticated)
         {
             string userid = User.Identity.Name;
             Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
             FormsAuthentication.SignOut();
         }
         Response.Redirect(ConfigSettings.PAGE_LOGIN, true);         
        }
    }
}