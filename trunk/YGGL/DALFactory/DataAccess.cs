using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using LYC.SqlServerDAL;

namespace LYC.DALFactory
{
    public sealed class DataAccess
    {
        private static readonly string path = ConfigurationManager.AppSettings["SqlDAL"];

        private DataAccess() { }

        public static LYC.IDAL.IUser CreateUser()   
        {
           
            string className = path + ".User";

            return (LYC.IDAL.IUser)Assembly.Load(path).CreateInstance(className);

        }
    }
}
