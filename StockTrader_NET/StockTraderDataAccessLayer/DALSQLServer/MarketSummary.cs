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
// MarketSummary is part of the SQLServer DAL for StockTrader.  This is called from the
// BSL to execute commands against the database.  It is constructed to use one SqlConnection per
// instance.  Hence, BSLs that use this DAL should always be instanced properly.
// The DAL will work with both ADO.NET and System.Transactions or ServiceComponents/Enterprise
// Services attributed transactions [autocomplete]. When using ADO.NET transactions,
// The BSL will control the transaction boundaries with calls to dal.BeginTransaction(); 
// dal.CommitTransaction(); dal.RollbackTransaction().
//===============================================================================================

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ConfigService.DALSQLHelper;
using Trade.BusinessServiceDataContract;
using Trade.IDAL;
using Trade.Utility;

namespace Trade.DALSQLServer
{
    public class MarketSummary :IMarketSummary
    {
        public MarketSummary()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public MarketSummary(SqlConnection conn, SqlTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        private SqlConnection _internalConnection;

        //Used only when doing ADO.NET transactions.
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
           _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        //Used only when doing ADO.NET transactions.
        public void RollBackTransaction()
        {
            if (_internalADOTransaction != null)
                _internalADOTransaction.Rollback();
            _internalADOTransaction = null;
        }

        //Used only when doing ADO.NET transactions.
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

        private const string SQL_SELECT_MARKETSUMMARY_GAINERS = "SET NOCOUNT ON; SELECT symbol, companyname, volume, price, open1, low, high, change1 from dbo.quote with (NOLOCK) where symbol like 's:1__' order by change1 desc";
        private const string SQL_SELECT_MARKETSUMMARY_LOSERS = "SET NOCOUNT ON; SELECT symbol, companyname, volume, price, open1, low, high, change1 from dbo.quote with (NOLOCK) where symbol like 's:1__' order by change1";
        private const string SQL_SELECT_MARKETSUMMARY_TSIA = "SET NOCOUNT ON; select SUM(price)/count(*) as TSIA from dbo.quote with (NOLOCK) where symbol like 's:1__'";
        private const string SQL_SELECT_MARKETSUMMARY_OPENTSIA = "SET NOCOUNT ON; select SUM(open1)/count(*) as openTSIA from dbo.quote with (NOLOCK) where symbol like 's:1__'";
        private const string SQL_SELECT_MARKETSUMMARY_VOLUME = "SET NOCOUNT ON; SELECT SUM(volume) from dbo.quote with (NOLOCK) where symbol like 's:1__'";
        private const string SQL_SELECT_QUOTE = "SET NOCOUNT ON; SELECT symbol, companyname, volume, price, open1, low, high, change1 from dbo.quote with (ROWLOCK) where symbol = @QuoteSymbol";
        private const string SQL_SELECT_QUOTE_NOLOCK = "SET NOCOUNT ON; SELECT symbol, companyname, volume, price, open1, low, high, change1 from dbo.quote with (NOLOCK) where symbol = @QuoteSymbol";
        private const string SQL_UPDATE_STOCKPRICEVOLUME = "SET NOCOUNT ON; UPDATE dbo.QUOTE WITH (ROWLOCK) SET PRICE=@Price, Low=@Low, High=@High, Change1 = @Price - open1, VOLUME=VOLUME+@Quantity WHERE SYMBOL=@QuoteSymbol";

        //Parameters
        private const string PARM_SYMBOL = "@QuoteSymbol";
        private const string PARM_PRICE = "@Price";
        private const string PARM_LOW = "@Low";
        private const string PARM_HIGH = "@High";
        private const string PARM_QUANTITY = "@Quantity";

        public void updateStockPriceVolume(double Quantity, QuoteDataModel quote)
        {
            try
            {
                SqlParameter[] updatestockpriceparm = GetUpdateStockPriceVolumeParameters();
                decimal priceChangeFactor = StockTraderUtility.getRandomPriceChangeFactor(quote.price);
                decimal newprice = quote.price * priceChangeFactor;
                if (newprice < quote.low)
                    quote.low = newprice;
                if (newprice > quote.high)
                    quote.high = newprice;
                updatestockpriceparm[0].Value = (decimal)newprice;
                updatestockpriceparm[1].Value = (float)Quantity;
                updatestockpriceparm[2].Value = quote.symbol;
                updatestockpriceparm[3].Value = quote.low;
                updatestockpriceparm[4].Value = quote.high;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_STOCKPRICEVOLUME, updatestockpriceparm);
                return;
            }
            catch 
            {
                throw;
            }
        }

        public QuoteDataModel getQuote(string symbol)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter(PARM_SYMBOL, SqlDbType.VarChar, 10);
                parm1.Value = symbol;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction,CommandType.Text, SQL_SELECT_QUOTE_NOLOCK,parm1);
                QuoteDataModel quote = null;
                if (rdr.HasRows)
                {
                    rdr.Read();
                    quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), rdr.GetDouble(2), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), rdr.GetDouble(7));
                }
                rdr.Close();
                return quote;
            }
            catch 
            {
                throw;
            }
        }

        public QuoteDataModel getQuoteForUpdate(string symbol)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter(PARM_SYMBOL, SqlDbType.VarChar, 10);
                parm1.Value = symbol;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_QUOTE, parm1);
                QuoteDataModel quote = null;
                if (rdr.HasRows)
                {
                    rdr.Read();
                    quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), rdr.GetDouble(2), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), rdr.GetDouble(7));
                }
                rdr.Close();
                return quote;
            }
            catch
            {
                throw;
            }
        }

        public MarketSummaryDataModelWS getMarketSummaryData()
        {
            try
            {
                decimal TSIA = (decimal)SQLHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_SELECT_MARKETSUMMARY_TSIA);
                decimal openTSIA = (decimal)SQLHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_OPENTSIA);
                double totalVolume = (double)SQLHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_VOLUME);
                SqlDataReader rdr = SQLHelper.ExecuteReaderNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_GAINERS);
                List<QuoteDataModel> topgainers = new List<QuoteDataModel>();
                List<QuoteDataModel> toplosers = new List<QuoteDataModel>();
                int i = 0;
                while (rdr.Read() && i++<5)
                {
                    QuoteDataModel quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), rdr.GetDouble(2), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), rdr.GetDouble(7));
                    topgainers.Add(quote);
                }
                rdr.Close();
                rdr = SQLHelper.ExecuteReaderNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_LOSERS);
                i = 0;
                while (rdr.Read() && i++ < 5)
                {
                    QuoteDataModel quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), rdr.GetDouble(2), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), rdr.GetDouble(7));
                    toplosers.Add(quote);
                }
                rdr.Close();
                MarketSummaryDataModelWS marketSummaryData = new MarketSummaryDataModelWS(TSIA, openTSIA, totalVolume, topgainers, toplosers);
                return marketSummaryData;
            }
            catch
            {
                throw;
            }
        }

        private static SqlParameter[] GetUpdateStockPriceVolumeParameters()
        {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_UPDATE_STOCKPRICEVOLUME);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
            parms = new SqlParameter[] {
                        new SqlParameter(PARM_PRICE, SqlDbType.Decimal, 14),
                        new SqlParameter(PARM_QUANTITY, SqlDbType.Float),
                        new SqlParameter(PARM_SYMBOL, SqlDbType.VarChar, StockTraderUtility.QUOTESYMBOL_MAX_LENGTH),
                        new SqlParameter(PARM_LOW, SqlDbType.Decimal, 14),
                        new SqlParameter(PARM_HIGH, SqlDbType.Decimal, 14)};
            // Add the parametes to the cached
            SQLHelper.CacheParameters(SQL_UPDATE_STOCKPRICEVOLUME, parms);
            }
            return parms;
        }
    }
}
