namespace
namespace GeoNameService
end namespace
forward
global type getplayersummery from nonvisualobject
end type
end forward

global type getplayersummery from nonvisualobject
end type
global getplayersummery getplayersummery

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public subroutine GetMessage( string urlArg1, string urlArg2)
end prototypes

public subroutine GetMessage( string urlArg1, string urlArg2);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage(urlArg1, urlArg2)
end subroutine
on getplayersummery.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={akey}&steamids={asteamid}", PBWebHttp.WebMessageFormat.Json!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={akey}&steamids={asteamid}"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Json!

end event


on getplayersummery.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
