$PBExportHeader$fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices.sru
forward
global type fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices from nonvisualobject
end type
end forward

global type fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices from nonvisualobject
end type
global fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices

type variables
    // General public variables
    public PBWCF.WCFConnection   wcfConnectionObject

    // General private variables
    private integer   m_revision
    private Sybase.PowerBuilder.WCFRuntime.Service   m_service

end variables

forward prototypes
    public subroutine emptyMethodAction()
    public subroutine isOnline()
    public function fundtraderproxy.AccountDataBean login(string userID, string password)
    public function fundtraderproxy.OrderDataBean[] getOrders(string userID)
    public function fundtraderproxy.AccountDataBean getAccountData(string userID)
    public function fundtraderproxy.AccountProfileDataBean getAccountProfileData(string userID)
    public function fundtraderproxy.AccountProfileDataBean updateAccountProfile(fundtraderproxy.AccountProfileDataBean profileData)
    public subroutine logout(string userID)
    public function fundtraderproxy.OrderDataBean buy(string userID, string symbol, double quantity, long orderProcessingMode)
    public function fundtraderproxy.OrderDataBean sell(string userID, long holdingID, long orderProcessingMode)
    public function fundtraderproxy.HoldingDataBean[] getHoldings(string userID)
    public function fundtraderproxy.AccountDataBean register(string userID, string password, string fullname, string address, string email, string creditcard, decimal openBalance)
    public function fundtraderproxy.OrderDataBean[] getClosedOrders(string userID)
    public function fundtraderproxy.MarketSummaryDataBeanWS getMarketSummary()
    public function fundtraderproxy.QuoteDataBean getQuote(string symbol)
    public function fundtraderproxy.HoldingDataBean getHolding(string userID, long holdingID)
    public function fundtraderproxy.OrderDataBean[] getTopOrders(string userID)
    public function fundtraderproxy.OrderDataBean sellEnhanced(string userID, long holdingID, double quantity)
end prototypes

public subroutine emptyMethodAction();
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("emptyMethodAction")

    // Pass arguments
    m_service.removeAllArguments()

    // Invoke the service
    m_service.Invoke()

    // Get return value
    return 
end subroutine


public subroutine isOnline();
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("isOnline")

    // Pass arguments
    m_service.removeAllArguments()

    // Invoke the service
    m_service.Invoke()

    // Get return value
    return 
end subroutine


public function fundtraderproxy.AccountDataBean login(string userID, string password);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("login")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(password, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.AccountDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.AccountDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.OrderDataBean[] getOrders(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getOrders")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean[]")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.AccountDataBean getAccountData(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getAccountData")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.AccountDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.AccountDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.AccountProfileDataBean getAccountProfileData(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getAccountProfileData")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.AccountProfileDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.AccountProfileDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.AccountProfileDataBean updateAccountProfile(fundtraderproxy.AccountProfileDataBean profileData);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("updateAccountProfile")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(profileData, "fundtraderproxy.AccountProfileDataBean", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.AccountProfileDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.AccountProfileDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public subroutine logout(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("logout")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Invoke the service
    m_service.Invoke()

    // Get return value
    return 
end subroutine


public function fundtraderproxy.OrderDataBean buy(string userID, string symbol, double quantity, long orderProcessingMode);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("buy")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(symbol, "System.String", false, false)
    m_service.addArgument(quantity, "System.Double", false, false)
    m_service.addArgument(orderProcessingMode, "System.Int32", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.OrderDataBean sell(string userID, long holdingID, long orderProcessingMode);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("sell")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(holdingID, "System.Int32", false, false)
    m_service.addArgument(orderProcessingMode, "System.Int32", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.HoldingDataBean[] getHoldings(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getHoldings")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.HoldingDataBean[]")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.HoldingDataBean svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.AccountDataBean register(string userID, string password, string fullname, string address, string email, string creditcard, decimal openBalance);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("register")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(password, "System.String", false, false)
    m_service.addArgument(fullname, "System.String", false, false)
    m_service.addArgument(address, "System.String", false, false)
    m_service.addArgument(email, "System.String", false, false)
    m_service.addArgument(creditcard, "System.String", false, false)
    m_service.addArgument(openBalance, "System.Decimal", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.AccountDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.AccountDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.OrderDataBean[] getClosedOrders(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getClosedOrders")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean[]")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.MarketSummaryDataBeanWS getMarketSummary();
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getMarketSummary")

    // Pass arguments
    m_service.removeAllArguments()

    // Set return type
    m_service.setReturnType("fundtraderproxy.MarketSummaryDataBeanWS")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.MarketSummaryDataBeanWS svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.QuoteDataBean getQuote(string symbol);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getQuote")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(symbol, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.QuoteDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.QuoteDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.HoldingDataBean getHolding(string userID, long holdingID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getHolding")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(holdingID, "System.Int32", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.HoldingDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.HoldingDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.OrderDataBean[] getTopOrders(string userID);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("getTopOrders")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean[]")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function fundtraderproxy.OrderDataBean sellEnhanced(string userID, long holdingID, double quantity);
    // Set Coonection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set method
    m_service.setMethodName("sellEnhanced")

    // Pass arguments
    m_service.removeAllArguments()
    m_service.addArgument(userID, "System.String", false, false)
    m_service.addArgument(holdingID, "System.Int32", false, false)
    m_service.addArgument(quantity, "System.Double", false, false)

    // Set return type
    m_service.setReturnType("fundtraderproxy.OrderDataBean")

    // Invoke the service
    m_service.Invoke()

    // Get return value
    fundtraderproxy.OrderDataBean svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


on fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create Sybase.PowerBuilder.WCFRuntime.Service

    // Revision Number
    m_revision = 1
    m_service.setRevision(m_revision)
    // Assembly name
    m_service.setAssemblyName("fundtraderproxy.dll")
    // Class name
    m_service.setClassName("fundtraderproxy.TradeServicesClient")
    // Prefix
    m_service.setPrefix("")

    wcfConnectionObject = create PBWCF.WCFConnection

    // EndpointAddress
    PBWCF.WCFEndpointAddress d_endpoint
    d_endpoint = create PBWCF.WCFEndpointAddress
    d_endpoint.URL = "http://kesser/TradeServiceWcf/TradeServiceWcf.svc"
    wcfConnectionObject.EndpointAddress = d_endpoint

    // Timeout
    wcfConnectionObject.Timeout = "00:10:00"
    // BindingType
    wcfConnectionObject.BindingType = PBWCF.WCFBindingType.BasicHttpBinding!
    // Binding
    PBWCF.WCFBasicHttpBinding d_binding
    d_binding= Create PBWCF.WCFBasicHttpBinding

    d_binding.TransferMode = PBWCF.WSTransferMode.BUFFERED!
    d_binding.MessageEncoding = PBWCF.WSMessageEncoding.TEXT!
    d_binding.TextEncoding = PBWCF.WSTextEncoding.UTF8!
    d_binding.MaxBufferPoolSize = 524288
    d_binding.MaxBufferSize = 65536
    d_binding.MaxReceivedMessageSize = 65536
    d_binding.AllowCookies = false
    d_binding.BypassProxyOnLocal = false
    d_binding.HostNameComparisonMode = PBWCF.WCFHostNameComparisonMode.STRONGWILDCARD!
    // ReaderQuotas
    d_binding.ReaderQuotas = Create PBWCF.WCFReaderQuotas
    d_binding.ReaderQuotas.MaxArrayLength = 16384
    d_binding.ReaderQuotas.MaxBytesPerRead = 4096
    d_binding.ReaderQuotas.MaxDepth = 32
    d_binding.ReaderQuotas.MaxNameTableCharCount = 16384
    d_binding.ReaderQuotas.MaxStringContentLength = 8192
    // Security
    d_binding.Security = Create PBWCF.BasicHttpSecurity
    d_binding.Security.SecurityMode = PBWCF.BasicHttpSecurityMode.NONE!

    wcfConnectionObject.BasicHttpBinding = d_binding

    // Proxy Security
    wcfConnectionObject.ProxyServer.CredentialType = PBWCF.HttpProxyCredentialType.NONE!
    wcfConnectionObject.ProxyServer.UseDefaultWebProxy = TRUE

    m_service.setConnectionOption(wcfConnectionObject)
end event

on fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy wcfConnectionObject
    destroy m_service
end event
