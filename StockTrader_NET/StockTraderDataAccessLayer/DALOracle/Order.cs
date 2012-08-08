//  .Net StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications

//===============================================================================================
// Order is part of the Oracle DAL for StockTrader.  This is called from the
// BSL to execute commands against the database.  It is constructed to use one OracleConnection per
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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Trade.BusinessServiceDataContract;
using Trade.IDAL;
using Trade.Utility;


namespace Trade.DALOracle
{
    public class Order : IOrder 
    {
        public Order()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public Order(OracleConnection conn)
        {
            _internalConnection = conn;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        private OracleConnection _internalConnection;

        //_internalADOTransaction: Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In OracleHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        private OracleTransaction _internalADOTransaction;
        
        //Used only when doing ADO.NET transactions.
        public void BeginADOTransaction()
        {
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
          _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        //Used only when explicitly using ADO.NET transactions from the BSL.
        public void RollBackTransaction()
        {
            _internalADOTransaction.Rollback();
            _internalADOTransaction = null;
        }

        //Used only when explicitly using ADO.NET transactions from the BSL.
        public void CommitADOTransaction()
        {
            _internalADOTransaction.Commit();
            _internalADOTransaction = null;
        }

        public void Open(string connString)
        {
            if (_internalConnection == null)
                _internalConnection = new OracleConnection(connString);
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
        }

        public void Close()
        {
            if (_internalConnection != null && _internalConnection.State != ConnectionState.Closed)
                _internalConnection.Close();
        }

        public void getSQLContextInfo()
        {
            return;
        }

        private const string SQL_INSERT_ORDER = "INSERT INTO ORDEREJB (ORDERID, OPENDATE, ORDERFEE, PRICE, QUOTE_SYMBOL, QUANTITY, ORDERTYPE, ORDERSTATUS, ACCOUNT_ACCOUNTID, HOLDING_HOLDINGID) VALUES (:OrderId, CURRENT_DATE,  :OrderFee, :Price, :QuoteSymbol, :Quantity, :OrderType, 'open', :accountId, :HoldingId)";
        private const string SQL_GET_ACCOUNTID = "SELECT ACCOUNTID FROM ACCOUNTEJB WHERE PROFILE_USERID = :userId";
        private const string SQL_GET_ACCOUNTID_ORDER = "SELECT ACCOUNT_ACCOUNTID FROM ORDEREJB  WHERE ORDERID=:OrderId";
        private const string SQL_INSERT_HOLDING = "INSERT INTO HOLDINGEJB (HOLDINGID, PURCHASEPRICE, QUANTITY, PURCHASEDATE, ACCOUNT_ACCOUNTID, QUOTE_SYMBOL) VALUES (:HoldingId, :PurchasePrice, :Quantity, :PurchaseDate, :AccountId, :QuoteSymbol)"; //uote.Price FROM dbo.HOLDING WITH (NOLOCK) INNER JOIN Quote ON Holding.Quote_Symbol=Quote.Symbol
        private const string SQL_SELECT_HOLDING = "SELECT HOLDINGEJB.HOLDINGID, HOLDINGEJB.QUANTITY, HOLDINGEJB.PURCHASEPRICE, HOLDINGEJB.PURCHASEDATE, HOLDINGEJB.QUOTE_SYMBOL,HOLDINGEJB.ACCOUNT_ACCOUNTID, QUOTEEJB.PRICE FROM HOLDINGEJB INNER JOIN QUOTEEJB ON HOLDINGEJB.QUOTE_SYMBOL=QUOTEEJB.SYMBOL WHERE HOLDINGID= :HoldingId";
        private const string SQL_DELETE_HOLDING = "DELETE FROM HOLDINGEJB WHERE HOLDINGID=:HoldingId";
        private const string SQL_GET_HOLDING_QUANTITY = "SELECT QUANTITY FROM HOLDINGEJB WHERE HOLDINGID=:HoldingId";
        private const string SQL_UPDATE_HOLDING = "UPDATE HOLDINGEJB SET QUANTITY=(QUANTITY - {0}) WHERE HOLDINGID=:HoldingId";
        private const string SQL_UPDATE_ORDER = "UPDATE ORDEREJB SET QUANTITY=:Quantity WHERE ORDERID=:OrderId";
        private const string SQL_CLOSE_ORDER = "UPDATE ORDEREJB SET ORDERSTATUS = :status, COMPLETIONDATE=CURRENT_DATE, HOLDING_HOLDINGID=:HoldingId, PRICE=:Price WHERE ORDERID = :OrderId";
        private const string SQL_GET_NEXT_ORDER_SEQ = "SELECT ORDEREJB_SEQ.NEXTVAL FROM DUAL";
        private const string SQL_GET_NEXT_HOLDING_SEQ = "SELECT HOLDINGEJB_SEQ.NEXTVAL FROM DUAL";

        //Parameters
        private const string PARM_SYMBOL = ":QuoteSymbol";
        private const string PARM_USERID = ":userId";
        private const string PARM_ORDERSTATUS = ":status";
        private const string PARM_QUANTITY = ":Quantity";
        private const string PARM_ORDERTYPE = ":OrderType";
        private const string PARM_ACCOUNTID = ":accountId";
        private const string PARM_ORDERID = ":OrderId";
        private const string PARM_HOLDINGID = ":HoldingId";
        private const string PARM_ORDERFEE = ":OrderFee";
        private const string PARM_PRICE = ":Price";
        private const string PARM_PURCHASEPRICE = ":PurchasePrice";
        private const string PARM_PURCHASEDATE = ":PurchaseDate";
        
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
                order.orderID = Convert.ToInt32(OracleHelper.ExecuteScalarNoParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_GET_NEXT_ORDER_SEQ));
                OracleParameter[] parm = new OracleParameter[]{new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20)};
                parm[0].Value = userID;
                order.accountID = Convert.ToInt32(OracleHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_GET_ACCOUNTID, parm));
                OracleParameter[] OrderParms = GetCreateOrderParameters();
                OrderParms[0].Value = order.orderID;
                OrderParms[1].Value = order.orderFee;
                OrderParms[2].Value = order.price;
                OrderParms[3].Value = order.symbol;
                OrderParms[4].Value = (float)order.quantity;
                OrderParms[5].Value = order.orderType;
                OrderParms[6].Value = order.accountID;
                OrderParms[7].Value = holdingID;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_ORDER, OrderParms);
                return order;
            }
            catch 
            {
                throw;
            }
        }

        public HoldingDataModel getHolding(int holdingID)
        {
            OracleParameter parm1 = new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32, 10);
            parm1.Value = holdingID;
            OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING, parm1);
            if (rdr.Read())
            {
                HoldingDataModel holding = new HoldingDataModel(Convert.ToInt32(rdr.GetDecimal(0)), Convert.ToDouble(rdr.GetDecimal(1)), rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4), Convert.ToInt32(rdr.GetDecimal(5)), rdr.GetDecimal(6));
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
                OracleParameter orderParm = new OracleParameter(PARM_ORDERID, OracleDbType.Int32, 10);
                orderParm.Value = order.orderID;
                order.accountID = Convert.ToInt32(OracleHelper.ExecuteScalarSingleParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_GET_ACCOUNTID_ORDER,orderParm));
                OracleParameter[] HoldingParms = GetCreateHoldingParameters();
                int holdingid = Convert.ToInt32(OracleHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_GET_NEXT_HOLDING_SEQ));
                HoldingParms[0].Value = holdingid;
                HoldingParms[1].Value = order.price;
                HoldingParms[2].Value = (float)order.quantity;
                HoldingParms[3].Value = order.openDate;
                HoldingParms[4].Value = order.accountID;
                HoldingParms[5].Value = order.symbol;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_HOLDING, HoldingParms);
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

                OracleParameter[] HoldingParms2 = GetUpdateHoldingParameters();
                HoldingParms2[0].Value = holdingid;
                string sql = String.Format(SQL_UPDATE_HOLDING, quantity.ToString());                
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, sql, HoldingParms2);
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
                OracleParameter[] HoldingParms2 = { new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32) };
                HoldingParms2[0].Value = holdingid;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_DELETE_HOLDING, HoldingParms2);
                return;
            }
            catch (Exception e)
            {
                throw new Exception("Error on Trade.DALSQLServer.Order.deleteHolding: " + e.Message);
            }
        }

        public void updateOrder(OrderDataModel order)
        {
            try
            {
                OracleParameter[] orderparms = GetUpdateOrderParameters();
                orderparms[0].Value = order.quantity;
                orderparms[1].Value = order.orderID;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_ORDER, orderparms);
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
                OracleParameter[] closeorderparm = GetCloseOrdersParameters();
                closeorderparm[0].Value = StockTraderUtility.ORDER_STATUS_CLOSED;
                if (order.orderType.Equals(StockTraderUtility.ORDER_TYPE_SELL))
                    closeorderparm[1].Value = DBNull.Value;
                else
                    closeorderparm[1].Value = order.holdingID;
                closeorderparm[2].Value = order.price;
                closeorderparm[3].Value = order.orderID;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_CLOSE_ORDER, closeorderparm);   
            }
            catch 
            {
                throw;
            }
            return;
        }

        private static OracleParameter[] GetCreateOrderParameters()
        {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_INSERT_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
                           new OracleParameter(PARM_ORDERID, OracleDbType.Int32, 10),
                           new OracleParameter(PARM_ORDERFEE, OracleDbType.Decimal, 14),
                           new OracleParameter(PARM_PRICE, OracleDbType.Decimal, 14),
	                  	   new OracleParameter(PARM_SYMBOL, OracleDbType.Varchar2, 20),
						   new OracleParameter(PARM_QUANTITY, OracleDbType.Double),
                           new OracleParameter(PARM_ORDERTYPE, OracleDbType.Varchar2,5),
                           new OracleParameter(PARM_ACCOUNTID, OracleDbType.Int32, 10),
                           new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32,10)};
                 // Add the parametes to the cached
                 OracleHelper.CacheParameters(SQL_INSERT_ORDER, parms);
            }
            return parms;
        }

            private static OracleParameter[] GetUpdateOrderParameters()
            {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_UPDATE_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {new OracleParameter(PARM_QUANTITY, OracleDbType.Double),
                                            new OracleParameter(PARM_ORDERID,OracleDbType.Int32)};
                             
                OracleHelper.CacheParameters(SQL_UPDATE_ORDER, parms);
            }
            return parms;
            }

        private static OracleParameter[] GetCreateHoldingParameters()
        {

            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_INSERT_HOLDING);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
                           new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32,10),   
                           new OracleParameter(PARM_PURCHASEPRICE, OracleDbType.Decimal, 14),
                           new OracleParameter(PARM_QUANTITY, OracleDbType.Double),
                           new OracleParameter(PARM_PURCHASEDATE, OracleDbType.TimeStamp),
                       	   new OracleParameter(PARM_ACCOUNTID, OracleDbType.Int32),
                		   new OracleParameter(PARM_SYMBOL, OracleDbType.Varchar2,20)};
                
                // Add the parametes to the cached
                OracleHelper.CacheParameters(SQL_INSERT_HOLDING, parms);
            }
            return parms;
        }

        private static OracleParameter[] GetUpdateHoldingParameters()
        {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_UPDATE_HOLDING);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32)};

                // Add the parametes to the cached
                OracleHelper.CacheParameters(SQL_UPDATE_HOLDING, parms);
            }
            return parms;
        }

        private static OracleParameter[] GetCloseOrdersParameters()
        {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_CLOSE_ORDER);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
                        new OracleParameter(PARM_ORDERSTATUS, OracleDbType.Varchar2,10),
                        new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32),
                        new OracleParameter(PARM_PRICE, OracleDbType.Decimal, 14),
                        new OracleParameter(PARM_ORDERID, OracleDbType.Int32)};

                // Add the parametes to the cached
                OracleHelper.CacheParameters(SQL_CLOSE_ORDER, parms);
            }
            return parms;
        }
    }
  }

