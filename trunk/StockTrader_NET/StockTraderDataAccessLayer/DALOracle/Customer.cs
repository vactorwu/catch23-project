//  .Net StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications


//===============================================================================================
// Customer is part of the Oracle DAL for StockTrader.  This is called from the
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
using System.Security.Cryptography;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Trade.BusinessServiceDataContract;
using Trade.IDAL;
using Trade.Utility;



namespace Trade.DALOracle
{
    public class Customer :ICustomer
    {
        public Customer()
        {
        }

        //Constructor for internal DAL-DAL calls to use an existing DB connection.
        public Customer (OracleConnection conn, OracleTransaction trans)
        {
            _internalConnection = conn;
            _internalADOTransaction = trans;
        }

        private OracleConnection _internalConnection;
        private OracleTransaction _internalADOTransaction=null;
        
        //Used only when doing ADO.NET transactions.
        //This will be completely ignored when null, and not attached to a cmd object
        //In OracleHelper unless it has been initialized explicitly in the BSL with a
        //dal.BeginADOTransaction().  See app config setting in web.config and 
        //Trade.BusinessServiceHost.exe.config "Use System.Transactions Globally" which determines
        //whether user wants to run with ADO transactions or System.Transactions.  The DAL itself
        //is built to be completely agnostic and will work with either.
        public void BeginADOTransaction()
        {
            if (_internalConnection.State != ConnectionState.Open)
                _internalConnection.Open();
           _internalADOTransaction = _internalConnection.BeginTransaction(IsolationLevel.ReadCommitted);
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
            if (_internalConnection!= null && _internalConnection.State!=ConnectionState.Closed)
                _internalConnection.Close();
        }

        public void getSQLContextInfo()
        {
            return;
        }

        private const string SQL_SELECT_HOLDINGS = @"SELECT HOLDINGEJB.HOLDINGID, HOLDINGEJB.QUANTITY, HOLDINGEJB.PURCHASEPRICE, HOLDINGEJB.PURCHASEDATE, HOLDINGEJB.QUOTE_SYMBOL,HOLDINGEJB.ACCOUNT_ACCOUNTID, QUOTEEJB.PRICE  
                                                    from holdingejb Inner Join QuoteEJB ON HoldingEJB.Quote_Symbol=QuoteEJB.Symbol WHERE HOLDINGEJB.ACCOUNT_ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNTEJB WHERE PROFILE_USERID = :UserId) ORDER BY HOLDINGEJB.HOLDINGID DESC";
        private const string SQL_SELECT_HOLDING_LOCK = @"SELECT HOLDINGEJB.HOLDINGID, HOLDINGEJB.ACCOUNT_ACCOUNTID, HOLDINGEJB.QUANTITY, HOLDINGEJB.PURCHASEPRICE, HOLDINGEJB.PURCHASEDATE,
                                                       HOLDINGEJB.QUOTE_SYMBOL, QUOTEEJB.PRICE FROM HOLDINGEJB Inner Join QuoteEJB ON HoldingEJB.Quote_Symbol=QuoteEJB.Symbol INNER JOIN ORDEREJB ON HOLDINGEJB.HOLDINGID = ORDEREJB.HOLDING_HOLDINGID WHERE (ORDEREJB.ORDERID = :OrderId)";
        private const string SQL_SELECT_HOLDING2 = "SELECT HOLDINGEJB.ACCOUNT_ACCOUNTID, HOLDINGEJB.QUANTITY, HOLDINGEJB.PURCHASEPRICE, HOLDINGEJB.PURCHASEDATE, HOLDINGEJB.QUOTE_SYMBOL, QUOTEEJB.PRICE FROM HOLDINGEJB Inner Join QuoteEJB ON HoldingEJB.Quote_Symbol=QuoteEJB.Symbol WHERE HOLDINGEJB.HOLDINGID=:holdingId AND HOLDINGEJB.ACCOUNT_ACCOUNTID = (SELECT ACCOUNTID FROM ACCOUNTEJB WHERE PROFILE_USERID = :UserId)";
        private const string SQL_SELECT_GET_CUSTOMER_BYUSERID = "SELECT accountejb.ACCOUNTID, accountejb.PROFILE_USERID, accountejb.CREATIONDATE, accountejb.OPENBALANCE, accountejb.LOGOUTCOUNT, accountejb.BALANCE, accountejb.LASTLOGIN, accountejb.LOGINCOUNT FROM accountejb WHERE accountejb.PROFILE_USERID = :UserId";
        private const string SQL_SELECT_CUSTOMERPROFILE_BYUSERID = "SELECT accountprofileejb.USERID, accountprofileejb.SALT, accountprofileejb.PASSWORD, accountprofileejb.FULLNAME, accountprofileejb.ADDRESS, accountprofileejb.EMAIL, accountprofileejb.CREDITCARD FROM accountprofileejb WHERE accountprofileejb.USERID = :UserId";
        private const string SQL_UPDATE_CUSTOMER_LOGIN = "UPDATE accountejb SET LOGINCOUNT = (LOGINCOUNT + 1), LASTLOGIN = CURRENT_DATE where PROFILE_USERID= :UserId";
        private const string SQL_SELECT_CUSTOMER_LOGIN = "SELECT accountejb.ACCOUNTID, accountejb.CREATIONDATE, accountejb.OPENBALANCE, accountejb.LOGOUTCOUNT, accountejb.BALANCE, accountejb.LASTLOGIN, accountejb.LOGINCOUNT FROM accountejb WHERE accountejb.PROFILE_USERID = :UserId";
        private const string SQL_UPDATE_LOGOUT = "UPDATE accountejb SET LOGOUTCOUNT = (LOGOUTCOUNT + 1) where PROFILE_USERID= :UserId";
        private const string SQL_UPDATE_ACCOUNTPROFILE = "UPDATE accountprofileejb SET ADDRESS=:Address, SALT=:Salt, PASSWORD=:Password, EMAIL=:Email, CREDITCARD = :CreditCard, FULLNAME=:FullName WHERE USERID= :UserId";
        private const string SQL_SELECT_CLOSED_ORDERS = "SELECT ORDERID, ORDERTYPE, ORDERSTATUS, COMPLETIONDATE, OPENDATE, QUANTITY, PRICE, ORDERFEE, QUOTE_SYMBOL FROM orderejb WHERE ACCOUNT_ACCOUNTID = (select accountid from accountejb where profile_userid =:UserId) AND ORDERSTATUS = 'closed'";
        private const string SQL_UPDATE_CLOSED_ORDERS = "UPDATE orderejb SET ORDERSTATUS = 'completed' where ORDERSTATUS = 'closed' AND ACCOUNT_ACCOUNTID = (select accountid from accountejb where profile_userid =:UserId)";
        private const string SQL_SELECT_ORDERS_BY_ID = "SELECT o.ORDERID, o.ORDERTYPE, o.ORDERSTATUS, o.OPENDATE, o.COMPLETIONDATE, o.QUANTITY, o.PRICE, o.ORDERFEE, o.QUOTE_SYMBOL from orderejb o where o.account_accountid = (select a.accountid from accountejb a where a.profile_userid = :UserId) AND ROWNUM <={0} ORDER BY o.ORDERID DESC";
        private const string SQL_INSERT_ACCOUNTPROFILE = "INSERT INTO accountprofileejb (ADDRESS, PASSWORD, USERID, EMAIL, CREDITCARD, FULLNAME, SALT) VALUES (:Address, :Password, :UserId, :Email, :CreditCard, :FullName, :Salt)";
        private const string SQL_INSERT_ACCOUNT = "INSERT INTO accountejb (ACCOUNTID, CREATIONDATE, OPENBALANCE, LOGOUTCOUNT, BALANCE, LASTLOGIN, LOGINCOUNT, PROFILE_USERID) VALUES (:accountId, CURRENT_DATE, :OpenBalance, :LogoutCount, :Balance, :LastLogin, :LoginCount, :UserId)";
        private const string SQL_DEBIT_ACCOUNT = "UPDATE ACCOUNTEJB SET BALANCE=(BALANCE-:Debit) WHERE ACCOUNTID=:AccountId";
        private const string SQL_GET_NEXT_ACCOUNT_SEQ = "SELECT ACCOUNTEJB_SEQ.NEXTVAL FROM DUAL";
        
        //Parameters
        private const string PARM_USERID = ":UserId";
        private const string PARM_HOLDINGID = ":holdingId";
        private const string PARM_ORDERID = ":OrderId";
        private const string PARM_ACCOUNTID = ":accountId";
        private const string PARM_PASSWORD = ":Password";
        private const string PARM_FULLNAME = ":FullName";
        private const string PARM_ADDRESS = ":Address";
        private const string PARM_EMAIL = ":Email";
        private const string PARM_CREDITCARD = ":CreditCard";
        private const string PARM_OPENBALANCE = ":OpenBalance";
        private const string PARM_LOGOUTCOUNT = ":LogoutCount";
        private const string PARM_BALANCE = ":Balance";
        private const string PARM_LASTLOGIN = ":LastLogin";
        private const string PARM_LOGINCOUNT = ":LoginCount";
        private const string PARM_TOPORDERS = ":TopOrders";
        private const string PARM_DEBIT = ":Debit";
        private const string PARM_SALT = ":Salt";
        
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
                    commandText = string.Format(SQL_SELECT_ORDERS_BY_ID, maxTop.ToString());
                }
                else
                {
                    commandText = string.Format(SQL_SELECT_ORDERS_BY_ID, maxDefault.ToString());
                }
                OracleParameter accountidparm = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                accountidparm.Value = userID;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, commandText,accountidparm);
                List<OrderDataModel> orders = new List<OrderDataModel>();
                while (rdr.Read())
                {
                    int orderid = Convert.ToInt32(rdr.GetDecimal(0));
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
                    OrderDataModel order = new OrderDataModel(orderid, rdr.GetString(1), rdr.GetString(2), openDate, (DateTime)completionDate, Convert.ToDouble(rdr.GetDecimal(5)), rdr.GetDecimal(6), rdr.GetDecimal(7), rdr.GetString(8));
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
                OracleParameter parm1 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                parm1.Value = userid;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text,SQL_SELECT_CUSTOMERPROFILE_BYUSERID, parm1);
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
                            valid = true;
                    }
                    rdr.Close();
                    if (valid)
                    {
                        OracleParameter profileparm1 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                        profileparm1.Value = userid;
                        rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_CUSTOMER_LOGIN, profileparm1);
                        rdr.Read();
                        AccountDataModel customer = new AccountDataModel(Convert.ToInt32(rdr.GetDecimal(0)), userid, rdr.GetDateTime(1), rdr.GetDecimal(2), Convert.ToInt32(rdr.GetDecimal(3)), rdr.GetDecimal(4), rdr.GetDateTime(5), Convert.ToInt32(rdr.GetDecimal(6) + 1));
                        rdr.Close();
                        OracleParameter profileparm2 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                        profileparm2.Value = userid;
                        OracleHelper.ExecuteNonQuerySingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_CUSTOMER_LOGIN, profileparm2);
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
                OracleParameter parm1 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                parm1.Value = userid;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_CUSTOMERPROFILE_BYUSERID,parm1);
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
                    OracleParameter parm1 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                    parm1.Value = userID;
                    OracleHelper.ExecuteNonQuery(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_UPDATE_LOGOUT, parm1);
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
                OracleParameter parm1 = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                parm1.Value = userID;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_GET_CUSTOMER_BYUSERID, parm1);
                if (rdr.Read())
                {
                    AccountDataModel customer = new AccountDataModel(Convert.ToInt32(rdr.GetDecimal(0)), rdr.GetString(1), rdr.GetDateTime(2), rdr.GetDecimal(3), Convert.ToInt32(rdr.GetDecimal(4)), rdr.GetDecimal(5), rdr.GetDateTime(6), Convert.ToInt32(rdr.GetDecimal(7)));
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
                OracleParameter useridparm = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                useridparm.Value = userId;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleParm(_internalConnection,_internalADOTransaction,CommandType.Text,SQL_SELECT_CLOSED_ORDERS,useridparm);
                List<OrderDataModel> closedorders = new List<OrderDataModel>();
                DateTime completionDate = DateTime.MinValue;
                while (rdr.Read())
                {
                    int orderid = Convert.ToInt32(rdr.GetDecimal(0));
                    DateTime openDate = (DateTime)rdr.GetDateTime(4);
                    try
                    {
                        completionDate = (DateTime)rdr.GetDateTime(3);
                    }
                    catch (Exception e) { if (e.Message.Equals("Data is Null. This method or property cannot be called on Null values.")) completionDate = DateTime.MinValue; }
                    OrderDataModel order = new OrderDataModel(orderid, rdr.GetString(1), rdr.GetString(2), openDate, completionDate, Convert.ToDouble(rdr.GetDecimal(5)), rdr.GetDecimal(6), rdr.GetDecimal(7), rdr.GetString(8));
                    order.orderStatus = StockTraderUtility.ORDER_STATUS_COMPLETED;
                    closedorders.Add(order);
                }
                if (rdr.HasRows)
                {
                    rdr.Close();
                    useridparm = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                    useridparm.Value = userId;
                    OracleHelper.ExecuteNonQuerySingleParm(_internalConnection,_internalADOTransaction, CommandType.Text,SQL_UPDATE_CLOSED_ORDERS,useridparm);
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
                OracleParameter useridparm = new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20);
                useridparm.Value = userID;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleParm(_internalConnection,_internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDINGS, useridparm);
                List<HoldingDataModel> holdings = new List<HoldingDataModel>();
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(Convert.ToInt32(rdr.GetDecimal(0)), Convert.ToDouble(rdr.GetDecimal(1)),rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4), Convert.ToInt32(rdr.GetDecimal(5)),rdr.GetDecimal(6));
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
                OracleParameter orderIDparm = new OracleParameter(PARM_ORDERID, OracleDbType.Int32);
                orderIDparm.Value = orderID;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRowSingleParm(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING_LOCK, orderIDparm);
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(Convert.ToInt32(rdr.GetDecimal(0)), Convert.ToInt32(rdr.GetDecimal(1)), Convert.ToDouble(rdr.GetDecimal(2)), rdr.GetDecimal(3), rdr.GetDateTime(4), rdr.GetString(5),rdr.GetDecimal(6));
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
                OracleParameter[] holdingidparms = new OracleParameter[]{new OracleParameter(PARM_HOLDINGID, OracleDbType.Int32), 
                                                new OracleParameter(PARM_USERID, OracleDbType.Varchar2, 20)};
                holdingidparms[0].Value = holdingid;
                holdingidparms[1].Value = userid;
                OracleDataReader rdr = OracleHelper.ExecuteReaderSingleRow(_internalConnection,_internalADOTransaction, CommandType.Text, SQL_SELECT_HOLDING2, holdingidparms);
                while (rdr.Read())
                {
                    HoldingDataModel holding = new HoldingDataModel(holdingid, Convert.ToInt32(rdr.GetDecimal(0)), Convert.ToDouble(rdr.GetDecimal(1)), rdr.GetDecimal(2), rdr.GetDateTime(3), rdr.GetString(4),rdr.GetDecimal(5));
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
                OracleParameter[] accountParms = GetUpdateAccountBalanceParameters();
                accountParms[0].Value = total;
                accountParms[1].Value = accountID;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_DEBIT_ACCOUNT, accountParms);
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
            OracleParameter[] AccountParms = GetCreateAccountParameters();
            customer.accountID = Convert.ToInt32(OracleHelper.ExecuteScalarNoParm(_internalConnection, _internalADOTransaction,CommandType.Text,SQL_GET_NEXT_ACCOUNT_SEQ));
            try
            {
                AccountParms[0].Value = customer.accountID;
                AccountParms[1].Value = customer.openBalance;
                AccountParms[2].Value = customer.logoutCount;
                AccountParms[3].Value = customer.balance;
                AccountParms[4].Value = customer.lastLogin;
                AccountParms[5].Value = customer.loginCount;
                AccountParms[6].Value = customer.profileID;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_INSERT_ACCOUNT, AccountParms);
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
            OracleParameter[] ProfileParms = GetCreateAccountProfileParameters();
            try
            {
                string salt = " ";
                if (useSaltedHash)
                {
                    SaltedHash sh = SaltedHash.Create(customerprofile.password);
                    salt = sh.Salt;
                    string hash = sh.Hash;
                    customerprofile.password = hash;
                }
                ProfileParms[0].Value = customerprofile.address;
                ProfileParms[1].Value = customerprofile.password;
                ProfileParms[2].Value = customerprofile.userID;
                ProfileParms[3].Value = customerprofile.email;
                ProfileParms[4].Value = customerprofile.creditCard;
                ProfileParms[5].Value = customerprofile.fullName;
                ProfileParms[6].Value = salt;
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction,CommandType.Text, SQL_INSERT_ACCOUNTPROFILE, ProfileParms);
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
                OracleParameter[] ProfileParms = GetUpdateAccountProfileParameters();
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
                OracleHelper.ExecuteNonQuery(_internalConnection, _internalADOTransaction, CommandType.Text, SQL_UPDATE_ACCOUNTPROFILE, ProfileParms);
                return customerprofile;
            }
            catch 
            {
                throw;
            }
        }

        private static OracleParameter[] GetUpdateAccountBalanceParameters()
        {
            // Get the paramters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_DEBIT_ACCOUNT);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {new OracleParameter(PARM_DEBIT, OracleDbType.Decimal, 14),
                                            new OracleParameter(PARM_ACCOUNTID, OracleDbType.Int32)};
                // Add the parameters to the cached
                OracleHelper.CacheParameters(SQL_DEBIT_ACCOUNT, parms);
            }
            return parms;
        }

        private static OracleParameter[] GetCreateAccountProfileParameters()
        {
            // Get the parameters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_INSERT_ACCOUNTPROFILE);          
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
        			  new OracleParameter(PARM_ADDRESS, OracleDbType.Varchar2, StockTraderUtility.ADDRESS_MAX_LENGTH),
		              new OracleParameter(PARM_PASSWORD, OracleDbType.Varchar2, StockTraderUtility.PASSWORD_MAX_LENGTH),
                      new OracleParameter(PARM_USERID, OracleDbType.Varchar2, StockTraderUtility.USERID_MAX_LENGTH),
                      new OracleParameter(PARM_EMAIL, OracleDbType.Varchar2, StockTraderUtility.EMAIL_MAX_LENGTH),
                      new OracleParameter(PARM_CREDITCARD, OracleDbType.Varchar2, StockTraderUtility.CREDITCARD_MAX_LENGTH),
                      new OracleParameter(PARM_FULLNAME, OracleDbType.Varchar2, StockTraderUtility.FULLNAME_MAX_LENGTH),
                      new OracleParameter(PARM_SALT, OracleDbType.Varchar2, StockTraderUtility.PASSWORD_MAX_LENGTH)};

                // Add the parametes to the cached
                OracleHelper.CacheParameters(SQL_INSERT_ACCOUNTPROFILE, parms);
            }
            return parms;
        }

        private static OracleParameter[] GetUpdateAccountProfileParameters()
        {
            // Get the parameters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_UPDATE_ACCOUNTPROFILE);
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
            		  new OracleParameter(PARM_ADDRESS, OracleDbType.Varchar2, StockTraderUtility.ADDRESS_MAX_LENGTH),
                      new OracleParameter(PARM_SALT, OracleDbType.Varchar2, StockTraderUtility.PASSWORD_MAX_LENGTH),
					  new OracleParameter(PARM_PASSWORD, OracleDbType.Varchar2, StockTraderUtility.PASSWORD_MAX_LENGTH),
                      new OracleParameter(PARM_EMAIL, OracleDbType.Varchar2, StockTraderUtility.EMAIL_MAX_LENGTH),
                      new OracleParameter(PARM_CREDITCARD, OracleDbType.Varchar2, StockTraderUtility.CREDITCARD_MAX_LENGTH),
                      new OracleParameter(PARM_FULLNAME, OracleDbType.Varchar2, StockTraderUtility.FULLNAME_MAX_LENGTH),
                      new OracleParameter(PARM_USERID, OracleDbType.Varchar2, StockTraderUtility.USERID_MAX_LENGTH)};
                
                // Add the parametes to the cached
                OracleHelper.CacheParameters(SQL_UPDATE_ACCOUNTPROFILE, parms);
            }
            return parms;
        }

        private static OracleParameter[] GetCreateAccountParameters()
        {
            // Get the parameters from the cache
            OracleParameter[] parms = OracleHelper.GetCacheParameters(SQL_INSERT_ACCOUNT);          
            // If the cache is empty, rebuild the parameters
            if (parms == null)
            {
                parms = new OracleParameter[] {
                    new OracleParameter(PARM_ACCOUNTID, OracleDbType.Int32),
					  new OracleParameter(PARM_OPENBALANCE, OracleDbType.Decimal),
                      new OracleParameter(PARM_LOGOUTCOUNT, OracleDbType.Int32),
                      new OracleParameter(PARM_BALANCE, OracleDbType.Decimal),
                      new OracleParameter(PARM_LASTLOGIN, OracleDbType.TimeStamp),
                      new OracleParameter(PARM_LOGINCOUNT, OracleDbType.Int32),
                      new OracleParameter(PARM_USERID, OracleDbType.Varchar2, StockTraderUtility.USERID_MAX_LENGTH)};
                
                // Add the parameters to the cached
                OracleHelper.CacheParameters(SQL_INSERT_ACCOUNT, parms);
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
