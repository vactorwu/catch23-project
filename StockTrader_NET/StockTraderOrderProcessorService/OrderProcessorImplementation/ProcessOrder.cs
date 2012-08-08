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
// A class that is used to process a submitted order within the StockTraderDB the database, with required 
// business logic.  
//======================================================================================================



using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using ConfigService.ServiceConfigurationUtility;
using Trade.IDAL;
using Trade.DALFactory;
using Trade.BusinessServiceDataContract;
using Trade.OrderProcessorContract;
using Trade.OrderProcessorServiceConfigurationSettings;
using Trade.Utility;

namespace Trade.OrderProcessorImplementation
{
    public class ProcessOrder 
    {
        private IOrder dalOrder;
         
        public void ProcessAndCompleteOrder(OrderDataModel order)
        {
            try
            {
                dalOrder = Trade.DALFactory.Order.Create(Settings.DAL);
                dalOrder.Open(Settings.TRADEDB_SQL_CONN_STRING);
                decimal total = 0;
                int holdingid = -1;
                QuoteDataModel quote = dalOrder.getQuoteForUpdate(order.symbol);
                //Get the latest trading price--this is the money going into (or out of) the users account.
                order.price = quote.price;

                //Calculate order total, and create/update the Holding. Whole holding 
                //sells delete the holding, partials sells update the holding with new amount
                //(and potentially the order too), and buys create a new holding.
                if (order.orderType == StockTraderUtility.ORDER_TYPE_BUY)
                {
                    holdingid = dalOrder.createHolding(order);
                    total = Convert.ToDecimal(order.quantity) * order.price + order.orderFee;
                }
                else
                    if (order.orderType == StockTraderUtility.ORDER_TYPE_SELL)
                    {
                        holdingid = sellHolding(order);
                        //We will assume here, since the holding cannot be found, it has already been sold,
                        //perhaps in another browser session.
                        if (holdingid == -1 )
                            return;
                        total = -1 * Convert.ToDecimal(order.quantity) * order.price + order.orderFee;
                    }

                //Debit/Credit User Account.  Note, if we did not want to allow unlimited margin
                //trading, we would change the ordering a bit and add the biz logic here to make
                //sure the user has enough money to actually buy the shares they asked for!

                //Now Update Account Balance.
                dalOrder.updateAccountBalance(order.accountID, total);

                //Update the stock trading volume and price in the quote table
                dalOrder.updateStockPriceVolume(order.quantity, quote);

                //Now perform our ACID tx test, if requested based on order type and acid stock symbols
                if (order.symbol.Equals(StockTraderUtility.ACID_TEST_BUY) && order.orderType == StockTraderUtility.ORDER_TYPE_BUY)
                    throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_ACID_BUY);
                else
                    if (order.symbol.Equals(StockTraderUtility.ACID_TEST_SELL) && order.orderType == StockTraderUtility.ORDER_TYPE_SELL)
                        throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_ACID_SELL);

                //Finally, close the order
                order.orderStatus = StockTraderUtility.ORDER_STATUS_CLOSED;
                order.completionDate = DateTime.Now;
                order.holdingID = holdingid;
                dalOrder.closeOrder(order);
                //Done!

                return;
            }
            catch 
            {
                throw;
            }
            finally
            {
                dalOrder.Close();
            }
        }

        int sellHolding(OrderDataModel order)
        {
            //Note, we need to make this call simply becuase holdingID is not a serialized
            //property on OrderDataBean; hence we cannot associate a sell order with a specific
            //holding without first searching for and retrieving the holding by order.OrderID.
            //A better approach would be to serialize the holdingID property on OrderDataModel,
            //then this extra query could be avoided.  However, we cannot here becuase WebSphere
            //Trade 6.1 will not interoperate from the JSP app to .NET backend services if we do.
            //Of course, Trade 6.1 could easily be updated to serialize this property on OrderDataBean,
            //and then no "compromise" would be needed.  But we want this to work with the existing IBM 
            //Trade 6.1 code as downloadable from IBM's site.
            HoldingDataModel holding = dalOrder.getHoldingForUpdate(order.orderID);
            if (holding == null)
                throw new Exception(StockTraderUtility.EXCEPTION_MESSAGE_INVALID_HOLDING_NOT_FOUND);
            order.accountID = holding.AccountID;

            //There are three distinct business cases here, each needs different treatment:  
            // a) Quantity requested is less than total shares in the holding -- update holding.  
            // b) Quantity requested is equal to total shares in the holding -- delete holding.  
            // c) Quantity requested is greater than total shares in the holding -- delete holding, update order table.  
            if (order.quantity < holding.quantity)
            {
                dalOrder.updateHolding(holding.holdingID, order.quantity);
            }
            else
                if (holding.quantity == order.quantity)
                {
                    dalOrder.deleteHolding(holding.holdingID);
                }
                else
                    //We now need to back-update the order record quantity to reflect
                    //fact not all shares originally requested were sold since the holding 
                    //had less shares in it, perhaps due to other orders 
                    //placed against that holding that completed before this one. So we will
                    //sell the remaining shares, but need to update the final order to reflect this.
                    if (order.quantity > holding.quantity)
                    {
                        dalOrder.deleteHolding(holding.holdingID);
                        order.quantity = holding.quantity;
                        order.accountID = holding.AccountID;
                        dalOrder.updateOrder(order);
                    }
            return holding.holdingID;
        }
    }
}

