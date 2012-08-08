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

//======================================================================================================
// An interface implemented by the DAL.
//======================================================================================================


using System;
using System.Collections;
using System.Data.SqlClient;
using Trade.BusinessServiceDataContract;

namespace Trade.IDAL
{
    public interface IOrder
    {
        void BeginADOTransaction();
        void RollBackTransaction();
        void CommitADOTransaction();
        void Open(string connString);
        void Close();
        OrderDataModel createOrder(string userID, string symbol, string orderType, double quantity, int holdingID );
        int createHolding(OrderDataModel order);
        void updateHolding(int holdingid, double quantity);
        void updateOrder(OrderDataModel order);
        void deleteHolding(int holdingid);
        QuoteDataModel getQuoteForUpdate(string symbol);
        void closeOrder(OrderDataModel order);
        void updateAccountBalance(int accountID, decimal total);
        void updateStockPriceVolume(double quantity, QuoteDataModel quote);
        HoldingDataModel getHoldingForUpdate(int orderID);
        HoldingDataModel getHolding(int holdingID);
        void getSQLContextInfo();
    }
}