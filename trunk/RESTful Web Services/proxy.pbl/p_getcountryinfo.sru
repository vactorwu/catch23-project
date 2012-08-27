namespace
namespace GeoNameService
end namespace
forward
global type p_getcountryinfo from nonvisualobject
end type
end forward

global type p_getcountryinfo from nonvisualobject
end type
global p_getcountryinfo p_getcountryinfo

type variables
    //Instance of REST Service
    private PBWebHttp.RestService m_service
    //Instance of  Connection Object
    public PBWebHttp.WebConnection restConnectionObject
end variables

forward prototypes
public function getcountryinfo.geonamesCountry GetMessage( string urlArg1, string urlArg2)
end prototypes

public function getcountryinfo.geonamesCountry GetMessage( string urlArg1, string urlArg2);
    // Update Connection Object
    m_service.ConnectionObject = restConnectionObject

    PBWebHttp.WebMessage msg
    //Invocation
    msg = m_service.GetMessage(urlArg1, urlArg2)

    //Convert the message to PowerBuilder data
    System.Object results[]
    results = msg.ToPBData("getcountryinfo.geonamesCountry")
    getcountryinfo.geonamesCountry result
    result = results[1]
    return result

end function
on p_getcountryinfo.create
call super::create
TriggerEvent( this, "constructor" )
end on

event constructor;
    m_service = create PBWebHttp.RestService("http://api.geonames.org/countryInfo?lang={plang}&country={pcountry}&username=demo&style=full", PBWebHttp.WebMessageFormat.Xml!)
    // Connection Object
    restConnectionObject = create PBWebHttp.WebConnection
    restConnectionObject.Endpoint = "http://api.geonames.org/countryInfo?lang={plang}&country={pcountry}&username=demo&style=full"
    restConnectionObject.ResponseMessageFormat = PBWebHttp.WebMessageFormat.Xml!

end event


on p_getcountryinfo.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event destructor;
    destroy m_service
end event
