using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Diagnostics;

 
namespace StockTrader
{
	public partial class Welcome
	{
		public Welcome()
		{
			this.InitializeComponent();

            LINK_BOX.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(LINK_BOX_MouseLeftButtonDown);
            
		}

        void LINK_BOX_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            using (Process pieProcess = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("http://msdn.microsoft.com/stocktrader", "");
                pieProcess.StartInfo = startInfo;
                try
                {
                    pieProcess.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to launch web browser", "Error");
                }
            }
        }
	}
}