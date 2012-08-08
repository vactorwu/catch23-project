using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Threading;
using Trade.BusinessServiceContract;
using Trade.BusinessServiceDataContract;
using StockTrader;

namespace Trade.StockTraderWebApplicationServiceClient
{
    /// <summary>
    /// This is the WCF client class for the remote Business Services, connecting to any Web 
    /// Service platform via WCF (.NET, J2EE etc.) that implements ITradeServices. The Channel
    /// Logic here, which does not implement the config system for load balancing (although it could)
    /// does still use a best-practice performance technique of caching an instance to an open channel
    /// vs. trying to recreate a new connection on every request.  This can have a substantial impact 
    /// on performance for WCF clients.  Note that since we do not implement load-balancing/config system
    /// here in this client, the logic is much simpler.  Again, the getter/setter logic is boiler plate
    /// code and can be copied within your WCF clients (vs. using all-generated client proxies).
    /// </summary>
    public class BusinessServiceClient : ITradeServices
    {
        public BusinessServiceClient()
        {
            try
            {
                if (_channel != null)
                    ((IChannel)_channel).Abort();
                if (_factory != null)
                    _factory.Abort();
                _channel = null;
                _factory = null;
            }
            catch
            { }
        }

        
        static private string _address;
        private static string binding;
        static public string Address
        {
            set
            {
                _address = value;
            }
            get
            {
                return _address;
            }
        }
        
private static object _channelLock = new object();

         //private static channel and factory instances
         private static ChannelFactory<ITradeServices> _factory;
         private static ITradeServices _channel;

         //the public property that allows the static instances to be safely shared.  It provides
         //the necessary locking logic to ensure the channel is valid, and if not, destroy it so it
         //can be re-initialized in a subsequent call.
         public ITradeServices Channel
         {
             get
             {
                 ITradeServices localChannel = _channel;
                 if (localChannel == null)
                 {
                     lock (_channelLock)
                     {
                         if (_factory == null)
                         {
                             if (ConfigInformation.isAzure)
                                 binding = "Client_Ws2007HttpBinding_T_Security_MCredential_UserName_BSL";
                             else
                                if (_address.Contains("net.tcp"))
                                    binding = "CustomTcpBinding_PrimaryTradeServices";
                                 else
                                    binding = "BasicHttpBinding_PrimaryTradeServices";
                             _factory = new ChannelFactory<ITradeServices>(binding, new EndpointAddress( _address ));
                             if (ConfigInformation.isAzure)
                             {
                                 ConfigInformation config = new ConfigInformation();
                                 _factory.Credentials.UserName.UserName = ConfigInformation.userName;
                                 _factory.Credentials.UserName.Password = ConfigInformation.password;
                             }
                         }
                         if (_channel == null)
                         {
                             _channel = _factory.CreateChannel();
                         }
                         return _channel;
                     }
                 }
                 return localChannel;
             }
             set
             {
                 lock (_channelLock)
                 {
                     if (_channel != null)
                     {

                         if (((IChannel)_channel).State != CommunicationState.Opened)
                         {
                             ((IChannel)_channel).Abort();
                             _channel = null;
                             if (_factory.State != CommunicationState.Opened)
                             {
                                 _factory.Abort();
                                 _factory = null;
                             }
                         }
                     }
                 }
             }
         }


        public void emptyMethodAction()
        {
            try
            {
                this.Channel.emptyMethodAction();
            }
            catch
            {
                this.Channel = null;
                throw;
            }
            
        }

        /// <summary>
        /// Logs user in/authenticates against StockTrader database.
        /// </summary>
        /// <param name="userid">User id to authenticate.</param>
        /// <param name="password">Password for authentication</param>
        public AccountDataModel login(string userID, string password)
        {
            try
            {
                return this.Channel.login(userID, password);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets recent orders for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getOrders(string userID)
        {
            try
            {
                return this.Channel.getOrders(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets account data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public AccountDataModel getAccountData(string userID)
        {
            try
            {
                return this.Channel.getAccountData(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets account profile data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public AccountProfileDataModel getAccountProfileData(string userID)
        {
            return this.Channel.getAccountProfileData(userID);
        }

        /// <summary>
        /// Updates account profile data for a user. 
        /// </summary>
        /// <param name="profileData">Profile data model class with updated info.</param>
        public AccountProfileDataModel updateAccountProfile(AccountProfileDataModel profileData)
        {
            try
            {
                return this.Channel.updateAccountProfile(profileData);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Logs a user out--updates logout count.
        /// </summary>
        /// <param name="userID">User id to logout.</param>
        public void logout(string userID)
        {
            try
            {
                this.Channel.logout(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
            return;
        }

        /// <summary>
        /// Performs a stock buy operation.
        /// </summary>
        /// <param name="userID">User id to create/submit order for.</param>
        /// <param name="symbol">Stock symbol to buy.</param>
        /// <param name="quantity">Shares to buy</param>
        public OrderDataModel buy(string userID, string symbol, double quantity, int orderProcessingMode)
        {
            try
            {
                return this.Channel.buy(userID, symbol, quantity, orderProcessingMode);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Performs a holding sell operation.
        /// </summary>
        /// <param name="userID">User id to create/submit order for.</param>
        /// <param name="holdingID">Holding id to sell off.</param>
        /// <param name="quantity">Shares to sell.</param>
        public OrderDataModel sell(string userID, int holdingID, int orderProcessingMode)
        {
            try
            {
                return this.Channel.sell(userID, holdingID, orderProcessingMode);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets holding data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<HoldingDataModel> getHoldings(string userID)
        {
            try
            {
                return this.Channel.getHoldings(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Registers/adds new user to database.
        /// </summary>
        /// <param name="userID">User id for account creation/login purposes as specified by user.</param>
        /// <param name="password">Password as specified by user.</param>
        /// <param name="fullname">Name as specified by user.</param>
        /// <param name="address">Address as specified by user.</param>
        /// <param name="email">Email as specified by user.</param>
        /// <param name="creditcard">Credit card number as specified by user.</param>
        /// <param name="openBalance">Open balance as specified by user. </param>
        public AccountDataModel register(string userID, string password, string fullname, string address, string email, string creditcard, decimal openBalance)
        {
            try
            {
                return this.Channel.register(userID, password, fullname, address, email, creditcard, openBalance);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets any closed orders for a user--orders that have been processed.  Also updates status to complete.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getClosedOrders(string userID)
        {
            try
            {
                return this.Channel.getClosedOrders(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets the current market summary.  This results in an expensive DB query in the DAL; hence look to cache data returned for 60 second or so.
        /// </summary>
        public MarketSummaryDataModelWS getMarketSummary()
        {
            try
            {
                return this.Channel.getMarketSummary();
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets a single quote based on symbol.
        /// </summary>
        /// <param name="symbol">Symbol to get data for.</param>
        public QuoteDataModel getQuote(string symbol)
        {
            try
            {
                return this.Channel.getQuote(symbol);
            }
            catch 
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets a single quote based on symbol.
        /// </summary>
        /// <param name="symbol">Symbol to get data for.</param>
        public List<QuoteDataModel> getQuotes(string symbolString)
        {
            try
            {
                return this.Channel.getQuotes(symbolString);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets a holding for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        /// <param name="holdingid">Holding id to retrieve data for.</param>
        public HoldingDataModel getHolding(string userID, int holdingID)
        {
            try
            {
                return this.Channel.getHolding(userID, holdingID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Gets specific top n orders for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getTopOrders(string userID)
        {
            try
            {
                return this.Channel.getTopOrders(userID);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Allows user to sell part of a holding vs. all.  Not implemented by Trade 6.1 on WebSphere.
        /// </summary>
        /// <param name="userID">User id to submit sell for.</param>
        /// <param name="holdingID">Holding id to sell.</param>
        /// <param name="quantity">Number of shares to sell.</param>
        public OrderDataModel sellEnhanced(string userID, int holdingID, double quantity)
        {
            try
            {
                return this.Channel.sellEnhanced(userID, holdingID, quantity);
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }

        /// <summary>
        /// Simple online check method.
        /// </summary>
        public void isOnline()
        {
            try
            {
                this.Channel.isOnline();
                return;
            }
            catch
            {
                this.Channel = null;
                throw;
            }
        }
    }
}
