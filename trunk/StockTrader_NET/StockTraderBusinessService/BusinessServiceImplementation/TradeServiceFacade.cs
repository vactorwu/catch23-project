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

//======================================================================================================
//  TradeServiceFacadeWcf:  The WCF/.NET 3.5 Web Service facade to TradeService.cs. 
//======================================================================================================


using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Threading;
using ConfigService.ServiceConfigurationUtility;
using Trade.BusinessServiceConfigurationSettings;
using Trade.BusinessServiceDataContract;
using Trade.BusinessServiceContract;
using Trade.Utility;

namespace Trade.BusinessServiceImplementation
{

    /// <summary>
    /// This is the service facade implementation for WCF-based Trade Business Services. It defines the business service layer operations
    /// that are implemented in the TradeService.cs implementation class.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class TradeServiceBSL : ITradeServices
    {
       
        static int loginCount;
        
        public TradeServiceBSL()
        {
            Interlocked.Increment(ref Trade.BusinessServiceConfigurationSettings.Settings.invokeCount);
        }

        //Used for online check in WCF proxy logic for clients to this service; employed in 
        //StockTrader load-balanced scenarios to ensure application-level failover of 
        //service-to-service remote calls to clusters running this service. 
        public void isOnline()
        {
            //We will not count this online check from Remote client as an activation, since its not a real operation.  Ultimately,
            //there is a TODO here, which is apply a WCF endpoint behavior, such that we count actual requests at the service op level.
            //Also note while this will be called from ConfigWeb for admin procedures (and discounted here); it will not be called
            //when using an external load balancer; or if the known node count=1.  There is no point in this check, if there is no
            //node with a different address to failover to.  So, for on-premise, this kicks in with 2 or more nodes, with the
            //benefit of service-op level failover.  With external load balancers, its up to them to decide at what level
            //they check online status.  It might be at the (virtual) machine level; or at the endpoint level.
            Interlocked.Decrement(ref Settings.invokeCount);
            return;
        }

        //Here only for some obscure Java interop with prior WebSphere versions.
        public void emptyMethodAction()
        {
            return;
        }

        public AccountDataModel login(string userid, string password)
        {
            loginCount++;
            if (Settings.DISPLAY_WEBSERVICE_LOGINS && (loginCount % Settings.LOGIN_ITERATIONSTO_DISPLAY == 0))
                ConfigUtility.writeConsoleMessage("Login request # " + loginCount.ToString() + " received. Login is for user id: " + userid + "\n",EventLogEntryType.Information,false, new Settings());
            TradeService service = new TradeService();
            return service.login(InputText(userid, StockTraderUtility.USERID_MAX_LENGTH), InputText(password, StockTraderUtility.PASSWORD_MAX_LENGTH));             
        }

        public List<OrderDataModel> getOrders(string userID)
        {
            TradeService service = new TradeService();
            return service.getOrders(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public AccountDataModel getAccountData(string userID)
        {
            TradeService service = new TradeService();
            return service.getAccountData(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public AccountProfileDataModel getAccountProfileData(string userID)
        {
            TradeService service = new TradeService();
            return service.getAccountProfileData(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public AccountProfileDataModel updateAccountProfile(AccountProfileDataModel profileData)
        {
            TradeService service = new TradeService();
            return service.updateAccountProfile(profileData);
        }

        public void logout(string userID)
        {
            TradeService service = new TradeService();
            service.logout(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
            return;
        }

        //note orderProcessing mode param is not used by StockTrader; instead
        //app picks this up from the application configuration.
        public OrderDataModel buy(string userID, string symbol, double quantity, int orderProcessingMode)
        {
            if (quantity <= 0)
                throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_BAD_ORDER_PARMS);
            TradeService service = new TradeService();
            return service.buy(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH), InputText(symbol, StockTraderUtility.QUOTESYMBOL_MAX_LENGTH), quantity, orderProcessingMode);
        }

        //note orderProcessing mode param is not used by StockTrader; instead
        //app picks this up from the application configuration.
        public OrderDataModel sell(string userID, int holdingID, int orderProcessingMode)
        {
            TradeService service = new TradeService();
            return service.sell(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH), holdingID, orderProcessingMode);
        }

        public List<HoldingDataModel> getHoldings(string userID)
        {
            TradeService service = new TradeService();
            return service.getHoldings(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public AccountDataModel register(string userID, string password, string fullname, string address, string email, string creditcard, decimal openBalance)
        {
            TradeService service = new TradeService();
            return service.register(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH), InputText(password, StockTraderUtility.PASSWORD_MAX_LENGTH), InputText(fullname, StockTraderUtility.FULLNAME_MAX_LENGTH), InputText(address, StockTraderUtility.ADDRESS_MAX_LENGTH), InputText(email, StockTraderUtility.EMAIL_MAX_LENGTH), InputText(creditcard, StockTraderUtility.CREDITCARD_MAX_LENGTH), openBalance);
        }

        public List<OrderDataModel> getClosedOrders(string userID)
        {
            TradeService service = new TradeService();
            return service.getClosedOrders(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public MarketSummaryDataModelWS getMarketSummary()
        {
            TradeService service = new TradeService();
            return service.getMarketSummary();
        }

        public QuoteDataModel getQuote(string symbol)
        {
            string _symbol = InputText(symbol, StockTraderUtility.QUOTESYMBOL_MAX_LENGTH);
            if (_symbol.Length == 0)
                return null;
            TradeService service = new TradeService();
            return service.getQuote(InputText(symbol, StockTraderUtility.QUOTESYMBOL_MAX_LENGTH));
        }

        public List<QuoteDataModel> getQuotes(string symbols)
        {
            string _symbols = InputText(symbols, 100);
            if (_symbols.Length == 0)
                return null;
            TradeService service = new TradeService();
            return service.getQuotes(_symbols);
        }

        public HoldingDataModel getHolding(string userID, int holdingID)
        {
            TradeService service = new TradeService();
            return service.getHolding(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH), holdingID);
        }

        public List<OrderDataModel> getTopOrders(string userID)
        {
            TradeService service = new TradeService();
            return service.getTopOrders(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH));
        }

        public OrderDataModel sellEnhanced(string userID, int holdingID, double quantity)
        {
            if (quantity <= 0)
                throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_BAD_ORDER_PARMS);
            TradeService service = new TradeService();
            return service.sellEnhanced(InputText(userID, StockTraderUtility.USERID_MAX_LENGTH), holdingID, quantity);
        }

       
        
        private string InputText(string inputString, int maxLength)
        {
            // check incoming parameters for null or blank string
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //chop the string incase the client-side max length
                //fields are bypassed to prevent buffer over-runs
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);
            }
            return inputString;
        }
    }
}