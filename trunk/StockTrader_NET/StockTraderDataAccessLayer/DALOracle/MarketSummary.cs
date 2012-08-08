//  .Net StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications

//===============================================================================================
// MarketSummary is part of the Oracle DAL for StockTrader.  This is called from the
// BSL to execute commands against the database.  It is constructed to use one OracleConnection per
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
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Trade.BusinessServiceDataContract;
using Trade.IDAL;
using System.Runtime.Serialization;
using Trade.Utility;

namespace Trade.DALOracle
{
    public class MarketSummary :IMarketSummary
    {
        public MarketSummary()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public MarketSummary(OracleConnection conn, OracleTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        //_internalConnection: Used by a DAL instance such that a DAL instance,
        //associated with a BSL instance, will work off a single connection between BSL calls.
        private OracleConnection _internalConnection;

        //Used only when doing ADO.NET transactions.
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
           _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        //Used only when doing ADO.NET transactions.
        public void RollBackTransaction()
        {
            _internalADOTransaction.Rollback();
            _internalADOTransaction = null;
        }

        //Used only when doing ADO.NET transactions.
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

        private const string SQL_SELECT_MARKETSUMMARY_GAINERS = "SELECT symbol, companyname, volume, price, open1, low, high, change1 from quoteejb where symbol like 's:1__' order by change1 desc";
        private const string SQL_SELECT_MARKETSUMMARY_LOSERS = "SELECT symbol, companyname, volume, price, open1, low, high, change1 from quoteejb where symbol like 's:1__' order by change1";
        private const string SQL_SELECT_MARKETSUMMARY_TSIA = "select SUM(price)/count(*) as TSIA from quoteejb where symbol like 's:1__'";
        private const string SQL_SELECT_MARKETSUMMARY_OPENTSIA = "select SUM(open1)/count(*) as openTSIA from quoteejb where symbol like 's:1__'";
        private const string SQL_SELECT_MARKETSUMMARY_VOLUME = "SELECT SUM(volume) from quoteejb where symbol like 's:1__'";
        private const string SQL_SELECT_QUOTE = "SELECT symbol, companyname, volume, price, open1, low, high, change1 from quoteejb with where symbol = :QuoteSymbol";
        private const string SQL_SELECT_QUOTE2 = "SELECT symbol, companyname, volume, price, open1, low, high, change1 from quoteejb where symbol = :QuoteSymbol";
        private const string SQL_UPDATE_STOCKPRICEVOLUME = "UPDATE QUOTEEJB SET PRICE=:Price, Low=:Low, High=:High, Change1=:Change1, VOLUME=:Volume WHERE SYMBOL=:QuoteSymbol";

        //Parameters
        private const string PARM_SYMBOL = ":QuoteSymbol";
        private const string PARM_PRICE = ":Price";
        private const string PARM_CHANGE = ":Change1";
        private const string PARM_VOLUME = ":Volume";
        private const string PARM_LOW = ":Low";
        private const string PARM_HIGH = ":High";
        private const string PARM_QUANTITY = ":Quantity";

        public void updateStockPriceVolume(double Quantity, QuoteDataModel quote)
        {
            try
            {
                OracleParameter[] updatestockpriceparm = GetUpdateStockPriceVolumeParameters();
                decimal priceChangeFactor = StockTraderUtility.getRandomPriceChangeFactor(quote.price);
                decimal newprice = quote.price * priceChangeFactor;
                if (newprice < quote.low)
                    quote.low = newprice;
                if (newprice > quote.high)
                    quote.high = newprice;
                updatestockpriceparm[0].Value = (decimal)newprice;
                updatestockpriceparm[1].Value = quote.low;
                updatestockpriceparm[2].Value = quote.high;
                updatestockpriceparm[3].Value = newprice - quote.open;
                updatestockpriceparm[4].Value = (decimal)(Quantity + quote.volume);
                updatestockpriceparm[5].Value = quote.symbol;
                
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_STOCKPRICEVOLUME, updatestockpriceparm);
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
                OracleParameter parm1 = new OracleParameter(PARM_SYMBOL, OracleDbType.Varchar2, 10);
                parm1.Value = symbol;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction,CommandType.Text, SQL_SELECT_QUOTE2,parm1);
                QuoteDataModel quote = null;
                if (rdr.HasRows)
                {
                    rdr.Read();
                    quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), Convert.ToDouble(rdr.GetDecimal(2)), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), Convert.ToDouble(rdr.GetDecimal(7)));
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
                OracleParameter parm1 = new OracleParameter(PARM_SYMBOL, OracleDbType.Varchar2, 10);
                parm1.Value = symbol;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_QUOTE2, parm1);
                QuoteDataModel quote = null;
                if (rdr.HasRows)
                {
                    rdr.Read();
                    quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), Convert.ToDouble(rdr.GetDecimal(2)), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), Convert.ToDouble(rdr.GetDecimal(7)));
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
                decimal TSIA = Convert.ToDecimal(OracleHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_SELECT_MARKETSUMMARY_TSIA));
                decimal openTSIA = Convert.ToDecimal(OracleHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_OPENTSIA));
                double totalVolume = Convert.ToDouble(OracleHelper.ExecuteScalarNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_VOLUME));
                OracleDataReader rdr = OracleHelper.ExecuteReaderNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_GAINERS);
                List<QuoteDataModel> topgainers = new List<QuoteDataModel>();
                List<QuoteDataModel> toplosers = new List<QuoteDataModel>();
                int i = 0;
                while (rdr.Read() && i++<5)
                {
                    QuoteDataModel quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), Convert.ToDouble(rdr.GetDecimal(2)), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), Convert.ToDouble(rdr.GetDecimal(7)));
                    topgainers.Add(quote);
                }
                rdr.Close();
                rdr = OracleHelper.ExecuteReaderNoParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_MARKETSUMMARY_LOSERS);
                i = 0;
                while (rdr.Read() && i++ < 5)
                {
                    QuoteDataModel quote = new QuoteDataModel(rdr.GetString(0), rdr.GetString(1), Convert.ToDouble(rdr.GetDecimal(2)), rdr.GetDecimal(3), rdr.GetDecimal(4), rdr.GetDecimal(5), rdr.GetDecimal(6), Convert.ToDouble(rdr.GetDecimal(7)));
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

        private static OracleParameter[] GetUpdateStockPriceVolumeParameters()
        {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_UPDATE_STOCKPRICEVOLUME);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
            parms = new OracleParameter[] {
                        new OracleParameter(PARM_PRICE, OracleDbType.Decimal, 14),
                        new OracleParameter(PARM_LOW, OracleDbType.Decimal, 14),
                        new OracleParameter(PARM_HIGH, OracleDbType.Decimal, 14),
                        new OracleParameter(PARM_CHANGE, OracleDbType.Decimal, 14),
                        new OracleParameter(PARM_VOLUME, OracleDbType.Decimal,14),
                        new OracleParameter(PARM_SYMBOL, OracleDbType.Varchar2, 10),
                        
                        };
            // Add the parametes to the cached
            OracleHelper.CacheParameters(SQL_UPDATE_STOCKPRICEVOLUME, parms);
            }
            return parms;
        }
    }
}
