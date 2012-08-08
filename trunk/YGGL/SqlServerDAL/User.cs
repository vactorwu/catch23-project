using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LYC.IDAL;
using LYC.Model;
using System.Data;
using System.Data.SqlClient;
using LYC.DBUtility;
namespace LYC.SqlServerDAL
{
    public class User:IUser
    {
        private const string SQL1 = "SELECT * FROM SS_OPERATE_USER WHERE YHDM = @YHDM";
        private const string SQL2 = "DELETE FROM SS_OPERATE_USER WHERE YHDM = @YHDM";
        private const string SQL3 = "SELECT * FROM SS_OPERATE_USER";
        private const string SQL4 = "INSERT INTO SS_OPERATE_USER VALUES(@XTSB,@YHDM,@YHZM,@YHMC,@YHKL)";
        private const string PARM_YHDM = "@YHDM";
        private const string PARM_XTSB = "@XTSB";
        private const string PARM_YHZM = "@YHZM";
        private const string PARM_YHMC = "@YHMC";
        private const string PARM_YHKL = "@YHKL";


        
        public UserInfo GetUser(string yhdm)
        {
            UserInfo user = new UserInfo();
            SqlParameter parm = new SqlParameter();
            parm.ParameterName = PARM_YHDM;
            parm.Value = yhdm;
            parm.SqlDbType = SqlDbType.VarChar;
            parm.Size = 4;
            using (SqlDataReader rdr = SQLHelper.ExecuteReaderWithParm(SQLHelper.ConnHIS,SQL1,parm))
            {
                 if (rdr.Read())
                {
                    user = new UserInfo(Convert.ToInt16(rdr["XTSB"]), rdr["YHDM"].ToString(), rdr["YHZM"].ToString(), rdr["YHMC"].ToString(), rdr["YHKL"].ToString());   
                }

            }
            return user;
        }

        public IList<UserInfo> GetUsers()
        {
            IList<UserInfo> users = new List<UserInfo>();

            using (SqlDataReader rdr = SQLHelper.ExecuteReader(SQLHelper.ConnHIS, SQL3))
            {
                while (rdr.Read())
                {
                    UserInfo user = new UserInfo(Convert.ToInt16(rdr["XTSB"]), rdr["YHDM"].ToString(), rdr["YHZM"].ToString(), rdr["YHMC"].ToString(), rdr["YHKL"].ToString());
                    users.Add(user);
                }
             
                return users;

            }
        }


        public int RemoveUser(string yhdm)
        {
            SqlParameter parm = new SqlParameter();
            parm.ParameterName = PARM_YHDM;
            parm.Value = yhdm;
            parm.SqlDbType = SqlDbType.VarChar;
            parm.Size = 4;
            int val = SQLHelper.ExecuteNonQuery(SQLHelper.ConnHIS, SQL2, parm);
            return val;
        }


        public int AddUser(UserInfo user)
        {
            SqlParameter[] parms = GetParms(user);
            int val = SQLHelper.ExecuteNonQueryWithParms(SQLHelper.ConnHIS, SQL4, parms);
            return val;
        }

        private  SqlParameter[] GetParms(UserInfo user)
        {

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARM_XTSB,SqlDbType.SmallInt),
                new SqlParameter(PARM_YHDM,SqlDbType.VarChar,4),
                new SqlParameter(PARM_YHZM,SqlDbType.VarChar,20),
                new SqlParameter(PARM_YHMC,SqlDbType.VarChar,10),
                new SqlParameter(PARM_YHKL,SqlDbType.VarChar,10)
            };
            parms[0].Value = user.Xtsb;
            parms[1].Value = user.Yhdm;
            parms[2].Value = user.Yhzm;
            parms[3].Value = user.Yhmc;
            parms[4].Value = user.Yhkl;
            return parms;
        }


        public IList<UserInfo> GetUsers(string yhdm)
        {
           
            IList<UserInfo> users = new List<UserInfo>();
            SqlParameter parm = new SqlParameter();
            parm.ParameterName = PARM_YHDM;
            parm.Value = yhdm;
            parm.SqlDbType = SqlDbType.VarChar;
            parm.Size = 4;
            using (SqlDataReader rdr = SQLHelper.ExecuteReaderWithParm(SQLHelper.ConnHIS, SQL1, parm))
            {
                if (rdr.Read())
                {
                    UserInfo user = new UserInfo(Convert.ToInt16(rdr["XTSB"]), rdr["YHDM"].ToString(), rdr["YHZM"].ToString(), rdr["YHMC"].ToString(), rdr["YHKL"].ToString());
                    users.Add(user);
                }

            }
            return users;
        }
    }
}
