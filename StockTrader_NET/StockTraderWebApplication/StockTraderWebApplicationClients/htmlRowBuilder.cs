//  Azure StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications

using System;
using System.Collections.Generic;
using System.Text;
using Trade.StockTraderWebApplicationModelClasses;

namespace Trade.StockTraderWebApplicationServiceClient
{
    /// <summary>
    /// Special class to construct a sorted list of holdings by stock symbol, pre-subtotaled and pre-formatted for ASPX repeater display.
    /// </summary>
    public class htmlRowBuilder
    {
        /// <summary>
        /// Logic to construct a sorted list of holdings by stock symbol, pre-subtotaled and pre-formatted for ASPX repeater display.
        /// </summary>
        /// <param name="holdingsUI">Holding data to format</param>
        public int buildPortfolioBySymbol(List<HoldingDataUI> holdingsUI)
        {
            if (holdingsUI == null || holdingsUI.Count < 1)
                return 0;
            string quoteSymbol = holdingsUI[0].quoteID;
            decimal quotePrice = holdingsUI[0].quotePriceDecimal;
            double subtotalquantity = 0;
            decimal subtotalmktvalue = 0;
            decimal subtotalbasis = 0;
            decimal subtotalgain = 0;
            int uniqueStockCount = 0;
            int count = holdingsUI.Count;
            int subtotaledlistcount = 0;
            for (int i = 0; i <= count; i++)
            {
                if (i == count)
                {
                    subtotaledlistcount--;
                }
                if (!quoteSymbol.Equals(holdingsUI[subtotaledlistcount].quoteID))
                {
                    uniqueStockCount += 1;
                    HoldingDataUI subtotalline = new HoldingDataUI(subtotalquantity, subtotalgain, subtotalmktvalue, subtotalbasis, quoteSymbol, quotePrice);
                    quoteSymbol = holdingsUI[subtotaledlistcount].quoteID;
                    quotePrice = holdingsUI[subtotaledlistcount].quotePriceDecimal;
                    subtotalgain = holdingsUI[subtotaledlistcount].gainDecimal;
                    subtotalquantity = holdingsUI[subtotaledlistcount].quantityDouble;
                    subtotalmktvalue = holdingsUI[subtotaledlistcount].marketValueDecimal;
                    subtotalbasis = holdingsUI[subtotaledlistcount].basisDecimal;
                    if (i != count)
                        holdingsUI[subtotaledlistcount].convertNumericsForDisplay(true);
                    else
                        subtotaledlistcount++;
                    holdingsUI.Insert(subtotaledlistcount++, subtotalline);
                    subtotaledlistcount++;
                }
                else
                {
                    subtotalgain += holdingsUI[subtotaledlistcount].gainDecimal;
                    subtotalquantity += Convert.ToDouble(holdingsUI[subtotaledlistcount].quantityDouble);
                    subtotalmktvalue += Convert.ToDecimal(holdingsUI[subtotaledlistcount].marketValueDecimal);
                    subtotalbasis += Convert.ToDecimal(holdingsUI[subtotaledlistcount].basisDecimal);
                    holdingsUI[subtotaledlistcount].convertNumericsForDisplay(true);
                    subtotaledlistcount++;
                }
            }
            return uniqueStockCount;  
        }
    }
}
