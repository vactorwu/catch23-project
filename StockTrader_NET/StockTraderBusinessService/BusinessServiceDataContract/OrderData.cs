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
// The OrderDataModel class, part of the DataContract for the StockTrader Business Services Layer.
//======================================================================================================


using System;
using System.Collections;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Trade.BusinessServiceDataContract
{
    /// <summary>
    /// This class is part of the WCF Data Contract for StockTrader Business Services.
    /// It defines the class used as the data model for order information. 
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://trade.samples.websphere.ibm.com", TypeName="OrderDataBean")]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://trade.samples.websphere.ibm.com",Name="OrderDataBean")]
    public sealed class OrderDataModel
	{
        private int _orderID;
        private string _orderType;
        private string _orderStatus;
        private DateTime _openDate;
        private DateTime _completionDate;
        private double _quantity;
        private decimal _price;
        private decimal _orderFee;
        private int _accountid;
        private int _holdingID;
        private string _symbol;

        public OrderDataModel()
        {
        }

        public OrderDataModel(int orderID, string orderType, string orderStatus, DateTime openDate, DateTime completionDate, double quantity, decimal price, decimal orderFee, string symbol)
        {
            this._orderID = orderID;
            this._orderType = orderType;
            this._orderStatus = orderStatus;
            this._openDate = openDate;
            this._completionDate = completionDate;
            this._quantity = quantity;
            this._price = price;
            this._orderFee = orderFee;
            this._symbol = symbol;
        }

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "orderID", Order = 1, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "orderID", Order = 1)]
		public int orderID
		{
			get
			{
				return _orderID;
			}
			
			set
			{
				this._orderID = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "orderType", Order = 2, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "orderType", Order = 2)]
		public string orderType
		{
			get
			{
				return _orderType;
			}
			
			set
			{
				this._orderType = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "orderStatus", Order = 3, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "orderStatus", Order = 3)]
		public string orderStatus
		{
			get
			{
				return _orderStatus;
			}
			
			set
			{
				this._orderStatus = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "openDate", Order = 4, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "openDate", Order = 4)]
		public DateTime openDate
		{
			get
			{
				return _openDate;
			}
			
			set
			{
				this._openDate = value;
			}
			
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "completionDate", Order = 5, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "completionDate", Order = 5)]
        public DateTime completionDate
		{
			get
			{
				return _completionDate;
			}
			
			set
			{
				this._completionDate = (DateTime) value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "quantity", Order = 6, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "quantity", Order = 6)]
		public double quantity
		{
			get
			{
				return _quantity;
			}
			
			set
			{
				this._quantity = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "price", Order = 7, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "price", Order = 7)]
		public decimal price
		{
			get
			{
				return _price;
			}
			
			set
			{
				this._price = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "orderFee", Order = 8, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "orderFee", Order = 8)]
		public decimal orderFee
		{
			get
			{
				return _orderFee;
			}
			
			set
			{
				this._orderFee = value;
			}
		}

        [System.Xml.Serialization.XmlElementAttribute(ElementName = "symbol", Order = 9, IsNullable = false)]
        [DataMember(IsRequired = false, Name = "symbol", Order = 9)]
		public string symbol
		{
			get
			{
				return _symbol;
			}
			
			set
			{
				this._symbol = value;
			}
		}
        [System.Xml.Serialization.XmlIgnore]
        public int accountID
        {
            get
            {
                return _accountid;
            }

            set
            {
                this._accountid = value;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public int holdingID
        {
            get
            {
                return _holdingID;
            }

            set
            {
                this._holdingID = value;
            }
        }
	}
}