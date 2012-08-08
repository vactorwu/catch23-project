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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Trade.StockTraderWebApplicationSettings;

namespace Trade.StockTraderWebApplicationModelClasses
{
    /// <summary>
    /// Model class for displaying market summary data in a web page.
    /// </summary>
   	public sealed class MarketSummaryDataUI
	{
        private decimal _tsia;
        private decimal _openTSIA;
        private double _volume;
        private List<QuoteDataUI> _topGainers;
        private List<QuoteDataUI> _topLosers;
        private DateTime _summaryDate;
        
        public MarketSummaryDataUI()
        {
        }

        public MarketSummaryDataUI(decimal tSIA, decimal openTSIA, double volume, List<QuoteDataUI> topGainers, List<QuoteDataUI> topLosers)
        {
            this._tsia = tSIA;
            this._openTSIA = openTSIA;
            this._volume = volume;
            this._topGainers = topGainers;
            this._topLosers = topLosers;
            this._summaryDate = DateTime.Now;
        }

        public MarketSummaryDataUI(decimal tSIA, decimal openTSIA, double volume, List<QuoteDataUI> topGainers, List<QuoteDataUI> topLosers, DateTime summaryDate )
        {
            this._tsia = tSIA;
            this._openTSIA = openTSIA;
            this._volume = volume;
            this._topGainers = topGainers;
            this._topLosers = topLosers;
            this._summaryDate = DateTime.Now;
        }

        public decimal gainPercent
        {
            get
            {
                return (_tsia - _openTSIA) / openTSIA * 100;
            }

        }

        public decimal TSIA
        {
            get
            {
                return _tsia;
            }

            set
            {
                this._tsia = value;
            }

        }
        
        public decimal openTSIA
		{
			get
			{
				return _openTSIA;
			}
			
			set
			{
				this._openTSIA = value;
			}
			
		}
        
		public double volume
		{
			get
			{
				return _volume;
			}
			
			set
			{
				this._volume = value;
			}
			
		}
        
		public List<QuoteDataUI> topGainers
		{
			get
			{
				return _topGainers;
			}
			
			set
			{
				this._topGainers = value;
			}
			
		}
        
		public List<QuoteDataUI> topLosers
		{
			get
			{
				return _topLosers;
			}
			
			set
			{
				this._topLosers = value;
			}
			
		}
      
		public DateTime summaryDate
		{
			get
			{
				return _summaryDate;
			}
			
			set
			{
				this._summaryDate = value;
			}
		}
    }
}