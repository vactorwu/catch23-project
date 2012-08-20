$PBExportHeader$weather_proxy_WeatherWebServiceSoapClient.sru
forward
global type weather_proxy_WeatherWebServiceSoapClient from nonvisualobject
end type
end forward

global type weather_proxy_WeatherWebServiceSoapClient from nonvisualobject
end type
global weather_proxy_WeatherWebServiceSoapClient weather_proxy_WeatherWebServiceSoapClient

type variables
    // General public variables
    public PBWCF.WCFConnection   wcfConnectionObject

    // General private variables
    private integer   m_revision
    private Sybase.PowerBuilder.WCFRuntime.Service   m_service

end variables

forward prototypes
    public function string[] getSupportCity(string byProvinceName)
    public function string[] getSupportProvince()
    public function System.Data.DataSet getSupportDataSet()
    public function string[] getWeatherbyCityName(string theCityName)
    public function string[] getWeatherbyCityNamePro(string theCityName, string theUserID)
end prototypes

public function string[] getSupportCity(string byProvinceName);
    // Set Connection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set Method
    m_service.setMethodName("getSupportCity")

    // Pass Arguments
    m_service.removeAllArguments()
    m_service.addArgument(byProvinceName, "System.String", false, false)

    // Set Return Type
    m_service.setReturnType("string[]")

    // Invoke the Service
    m_service.Invoke()

    // Get Return Value
    string svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function string[] getSupportProvince();
    // Set Connection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set Method
    m_service.setMethodName("getSupportProvince")

    // Pass Arguments
    m_service.removeAllArguments()

    // Set Return Type
    m_service.setReturnType("string[]")

    // Invoke the Service
    m_service.Invoke()

    // Get Return Value
    string svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function System.Data.DataSet getSupportDataSet();
    // Set Connection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set Method
    m_service.setMethodName("getSupportDataSet")

    // Pass Arguments
    m_service.removeAllArguments()

    // Set Return Type
    m_service.setReturnType("System.Data.DataSet")

    // Invoke the Service
    m_service.Invoke()

    // Get Return Value
    System.Data.DataSet svcReturnValue
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function string[] getWeatherbyCityName(string theCityName);
    // Set Connection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set Method
    m_service.setMethodName("getWeatherbyCityName")

    // Pass Arguments
    m_service.removeAllArguments()
    m_service.addArgument(theCityName, "System.String", false, false)

    // Set Return Type
    m_service.setReturnType("string[]")

    // Invoke the Service
    m_service.Invoke()

    // Get Return Value
    string svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


public function string[] getWeatherbyCityNamePro(string theCityName, string theUserID);
    // Set Connection Object
    m_service.setConnectionOption(wcfConnectionObject)

    // Set Method
    m_service.setMethodName("getWeatherbyCityNamePro")

    // Pass Arguments
    m_service.removeAllArguments()
    m_service.addArgument(theCityName, "System.String", false, false)
    m_service.addArgument(theUserID, "System.String", false, false)

    // Set Return Type
    m_service.setReturnType("string[]")

    // Invoke the Service
    m_service.Invoke()

    // Get Return Value
    string svcReturnValue[]
    svcReturnValue = m_service.getReturnValue()
    return svcReturnValue
end function


on weather_proxy_WeatherWebServiceSoapClient.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create Sybase.PowerBuilder.WCFRuntime.Service

    // Revision Number
    m_revision = 1
    m_service.setRevision(m_revision)
    // Assembly name
    m_service.setAssemblyName("weather_proxy.dll")
    // Class name
    m_service.setClassName("weather_proxy.WeatherWebServiceSoapClient")
    // Prefix
    m_service.setPrefix("")

    wcfConnectionObject = create PBWCF.WCFConnection

    // EndpointAddress
    PBWCF.WCFEndpointAddress d_endpoint
    d_endpoint = create PBWCF.WCFEndpointAddress
    d_endpoint.URL = "http://www.webxml.com.cn/WebServices/WeatherWebService.asmx"
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

on weather_proxy_WeatherWebServiceSoapClient.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy wcfConnectionObject
    destroy m_service
end event
