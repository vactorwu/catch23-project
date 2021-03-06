$PBExportHeader$n_steam.sru
namespace
using LYC.Steam.Proxy.ISteamWebAPIUtil
using System.Net
using Microsoft.Win32
using System.IO
using System.Text
using LYC.Steam.Proxy.ISteamOAuth2
using LYC.Steam.Proxy.ISteamApps
end namespace

forward
global type n_steam from NonVisualObject
end type
end forward

global type n_steam from NonVisualObject
end type
global n_steam n_steam

type variables
private:
 String accessToken;
 String umqid;
 String steamid;
 int message = 0;
 
 String APIKEY = "9412685B3F02C602E4814C0A26F8A004"
 String RESPONSE_FORMAT = 'XML'
end variables

forward prototypes
public function ISteamWebAPIUtil_GetServerInfo.response GetServerInfo ()
public function ISteamWebAPIUtil_GetSupportedAPIList.apilist GetSupportedAPIList ()
public function string RequestGet (string as_get)
public function string RequestPost (string as_post)
public subroutine GetCaptchaGID ()
public function ISteamApps_GetAppList.applist GetAppList ()
end prototypes

public function ISteamWebAPIUtil_GetServerInfo.response GetServerInfo ();ISteamWebAPIUtil_GetServerInfo.response l_response

p_ISteamWebAPIUtil_GetServerInfo  l_GetServerInfo
l_GetServerInfo = create p_ISteamWebAPIUtil_GetServerInfo

try
	l_response = l_GetServerInfo.GetMessage(RESPONSE_FORMAT)
catch(System.Exception ex)
end try

return l_response
end function

public function ISteamWebAPIUtil_GetSupportedAPIList.apilist GetSupportedAPIList ();ISteamWebAPIUtil_GetSupportedAPIList.apilist l_apilist

p_ISteamWebAPIUtil_GetSupportedAPIList  l_GetSupportedAPIList
l_GetSupportedAPIList = create p_ISteamWebAPIUtil_GetSupportedAPIList

try
	l_apilist = l_GetSupportedAPIList.GetMessage(RESPONSE_FORMAT,APIKEY)
catch(System.Exception ex)
end try

return l_apilist
end function

public function string RequestGet (string as_get); //system.net.servicepointmanager.expect100continue = false;
 //httpwebrequest request = webrequest.create( "http://63.228.223.110/" + as_get );
    //request.host = "api.steampowered.com";
    //request.protocolversion = httpversion.version11;
	//request.accept = "*/*";
	//request.headers[httprequestheader.acceptencoding] = "gzip, deflate";
	//request.headers[httprequestheader.acceptlanguage] = "zh-cn";
	//request.useragent = "mozilla/5.0 (compatible; msie 9.0; windows nt 6.1; wow64; trident/5.0)";
	//
	//try
		//httpwebresponse response = request.getresponse();
		//if(integer(response.statuscode) <> 200) then return null
		//streamreader srd = create streamreader(response.getresponsestream())
		//string src = srd.readtoend()
		//return src
	//catch(webexception ex)
		//return null
	//end try
		//return src
		
		return "s"
end function

public function string RequestPost (string as_post);//System.Net.ServicePointManager.Expect100Continue = false;
//
 //HttpWebRequest request = WebRequest.Create( "http://63.228.223.110/" + as_get )
    //request.Host = "api.steampowered.com";
    //request.ProtocolVersion = HttpVersion.Version11;
	//request.Accept = "*/*";
	//request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
	//request.Headers[HttpRequestHeader.AcceptLanguage] = "zh-CN";
	//request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
	//request.Method = "POST"
	//
	//byte postBytes[] = System.Text.Encoding.ASCII.GetBytes(as_post)
	//request.ContentType = "application/x-www-form-urlencoded";
    //request.ContentLength = postBytes.Length;
	//
	//Stream srd = request.GetRequestStream()
	//srd.Write(postBytes,0,postBytes.Length)
	//srd.Close();
//
	//try
		//HttpWebResponse response = request.GetResponse();
		//if(integer(response.StatusCode) <> 200) then return null
		//StreamReader srd = create StreamReader(response.GetResponseStream())
		//string src = srd.ReadToEnd()
		//return src
		//
	//catch(WebException ex)
		//return null
		//
	//end try
		//return src
		
		return "s"
end function

public subroutine GetCaptchaGID ();p_ISteamOAuth2_GetCaptchaGID l_ISteamOAuth2_GetCaptchaGID
l_ISteamOAuth2_GetCaptchaGID = create p_ISteamOAuth2_GetCaptchaGID

try
	l_ISteamOAuth2_GetCaptchaGID.PostMessage(APIKEY)
catch(System.Exception ex)
end try
end subroutine

public function ISteamApps_GetAppList.applist GetAppList ();ISteamApps_GetAppList.applist l_applist

p_ISteamApps_GetAppList l_GetAppList
l_GetAppList = create p_ISteamApps_GetAppList

try
	l_applist = l_GetAppList.GetMessage(RESPONSE_FORMAT)	
catch(System.Exception ex)
end try
return l_applist
end function

on n_steam.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_steam.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
