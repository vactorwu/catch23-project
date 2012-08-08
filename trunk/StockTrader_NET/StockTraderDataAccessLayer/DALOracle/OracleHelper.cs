//  .Net StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications

using System;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Collections;

namespace Trade.DALOracle
{

    /// <summary>
    /// The OracleHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of Oracle Corporation's ODP for 
    /// .NET OracleClient.
    /// </summary>
    public abstract class OracleHelper
    {

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());


        /// <summary>
        /// Create and execute a command to return DataReader after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">The single OracleParameter object to bind to the query.</param>
        public static OracleDataReader ExecuteReaderSingleParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter singleParm)
        {
            OracleCommand cmd = new OracleCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a single-row DataReader after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">The single OracleParameter object to bind to the query.</param>
        public static OracleDataReader ExecuteReaderSingleRowSingleParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter singleParm)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParm);
            OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a single-row DataReader after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of OracleParameter objects to bind to the query.</param>
        public static OracleDataReader ExecuteReaderSingleRow(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader, no parameters used in the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        public static OracleDataReader ExecuteReaderNoParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a DataReader after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of OracleParameter objects to bind to the query.</param>
        public static OracleDataReader ExecuteReader(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleResult);
            return rdr;
        }

        /// <summary>
        /// Create and execute a command to return a single scalar (int) value after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of OracleParameter objects to bind to the query.</param>
        public static int ExecuteScalar(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            PrepareCommand(cmd, cmdParms);
            int val = Convert.ToInt32(cmd.ExecuteScalar());
            return val;
        }


        /// <summary>
        /// Create and execute a command to return a single scalar (int) value after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">A OracleParameter object to bind to the query.</param>
        public static int ExecuteScalarSingleParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter singleParm)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            cmd.Parameters.Add(singleParm);
            int val = Convert.ToInt32(cmd.ExecuteScalar());
            return val;
        }

        /// <summary>
        /// Create and execute a command to return a single scalar (int) value. No parameters will be bound to the command.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParm">A OracleParameter object to bind to the query.</param>
        public static object ExecuteScalarNoParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = cmdText;
            cmd.Connection = conn;
            object val = cmd.ExecuteScalar();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to multiple parameters.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="cmdParms">An array of OracleParameter objects to bind to the query.</param>
        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            PrepareCommand(cmd, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParam">A OracleParameter object to bind to the query.</param>
        public static int ExecuteNonQuerySingleParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter singleParam)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(singleParam);
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// Create and execute a command that returns no result set after binding to a single parameter.
        /// </summary>
        /// <param name="conn">Connection to execute against. If not open, it will be here.</param>
        /// <param name="trans">ADO transaction.  If null, will not be attached to the command</param>
        /// <param name="cmdType">Type of ADO command; such as Text or Procedure</param>
        /// <param name="cmdText">The actual SQL or the name of the Stored Procedure depending on command type</param>
        /// <param name="singleParam">A OracleParameter object to bind to the query.</param>
        public static int ExecuteNonQueryNoParm(OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            int val = cmd.ExecuteNonQuery();
            return val;
        }

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static OracleParameter[] GetCacheParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">OracleCommand object</param>
        /// <param name="conn">OracleConnection object</param>
        /// <param name="trans">OracleTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(OracleCommand cmd, OracleParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    OracleParameter parm = (OracleParameter)cmdParms[i];
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}