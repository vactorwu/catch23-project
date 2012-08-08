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
	public partial class Portfolio
	{
        new public PieTip ToolTip;
        private bool isBySymbol;
        DataProvider dataRows;
        List<PieElement> rowsGraphData;

		public Portfolio()
		{
            isBySymbol = false;
			this.InitializeComponent();

            GridColumn column = new GridColumn();
            column.setLabel("Holding ID");
            column.Width = 95;
            PortfolioGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Purchase\nDate");
            column.Width = 150;
            PortfolioGrid.addColumn(column);

            column = new GridColumn();
            column.setLabel("Symbol");
            column.Width = 90;
            PortfolioGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Quantity");
            column.Width = 70;
            PortfolioGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Purchase\nPrice");
            column.Width = 90;
            PortfolioGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Current\nPrice");
            column.Width = 90;
            PortfolioGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Purchase\nBasis");
            column.Width = 90;
            PortfolioGrid.addColumn(column);

            column = new GridColumnTextRight();
            column.setLabel("Market\nValue");
            column.Width = 90;
            PortfolioGrid.addColumn(column);

            column = new GridColumnGain();
            column.setLabel("Gain (Loss)");
            column.Width = 110;
            PortfolioGrid.addColumn(column);

            column = new GridColumnSell();
            column.setLabel("Trade");
            column.Width = 72;
            PortfolioGrid.addColumn(column);

            PortfolioGrid.ColumnSortChanged += new EventHandler(PortfolioGrid_ColumnSortChanged);

            ToolTip = new PieTip();
            ToolTip.Visibility = Visibility.Hidden;
            LayoutRoot.Children.Add(ToolTip);

            PortfolioGrid.RowHighLighted += new System.Windows.Input.MouseEventHandler(onPortfolioGridRowHighLighted);
            PortfolioGrid.RowMouseMove += new System.Windows.Input.MouseEventHandler(onPortfolioGridRowMouseMove);
            PortfolioGrid.SellRow += new EventHandler(PortfolioGrid_SellRow);
            GetQuote.ClickGetQuote += new EventHandler(GetQuote_ClickGetQuote);
		}

        void PortfolioGrid_ColumnSortChanged(object sender, EventArgs e)
        {
            if (PortfolioGrid.SortColumn.Label.Text.Equals("Symbol"))
            {
                PortfolioGrid.AlternateColors = false;
                if (isBySymbol)
                    PortfolioGrid.setData(dataRows);

                PortfolioGrid.SortAlphabetically();
                
                dataRows = new DataProvider(PortfolioGrid.data);
                dataRows.SortColumn = PortfolioGrid.data.SortColumn;

                isBySymbol = true;

                int quantity = 0;
                decimal basis = 0;
                decimal marketValue = 0;
                decimal gain = 0;
                string symbol = null;

                decimal basisTotal = 0;
                decimal marketValueTotal = 0;
                decimal gainTotal = 0;
                RowDataProvider row;
                int group = 0;
                List<PieElement> graphData = new List<PieElement>();
                for (int i = 0; true; i++)
                {
                    if (i > PortfolioGrid.data.Count) break;
                    if (i == PortfolioGrid.data.Count || (symbol != null && !symbol.Equals(PortfolioGrid.data[i][2].value)))
                    {
                        row = new RowDataProvider();
                        row.id = group;

                        // id, date, symbol, quantity, purchase price, current price, basis, market value, gain, trade
                        row.Add(new GridColumnData());
                        row[0].value = "";
                        row[0].numeric = 0;

                        row.Add(new GridColumnData());
                        row[1].value = "";
                        row[1].numeric = 0;

                        row.Add(new GridColumnData());
                        row[2].value = "";// PortfolioGrid.data[i - 1][2].value;
                        row[2].numeric = 0;

                        row.Add(new GridColumnData());
                        row[3].value = string.Format("{0:0,0}", quantity);
                        row[3].numeric = (int)quantity;

                        row.Add(new GridColumnData());
                        row[4].value = "";// string.Format("{0:C}", holdings[i].purchasePrice);
                        row[4].numeric = 0;// (int)(holdings[i].purchasePrice * 100);

                        row.Add(new GridColumnData());
                        row[5].value = PortfolioGrid.data[i - 1][5].value;// string.Format("{0:C}", quote.price);
                        row[5].numeric = PortfolioGrid.data[i - 1][5].numeric;

                        row.Add(new GridColumnData());
                        row[6].value = string.Format("{0:C}", basis/100);
                        row[6].numeric = (int)(basis );

                        row.Add(new GridColumnData());
                        row[7].value = string.Format("{0:C}", marketValue/100);
                        row[7].numeric = (int)(marketValue);

                        row.Add(new GridColumnGainData());
                        row[8].value = string.Format("{0:C}", gain/100);
                        row[8].numeric = (int)(gain);
                        (row[8] as GridColumnGainData).increased = gain == 0 ? 0 : (gain > 0 ? 1 : -1);

                        row.Add(new GridColumnData());
                        row[9].value = "";// "SELL";
                        row[9].numeric = 0;// i;

                        graphData.Add(new PieElement(PortfolioGrid.data[i - 1][2].value, (double)marketValue/100));
                        group++;

                        PortfolioGrid.data.Insert(i, row);
                        i++;

                        basisTotal += basis;
                        marketValueTotal += marketValue;
                        gainTotal += gain;
                        if (i < PortfolioGrid.data.Count)
                        {
                            PortfolioGrid.data[i].id = group;
                            symbol = PortfolioGrid.data[i][2].value;
                            quantity = PortfolioGrid.data[i][3].numeric;
                            basis = PortfolioGrid.data[i][6].numeric;
                            marketValue = PortfolioGrid.data[i][7].numeric;
                            gain = PortfolioGrid.data[i][8].numeric;
                        }
                    }
                    else
                    {
                        PortfolioGrid.data[i].id = group;
                        quantity += PortfolioGrid.data[i][3].numeric;
                        symbol = PortfolioGrid.data[i][2].value;
                        basis += PortfolioGrid.data[i][6].numeric;
                        marketValue += PortfolioGrid.data[i][7].numeric;
                        gain += PortfolioGrid.data[i][8].numeric;
                    }
                }
                row = new RowDataProvider();
                row.id = -1;

                // id, date, symbol, quantity, purchase price, current price, basis, market value, gain, trade
                row.Add(new GridColumnData());
                row[0].value = "";
                row[0].numeric = 0;

                row.Add(new GridColumnData());
                row[1].value = "";
                row[1].numeric = 0;

                row.Add(new GridColumnData());
                row[2].value = "";// PortfolioGrid.data[i - 1][2].value;
                row[2].numeric = 0;

                row.Add(new GridColumnData());
                row[3].value = "";
                row[3].numeric = 0;

                row.Add(new GridColumnData());
                row[4].value = "";// string.Format("{0:C}", holdings[i].purchasePrice);
                row[4].numeric = 0;// (int)(holdings[i].purchasePrice * 100);

                row.Add(new GridColumnData());
                row[5].value = "Totals";
                row[5].numeric = 0;

                row.Add(new GridColumnData());
                row[6].value = string.Format("{0:C}", basisTotal/100);
                row[6].numeric = (int)(basisTotal);

                row.Add(new GridColumnData());
                row[7].value = string.Format("{0:C}", marketValueTotal / 100);
                row[7].numeric = (int)(marketValueTotal);

                row.Add(new GridColumnGainData());
                row[8].value = string.Format("{0:C}", gainTotal / 100);
                row[8].numeric = (int)(gainTotal);
                (row[8] as GridColumnGainData).increased = gainTotal == 0 ? 0 : (gainTotal > 0 ? 1 : -1);

                row.Add(new GridColumnData());
                row[9].value = "";// "SELL";
                row[9].numeric = 0;// i;
                PortfolioGrid.data.Add(row);
                PortfolioGrid.setData(PortfolioGrid.data);
                ToolTip.setData(graphData);
            }
            else
            {
                PortfolioGrid.AlternateColors = true;
                isBySymbol = false;
                if (dataRows != null)
                {
                    PortfolioGrid.setData(dataRows);
                    PortfolioGrid.Sort();
                    ToolTip.setData(rowsGraphData);
                }
                dataRows = null;
            }
        }

        void PortfolioGrid_SellRow(object sender, EventArgs e)
        {
            Trade.ID = PortfolioGrid.data[PortfolioGrid.HighLightedRow][0].value;
            Trade.Action = TradeAction.Sell;
            Nav.SetTradeSection();
        }

        void GetQuote_ClickGetQuote(object sender, EventArgs e)
        {
            Quotes.QuotesParameter = GetQuote.Text;
            Nav.SetSection(5);
        }

        void onPortfolioGridRowMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point move = e.GetPosition(LayoutRoot);
            ToolTip.Margin = new Thickness(move.X + 20, move.Y + 20, 0, 0);
            if (PortfolioGrid.HighLightedRow != -1)
                ToolTip.Select(PortfolioGrid.data[PortfolioGrid.HighLightedRow].id);
        }

        void onPortfolioGridRowHighLighted(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point move = e.GetPosition(LayoutRoot);
            if (PortfolioGrid.HighLightedRow == -1)
                ToolTip.Show(false);
            else
            {
                ToolTip.Margin = new Thickness(move.X + 20, move.Y + 20, 0, 0);
                ToolTip.Visibility = Visibility.Visible;
                ToolTip.Show(true);
            }

        }

        public override void onSectionReady()
        {
            PortfolioGrid.ApplyEffects();

            // load data
            if (App.BSL == null || Login.UserInfo == null)
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            try
            {
                List<HoldingDataModel> holdings = App.BSL.getHoldings(Login.UserInfo.profileID);
                Update.Text = "as of " + DateTime.Now;
                DataProvider data = new DataProvider();
                List<PieElement> graphData = rowsGraphData = new List<PieElement>();
                for (int i = 0; i < holdings.Count; i++)
                {
                    QuoteDataModel quote = App.BSL.getQuote(holdings[i].quoteID);

                    decimal marketValue = (decimal)holdings[i].quantity * quote.price;
                    decimal basis = (decimal)holdings[i].quantity * (decimal)holdings[i].purchasePrice;
                    decimal gain = marketValue - basis;

                    graphData.Add(new PieElement(quote.symbol, (double)marketValue));


                    RowDataProvider row = new RowDataProvider();
                    row.id = i;

                    // id, date, symbol, quantity, purchase price, current price, basis, market value, gain, trade
                    row.Add(new GridColumnData());
                    row[0].value = holdings[i].holdingID.ToString();
                    row[0].numeric = holdings[i].holdingID;

                    row.Add(new GridColumnData());
                    row[1].value = holdings[i].purchaseDate.ToString();
                    row[1].numeric = (int)(holdings[i].purchaseDate.Ticks / 10000);

                    row.Add(new GridColumnData());
                    row[2].value = holdings[i].quoteID;
                    row[2].numeric = i;

                    row.Add(new GridColumnData());
                    row[3].value = string.Format("{0:0,0}", holdings[i].quantity);
                    row[3].numeric = (int)holdings[i].quantity;

                    row.Add(new GridColumnData());
                    row[4].value = string.Format("{0:C}", holdings[i].purchasePrice);
                    row[4].numeric = (int)(holdings[i].purchasePrice * 100);

                    row.Add(new GridColumnData());
                    row[5].value = string.Format("{0:C}", quote.price);
                    row[5].numeric = (int)(quote.price * 100);

                    row.Add(new GridColumnData());
                    row[6].value = string.Format("{0:C}", basis);
                    row[6].numeric = (int)(basis * 100);

                    row.Add(new GridColumnData());
                    row[7].value = string.Format("{0:C}", marketValue);
                    row[7].numeric = (int)(marketValue * 100);

                    row.Add(new GridColumnGainData());
                    row[8].value = string.Format("{0:C}", gain);
                    row[8].numeric = (int)(gain * 100);
                    (row[8] as GridColumnGainData).increased = gain == 0 ? 0 : (gain > 0 ? 1 : -1);

                    row.Add(new GridColumnData());
                    row[9].value = "SELL";
                    row[9].numeric = i;

                    data.Add(row);
                }

                PortfolioGrid.setData(data);
                ToolTip.setData(graphData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StockTrader - Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            this.Visibility = Visibility.Visible;
        }

        public override void onSectionRemove()
        {
            PortfolioGrid.RemoveEffects();
        }
	}
}