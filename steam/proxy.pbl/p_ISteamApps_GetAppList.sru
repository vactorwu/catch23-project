$PBExportHeader$p_ISteamApps_GetAppList.sru
namespace
namespace LYC.Steam.Proxy.ISteamApps
end namespace

forward
global type p_ISteamApps_GetAppList from NonVisualObject
end type
end forward

global type p_ISteamApps_GetAppList from NonVisualObject
end type
global p_ISteamApps_GetAppList p_ISteamApps_GetAppList

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public function ISteamApps_GetAppList.applist GetMessage (string urlArg1)
end prototypes

public function ISteamApps_GetAppList.applist GetMessage (string urlArg1);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage(urlArg1)

    //Convert the message to PowerBuilder data
    System.Object results[]
    results = msg.ToPBData("ISteamApps_GetAppList.applist")
    ISteamApps_GetAppList.applist result
    result = results[1]
    return result
end function

on p_ISteamApps_GetAppList.create
call super::create
TriggerEvent( this, "constructor" )
end on

on p_ISteamApps_GetAppList.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format={aformat}", PBWebHttp.WebMessageFormat.Xml!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamApps/GetAppList/v0002/?format={aformat}"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Xml!
	restConnectionObject.MaxMessageLength = 65535 * 65535 * 65535
end event

event destructor;
    destroy m_service
end event
