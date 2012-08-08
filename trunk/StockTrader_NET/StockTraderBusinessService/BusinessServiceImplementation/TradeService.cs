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
// The implementation class for the BSL, defining all the business logic.
//======================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Reflection;
using System.Configuration;
using System.Security.Policy;
using System.Data.SqlClient;
using ConfigService.ServiceConfigurationBase;
using ConfigService.ServiceConfigurationUtility;
using ConfigService.ServiceConfigurationHelper;
using ConfigService.ServiceNodeCommunication.DataContract;
using Trade.IDAL;
using Trade.DALFactory;
using Trade.BusinessServiceConfigurationSettings;
using Trade.BusinessServiceDataContract;
using Trade.BusinessServiceContract;
using Trade.Utility;
using Trade.OrderProcessorAsyncClient;
using Trade.OrderProcessorContract;
//using Microsoft.ApplicationServer.Caching;


namespace Trade.BusinessServiceImplementation
{
    /// <summary>
    ///  Tradeservice.cs. This is the base service implementation class--the Business Service Layer or BSL.
    ///  It holds the actual implementation of the business logic for the various
    ///  business service methods or operations, save for the order submit and processing operations for buys and sells
    ///  (these are contained in the separate project/namespace SharedOrderProcessClass). Both the 
    ///  ASMX and WCF Service facades flow through this class by exposing a service interface to this logic. 
    ///  This class makes calls to the separate, logical data access layer (DAL). It activates the database 
    ///  layer via a factory model, loading a specific DAL on demand, vs direcly referencing
    ///  it in the VS project. This helps to further 'decouple' the BSL from the underlying database it operates against.
    /// </summary>
    public class TradeService : ITradeServices
    {
        private IOrder dalOrder;
        private ICustomer dalCustomer;
        private IMarketSummary dalMarketSummary; 
        
        //The following are the core functional service method implementations that
        //provide for the business functions/back-end processing of the application.
        //The BSL uses a separate Data Access Layer (DAL). The DAL is designed 
        //to isolate all data access is a separate logical tier, making changes/maintenance to the two tiers
        //independent and hence eaier.  This class is instanced ona per-call basis, and each instance
        //creates an instance of the DAL layer for its use.  The design is very high performance and stateless on the
        //middle tier.

        public TradeService()
        {
            
        }

        public void emptyMethodAction()
        {
            return;
        }

        public void isOnline()
        {
            return;
        }

        /// <summary>
        /// Logs user in/authenticates against StockTrader database.
        /// </summary>
        /// <param name="userid">User id to authenticate.</param>
        /// <param name="password">Password for authentication</param>
        public AccountDataModel login(string userid, string password) 
        {
            //Create instance of a DAL, which could be designed for any type of DB backend.
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            
            //As feature of the StockTrader DAL, you will see dal.Open, dal.BeginTransaction, dal.CommitTransaction,
            //dal.AbortTransaction and dal.Close methods being invoked in the BSL. The pattern  within this BSL is:
            //a) Create an instance of the DAL; 
            //b) Open the DAL; 
            //c) Start a transaction only if necessary (more than one update/insert/delete involved);
            //d) You get to pick ADO.NET transaction or System.Transactions or ServicedComponent, it will work with
            //   all of the above; StockTrader lets you choose ADO.NET txs or System.Transactions via config.
            //e) Close the DAL. This releases the DAL's internal connection back to the connection pool.

            //The implementation "hides" the type of database being used from the BSL, so this BSL will work
            //with any type of database you create a DAL for, with no changes in the BSL whatsoever. 
            
            //System.Transactions and SQL Server 2005 and above and Oracle databases work together
            //with a new feature called "lightweight transactions"; which means you do not need to have the
            //same performance penalty you got with Serviced Components for always invoking the tx as a full
            //two-phase operation with DTC logging.  If operating against a single database to SQL Server or Oracle,
            //across one or more connections involved in a tx, System.Transactions will not promote to a DTC-coordinated tx; and hence will be much faster.
            //If there are mulitple databases or multiple resources (for example, MSMQ and a database) 
            //used with a System.Transaction tx, on the other hand, the tx will be automatically promoted to the required distributed tx, two-phase commit
            //with DTC logging required. Our StockTrader DAL is designed to:

            // 1.  Hide DB implementation from BSL so we maintain clean separation of BSL from DAL.
            // 2.  Let you freely call into the DAL from BSL methods as many times as you want *without*
            //     creating new separate DB connections
            // 3.  As a by-product, it also helps you use ADO.NET transactions without worrying about
            //     passing DB connections/transaction objects between tiers; maintaining cleaner separation
            //     of BSL from DAL.  If using ADO.NET txs; you can accomplish DB-implementation isolation also with
            //     the Provider Factories introduced with ADO.NET 2.0/.NET 2.0: see for details:

            //             http://msdn2.microsoft.com/en-us/library/ms379620(VS.80).aspx


            
            //Note Open() is not really necessary, since the DAL will open a new connection automatically 
            //if its internal connection is not already open.  It's also free to open up more connections, if desired.
            //We use Open() to stick with a consistent pattern in this application, since the Close() method IS
            //important.  Look for this pattern in all BSL methods below; with a transaction scope defined
            //only for operations that actually require a transaction per line (c) above.
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);
            try
            {
                return dalCustomer.login(userid, password, Settings.USE_SALTEDHASH_PASSWORDS);
            }
            catch
            {
                throw;
            }
            finally
            {
                //Always close the DAL, this releases its primary DB connection.
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Logs a user out--updates logout count.
        /// </summary>
        /// <param name="userID">User id to logout.</param>
        public void logout(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                dalCustomer.logOutUser(userID);
                return;
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets account data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public AccountDataModel getAccountData(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getCustomerByUserID(userID);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets account profile data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public AccountProfileDataModel getAccountProfileData(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getAccountProfileData(userID);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets recent orders for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getOrders(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getOrders(userID, false, Settings.MAX_QUERY_TOP_ORDERS,Settings.MAX_QUERY_ORDERS);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets specific top n orders for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getTopOrders(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getOrders(userID, true, Settings.MAX_QUERY_TOP_ORDERS,Settings.MAX_QUERY_ORDERS);
            }
            catch
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets any closed orders for a user--orders that have been processed.  Also updates status to complete.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<OrderDataModel> getClosedOrders(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getClosedOrders(userID);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }

        }

        /// <summary>
        /// Gets holding data for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        public List<HoldingDataModel> getHoldings(string userID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getHoldings(userID);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }

        /// <summary>
        /// Gets a holding for a user.  Transforms data from DataContract to model UI class for HTML display.
        /// </summary>
        /// <param name="userID">User id to retrieve data for.</param>
        /// <param name="holdingID">Holding id to retrieve data for.</param>
        public HoldingDataModel getHolding(string userID, int holdingID)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.getHolding(userID, holdingID);
            }
            catch
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
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
            

            //Switch is two let you configure which transaction model you want to benchmark/test.
            switch (Settings.TRANSACTION_MODEL)
            {
                case (StockTraderUtility.TRANSACTION_MODEL_SYSTEMDOTTRANSACTION_TRANSACTION):
                    {
                        //This short try/catch block is introduced to deal with idle-timeout on SQL Azure
                        //connections.  It may not be required in the near future, but as of publication
                        //SQL Azure disconnects idle connections after 30 minutes.  While command retry-logic
                        //in the DAL automatically deals with this, when performing a tx, with the BSL handling
                        //tx boundaries, we want to go into the tx with known good connections.  The try/catch below
                        //ensures this.
                        try
                        {
                            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
                            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);
                            dalCustomer.getSQLContextInfo();
                        }
                        catch { }
                        finally { dalCustomer.Close(); }
                         System.Transactions.TransactionOptions txOps = new TransactionOptions();
                         txOps.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                         txOps.Timeout = TimeSpan.FromSeconds((double)Settings.SYSTEMDOTTRANSACTION_TIMEOUT);
                         //Start our System.Transactions tx with the options set above.  System.Transactions
                         //will handle rollbacks automatically if there is an exception; note the 
                         //difference between the System.Transaction case and the ADO.NET transaction case;
                         //and where the dal.Open() happens (which opens a 'hidden' DB connection in DAL).  
                         //System.Transactions will automatically enlist ANY connection
                         //opened within the tx scope in the transaction for you.  Since it supports distributed
                         //tx's; it frees you quite a bit, with the caveat of the overhead of doing a distributed
                         //tx when you do not need one.  Hence: lightweight System.Transactions with  an auto-promote to DTC
                         //only if needed, meaning two or more operations use database connections spanning physicall different databases.
                         //ADO.NET txs always require an already-open connection before starting a tx, and txs cannot span multiple databases, just tables.
                         using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required, txOps))
                         {
                                //Now open the connection, after entering tx scope.
                                dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
                                try
                                {
                                    AccountDataModel newCustomer = addNewRegisteredUser(userID, password, fullname, address, email, creditcard, openBalance);
                                    //Scope complete, commit work.
                                    tx.Complete();
                                    return newCustomer;
                                }
                                catch 
                                {
                                    //no rollback needed, infrastructure will never commit without
                                    //scope.Complete() and immediately issue rollback on and unhandled
                                    //exception.
                                    throw;
                                }
                                finally
                                {
                                    dalCustomer.Close(); 
                                }
                         }
                    }

                case (StockTraderUtility.TRANSACTION_MODEL_ADONET_TRANSACTION):
                    {
                            //ADO.NET TX case:  First you need to open the connecton.
                            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;

                            //Now you start TX 
                            dalCustomer.BeginADOTransaction();
                            try
                            {
                                AccountDataModel newCustomer = addNewRegisteredUser(userID, password, fullname, address, email, creditcard, openBalance);
                                //done, commit.
                                dalCustomer.CommitADOTransaction();
                                return newCustomer;
                            }
                            catch 
                            {
                                //explicit rollback needed.
                                dalCustomer.RollBackTransaction();
                                throw;
                            }
                            finally
                            {
                                //ALWAYS call dal.Close is using StockTrader DAL implementation;
                                //this is equivalent to calling Connection.Close() in the DAL --
                                //but for a generic DB backend as far as the BSL is concerned.
                                dalCustomer.Close();
                            }
                    }
            }
            throw new Exception(Settings.ENABLE_GLOBAL_SYSTEM_DOT_TRANSACTIONS_CONFIGSTRING + ": " + StockTraderUtility.EXCEPTION_MESSAGE_INVALID_TXMMODEL_CONFIG + " Repository ConfigKey table.");
        }

        /// <summary>
        /// Adds user account data to Account table and also profile data to AccountProfile table.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="fullname"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <param name="creditcard"></param>
        /// <param name="openBalance"></param>
        /// <returns></returns>
        private AccountDataModel addNewRegisteredUser(string userID, string password, string fullname, string address, string email, string creditcard, decimal openBalance)
        {
            AccountProfileDataModel customerprofile = new AccountProfileDataModel(userID, password, fullname, address, email, creditcard);
            dalCustomer.insertAccountProfile(customerprofile, Settings.USE_SALTEDHASH_PASSWORDS);
           
            //Check our acid test conditions here for transactional testing; we want to test part way through
            //the register operations from the BSL, to make sure database is never left in state with one
            //insert above going through, and the one below not--the entire BSL operation needs to be
            //treated as one logical unit of work. Also note the ordering of operations here:
            //since trying to register a non-unique userid might be something that happens frequently in the real
            //world, lets do the insert that would fail on this condition first (accountprofile); 
            //rather than wait and do it last.
            if (customerprofile.userID.Equals(StockTraderUtility.ACID_TEST_USER))
                throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_ACID_REGISTRATION);
            AccountDataModel customer = new AccountDataModel(0, userID, DateTime.Now, (decimal)openBalance, 0, (decimal)openBalance, DateTime.Now, 0);
            dalCustomer.insertAccount(customer);
            return customer;
        }

        /// <summary>
        /// Updates account profile data for a user. 
        /// </summary>
        /// <param name="profileData">Profile data model class with updated info.</param>
        public AccountProfileDataModel updateAccountProfile(AccountProfileDataModel profileData)
        {
            dalCustomer = Trade.DALFactory.Customer.Create(Settings.DAL);
            dalCustomer.Open(Settings.TRADEDB_SQL_CONN_STRING);;
            try
            {
                return dalCustomer.update(profileData, Settings.USE_SALTEDHASH_PASSWORDS);
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalCustomer.Close();
            }
        }


        /// <summary>
        /// Gets the current market summary.  This results in an expensive DB query in the DAL; hence look to cache data returned for 60 second or so.
        /// </summary>
        public MarketSummaryDataModelWS getMarketSummary()
        {
            MarketSummaryDataModelWS returnData=null;
                dalMarketSummary = Trade.DALFactory.MarketSummary.Create(Settings.DAL);
                dalMarketSummary.Open(Settings.TRADEDB_SQL_CONN_STRING);
                try
                {
                    returnData = dalMarketSummary.getMarketSummaryData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    dalMarketSummary.Close();
                }
            return returnData;
        }
       

        // *******************************Uncomment this method and comment out the other to implement AppFabric caching for market summary 

        /*
        
        /// <summary>
        /// Gets the current market summary.  This results in an expensive DB query in the DAL; hence look to cache data returned for 60 second or so.
        /// </summary>
        public MarketSummaryDataModelWS getMarketSummary()
        {
            // ============ To use WINDOWS SERVER App Fabric Caching (on-premise only): uncomment at top of page 'using Microsoft.ApplicationServer.Caching;'
            //              Add references to the host program (TradeWebBSL, BusinessServiceHost, BusinessServiceConsole and/or  BusinessServiceNTServiceHost project(s):
            //              \stocktrader\sharedlibraries\winserverappfabric\*.dll  (all assemblies in this directory).  
            //              Add reference to: \stocktrader\sharedlibraries\configuration\ConfigService.WinServerAppFabricUtility.dll
            //
            // ============ To use WINDOWS AZURE App Fabric Caching (on-premise or on-azure): uncomment at top of page 'using Microsoft.ApplicationServer.Caching;'
            //              Add references to the host program (TradeWebBSL, BusinessServiceHost, BusinessServiceConsole and/or  BusinessServiceNTServiceHost project(s):
            //              \stocktrader\sharedlibraries\winazureappfabric\*.dll  (all assemblies in this directory).  
            //              Add reference to: \stocktrader\sharedlibraries\configuration\ConfigService.AzureAppFabricConfigUtility.dll

            //This is an example of using ConfigService distributed cache utility to access a Win Server or Win Azure AppFabric cache
            //without any cache config info embedded in config.  The utility returns a pre-tuned client, already connected to the
            //distributed cache so its high performance.  First, you must find the cache you want from the Settings.connectedCaches list.
            //This list is automatically created based on using ConfigWeb inherited settings to specified connected_caches.
            //Note, your app may use as many different caches (namespaces on Azure, or named caches on Win Server) as you want.
            //Just add more keys to ConfigSettings (currently, keys are defined for up to three different caches from a single app.
            //Also note below, on Azure AppFabric, there is only one named cache 'default'; but you can create many different namespaces, 
            //hence, many different caches (they will all just be named 'default', but the different namespaces mean they reside om
            //different servers (and maybe different geographic regions, depending on how you setup the App Fabric cache namespace(s).
            //At any rate, StockTrader as an app does not present a lot of opportunity to use a cache, since most information should not be
            //stale to the user.  However, market summary is an exception--who cares if updated only every 30 seconds or so?  Plus, its an
            //expensive query.  The below code is for example only.  Yo can also use ASP.NET local output caching from the Web app to cache the
            //results of the call to this method (or, put in a distributed cache the StockTrader Web app uses.  But, it is useful to
            //also cache the data here on the Web Service tier itself; since other clients may be calling this method a lot, putting stress
            //on the database. 

            //The differnce between using a local ASP.NET cache vs. a AppFabric distributed cache is one makes sure all nodes/instances in a scale-out 
            //cluster are using/presenting the exact same data; local .NET caching means nodes will be out of sync.  So it depends on the data
            //and the scenario on when to use a distributed cache vs. a node-local cache. You can never use a local cache for session data
            //in a scale-out scenario: the nodes must be using the same cache.  
            
            
            //Finally, most of the logic below to use the Config Service cache utility could be put into a re-usable method, if an app was going to use the utility in lots of places.


            MarketSummaryDataModelWS returnData=null;
            DistributedCaches myStockTraderMktSummaryCache=null;
            DataCache theCache =null;
            //First we need to find the actual cache we are interested in using, the service domain might be using many
            //different caches, potentially on different servers/azure namespaces.  We will look up by name.  However,
            //we really should look up by name and Azure namespace, since all Azure AppFabric caches are named 'default.' (as 
            //of this writing. On-premise, Win Server AppFabric clusters can have named caches with different names/properties.
            if (Settings.connectedCaches!=null)
                myStockTraderMktSummaryCache = Settings.connectedCaches.Find(delegate (DistributedCaches cacheExists) {return cacheExists.Name.ToLower().Equals("default");});
            //Ok, if we have it, keep going.  If you have not setup a distributed cache, the logic below simply gets it from the DB
            //on every call.
            if (myStockTraderMktSummaryCache!=null)
            {
                //If using the ConfigServer pre-tuned DistributedCacheUtility, make sure to put the getDataCache call in a try-catch,
                //and call closeDataCache on an exception.  An exception can happen if for any reason the
                //cache connection from the client was broken.  The closeDataCache method ensure the cache is
                //disposed of, a new factory created and new connection established on any subsequent calls.
                try
                {
                    theCache = (DataCache)DistributedCacheUtility.getDataCache(myStockTraderMktSummaryCache, false, new Settings());
                    returnData = (MarketSummaryDataModelWS)theCache.Get("mktSummary");
                }
                catch
                {
                    DistributedCacheUtility.closeDataCache(myStockTraderMktSummaryCache, new Settings());
                }
            }
            //Ok, it was either not in the cache, or the cache was offline--so we must go get it from the DB.  And then stuff
            //it into the cache so next caller does not have to goto the DB.
            if (returnData == null)
            {
                dalMarketSummary = Trade.DALFactory.MarketSummary.Create(Settings.DAL);
                dalMarketSummary.Open(Settings.TRADEDB_SQL_CONN_STRING);
                try
                {
                    returnData = dalMarketSummary.getMarketSummaryData();
                    if (theCache != null && returnData!=null)
                        try
                        {
                            theCache.Put("mktSummary", returnData, new TimeSpan(0,0,60));
                        }
                        catch (Exception ePut)
                        {
                            ConfigUtility.writeConfigConsoleMessage("Warning:  a cache put operation failed.\n\n" + ePut.ToString(), System.Diagnostics.EventLogEntryType.Warning, true, new Settings());
                        }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    dalMarketSummary.Close();
                }
            }
            return returnData;
        }

         * */
        
        /// <summary>
        /// Gets a single quote based on symbol.
        /// </summary>
        /// <param name="symbol">Symbol to get data for.</param>
        public QuoteDataModel getQuote(string symbol)
        {
            dalMarketSummary = Trade.DALFactory.MarketSummary.Create(Settings.DAL);
            dalMarketSummary.Open(Settings.TRADEDB_SQL_CONN_STRING);
            try
            {
                return dalMarketSummary.getQuote(symbol);
            }
            catch
            {
                throw;
            }
            finally
            {
                dalMarketSummary.Close();
            }
        }

        /// <summary>
        /// Gets a quotes based on symbols string.
        /// </summary>
        /// <param name="symbol">Symbol to get data for.</param>
        public List<QuoteDataModel> getQuotes(string symbols)
        {
            dalMarketSummary = Trade.DALFactory.MarketSummary.Create(Settings.DAL);
            dalMarketSummary.Open(Settings.TRADEDB_SQL_CONN_STRING);
            try
            {
                string[] quotes = symbols.Split(new char[] { ' ', ',', ';' });
                List<QuoteDataModel> quoteList = new List<QuoteDataModel>();
                foreach (string quote in quotes)
                {
                    string stringquotetrim = quote.Trim();
                    if (!stringquotetrim.Equals(""))
                    {
                        QuoteDataModel quoteData = dalMarketSummary.getQuote(stringquotetrim);
                        if (quoteData != null)
                        {
                            quoteList.Add(quoteData);
                        }
                    }
                }
                return quoteList;
            }
            catch
            {
                throw;
            }
            finally
            {
                dalMarketSummary.Close();
            }
        }
       
                

        /// <summary>
        /// Performs a holding sell operation.
        /// Note orderProcessing mode param is not used by StockTrader; instead
        /// app picks this up from the application configuration.
        /// </summary>
        /// <param name="userID">User id to create/submit order for.</param>
        /// <param name="holdingID">Holding id to sell off.</param>
        /// <param name="orderProcessingMode">Not used, set to zero.</param>
        public OrderDataModel sell(string userID, int holdingID, int orderProcessingMode)
        {
            //In the case of running in 'Sync_InProcess' mode, then the PlaceOrder method 
            //will synchronously invoke the processOrder method as part of this service, and not make an 
            //additional remote service-to-sevice call via WCF. See ProcessOrder.cs.
            //note, this method always sells entire holding, quantity is not passed in.  This is default behavior of WebSphere Trade 6.1
            return placeOrder(StockTraderUtility.ORDER_TYPE_SELL, userID, holdingID, null, (double)0);
        }


        /// <summary>
        /// Allows user to sell part of a holding vs. all.  
        /// This is added functionality that .NET StockTrader implements to allow selling of partial 
        /// holdings, vs. liquidating the entire holding at once.  
        /// </summary>
        /// <param name="userID">User id to submit sell for.</param>
        /// <param name="holdingID">Holding id to sell.</param>
        /// <param name="quantity">Number of shares to sell.</param>
        public OrderDataModel sellEnhanced(string userID, int holdingID, double quantity)
        {
            return placeOrder(StockTraderUtility.ORDER_TYPE_SELL_ENHANCED, userID, holdingID, null, quantity);
        }

        /// <summary>
        /// Performs a stock buy operation.
        /// Note orderProcessing mode param is not used by StockTrader; instead
        /// app picks this up from the application configuration.  
        /// </summary>
        /// <param name="userID">User id to create/submit order for.</param>
        /// <param name="symbol">Stock symbol to buy.</param>
        /// <param name="quantity">Shares to buy.</param>
        ///<param name="orderProcessingMode">Not used.</param>
        public OrderDataModel buy(string userID, string symbol, double quantity, int orderProcessingMode)
        {
            return placeOrder(StockTraderUtility.ORDER_TYPE_BUY, userID, 0, symbol, quantity);
        }
        
        
        /// <summary>
        /// This is the business logic for placing orders, handling txs.
        /// StockTrader allows the user to select whether to use System.Transactions
        /// or use ADO.NET Provider-supplied transactions for order placement (buy, sell); and new user registrations.
        /// The best choice for performance will vary based on the backend database being used with the 
        /// application. 
        /// </summary>
        /// <param name="orderType">Buy or sell type.</param>
        /// <param name="userID">User id to create/submit order for.</param>
        /// <param name="holdingID">Holding id to sell.</param>
        /// <param name="symbol">Stock symbol to buy.</param>
        /// <param name="quantity">Shares to buy.</param>
        public OrderDataModel placeOrder(string orderType, string userID, int holdingID, string symbol, double quantity)
        {
            OrderDataModel order = null;
            HoldingDataModel holding = new HoldingDataModel();
            switch (Settings.TRANSACTION_MODEL)
            {
                case (StockTraderUtility.TRANSACTION_MODEL_SYSTEMDOTTRANSACTION_TRANSACTION):
                    {
                        //This short try/catch block is introduced to deal with idle-timeout on SQL Azure
                        //connections.  It may not be required in the near future, but as of publication
                        //SQL Azure disconnects idle connections after 30 minutes.  While command retry-logic
                        //in the DAL automatically deals with this, when performing a tx, with the BSL handling
                        //tx boundaries, we want to go into the tx with known good connections.  The try/catch below
                        //ensures this.
                        try
                        {
                            dalOrder = Trade.DALFactory.Order.Create(Settings.DAL);
                            dalOrder.Open(Settings.TRADEDB_SQL_CONN_STRING);
                            dalOrder.getSQLContextInfo();
                        }
                        catch { }
                        finally { dalOrder.Close(); }
                        System.Transactions.TransactionOptions txOps = new TransactionOptions();
                        txOps.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                        txOps.Timeout = TimeSpan.FromSeconds(Settings.SYSTEMDOTTRANSACTION_TIMEOUT);
                        //Start our System.Transactions tx with the options set above.
                        using (TransactionScope tx = new TransactionScope(TransactionScopeOption.Required, txOps))
                        {
                            dalOrder.Open(Settings.TRADEDB_SQL_CONN_STRING);
                            try
                            {
                                //Business Step 1:  create the order header.
                                order = createOrder(orderType, userID, holdingID, symbol, quantity, ref holding);

                                //Business Step 2:  Determine which order processing mode to use,
                                //and either process order right away (sync) and in-process with 
                                //calling ASP.NET Web app; or interface with the
                                //async WCF Order Processing Service (cooler) via a service-to-service call,
                                //distributed/remote.
                                if (Settings.ORDER_PROCESSING_MODE != StockTraderUtility.OPS_INPROCESS)
                                {
                                    //Fire up our async client;  we follow the same model here as with the
                                    //StockTrader Web App in that we do not talk 'directly' to the generated proxy
                                    //for the service; rather we channel all calls through a single 
                                    //class that then talks to the service proxy.  This will aid in more
                                    //easily knowing where communication and proxy logic sits; and make changes to services
                                    //you might want to interface with vs. littering proxy calls throughout the
                                    //business tier itself.
                                    TradeOrderServiceAsyncClient asyncclient = new TradeOrderServiceAsyncClient(Settings.ORDER_PROCESSING_MODE, new Settings());
                                       asyncclient.processOrderASync(order);
                                }
                                else
                                {
                                    processOrderSync(order, holding);
                                }

                                //Commit!
                                tx.Complete();
                                return order;
                            }
                            //If per chance you are doing step-through debugging through here and are getting a
                            // "TRANSACTION HAS ABORTED" exception and do not know why,
                            //it's quite likely you are hitting the 15-second timeout we set in
                            //ConfigurationSettings for the System.Transaction options so its just doing what we told it to. 
                            //Simply adjust timeout higher, recompile if you need to.
                            catch
                            {
                                throw;
                            }
                            finally
                            {
                                dalOrder.Close();
                            }
                        }
                    }
               
                //Repeat for ADO.NET transactions config option case. 
                case (StockTraderUtility.TRANSACTION_MODEL_ADONET_TRANSACTION):
                    {
                        if (Settings.ORDER_PROCESSING_MODE == StockTraderUtility.OPS_SELF_HOST_MSMQ)
                            goto case StockTraderUtility.TRANSACTION_MODEL_SYSTEMDOTTRANSACTION_TRANSACTION;
                        dalOrder.Open(Settings.TRADEDB_SQL_CONN_STRING);
                        dalOrder.BeginADOTransaction();
                        try
                        {
                            //Business Step 1:  create the order header.
                            order = createOrder(orderType, userID, holdingID, symbol, quantity, ref holding);

                            //Business Step 2:  Determine which order processing mode to use,
                            //and either process order right away (sync); or interface with the
                            //async WCF Order Processing Service (cooler) via a service-to-service call.
                            if (Settings.ORDER_PROCESSING_MODE != StockTraderUtility.OPS_INPROCESS)
                            {
                                //Fire up our async client;  we follow the same model here as with the
                                //StockTrader Web App in that we do not talk 'directly' to the generated proxy
                                //for the service; rather we channel all calls through a single 
                                //class that then talks to the service proxy.  This will aid in more
                                //easily knowing where communication and proxy logic sits; and make changes to services
                                //you might want to interface with vs. littering proxy calls throughout the
                                //business tier itself.
                                TradeOrderServiceAsyncClient asyncclient = new TradeOrderServiceAsyncClient(Settings.ORDER_PROCESSING_MODE, new Settings());
                                    asyncclient.processOrderASync(order);
                                dalOrder.CommitADOTransaction();
                            }
                            else
                            {
                                processOrderSync(order, holding);
                            }
                            dalOrder.CommitADOTransaction();
                            return order;
                        }
                        catch 
                        {
                            dalOrder.RollBackTransaction();
                            throw;
                        }
                        finally
                        {
                            dalOrder.Close();
                        }
                    }
            }
            throw new Exception(Settings.ENABLE_GLOBAL_SYSTEM_DOT_TRANSACTIONS_CONFIGSTRING + ": " + StockTraderUtility.EXCEPTION_MESSAGE_INVALID_TXMMODEL_CONFIG + " Repository ConfigKey table.");
        }

        
        /// <summary>
        /// Business logic to create the order header.
        /// The order header is always created synchronously by Trade; its the actual
        /// processing of the order that can be done asynchrounously via the WCF service.
        /// If, however, this service cannot communicate with the async order processor,
        /// the order header is rolled back out of the database since we are wrapped in a tx here
        /// either ADO.NET tx or System.TransactionScope as noted above, based on user setting.
        /// </summary>
        private OrderDataModel createOrder(string orderType, string userID, int holdingID, string symbol, double quantity, ref HoldingDataModel holding)
        {
            OrderDataModel order;
            switch (orderType)
            {
                case StockTraderUtility.ORDER_TYPE_SELL:
                    {
                        holding = dalOrder.getHolding(holdingID);
                        if (holding == null)
                            throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_INVALID_HOLDING_NOT_FOUND);
                        order = dalOrder.createOrder(userID, holding.quoteID, StockTraderUtility.ORDER_TYPE_SELL, holding.quantity, holdingID);
                        break;
                    }
                //StockTrader 5 allows users to sell part
                //of a holding, not required to sell all shares at once.  This business logic
                //on the processing side of the pipe is more tricky.  Have to check for 
                //conditions like another order coming in and the shares do not exist anymore 
                //in the holding, etc.  This is not done here--it is done in the ProcessOrder class.
                //Here we are merely creatingt he order header as with all orders--note its just the *quantity* variable
                //that varies with SELL_ENHANCED case vs. SELL.  SELL always uses the total
                //number of shares in the holding to sell; here we have obtained from the
                //requesting service (for example, the StockTrader Web App), how many shares
                //the user actually wants to sell.
                case StockTraderUtility.ORDER_TYPE_SELL_ENHANCED:
                    {
                        holding = dalOrder.getHolding(holdingID);
                        if (holding == null)
                            throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_INVALID_HOLDING_NOT_FOUND);
                        //If user requests to sell more shares than in holding, we will just invoke the core
                        //sell operation which liquidates/sells the entire holding. Seems logical--they will
                        //get proper notification based on the client order alert in the Web app how many
                        //were actually sold.
                        if (quantity > holding.quantity)
                        {
                            goto case StockTraderUtility.ORDER_TYPE_SELL;
                        }
                        else
                        {
                            order = dalOrder.createOrder(userID, holding.quoteID, StockTraderUtility.ORDER_TYPE_SELL, quantity, holdingID);
                            break;
                        }
                    }
                case StockTraderUtility.ORDER_TYPE_BUY:
                    {
                        //Buys are easier business case!  Especially when on unlimited margin accounts :-).
                        order = dalOrder.createOrder(userID, symbol, StockTraderUtility.ORDER_TYPE_BUY, quantity, -1);
                        break;
                    }
                default:
                    throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_BADORDERTYPE);
            }
            return order;
        }

        /// <summary>
        /// Business logic to synchrounously process the order within BSL layer, used with OrderMode="Sync."
        /// </summary>
        /// <param name="order">Order information model class with header info.</param>
        /// <param name="holding">Holding model class with holding to sell if applicable.</param>
        public void processOrderSync(OrderDataModel order, HoldingDataModel holding)
        {
            try
            {
                decimal total = 0;
                int holdingid = -1;
                QuoteDataModel quote = dalOrder.getQuoteForUpdate(order.symbol);
                //Get the latest trading price--this is the money going into (or out of) the users account.
                order.price = quote.price;

                //Calculate order total, and create/update the Holding. Whole holding 
                //sells delete the holding, partials sells update the holding with new amount
                //(and potentially the order too), and buys create a new holding.
                if (order.orderType == StockTraderUtility.ORDER_TYPE_BUY)
                {
                    holdingid = dalOrder.createHolding(order);
                    total = Convert.ToDecimal(order.quantity) * order.price + order.orderFee;
                }
                else
                    if (order.orderType == StockTraderUtility.ORDER_TYPE_SELL)
                    {
                        holdingid = sellHolding(order, holding);
                        total = -1 * Convert.ToDecimal(order.quantity) * order.price + order.orderFee;
                    }

                //Debit/Credit User Account.  Note, if we did not want to allow unlimited margin
                //trading, we would change the ordering a bit and add the biz logic here to make
                //sure the user has enough money to actually buy the shares they asked for!

                //Now Update Account Balance.
                dalOrder.updateAccountBalance(order.accountID, total);

                //Update the stock trading volume and price in the quote table
                dalOrder.updateStockPriceVolume(order.quantity, quote);

                //Now perform our ACID tx test, if requested based on order type and acid stock symbols
                if (order.symbol.Equals(StockTraderUtility.ACID_TEST_BUY) && order.orderType == StockTraderUtility.ORDER_TYPE_BUY)
                    throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_ACID_BUY);
                else
                    if (order.symbol.Equals(StockTraderUtility.ACID_TEST_SELL) && order.orderType == StockTraderUtility.ORDER_TYPE_SELL)
                        throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_ACID_SELL);

                //Finally, close the order
                order.orderStatus = StockTraderUtility.ORDER_STATUS_CLOSED;
                order.completionDate = DateTime.Now;
                order.holdingID = holdingid;
                dalOrder.closeOrder(order);
                //Done!

                return;
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Sell the holding.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="holding"></param>
        /// <returns></returns>
        int sellHolding(OrderDataModel order, HoldingDataModel holding)
        {
            int holdingid = holding.holdingID;
            //There are three distinct business cases here, each needs different treatment:  
            // a) Quantity requested is less than total shares in the holding -- update holding.  
            // b) Quantity requested is equal to total shares in the holding -- delete holding.  
            // c) Quantity requested is greater than total shares in the holding -- delete holding, update order table.  
            if (order.quantity < holding.quantity)
            {
                dalOrder.updateHolding(holdingid, order.quantity);
            }
            else
                if (holding.quantity == order.quantity)
                {
                    dalOrder.deleteHolding(holdingid);
                }
                else
                    //We now need to back-update the order record quantity to reflect
                    //fact not all shares originally requested were sold since the holding 
                    //had less shares in it, perhaps due to other orders 
                    //placed against that holding that completed before this one. So we will
                    //sell the remaining shares, but need to update the final order to reflect this.
                    if (order.quantity > holding.quantity)
                    {
                        dalOrder.deleteHolding(holdingid);
                        order.quantity = holding.quantity;
                        dalOrder.updateOrder(order);
                    }
            return holdingid;
        }
    }
}