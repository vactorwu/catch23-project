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
	public partial class _Default : _Default_Sy
	{
		public UpdatePanel upTop;
		protected override bool IsAjaxPage()
		{
			return true;
		}
		
		public override UpdatePanel GetTopUpdatePanel()
		{
			return upTop;
		}
	}
}
