using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Collections.Generic;
using StockTrader.Graphing;
using Trade.BusinessServiceDataContract;

namespace StockTrader
{
    public partial class Order
    {
        Alert AlertBox;
        static public TradeAction Action = TradeAction.None;

        public Order()
        {
            this.InitializeComponent();

            GridColumn column = new GridColumn();
            column.setLabel("Order ID");
            column.Width = 95;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Order Status");
            column.Width = 140;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Creation Date");
            column.Width = 190;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Completion Date");
            column.Width = 162;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Txn Fee");
            column.Width = 90;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Type");
            column.Width = 90;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Symbol");
            column.Width = 90;
            OrderGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Quantity");
            column.Width = 90;
            OrderGrid.addColumn(column);


            OrderGrid.RowHighLighted += new System.Windows.Input.MouseEventHandler(onOrderGridRowHighLighted);
            OrderGrid.RowMouseMove += new System.Windows.Input.MouseEventHandler(onOrderGridRowMouseMove);
            OrderGrid.SellRow += new EventHandler(OrderGrid_SellRow);
            GetQuote.ClickGetQuote += new EventHandler(GetQuote_ClickGetQuote);
        }

        void OrderGrid_SellRow(object sender, EventArgs e)
        {
            Trade.ID = OrderGrid.data[OrderGrid.HighLightedRow][0].value;
            Trade.Action = TradeAction.Sell;
            Nav.SetTradeSection();
        }

        void GetQuote_ClickGetQuote(object sender, EventArgs e)
        {
            Quotes.QuotesParameter = GetQuote.Text;
            Nav.SetSection(5);
        }

        void onOrderGridRowMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        void onOrderGridRowHighLighted(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        public void LoadData()
        {
            if (App.BSL == null || Login.UserInfo == null)
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            OrderDataModel order = null;
            if (Action == TradeAction.Buy)
            {
                try
                {
                    if ((order = App.BSL.buy(Login.UserInfo.profileID, Trade.ID, Trade.Quantity, 0)) != null)
                    {
                        AlertMessage("Order " + order.orderID + " to buy " + order.quantity + " shares of " + order.symbol + " has been submitted for processing.");
                    }
                    
                }
                catch (Exception ex)
                {
                    AlertMessage("Failed to process buy order, exception: " + ex.ToString());
                }
            }
            else if( Action == TradeAction.Sell )
            {
                try
                {
                    if (Trade.Quantity == 0)
                    {
                        order = App.BSL.sell(Login.UserInfo.profileID, Convert.ToInt32(Trade.ID), 0);
                    }
                    else
                    {
                        order = App.BSL.sellEnhanced(Login.UserInfo.profileID, Convert.ToInt32(Trade.ID), Trade.Quantity);
                    }
                    if (order != null)
                    {
                        AlertMessage("Order " + order.orderID + " to sell " + order.quantity + " shares of " + order.symbol + " has been submitted for processing.");
                    }
                }
                catch (Exception ex)
                {
                    AlertMessage("Failed to process sell order, Exception: " + ex.ToString());
                }
            }
            Action = TradeAction.None;

            try
            {
                List<OrderDataModel> orders = App.BSL.getOrders(Login.UserInfo.profileID);
                Update.Text = "as of " + DateTime.Now;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Visibility = Visibility.Visible;
        }

        public override void onSectionReady()
        {
            OrderGrid.ApplyEffects();

            // load data

            LoadData();

        }

        public override void onSectionRemove()
        {
            OrderGrid.RemoveEffects();
        }

        void AlertMessage(string m)
        {
            if (AlertBox == null)
            {
                AlertBox = new Alert();
                AlertBox.RenderTransform = new TranslateTransform(0, -70);

                OrderGrid.Height = 373;
                DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(30, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                AlertBox.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);

                LayoutRoot.Children.Add(AlertBox);

                da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(100, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                OrderGrid.RenderTransform = new TranslateTransform();
                OrderGrid.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);
            }
            AlertBox.Text = m;
        }
    }
}