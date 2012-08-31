namespace
namespace LYC.Steam.Proxy.ISteamEconomy
end namespace
forward
global type p_ISteamEconomy_GetAssetPrices from nonvisualobject
end type
end forward

global type p_ISteamEconomy_GetAssetPrices from nonvisualobject
end type
global p_ISteamEconomy_GetAssetPrices p_ISteamEconomy_GetAssetPrices

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public function ISteamEconomy_GetAssetPrices.result GetMessage( string urlArg1, string urlArg2, string urlArg3)
end prototypes

public function ISteamEconomy_GetAssetPrices.result GetMessage( string urlArg1, string urlArg2, string urlArg3);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage(urlArg1, urlArg2, urlArg3)

    //Convert the message to PowerBuilder data
    System.Object results[]
    results = msg.ToPBData("ISteamEconomy_GetAssetPrices.result")
    ISteamEconomy_GetAssetPrices.result result
    result = results[1]
    return result

end function
on p_ISteamEconomy_GetAssetPrices.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamEconomy/GetAssetPrices/v0001/?format={aformat}&appid={aappid}&key={akey}", PBWebHttp.WebMessageFormat.Xml!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamEconomy/GetAssetPrices/v0001/?format={aformat}&appid={aappid}&key={akey}"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Xml!

end event


on p_ISteamEconomy_GetAssetPrices.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
