
forward
global type  from nonvisualobject
end type
end forward

global type  from nonvisualobject
end type
global  

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public subroutine GetMessage( )
end prototypes

public subroutine GetMessage( );
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage()
end subroutine
on .create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=9412685B3F02C602E4814C0A26F8A004&steamids=76561197960435530", PBWebHttp.WebMessageFormat.Json!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=9412685B3F02C602E4814C0A26F8A004&steamids=76561197960435530"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Json!

end event


on .destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
