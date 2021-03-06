namespace
namespace LYC.Steam.Proxy.ISteamWebAPIUtil
end namespace
forward
global type p_ISteamWebAPIUtil_GetServerInfo from nonvisualobject
end type
end forward

global type p_ISteamWebAPIUtil_GetServerInfo from nonvisualobject
end type
global p_ISteamWebAPIUtil_GetServerInfo p_ISteamWebAPIUtil_GetServerInfo

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public function ISteamWebAPIUtil_GetServerInfo.response GetMessage( string urlArg1)
end prototypes

public function ISteamWebAPIUtil_GetServerInfo.response GetMessage( string urlArg1);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage(urlArg1)

    //Convert the message to PowerBuilder data
    System.Object results[]
    results = msg.ToPBData("ISteamWebAPIUtil_GetServerInfo.response")
    ISteamWebAPIUtil_GetServerInfo.response result
    result = results[1]
    return result

end function
on p_ISteamWebAPIUtil_GetServerInfo.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v0001/?format={aformat}", PBWebHttp.WebMessageFormat.Xml!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v0001/?format={aformat}"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Xml!

end event


on p_ISteamWebAPIUtil_GetServerInfo.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
