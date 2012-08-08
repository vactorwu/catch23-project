using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace LYC.DBUtility
{
    public abstract class SQLHelper
    {
        //public static readonly string DbProvider = ConfigurationManager.AppSettings["provider"];
        public static readonly string ConnHIS = ConfigurationManager.ConnectionStrings["HISDbProvider"].ConnectionString;

        public static void DbConnect(SqlConnection conn){
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, string cmdText)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            try
            {
                DbConnect(conn);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader ExecuteReaderWithParm(string connectionString, string cmdText,SqlParameter parm)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(parm);
            try
            {
                DbConnect(conn);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr;
            }
            catch
            {
                throw;
            }
            finally
            {
               
            }
        }

        public static int ExecuteNonQuery(string connectionString, string cmdText, SqlParameter parm)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlTransaction tx = null;
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.Parameters.Add(parm);
            try
            {
                DbConnect(conn);
                tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                int val = cmd.ExecuteNonQuery();
                tx.Commit();
                return val;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx.Rollback();
                return 0;
            }
            finally
            {
                
            }
        }
        public static int ExecuteNonQueryWithParms(string connectionString, string cmdText, SqlParameter[] parms)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (parms != null)
            {
                foreach (SqlParameter parm in parms)
                    cmd.Parameters.Add(parm);
            }
            try
            {
                DbConnect(conn);
                int val = cmd.ExecuteNonQuery();
                return val;
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

    }

}
