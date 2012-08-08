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
	public partial class Home
	{
		public Home()
		{
			this.InitializeComponent();

            for (int i = 0; i < 2; i++)
            {
                GridColumn column = new GridColumn();
                column.setLabel("Symbol");
                column.Width = 127;
                if( i == 0 )
                    GainGrid.addColumn(column);
                else
                    LoseGrid.addColumn(column);

                column = new GridColumnGain();
                column.setLabel("Price");
                column.Width = 144;
                if (i == 0)
                    GainGrid.addColumn(column);
                else
                    LoseGrid.addColumn(column);

                column = new GridColumnGain();
                column.setLabel("Change");
                column.Width = 118;
                if (i == 0)
                    GainGrid.addColumn(column);
                else
                    LoseGrid.addColumn(column);
            }
		}

        private delegate List<HoldingDataModel> GetHoldingsAsync();
        List<HoldingDataModel> GetHoldings()
        {
            return App.BSL.getHoldings(Login.UserInfo.profileID);
        }

        private delegate AccountDataModel GetCustomerAsync();
        AccountDataModel GetCustomer()
        {
            return App.BSL.getAccountData(Login.UserInfo.profileID);
        }

        public override void onSectionReady()
        {
            if (App.BSL == null || Login.UserInfo == null)
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            Update.Text = "as of " + DateTime.Now;

            try
            {
                GetHoldingsAsync caller1 = new GetHoldingsAsync(GetHoldings);
                // Initiate the asychronous call.
                IAsyncResult result1 = caller1.BeginInvoke(null, null);
                // Repeat
                GetCustomerAsync caller2 = new GetCustomerAsync(GetCustomer);
                // Initiate the asychronous call.
                IAsyncResult result2 = caller2.BeginInvoke(null, null);
                MarketSummaryDataModelWS summary = App.BSL.getMarketSummary();
                List<HoldingDataModel> holdings = caller1.EndInvoke(result1);
                AccountDataModel customer = caller2.EndInvoke(result2);
                decimal marketValue = 0;
                decimal gain = 0;
                decimal basis = 0;
                decimal _gain = (decimal)0.00;
                decimal percentGain = (decimal)0.00;

                for (int i = 0; i < holdings.Count; i++)
                {
                    QuoteDataModel quote = App.BSL.getQuote(holdings[i].quoteID);
                    decimal _marketValue = (decimal)holdings[i].quantity * quote.price;
                    decimal _basis = (decimal)holdings[i].quantity * (decimal)holdings[i].purchasePrice;
                    gain += _marketValue - _basis;
                    marketValue += _marketValue;
                    basis += _basis;
                }

                AccountID.Text = customer.accountID.ToString();
                CreationDate.Text = customer.creationDate.ToString();
                LoginCount.Text = customer.loginCount.ToString();
                OpenBalance.Text = string.Format("{0:C}", customer.openBalance);
                Balance.Text = string.Format("{0:C}", customer.balance);
                NumHoldings.Text = holdings.Count.ToString();
                HoldingsTotal.Text = string.Format("{0:C}", marketValue);
                decimal totalcashandholdings = marketValue + customer.balance;
                SumOfCashHoldings.Text = string.Format("{0:C}", totalcashandholdings);
                _gain = totalcashandholdings - customer.openBalance;
                if (customer.openBalance != 0)
                    percentGain = gain / customer.openBalance * 100;
                else
                    percentGain = 0;

                Gain.Text = string.Format("{0:C}", _gain);
                PercentGain.Text = string.Format("{0:N}%", percentGain);
                if (_gain > 0)
                {
                    PercentGain.Foreground = new SolidColorBrush(Color.FromRgb(37, 120, 32));

                    GainIcon icon = new GainIcon();
                    icon.VerticalAlignment = VerticalAlignment.Bottom;
                    icon.HorizontalAlignment = HorizontalAlignment.Left;
                    icon.Margin = new Thickness(PercentGain.Margin.Left + PercentGain.ActualWidth + 40, PercentGain.Margin.Top, PercentGain.Margin.Right, PercentGain.Margin.Bottom);
                    LayoutRoot.Children.Add(icon);
                }
                else if (_gain < 0)
                {
                    PercentGain.Foreground = new SolidColorBrush(Color.FromRgb(191, 0, 0));
                    LossIcon icon = new LossIcon();
                    icon.VerticalAlignment = VerticalAlignment.Bottom;
                    icon.HorizontalAlignment = HorizontalAlignment.Left;
                    icon.Margin = new Thickness(PercentGain.Margin.Left + PercentGain.ActualWidth + 40, PercentGain.Margin.Top, PercentGain.Margin.Right, PercentGain.Margin.Bottom);
                    LayoutRoot.Children.Add(icon);

                }
                else
                    PercentGain.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));

                
                TSIA.Text = String.Format("{0:N}", summary.TSIA);
                Volume.Text = String.Format("{0:N}", summary.volume);

                DataProvider data = new DataProvider();
                for (int i = 0; i < summary.topGainers.Count; i++)
                {
                    QuoteDataModel quote = summary.topGainers[i];



                    RowDataProvider row = new RowDataProvider();
                    row.id = i;

                    // symbol, current price, gain
                    row.Add(new GridColumnData());
                    row[0].value = quote.symbol;
                    row[0].numeric = i;

                    row.Add(new GridColumnGainData());
                    row[1].value = string.Format("{0:C}", quote.price);
                    row[1].numeric = (int)(quote.price * 100);
                    (row[1] as GridColumnGainData).increased = quote.change == 0 ? 0 : (quote.change > 0 ? 1 : -1);

                    row.Add(new GridColumnGainData());
                    row[2].value = string.Format("{0:C}", quote.change);
                    row[2].numeric = (int)(quote.change * 100);
                    (row[2] as GridColumnGainData).increased = quote.change == 0 ? 0 : (quote.change > 0 ? 1 : -1);

                    data.Add(row);
                }

                GainGrid.setData(data);

                data = new DataProvider();
                for (int i = 0; i < summary.topLosers.Count; i++)
                {
                    QuoteDataModel quote = summary.topLosers[i];




                    RowDataProvider row = new RowDataProvider();
                    row.id = i;

                    // symbol, current price, gain
                    row.Add(new GridColumnData());
                    row[0].value = quote.symbol;
                    row[0].numeric = i;

                    row.Add(new GridColumnGainData());
                    row[1].value = string.Format("{0:C}", quote.price);
                    row[1].numeric = (int)(quote.price * 100);
                    (row[1] as GridColumnGainData).increased = quote.change == 0 ? 0 : (quote.change > 0 ? 1 : -1);

                    row.Add(new GridColumnGainData());
                    row[2].value = string.Format("{0:C}", quote.change);
                    row[2].numeric = (int)(quote.change * 100);
                    (row[2] as GridColumnGainData).increased = quote.change == 0 ? 0 : (quote.change > 0 ? 1 : -1);

                    data.Add(row);
                }

                LoseGrid.setData(data);

                SessionCreateDate.Text = Login.LoginTime.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Visibility = Visibility.Visible;
        }
	}
}