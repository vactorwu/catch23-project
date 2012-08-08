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

namespace StockTrader
{
	public partial class OrderAlert
	{
		public OrderAlert()
		{
			this.InitializeComponent();

            GridColumn column = new GridColumn();
            column.setLabel("Order ID");
            column.Width = 80;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Order Status");
            column.Width = 80;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Creation Date");
            column.Width = 140;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Completion Date");
            column.Width = 140;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Txn Fee");
            column.Width = 60;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Type");
            column.Width = 40;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Symbol");
            column.Width = 50;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Quantity");
            column.Width = 50;
            OrderGrid.addColumn(column);

            CloseButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(CloseButton_MouseLeftButtonDown);
            CloseButton.Label = "OK";
		}

        void CloseButton_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }

        public void setData( List<OrderDataModel> orders )
        {
            OrderDataModel order = null;
            DataProvider data = new DataProvider();
            for (int i = 0; i < orders.Count; i++)
            {
                order = orders[i];
                data.Add(new RowDataProvider());
                data[i].id = i;

                data[i].Add(new GridColumnData());
                data[i][0].value = order.orderID.ToString();
                data[i][0].numeric = order.orderID;

                data[i].Add(new GridColumnData());
                data[i][1].value = order.orderStatus;
                data[i][1].numeric = order.orderStatus.Equals("closed") ? 0 : 1;

                data[i].Add(new GridColumnData());
                data[i][2].value = order.openDate.ToString();
                data[i][2].numeric = (int)(order.openDate.Ticks / 10000);

                data[i].Add(new GridColumnData());
                data[i][3].value = order.completionDate.ToString();
                data[i][3].numeric = (int)(order.completionDate.Ticks / 10000);

                data[i].Add(new GridColumnData());
                data[i][4].value = string.Format("{0:C}", order.orderFee);
                data[i][4].numeric = (int)(order.orderFee * 100);

                data[i].Add(new GridColumnData());
                data[i][5].value = order.orderType;
                data[i][5].numeric = order.orderType.Equals("buy") ? 1 : 0;

                data[i].Add(new GridColumnData());
                data[i][6].value = order.symbol;
                data[i][6].numeric = (int)(order.quantity * 100);

                data[i].Add(new GridColumnData());
                data[i][7].value = order.quantity.ToString();
                data[i][7].numeric = (int)(order.quantity * 100);
            }

            OrderGrid.setData(data);
        }
	}
}