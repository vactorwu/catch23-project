using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace StockTrader
{
	public partial class Alert
	{
		public Alert()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
		}

        public string Text
        {
            set
            {
                Message.Text = value;
            }
        }
	}
}