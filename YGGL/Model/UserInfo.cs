using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LYC.Model
{
    public class UserInfo
    {
        private short xtsb;
        private string yhdm;
        private string yhzm;
        private string yhmc;
        private string yhkl;

        public UserInfo()
        {

        }

        public UserInfo(short xtsb, string yhdm, string yhzm, string yhmc, string yhkl)
        {
            this.xtsb = xtsb;
            this.yhdm = yhdm;
            this.yhzm = yhzm;
            this.yhmc = yhmc;
            this.yhkl = yhkl;
        }

        public short Xtsb
        {
            set;
            get;
        }
        public string Yhdm
        {
            set;
            get;
        }
        public string Yhzm
        {
            set;
            get;
        }
        public string Yhmc
        {
            set;
            get;
        }
        public string Yhkl
        {
            set;
            get;
        }
        
        
    }
}
