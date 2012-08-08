using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Trade.BusinessServiceDataContract;

namespace StockTrader
{
    public enum TradeAction
    {
        Buy,
        Sell,
        None
    }
	public partial class Trade
	{
        static public TradeAction Action;
        static public string ID;
        public static double Quantity;
        private string LastAmount;

        Alert AlertBox;

		public Trade()
		{
			this.InitializeComponent();

			// Insert code required on object creation below this point.
            Message.Text = "Buy/Sell Shares";
            Amount.TextChanged +=new TextChangedEventHandler(Amount_TextChanged);
            LastAmount = "";
		}

        void Amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = Amount.CaretIndex;
            try
            {
                Amount.Text = Convert.ToInt32(Amount.Text).ToString();
            }
            catch (Exception)
            {
                if (!((Amount.Text.Length == 1 && Amount.Text[0] == '-') || (Amount.Text.Length < 1)))
                    Amount.Text = LastAmount;
            }
            LastAmount = Amount.Text;
            try
            {
                Amount.CaretIndex = i;
            }
            catch (Exception)
            {
            }
        }

        public void LoadData()
        {
            try
            {
                if (App.BSL == null || Login.UserInfo == null)
                {
                    AlertMessage("You are not connected! Make sure you have configured and logged in.");
                    return;
                }

                if (Action == TradeAction.Buy)
                {
                    Btn.Label = "Buy";
                    QuoteDataModel quote = App.BSL.getQuote(ID);
                    Message.Text = "You have requested to buy shares of " + ID + " which is currently trading at " + string.Format("{0:C}", quote.price);
                    Amount.Text = LastAmount = "";
                }
                else
                {
                    Btn.Label = "Sell";
                    
                    if (Configuration.info.isWebSphere)
                    {
                        Message.Text = "You have requested to sell all of your holding " + ID + ".";
                        LastAmount = Amount.Text = "0";
                    }
                    else
                    {
                        HoldingDataModel holding = App.BSL.getHolding(Login.UserInfo.profileID, Convert.ToInt32(ID));
                        Message.Text = "You have requested to sell all or part of your holding " + ID + ".  This holding has a total of " + holding.quantity + " shares of stock " + holding.quoteID + ".  Please indicate how many shares to sell.";
                        LastAmount = Amount.Text = holding.quantity.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Btn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Btn_MouseLeftButtonDown);
        }

        void Btn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Amount.Text = Math.Max(0, Convert.ToInt32(Amount.Text)).ToString();
            }
            catch (Exception)
            {
                Amount.Text = LastAmount = "0";
            }

            Quantity = Convert.ToDouble( Amount.Text );
            Order.Action = Action;
            Nav.SetOrderSection();
        }

        void AlertMessage(string m)
        {
            if (AlertBox == null)
            {
                AlertBox = new Alert();
                AlertBox.RenderTransform = new TranslateTransform(0, -100);
                LayoutRoot.Children.Add(AlertBox);


                DoubleAnimationUsingKeyFrames da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(0, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                AlertBox.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);

                da = new DoubleAnimationUsingKeyFrames();
                da.Duration = TimeSpan.FromMilliseconds(300);
                da.KeyFrames.Add(new SplineDoubleKeyFrame(100, TimeSpan.FromMilliseconds(300), new KeySpline(0.0, 0.6, 0.0, 0.9)));
                //da.Completed += new EventHandler(onEntered);
                Pane.RenderTransform = new TranslateTransform();
                Pane.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);
            }
            AlertBox.Text = m;
        }

        public override void onSectionReady()
        {
            LoadData();
            if ((Configuration.info.isWebSphere) && Action == TradeAction.Sell)
            {
                TextRect.Visibility =
                Amount.Visibility =
                Label.Visibility = Visibility.Hidden;
                Btn.Margin = new Thickness(161, 43.001, 0, 0);
            }
            else
            {
                TextRect.Visibility =
                Amount.Visibility =
                Label.Visibility = Visibility.Visible;
                Btn.Margin = new Thickness(271.497, 43.001, 0, 0);
            }
        }
	}
}