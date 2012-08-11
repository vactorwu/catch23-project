using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Sybase.PowerBuilder;
using Sybase.PowerBuilder.Web;

namespace PBWebApp
{
	public partial class _Default_Sy : PBMainPage
	{
		public PlaceHolder mainWindow;
 			
		protected override PBSession CreateSession()
		{
			c__smjl.InitSession();
			return Sybase.PowerBuilder.Web.PBSession.CreateSession(typeof(c__smjl));
		}

		public override PlaceHolder GetPlaceHolder()
		{
			return mainWindow;
		}
	}
}
