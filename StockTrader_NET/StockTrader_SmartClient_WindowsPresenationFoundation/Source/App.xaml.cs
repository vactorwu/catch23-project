using System;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Timers;
using Trade.StockTraderWebApplicationServiceClient;
using Trade.BusinessServiceDataContract;
using System.Collections.Generic;
using System.Threading;

namespace StockTrader
{
	public partial class App: System.Windows.Application
	{
        public static BusinessServiceClient BSL;
        public static System.Timers.Timer timer;

        public App()
        {
            BSL = null;
            timer = new System.Timers.Timer(10000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            
        }

        public void startTimer()
        {
            timer.Start();
        }

        public void stopTimer()
        {
            timer.Stop();
        }

        private void CheckOrders()
        {
            try
            {
                List<OrderDataModel> orders = App.BSL.getClosedOrders(Login.UserInfo.profileID);
                if (orders.Count > 0)
                {
                    OrderAlert alert = new OrderAlert();
                    alert.setData(orders);
                    alert.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (App.BSL == null || Login.UserInfo == null) return;

            Thread check = new Thread(CheckOrders);
            check.SetApartmentState(ApartmentState.STA);  //Many WPF UI elements need to be created inside STA
            check.Start();
        }
	}
}
