//  .NET StockTrader Sample WCF Application for Benchmarking, Performance Analysis and Design Considerations for Service-Oriented Applications
//                   4/10/2011: Updated to version 5.0, with notable enhancements for optional Windows Azure hosting, cross-browser and mobile-browser compatibility, and 
//                   new performance ehancements  See: 
//                                  1. Technical overview paper: http://download.microsoft.com/download/7/C/9/7C9F7B89-8AF0-4433-AB3A-B615C8EF9484/Trade5Overview.pdf
//                                  2. MSDN Site with downloads, additional information: http://msdn.microsoft.com/stocktrader
//                                  3. Discussion Forum: http://social.msdn.microsoft.com/Forums/en-US/dotnetstocktradersampleapplication
//                                  4. Live on Windows Azure: https://azurestocktrader.cloudapp.net
//                                   
//
//  Configuration Service 5 Notes:
//                      The application implements Configuration Service 5.0, for which the source code also ships in the sample. However, the .NET StockTrader 5
//                      sample is a general-purpose performance sample application for Windows Server and Windows Azure even if you are not implementing the Configuration Service. 
//                      
//

using System;
using System.Collections.Generic;
using System.Text;
using Trade.StockTraderWebApplicationSettings;

namespace Trade.StockTraderWebApplicationModelClasses
{
    /// <summary>
    /// Model class for displaying special/augmented order data in a web page.
    /// </summary>
    public sealed class TotalOrdersUI
    {
        private List<OrderDataUI> _orders;
        private decimal _subtotalSell;
        private decimal _subtotalBuy;
        private decimal _txnFeesSubtotal;
        
        public TotalOrdersUI()
        {

        }

        public TotalOrdersUI(List<OrderDataUI>orders, decimal subtotalSell,decimal subtotalBuy, decimal txnFeesSubtotal)
        {
            _orders = orders;
            _subtotalSell = subtotalSell;
            _subtotalBuy = subtotalBuy;
            _txnFeesSubtotal = txnFeesSubtotal;            
        }

        public List<OrderDataUI> orders
        {
            get
            {
                return _orders;
            }
        }

        public decimal subtotalSell
        {
            get
            {
                return _subtotalSell;
            }
        }

        public decimal subtotalBuy
        {
            get
            {
                return _subtotalBuy;
            }
        }

        public decimal txnFeesSubtotal
        {
            get
            {
                return _txnFeesSubtotal;
            }
        }

        public string netImpactCashBalance
        {
            get
            {
                if ((this._subtotalSell - this._subtotalBuy - this._txnFeesSubtotal) > 0)
                    return String.Format("<div style=\"color:palegreen;\">{0:C}" + Settings.UPARROWLINK, (this._subtotalSell - this._subtotalBuy - this._txnFeesSubtotal));
                else
                    if ((this._subtotalSell - this._subtotalBuy - this._txnFeesSubtotal) < 0)
                        return String.Format("<div style=\"color:#A40707;\">{0:C}" + Settings.DOWNARROWLINK, (this._subtotalSell - this._subtotalBuy - this._txnFeesSubtotal));
                    else
                        return String.Format("{0:C}", (this._subtotalSell - this._subtotalBuy - this._txnFeesSubtotal));
            }
        }
    }
}