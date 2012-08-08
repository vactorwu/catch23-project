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
//                      The StockTraderWebApplicationConfigurationImplementation and StockTraderWebApplicationSettings projects
//                      are projects in this solution that are part of the Configuration Service implementation for StockTrader 5.  A version of StockTrader that did not implement
//                      the Configuration Service would not have these projects, and code would be written instead to load <appSettings> from web.config; as well as its own WCF logic
//                      for remote calls, vs. using the base classes (through inheritence) that the Configuration Service provides.
//                      The StockTraderWebApplicationServiceClient is the WCF client for the remote BSL layer, and inherits from the Configuration Service 5
//                      base class, a client that provides additional performance, load balancing and failover functionality for WCF services.  
//                  
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Threading;
using Trade.StockTraderWebApplicationSettings;
using Trade.StockTraderWebApplicationServiceClient;
using Trade.StockTraderWebApplicationModelClasses;
using Trade.Utility;

namespace Trade.Web
{
    /// <summary>
    /// Displays orders via a repeater, used on Account.aspx page.
    /// </summary>
    public partial class AccountOrders : System.Web.UI.UserControl
    {
        public int totalOrders;
        public int ordersRequested;
        public TotalOrdersUI orderData;

        protected override void OnLoad(EventArgs e)
        {
            //Thread.Sleep(10000);
            string userid = HttpContext.Current.User.Identity.Name;
            if (userid != null)
            {
                string action = Request["action"];
                if (!(Request["action"] == null) && !Input.InputText(Request["action"], StockTraderUtility.EXPRESSION_NAME_13))
                {
                    FormsAuthentication.SignOut();
                    Response.Redirect(Settings.PAGE_LOGIN, true);
                }
                else
                {
                    BSLClient businessServicesClient = new BSLClient();
                    if (action == "showtoporders" && (Settings.ACCESS_MODE != StockTraderUtility.BSL_SOAP))
                    {
                        ordersRequested = Settings.MAX_DISPLAY_TOP_ORDERS;
                        orderData = businessServicesClient.getTopOrders(userid);
                    }
                    else
                    {
                        ordersRequested = Settings.MAX_DISPLAY_ORDERS;
                        orderData = businessServicesClient.getOrders(userid);
                    }
                    if (orderData.orders.Count != 0)
                    {
                        AccountOrdersRepeater.DataSource = orderData.orders;
                        AccountOrdersRepeater.DataBind();
                    }
                    totalOrders = orderData.orders.Count;
                }
            }
        }
    }
}