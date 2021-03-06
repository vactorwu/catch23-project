namespace
namespace LYC.Steam.Proxy.ISteamOAuth2
end namespace
forward
global type p_ISteamOAuth2_GetCaptchaGID from nonvisualobject
end type
end forward

global type p_ISteamOAuth2_GetCaptchaGID from nonvisualobject
end type
global p_ISteamOAuth2_GetCaptchaGID p_ISteamOAuth2_GetCaptchaGID

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public subroutine PostMessage( string urlArg1)
end prototypes

public subroutine PostMessage( string urlArg1);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg

    string urlargs[]
    urlargs = create string[1]
    urlargs[1] = urlArg1
    //Invocation
    msg = m_service.PostMessage(urlargs)
end subroutine
on p_ISteamOAuth2_GetCaptchaGID.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.steampowered.com/ISteamOAuth2/getCaptchaGID/v0001/?client_id={aclient_id}", PBWebHttp.WebMessageFormat.Xml!, PBWebHttp.WebMessageFormat.Xml!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.steampowered.com/ISteamOAuth2/getCaptchaGID/v0001/?client_id={aclient_id}"
    restConnectionObject.RequestMessageFormat = PBWebHttp.WebMessageFormat.Xml!
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Xml!

end event


on p_ISteamOAuth2_GetCaptchaGID.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
