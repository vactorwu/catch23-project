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

//===============================================================================================
// Order is part of the SQLServer DAL for StockTrader.  This is called from the
// BSL to execute commands against the database.  It is constructed to use one SqlConnection per
// instance.  Hence, BSLs that use this DAL should always be instanced properly.
// The DAL will work with both ADO.NET and System.Transactions or ServiceComponents/Enterprise
// Services attributed transactions [autocomplete]. When using ADO.NET transactions,
// The BSL will control the transaction boundaries with calls to dal.BeginTransaction(); 
// dal.CommitTransaction(); dal.RollbackTransaction().
//===============================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Trade.BusinessServiceDataContract;
using ConfigService.DALSQLHelper;
using Trade.IDAL;
using Trade.Utility;


namespace Trade.DALSQLServer
{
    public class Order : IOrder 
    {
        public Order()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public Order(SqlConnection conn)
        {
            _internalConnection = conn;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        private SqlConnection _internalConnection;

        //_internalADOTransaction: Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In SQLHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        private SqlTransaction _internalADOTransaction;
        
        //Used only when doing ADO.NET transactions.
        public void BeginADOTransaction()
        {
            if (_internalConnection.State != ConnectionState.Open)
            {
                _internalConnection.Open();
            }
            getSQLContextInfo();
          _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        //Used only when explicitly using ADO.NET transactions from the BSL.
        public void RollBackTransaction()
        {
            if (_internalADOTransaction != null) 
                _internalADOTransaction.Rollback();
            _internalADOTransaction = null;
        }

        //Used only when explicitly using ADO.NET transactions from the BSL.
        public void CommitADOTransaction()
        {
            if (_internalADOTransaction != null) 
                _internalADOTransaction.Commit();
            _internalADOTransaction = null;
        }

        public void Open(string connString)
        {
            if (_internalConnection == null)
                _internalConnection = new SqlConnection(connString);
            if (_internalConnection.State != ConnectionState.Open)
            {
                _internalConnection.Open();
            }
        }

        public void Close()
        {
            if (_internalConnection != null && _internalConnection.State != ConnectionState.Closed)
                _internalConnection.Close();
        }

        private const string SQL_INSERT_ORDER = "SET NOCOUNT ON; INSERT INTO dbo.ORDERS (OPENDATE, ORDERFEE, PRICE, QUOTE_SYMBOL, QUANTITY, ORDERTYPE, ORDERSTATUS, ACCOUNT_ACCOUNTID, HOLDING_HOLDINGID) VALUES (GETDATE(),  @OrderFee, @Price, @QuoteSymbol, @Quantity, @OrderType, 'open', @accountId, @HoldingId); SELECT ID=@@IDENTITY";
        private const string SQL_GET_ACCOUNTID = "SET NOCOUNT ON; SELECT ACCOUNTID FROM dbo.ACCOUNT WHERE PROFILE_USERID = @userId";
        private const string SQL_GET_ACCOUNTID_ORDER = "SET NOCOUNT ON; SELECT ACCOUNT_ACCOUNTID FROM dbo.ORDERS WHERE ORDERID=@OrderId";
        private const string SQL_INSERT_HOLDING = "SET NOCOUNT ON; INSERT INTO dbo.HOLDING (PURCHASEPRICE, QUANTITY, PURCHASEDATE, ACCOUNT_ACCOUNTID, QUOTE_SYMBOL) VALUES (@PurchasePrice, @Quantity, @PurchaseDate, @AccountId, @QuoteSymbol); SELECT ID=@@IDENTITY";
        private const string SQL_SELECT_HOLDING = "SET NOCOUNT ON; SELECT HOLDING.HOLDINGID, HOLDING.QUANTITY, HOLDING.PURCHASEPRICE, HOLDING.PURCHASEDATE, HOLDING.QUOTE_SYMBOL,HOLDING.ACCOUNT_ACCOUNTID, Quote.Price FROM dbo.HOLDING WITH (NOLOCK) INNER JOIN Quote ON Holding.Quote_Symbol=Quote.Symbol WHERE HOLDINGID= @HoldingId";
        private const string SQL_DELETE_HOLDING = "SET NOCOUNT ON; DELETE FROM dbo.HOLDING WITH (ROWLOCK) WHERE HOLDINGID=@HoldingId";
        private const string SQL_GET_HOLDING_QUANTITY = "SET NOCOUNT ON; SELECT QUANTITY FROM dbo.HOLDING WITH (ROWLOCK) WHERE HOLDINGID=@HoldingId";
        private const string SQL_UPDATE_HOLDING = "SET NOCOUNT ON; UPDATE dbo.HOLDING WITH (ROWLOCK) SET QUANTITY=QUANTITY-@Quantity WHERE HOLDINGID=@HoldingId";
        private const string SQL_UPDATE_ORDER = "SET NOCOUNT ON; UPDATE dbo.ORDERS WITH (ROWLOCK) SET QUANTITY=@Quantity WHERE ORDERID=@OrderId";
        private const string SQL_CLOSE_ORDER = "SET NOCOUNT ON; UPDATE dbo.ORDERS WITH (ROWLOCK) SET ORDERSTATUS = @status, COMPLETIONDATE=GetDate(), HOLDING_HOLDINGID=@HoldingId, PRICE=@Price WHERE ORDERID = @OrderId";

        //Parameters
        private const string PARM_SYMBOL = "@QuoteSymbol";
        private const string PARM_USERID = "@userId";
        private const string PARM_ORDERSTATUS = "@status";
        private const string PARM_QUANTITY = "@Quantity";
        private const string PARM_ORDERTYPE = "@OrderType";
        private const string PARM_ACCOUNTID = "@accountId";
        private const string PARM_ORDERID = "@OrderId";
        private const string PARM_HOLDINGID = "@HoldingId";
        private const string PARM_ORDERFEE = "@OrderFee";
        private const string PARM_PRICE = "@Price";
        private const string PARM_PURCHASEPRICE = "@PurchasePrice";
        private const string PARM_PURCHASEDATE = "@PurchaseDate";
        
        public void getSQLContextInfo()
        {
            string sql = "SELECT 0"; 
            SQLHelper.ExecuteScalarNoParm(_internalConnection, _internalADOTransaction, CommandType.Text, sql);
            return;
        }
 
        public QuoteDataModel getQuoteForUpdate(string symbol)
        {
            //Cross-DAL calls pass in their own connection if they want to ensure commans are
            //executed on the same connection and optional ADO transaction.  If 
            //_internalADOTransaction is null, as with all DAL classes, it will be ignored.
            MarketSummary marketsummaryDal = new MarketSummary(_internalConnection, _internalADOTransaction);
            return marketsummaryDal.getQuoteForUpdate(symbol);
        }

        public void updateStockPriceVolume(double quantity, QuoteDataModel quote)
        {
            //See note above: want to use existing connection
            MarketSummary marketSummaryDal = new MarketSummary(_internalConnection, _internalADOTransaction);
            marketSummaryDal.updateStockPriceVolume(quantity, quote);
            return;
        }

        public HoldingDataModel getHoldingForUpdate(int orderID)
        {
            //See note above: want to use existing connection
            Customer customerDal = new Customer(_internalConnection, _internalADOTransaction);
            return customerDal.getHoldingForUpdate(orderID);
        }

        public void updateAccountBalance(int accountID, decimal total)
        {
           //See note above: want to use existing connection
           Customer customerDal = new Customer(_internalConnection, _internalADOTransaction);
           customerDal.updateAccountBalance(accountID, total);
           return;
        }
        
        public OrderDataModel createOrder(string userID, string symbol, string orderType, double quantity, int holdingID )
        {
            try
            {
                DateTime dt = DateTime.MinValue;
                int orderid = 0;
                OrderDataModel order = new OrderDataModel(orderid, orderType, StockTraderUtility.ORDER_STATUS_OPEN, DateTime.Now, DateTime.MinValue, quantity, (decimal)1, StockTraderUtility.getOrderFee(orderType), symbol);
                order.holdingID = holdingID;
                SqlParameter[] parm = new SqlParameter[]{new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20)};
                parm[0].Value = userID;
                order.accountID = Convert.ToInt32(SQLHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_GET_ACCOUNTID, parm));
                SqlParameter[] OrderParms = GetCreateOrderParameters();
                OrderParms[0].Value = order.orderFee;
                OrderParms[1].Value = order.price;
                OrderParms[2].Value = order.symbol;
                OrderParms[3].Value = (float)order.quantity;
                OrderParms[4].Value = order.orderType;
                OrderParms[5].Value = order.accountID;
                OrderParms[6].Value = holdingID;
                order.orderID = Convert.ToInt32(SQLHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_ORDER, OrderParms));
                return order;
            }
            catch 
            {
                throw;
            }
        }

        public HoldingDataModel getHolding(int holdingID)
        {
            SqlParameter parm1 = new SqlParameter(PARM_HOLDINGID, SqlDbType.Int, 10);
            parm1.Value = holdingID;
            SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING, parm1);
            if (rdr.Read())
            {
                HoldingDataModel holding = new HoldingDataModel(rdr.GetInt32(0), rdr.GetDouble(1), rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDecimal(6));
                rdr.Close();
                return holding;
            }
            rdr.Close();
            return null;
        }

        public int createHolding(OrderDataModel order)
        {
            try
            {
                SqlParameter orderParm = new SqlParameter(PARM_ORDERID, SqlDbType.Int, 10);
                orderParm.Value = order.orderID;
                order.accountID = Convert.ToInt32(SQLHelper.ExecuteScalarSingleParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_GET_ACCOUNTID_ORDER,orderParm));
                SqlParameter[] HoldingParms = GetCreateHoldingParameters();
                HoldingParms[0].Value = order.price;
                HoldingParms[1].Value = (float)order.quantity;
                HoldingParms[2].Value = order.openDate;
                HoldingParms[3].Value = order.accountID;
                HoldingParms[4].Value = order.symbol;
                int holdingid = SQLHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_HOLDING, HoldingParms);
                return holdingid;
            }
            catch
            {
                throw;
            }
        }

        public void updateHolding(int holdingid, double quantity)
        {
            try
            {
                SqlParameter[] HoldingParms2 = GetUpdateHoldingParameters();
                HoldingParms2[0].Value = holdingid;
                HoldingParms2[1].Value = quantity;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_HOLDING, HoldingParms2);
                HoldingDataModel holding = new HoldingDataModel();
                return;
            }
            catch
            {
                throw;
            }
        }

        public void deleteHolding(int holdingid)
        {
            try
            {
                SqlParameter[] HoldingParms2 = { new SqlParameter(PARM_HOLDINGID, SqlDbType.Int) };
                HoldingParms2[0].Value = holdingid;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_DELETE_HOLDING, HoldingParms2);
                return;
            }
            catch
            {
                throw;
            }
        }

        public void updateOrder(OrderDataModel order)
        {
            try
            {
                SqlParameter[] orderparms = GetUpdateOrderParameters();
                orderparms[0].Value = order.quantity;
                orderparms[1].Value = order.orderID;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_ORDER, orderparms);
            }
            catch
            {
                throw; 
            }
            return;
        }

        public void closeOrder(OrderDataModel order)
        {
            try
            {
                SqlParameter[] closeorderparm = GetCloseOrdersParameters();
                closeorderparm[0].Value = StockTraderUtility.ORDER_STATUS_CLOSED;
                if (order.orderType.Equals(StockTraderUtility.ORDER_TYPE_SELL))
                    closeorderparm[1].Value = DBNull.Value;
                else
                    closeorderparm[1].Value = order.holdingID;
                closeorderparm[2].Value = order.price;
                closeorderparm[3].Value = order.orderID;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_CLOSE_ORDER, closeorderparm);   
            }
            catch
            {
                throw;
            }
            return;
        }

        private static SqlParameter[] GetCreateOrderParameters()
        {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_INSERT_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
                           new SqlParameter(PARM_ORDERFEE, SqlDbType.Decimal, 14),
                           new SqlParameter(PARM_PRICE, SqlDbType.Decimal, 14),
	                  	   new SqlParameter(PARM_SYMBOL, SqlDbType.VarChar, 20),
						   new SqlParameter(PARM_QUANTITY, SqlDbType.Float),
                           new SqlParameter(PARM_ORDERTYPE, SqlDbType.VarChar,5),
                           new SqlParameter(PARM_ACCOUNTID, SqlDbType.Int, 10),
                           new SqlParameter(PARM_HOLDINGID, SqlDbType.Int,10)};
                 // Add the parametes to the cached
                 SQLHelper.CacheParameters(SQL_INSERT_ORDER, parms);
            }
            return parms;
        }

            private static SqlParameter[] GetUpdateOrderParameters()
            {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_UPDATE_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {new SqlParameter(PARM_QUANTITY, SqlDbType.Float),
                                            new SqlParameter(PARM_ORDERID,SqlDbType.Int)};
                             
                SQLHelper.CacheParameters(SQL_UPDATE_ORDER, parms);
            }
            return parms;
            }

        private static SqlParameter[] GetCreateHoldingParameters()
        {

            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_INSERT_HOLDING);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
                           new SqlParameter(PARM_PURCHASEPRICE, SqlDbType.Decimal, 14),
                           new SqlParameter(PARM_QUANTITY, SqlDbType.Float),
                           new SqlParameter(PARM_PURCHASEDATE, SqlDbType.DateTime),
                       	   new SqlParameter(PARM_ACCOUNTID, SqlDbType.Int),
                		   new SqlParameter(PARM_SYMBOL, SqlDbType.VarChar,20)};
                
                // Add the parametes to the cached
                SQLHelper.CacheParameters(SQL_INSERT_HOLDING, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetUpdateHoldingParameters()
        {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_UPDATE_HOLDING);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {new SqlParameter(PARM_HOLDINGID, SqlDbType.Int),
                                            new SqlParameter(PARM_QUANTITY, SqlDbType.Float)};
                // Add the parametes to the cached
                SQLHelper.CacheParameters(SQL_UPDATE_HOLDING, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetCloseOrdersParameters()
        {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_CLOSE_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
                        new SqlParameter(PARM_ORDERSTATUS, SqlDbType.VarChar,10),
                        new SqlParameter(PARM_HOLDINGID, SqlDbType.Int),
                        new SqlParameter(PARM_PRICE, SqlDbType.Decimal, 14),
                        new SqlParameter(PARM_ORDERID, SqlDbType.Int)};

                // Add the parametes to the cached
                SQLHelper.CacheParameters(SQL_CLOSE_ORDER, parms);
            }
            return parms;
        }
    }
  }

