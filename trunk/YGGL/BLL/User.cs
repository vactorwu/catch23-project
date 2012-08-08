using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LYC.IDAL;
using LYC.DALFactory;
using LYC.Model;

namespace LYC.BLL
{
    public class User
    {
        private static readonly IUser dal = LYC.DALFactory.DataAccess.CreateUser();

        public UserInfo getUser(string yhdm)
        {
            return dal.GetUser(yhdm);
            //return new UserInfo();
        }
        public IList<UserInfo> GetUser(string yhdm)
        {
            return dal.GetUsers(yhdm);
        }
    }
}
