using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ConfigService.ServiceConfigurationUtility;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ConfigService.ServiceConfiguration.Web;


namespace WebRole1
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            ServicePointManager.DefaultConnectionLimit = 64;
            /*Uncomment  these next two lines if you want to accept test certificates.
            Else you will not be able to connect via https/SSL with a test certificate, and hence 
            only a valid 'issued' certificate will work with WCF from configweb to the config service.
            * 
            */
            ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(CertCheck.EasyCertCheck);

            ConfigUtility.setAzureRuntime(true);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
