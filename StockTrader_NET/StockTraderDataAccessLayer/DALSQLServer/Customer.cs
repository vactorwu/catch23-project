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
// Customer is part of the SQLServer DAL for StockTrader.  This is called from the
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
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Security.Cryptography;
using ConfigService.DALSQLHelper;
using Trade.BusinessServiceDataContract;
using Trade.IDAL;
using Trade.Utility;




namespace Trade.DALSQLServer
{
    public class Customer :ICustomer
    {
        public Customer()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public Customer (SqlConnection conn, SqlTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        private SqlConnection _internalConnection;
        private SqlTransaction _internalADOTransaction=null;
        
        //Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In SQLHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        public void BeginADOTransaction()
        {
            if (_internalConnection.State != ConnectionState.Open)
            {
                _internalConnection.Open();
            }
            getSQLContextInfo();
           _internalADOTransaction = _internalConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        //Used only when doing ADO.NET transactions.
        public void RollBackTransaction()
        {
            if (_internalADOTransaction!=null)
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
            if (_internalConnection!= null && _internalConnection.State!=ConnectionState.Closed)
                _internalConnection.Close();
        }

        
        private const string SQL_SELECT_HOLDINGS = @"SET NOCOUNT ON; SELECT TOP 250 HOLDING.HOLDINGID, HOLDING.QUANTITY, HOLDING.PURCHASEPRICE, HOLDING.PURCHASEDATE, HOLDING.QUOTE_SYMBOL,HOLDING.ACCOUNT_ACCOUNTID, Quote.Price 
                                                    from dbo.holding Inner Join Quote ON Holding.Quote_Symbol=Quote.Symbol WHERE HOLDING.ACCOUNT_ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNT WHERE PROFILE_USERID = @UserId) ORDER BY HOLDING.HOLDINGID DESC";
        private const string SQL_SELECT_HOLDING_LOCK = @"SET NOCOUNT ON; SELECT dbo.HOLDING.HOLDINGID, HOLDING.ACCOUNT_ACCOUNTID, HOLDING.QUANTITY, HOLDING.PURCHASEPRICE, HOLDING.PURCHASEDATE, HOLDING.QUOTE_SYMBOL, Quote.Price FROM dbo.HOLDING WITH (ROWLOCK) Inner Join Quote WITH (ROWLOCK) ON Holding.Quote_Symbol=Quote.Symbol INNER JOIN ORDERS WITH (ROWLOCK) ON HOLDING.HOLDINGID = ORDERS.HOLDING_HOLDINGID
                                                       WHERE (ORDERS.ORDERID = @OrderId)";
        private const string SQL_SELECT_HOLDING_NOLOCK = "SET NOCOUNT ON; SELECT HOLDING.ACCOUNT_ACCOUNTID, HOLDING.QUANTITY, HOLDING.PURCHASEPRICE, HOLDING.PURCHASEDATE, HOLDING.QUOTE_SYMBOL, Quote.Price FROM HOLDING WITH(NOLOCK) Inner Join Quote On Holding.Quote_Symbol=Quote.Symbol WHERE HOLDING.HOLDINGID=@holdingId AND HOLDING.ACCOUNT_ACCOUNTID = (SELECT ACCOUNTID FROM dbo.ACCOUNT WITH (NOLOCK) WHERE PROFILE_USERID = @UserId)";
        private const string SQL_SELECT_GET_CUSTOMER_BYUSERID = "SET NOCOUNT ON; SELECT account.ACCOUNTID, account.PROFILE_USERID, account.CREATIONDATE, account.OPENBALANCE, account.LOGOUTCOUNT, account.BALANCE, account.LASTLOGIN, account.LOGINCOUNT FROM account WHERE account.PROFILE_USERID = @UserId";
        private const string SQL_SELECT_CUSTOMERPROFILE_BYUSERID = "SET NOCOUNT ON; SELECT accountprofile.USERID,accountprofile.SALT, accountprofile.PASSWORD, accountprofile.FULLNAME, accountprofile.ADDRESS, accountprofile.EMAIL, accountprofile.CREDITCARD FROM dbo.accountprofile WITH (NOLOCK) WHERE accountprofile.USERID = @UserId";
        private const string SQL_SELECT_UPDATE_CUSTOMER_LOGIN = "SET NOCOUNT ON; UPDATE dbo.account WITH (ROWLOCK) SET LOGINCOUNT = (LOGINCOUNT + 1), LASTLOGIN = CURRENT_TIMESTAMP where PROFILE_USERID= @UserId; SELECT account.ACCOUNTID, account.CREATIONDATE, account.OPENBALANCE, account.LOGOUTCOUNT, account.BALANCE, account.LASTLOGIN, account.LOGINCOUNT FROM dbo.account WITH (ROWLOCK) WHERE account.PROFILE_USERID = @UserId";
        private const string SQL_UPDATE_LOGOUT = "SET NOCOUNT ON; UPDATE dbo.account WITH (ROWLOCK) SET LOGOUTCOUNT = (LOGOUTCOUNT + 1) where PROFILE_USERID= @UserId";
        private const string SQL_UPDATE_ACCOUNTPROFILE = "SET NOCOUNT ON; UPDATE dbo.accountprofile WITH (ROWLOCK) SET ADDRESS=@Address, SALT=@Salt, PASSWORD=@Password, EMAIL=@Email, CREDITCARD = @CreditCard, FULLNAME=@FullName WHERE USERID= @UserId";
        private const string SQL_SELECT_CLOSED_ORDERS = "SET NOCOUNT ON; SELECT ORDERID, ORDERTYPE, ORDERSTATUS, COMPLETIONDATE, OPENDATE, QUANTITY, PRICE, ORDERFEE, QUOTE_SYMBOL FROM dbo.orders WHERE ACCOUNT_ACCOUNTID = (select accountid from dbo.account WITH(NOLOCK) where profile_userid =@UserId) AND ORDERSTATUS = 'closed'";
        private const string SQL_UPDATE_CLOSED_ORDERS = "SET NOCOUNT ON; UPDATE dbo.orders WITH (ROWLOCK) SET ORDERSTATUS = 'completed' where ORDERSTATUS = 'closed' AND ACCOUNT_ACCOUNTID = (select accountid from dbo.account WITH (NOLOCK) where profile_userid =@UserId)";
        private const string SQL_SELECT_ORDERS_BY_ID = " o.ORDERID, o.ORDERTYPE, o.ORDERSTATUS, o.OPENDATE, o.COMPLETIONDATE, o.QUANTITY, o.PRICE, o.ORDERFEE, o.QUOTE_SYMBOL from dbo.orders o where o.account_accountid = (select a.accountid from dbo.account a WITH (NOLOCK)  where a.profile_userid = @UserId) ORDER BY o.ORDERID DESC";
        private const string SQL_INSERT_ACCOUNTPROFILE = "SET NOCOUNT ON; INSERT INTO dbo.accountprofile VALUES (@Address, @Password, @UserId, @Email, @CreditCard, @FullName, @Salt)";
        private const string SQL_INSERT_ACCOUNT = "SET NOCOUNT ON; INSERT INTO dbo.account (CREATIONDATE, OPENBALANCE, LOGOUTCOUNT, BALANCE, LASTLOGIN, LOGINCOUNT, PROFILE_USERID) VALUES (GetDate(), @OpenBalance, @LogoutCount, @Balance, @LastLogin, @LoginCount, @UserId); SELECT ID=@@IDENTITY";
        private const string SQL_DEBIT_ACCOUNT = "SET NOCOUNT ON; UPDATE dbo.ACCOUNT WITH (ROWLOCK) SET BALANCE=(BALANCE-@Debit) WHERE ACCOUNTID=@AccountId";

        //Parameters
        private const string PARM_USERID = "@UserId";
        private const string PARM_HOLDINGID = "@holdingId";
        private const string PARM_ORDERID = "@OrderId";
        private const string PARM_ACCOUNTID = "@accountId";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_FULLNAME = "@FullName";
        private const string PARM_ADDRESS = "@Address";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_CREDITCARD = "@CreditCard";
        private const string PARM_OPENBALANCE = "@OpenBalance";
        private const string PARM_LOGOUTCOUNT = "@LogoutCount";
        private const string PARM_BALANCE = "@Balance";
        private const string PARM_LASTLOGIN = "@LastLogin";
        private const string PARM_LOGINCOUNT = "@LoginCount";
        private const string PARM_TOPORDERS = "@TopOrders";
        private const string PARM_DEBIT = "@Debit";
        private const string PARM_SALT = "@Salt";


        public void getSQLContextInfo()
        {
            string sql = "SELECT 0";
            SQLHelper.ExecuteScalarNoParm(_internalConnection, _internalADOTransaction, CommandType.Text, sql);
            return;
        }

        public List<OrderDataModel> getOrders(string userID, bool top, int maxTop, int maxDefault)
        {
            
            //Here you can configure between two settings: top default to display
            //is MAX_DISPLAY_ORDERS; the upper limit is MAX_DISPLAY_TOP_ORDERS.
            //Set these in Web.Config/Trade.BusinessServiceHost.exe.config; and those will be the toggle 
            //choices for the user in the Account.aspx page.
            try
            {
                string commandText;
                if (top)
                {
                    commandText = "Select Top " + maxTop.ToString() + SQL_SELECT_ORDERS_BY_ID;
                }
                else
                {
                    commandText = "Select Top " + maxDefault.ToString() + SQL_SELECT_ORDERS_BY_ID;
                }
                SqlParameter accountidparm = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                accountidparm.Value = userID;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, commandText,accountidparm);
                List<OrderDataModel> orders = new List<OrderDataModel>();
                while (rdr.Read())
                {
                    int orderid = rdr.GetInt32(0);
                    DateTime openDate = (DateTime)rdr.GetDateTime(3);
                    Object completionDate = null;
                    //can be null
                    try
                    {
                        if (!Convert.IsDBNull(rdr.GetDateTime(4)))
                            completionDate = rdr.GetDateTime(4);
                        else
                            completionDate = DateTime.MinValue;
                    }
                    catch (Exception e) 
                    {
                        string message = e.Message;
                        completionDate = DateTime.MinValue; 
                    }
                    OrderDataModel order = new OrderDataModel(orderid, rdr.GetString(1), rdr.GetString(2), openDate, (DateTime)completionDate, rdr.GetDouble(5), rdr.GetDecimal(6), rdr.GetDecimal(7), rdr.GetString(8));
                    orders.Add(order);
                }
                rdr.Close();
                return orders;
            }
            catch 
            {
                throw;
            }
        }

        public AccountDataModel login(string userid, string password, bool useSaltedHash)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                parm1.Value = userid;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text,SQL_SELECT_CUSTOMERPROFILE_BYUSERID, parm1);
                if (rdr.Read())
                {
                    string salt = rdr.GetString(1);
                    string userPassword = rdr.GetString(2);
                    rdr.Close();
                    bool valid = false;
                    if (useSaltedHash)
                    {
                        SaltedHash ver = SaltedHash.Create(salt, userPassword);
                        valid = ver.Verify(password);
                    }
                    else
                    {
                        if (password.Equals(userPassword))
                            valid=true;
                    }
                    if (valid)
                    {
                        SqlParameter profileparm1 = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                        profileparm1.Value = userid;
                        rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_UPDATE_CUSTOMER_LOGIN, profileparm1);
                        rdr.Read();
                        AccountDataModel customer = new AccountDataModel(rdr.GetInt32(0), userid, rdr.GetDateTime(1), rdr.GetDecimal(2), rdr.GetInt32(3), rdr.GetDecimal(4), rdr.GetDateTime(5), rdr.GetInt32(6) + 1);
                        rdr.Close();
                        return customer;
                    }
                    rdr.Close();
                }
                return null;
            }
            catch 
            {
                throw;
            }
        }

        public AccountProfileDataModel getAccountProfileData(string userid)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                parm1.Value = userid;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_CUSTOMERPROFILE_BYUSERID,parm1);
                if (rdr.Read())
                {
                    AccountProfileDataModel customerprofile = new AccountProfileDataModel(rdr.GetString(0), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5), rdr.GetString(6));
                    rdr.Close();
                    return customerprofile;
                }
                rdr.Close();
                return null;
            }
            catch
            {
                throw;
            }
        }

        public void logOutUser(string userID)
        {
             try
             {
                    SqlParameter parm1 = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                    parm1.Value = userID;
                    SQLHelper.ExecuteNonQuery(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_UPDATE_LOGOUT, parm1);
                    return;
             }
             catch
             {
                 throw;
             }
        }

        public AccountDataModel getCustomerByUserID(string userID)
        {
            try
            {
                SqlParameter parm1 = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                parm1.Value = userID;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_GET_CUSTOMER_BYUSERID, parm1);
                if (rdr.Read())
                {
                    AccountDataModel customer = new AccountDataModel(rdr.GetInt32(0), rdr.GetString(1), rdr.GetDateTime(2), rdr.GetDecimal(3), rdr.GetInt32(4), rdr.GetDecimal(5), rdr.GetDateTime(6), rdr.GetInt32(7));
                    rdr.Close();
                    return customer;
                }
                rdr.Close();
                return null;
            }
            catch 
            {
                throw;
            }
        }

        public List<OrderDataModel> getClosedOrders(string userId)
        {
            try
            {
                SqlParameter useridparm = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                useridparm.Value = userId;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_CLOSED_ORDERS,useridparm);
                List<OrderDataModel> closedorders = new List<OrderDataModel>();
                DateTime completionDate = DateTime.MinValue;
                while (rdr.Read())
                {
                    int orderid = rdr.GetInt32(0);
                    DateTime openDate = (DateTime)rdr.GetDateTime(4);
                    try
                    {
                        completionDate = (DateTime)rdr.GetDateTime(3);
                    }
                    catch (Exception e) { if (e.Message.Equals("Data is Null. This method or property cannot be called on Null values.")) completionDate = DateTime.MinValue; }
                    OrderDataModel order = new OrderDataModel(orderid, rdr.GetString(1), rdr.GetString(2), openDate, completionDate, rdr.GetDouble(5), rdr.GetDecimal(6), rdr.GetDecimal(7), rdr.GetString(8));
                    order.orderStatus = StockTraderUtility.ORDER_STATUS_COMPLETED;
                    closedorders.Add(order);
                }
                if (rdr.HasRows)
                {
                    rdr.Close();
                    useridparm = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                    useridparm.Value = userId;
                    SQLHelper.ExecuteNonQuerySingleParm(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_UPDATE_CLOSED_ORDERS,useridparm);
                }
                else
                    rdr.Close();
                return closedorders;
            }
            catch
            {
                throw;
            }
        }

        public List<HoldingDataModel> getHoldings(string userID)
        {
            try
            {
                SqlParameter useridparm = new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20);
                useridparm.Value = userID;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleParm(_internalConnection,_internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDINGS, useridparm);
                List<HoldingDataModel> holdings = new List<HoldingDataModel>();
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(rdr.GetInt32(0), rdr.GetDouble(1), rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4),rdr.GetInt32(5), rdr.GetDecimal(6));
                    holdings.Add(holding);
                }
                rdr.Close();
                return holdings;
            }
            catch 
            {
                throw;
            }
        }
       
       public HoldingDataModel getHoldingForUpdate(int orderID)
       {
            try
            {
                SqlParameter orderIDparm = new SqlParameter(PARM_ORDERID, SqlDbType.Int);
                orderIDparm.Value = orderID;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING_LOCK, orderIDparm);
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetDouble(2), rdr.GetDecimal(3), rdr.GetDateTime(4), rdr.GetString(5), rdr.GetDecimal(6));
                    rdr.Close();
                    return holding;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public HoldingDataModel getHolding(string userid, int holdingid)
        {
            try
            {
                SqlParameter[] holdingidparms = new SqlParameter[]{new SqlParameter(PARM_HOLDINGID, SqlDbType.Int), 
                                                new SqlParameter(PARM_USERID, SqlDbType.VarChar, 20)};
                holdingidparms[0].Value = holdingid;
                holdingidparms[1].Value = userid;
                SqlDataReader rdr = SQLHelper.ExecuteReaderSingleRow(_internalConnection,_internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING_NOLOCK, holdingidparms);
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(holdingid, rdr.GetInt32(0), rdr.GetDouble(1), rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4), rdr.GetDecimal(5));
                    rdr.Close();
                    return holding;
                }
                return null;
            }
            catch 
            {
                throw;
            }
        }

        public void updateAccountBalance(int accountID, decimal total)
        {
            try
            {
                // Get the parameters from the cache
                SqlParameter[] accountParms = GetUpdateAccountBalanceParameters();
                accountParms[0].Value = total;
                accountParms[1].Value = accountID;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_DEBIT_ACCOUNT, accountParms);
            }
            catch 
            {
                throw; 
            }
            return;
        }

        public void insertAccount(AccountDataModel customer)
        {
            // Get the parameters from the cache
            SqlParameter[] AccountParms = GetCreateAccountParameters();
            try
            {
                AccountParms[0].Value = customer.openBalance;
                AccountParms[1].Value = customer.logoutCount;
                AccountParms[2].Value = customer.balance;
                AccountParms[3].Value = customer.lastLogin;
                AccountParms[4].Value = customer.loginCount;
                AccountParms[5].Value = customer.profileID;
                customer.accountID = Convert.ToInt32(SQLHelper.ExecuteScalar(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_ACCOUNT, AccountParms));
                return;
            }
            catch 
            {
                throw;
            }
        }

        public void insertAccountProfile(AccountProfileDataModel customerprofile, bool useSaltedHash)
        {
            // Get the paramters from the cache
            SqlParameter[] ProfileParms = GetCreateAccountProfileParameters();
            string salt = " ";
            try
            {
                if (useSaltedHash)
                {
                    SaltedHash sh = SaltedHash.Create(customerprofile.password);
                    salt = sh.Salt;
                    string hash = sh.Hash;
                    customerprofile.password = hash;
                }
                ProfileParms[0].Value = customerprofile.address;
                ProfileParms[1].Value = salt;
                ProfileParms[2].Value = customerprofile.password;
                ProfileParms[3].Value = customerprofile.userID;
                ProfileParms[4].Value = customerprofile.email;
                ProfileParms[5].Value = customerprofile.creditCard;
                ProfileParms[6].Value = customerprofile.fullName;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction,CommandType.Text, SQL_INSERT_ACCOUNTPROFILE, ProfileParms);
            }
            catch 
            {
                throw;
            }
        }

        public AccountProfileDataModel update(AccountProfileDataModel customerprofile, bool useSaltedHash)
        {
            try
            {
                
                // Get the paramters from the cache
                SqlParameter[] ProfileParms = GetUpdateAccountProfileParameters();
                string salt = " ";
                if (useSaltedHash)
                {
                    SaltedHash sh = SaltedHash.Create(customerprofile.password);
                    salt = sh.Salt;
                    string hash = sh.Hash;
                    customerprofile.password = hash;
                }
                ProfileParms[0].Value = customerprofile.address;
                ProfileParms[1].Value = salt;
                ProfileParms[2].Value = customerprofile.password;
                ProfileParms[3].Value = customerprofile.email;
                ProfileParms[4].Value = customerprofile.creditCard;
                ProfileParms[5].Value = customerprofile.fullName;
                ProfileParms[6].Value = customerprofile.userID;
                SQLHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_ACCOUNTPROFILE, ProfileParms);
                return customerprofile;
            }
            catch 
            {
                throw;
            }
        }

        private static SqlParameter[] GetUpdateAccountBalanceParameters()
        {
            // Get the paramters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_DEBIT_ACCOUNT);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {new SqlParameter(PARM_DEBIT, SqlDbType.Decimal, 14),
                                            new SqlParameter(PARM_ACCOUNTID, SqlDbType.Int)};
                // Add the parameters to the cached
                SQLHelper.CacheParameters(SQL_DEBIT_ACCOUNT, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetCreateAccountProfileParameters()
        {
            // Get the parameters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_INSERT_ACCOUNTPROFILE);          
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
        			  new SqlParameter(PARM_ADDRESS, SqlDbType.VarChar, StockTraderUtility.ADDRESS_MAX_LENGTH),
                      new SqlParameter(PARM_SALT, SqlDbType.VarChar, StockTraderUtility.SALT_MAXLENGTH),
		              new SqlParameter(PARM_PASSWORD, SqlDbType.VarChar, StockTraderUtility.PASSWORD_MAX_LENGTH),
                      new SqlParameter(PARM_USERID, SqlDbType.VarChar, StockTraderUtility.USERID_MAX_LENGTH),
                      new SqlParameter(PARM_EMAIL, SqlDbType.VarChar, StockTraderUtility.EMAIL_MAX_LENGTH),
                      new SqlParameter(PARM_CREDITCARD, SqlDbType.VarChar, StockTraderUtility.CREDITCARD_MAX_LENGTH),
                      new SqlParameter(PARM_FULLNAME, SqlDbType.VarChar, StockTraderUtility.FULLNAME_MAX_LENGTH)};

                // Add the parametes to the cached
                SQLHelper.CacheParameters(SQL_INSERT_ACCOUNTPROFILE, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetUpdateAccountProfileParameters()
        {
            // Get the parameters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_UPDATE_ACCOUNTPROFILE);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
            		  new SqlParameter(PARM_ADDRESS, SqlDbType.VarChar, StockTraderUtility.ADDRESS_MAX_LENGTH),
                      new SqlParameter(PARM_SALT, SqlDbType.VarChar, StockTraderUtility.PASSWORD_MAX_LENGTH),
					  new SqlParameter(PARM_PASSWORD, SqlDbType.VarChar, StockTraderUtility.PASSWORD_MAX_LENGTH),
                      new SqlParameter(PARM_EMAIL, SqlDbType.VarChar, StockTraderUtility.EMAIL_MAX_LENGTH),
                      new SqlParameter(PARM_CREDITCARD, SqlDbType.VarChar, StockTraderUtility.CREDITCARD_MAX_LENGTH),
                      new SqlParameter(PARM_FULLNAME, SqlDbType.VarChar, StockTraderUtility.FULLNAME_MAX_LENGTH),
                      new SqlParameter(PARM_USERID, SqlDbType.VarChar, StockTraderUtility.USERID_MAX_LENGTH)};
                
                // Add the parametes to the cached
                SQLHelper.CacheParameters(SQL_UPDATE_ACCOUNTPROFILE, parms);
            }
            return parms;
        }

        private static SqlParameter[] GetCreateAccountParameters()
        {
            // Get the parameters from the cache
            SqlParameter[] parms = SQLHelper.GetCacheParameters(SQL_INSERT_ACCOUNT);          
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new SqlParameter[] {
					  new SqlParameter(PARM_OPENBALANCE, SqlDbType.Decimal),
                      new SqlParameter(PARM_LOGOUTCOUNT, SqlDbType.Int),
                      new SqlParameter(PARM_BALANCE, SqlDbType.Decimal),
                      new SqlParameter(PARM_LASTLOGIN, SqlDbType.DateTime),
                      new SqlParameter(PARM_LOGINCOUNT, SqlDbType.Int),
                      new SqlParameter(PARM_USERID, SqlDbType.VarChar, StockTraderUtility.USERID_MAX_LENGTH)};
                
                // Add the parameters to the cached
                SQLHelper.CacheParameters(SQL_INSERT_ACCOUNT, parms);
            }
            return parms;
        }
    }

    public sealed class SaltedHash
    {
        public string Salt { get { return _salt; } }
        public string Hash { get { return _hash; } }

        public static SaltedHash Create(string password)
        {
            string salt = _createSalt();
            string hash = _calculateHash(salt, password);
            return new SaltedHash(salt, hash);
        }

        public static SaltedHash Create(string salt, string hash)
        {
            return new SaltedHash(salt, hash);
        }

        public bool Verify(string password)
        {
            string h = _calculateHash(_salt, password);
            return _hash.Equals(h);
        }

        private SaltedHash(string s, string h)
        {
            _salt = s;
            _hash = h;
        }

        private static string _createSalt()
        {
            byte[] r = _createRandomBytes(saltLength);
            return Convert.ToBase64String(r);
        }

        private static byte[] _createRandomBytes(int len)
        {
            byte[] r = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(r);
            return r;
        }

        private static string _calculateHash(string salt, string password)
        {
            byte[] data = _toByteArray(salt + password);
            byte[] hash = _calculateHash(data);
            return Convert.ToBase64String(hash);
        }

        private static byte[] _calculateHash(byte[] data)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(data);
        }

        private static byte[] _toByteArray(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        private readonly string _salt;
        private readonly string _hash;
        private const int saltLength = 12;
    }
}
