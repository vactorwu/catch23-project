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
    public partial class Quotes
    {
        public static string QuotesParameter;

        public Quotes()
        {
            this.InitializeComponent();

            QuotesParameter = "s:0;s:1;s:2";

            GridColumn column = new GridColumn();
            column.setLabel("Symbol");
            column.Width = 95;
            QuoteGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Company");
            column.Width = 150;
            QuoteGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Volume");
            column.Width = 90;
            QuoteGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Price Range");
            column.Width = 180;
            QuoteGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Open Price");
            column.Width = 110;
            QuoteGrid.addColumn(column);

            column = new GridColumnGain();
            column.setLabel("Current Price");
            column.Width = 140;
            QuoteGrid.addColumn(column);
            
            column = new GridColumnGain();
            column.setLabel("Gain (Loss)");
            column.Width = 110;
            QuoteGrid.addColumn(column);

            column = new GridColumnSell();
            column.setLabel("Trade");
            column.Width = 72;
            QuoteGrid.addColumn(column);

            QuoteGrid.RowHighLighted += new System.Windows.Input.MouseEventHandler(onQuoteGridRowHighLighted);
            QuoteGrid.RowMouseMove += new System.Windows.Input.MouseEventHandler(onQuoteGridRowMouseMove);
            GetQuote.ClickGetQuote += new EventHandler(GetQuote_ClickGetQuote);
            QuoteGrid.SellRow += new EventHandler(QuoteGrid_SellRow);

        }

        void QuoteGrid_SellRow(object sender, EventArgs e)
        {
            Trade.ID = QuoteGrid.data[QuoteGrid.HighLightedRow][0].value;
            Trade.Action = TradeAction.Buy;
            Nav.SetTradeSection();
        }

        void GetQuote_ClickGetQuote(object sender, EventArgs e)
        {
            Quotes.QuotesParameter = GetQuote.Text;
            LoadData();
        }


        void onQuoteGridRowMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        void onQuoteGridRowHighLighted(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }

        public void LoadData()
        {
            // get quotes based on QuotesParameter
            if (App.BSL == null || Login.UserInfo == null) return;

            try
            {
                Update.Text = "as of " + DateTime.Now;
                GetQuote.Text = QuotesParameter;
                string[] quotes = QuotesParameter.Split(new char[] { ' ', ',', ';' });
                DataProvider data = new DataProvider();
                foreach (string quote in quotes)
                {
                    string quoteTrim = quote.Trim();
                    if (!quoteTrim.Equals(""))
                    {
                        QuoteDataModel quoteData = App.BSL.getQuote(quoteTrim);
                        if (quoteData == null) continue;

                        RowDataProvider row = new RowDataProvider();
                        data.Add(row);
                        row.id = data.Count - 1;

                        GridColumnData symbol = new GridColumnData();
                        symbol.value = quoteData.symbol;
                        symbol.numeric = (int)(quoteData.price * 100);
                        row.Add(symbol);

                        GridColumnData company = new GridColumnData();
                        company.value = quoteData.companyName;
                        company.numeric = (int)(quoteData.price * 100);
                        row.Add(company);

                        GridColumnData volume = new GridColumnData();
                        volume.value = string.Format("{0:0,0}", quoteData.volume);
                        volume.numeric = (int)(quoteData.volume * 100);
                        row.Add(volume);

                        GridColumnData range = new GridColumnData();
                        range.value = string.Format("{0:C}", quoteData.low) + " - " + string.Format("{0:C}", quoteData.high);
                        range.numeric = (int)(quoteData.low * 100);
                        row.Add(range);

                        GridColumnData open = new GridColumnData();
                        open.value = string.Format("{0:C}", quoteData.open);
                        open.numeric = (int)(quoteData.low * 100);
                        row.Add(open);

                        GridColumnGainData current = new GridColumnGainData();
                        current.value = string.Format("{0:C}", quoteData.price);
                        current.numeric = (int)(quoteData.price * 100);
                        current.increased = quoteData.change == 0 ? 0 : (quoteData.change > 0 ? 1 : -1);
                        row.Add(current);

                        GridColumnGainData gain = new GridColumnGainData();
                        gain.value = string.Format("{0:C}", quoteData.change);
                        gain.numeric = (int)(quoteData.change * 100);
                        gain.increased = quoteData.change == 0 ? 0 : (quoteData.change > 0 ? 1 : -1);
                        row.Add(gain);

                        GridColumnData buy = new GridColumnData();
                        buy.value = "BUY";
                        buy.numeric = (int)(quoteData.change * 100);
                        row.Add(buy);
                    }
                }

                QuoteGrid.setData(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public override void onSectionReady()
        {
            QuoteGrid.ApplyEffects();
            LoadData();
        }

        public override void onSectionRemove()
        {
            QuoteGrid.RemoveEffects();
        }
    }
}