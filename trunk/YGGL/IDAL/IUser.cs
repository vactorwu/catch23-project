using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LYC.Model;

namespace LYC.IDAL
{
    public interface IUser
    {
        IList<UserInfo> GetUsers();
        UserInfo GetUser(string yhdm);
        int RemoveUser(string yhdm);
        int AddUser(UserInfo user);
        IList<UserInfo> GetUsers(string yhdm);
    }
}
