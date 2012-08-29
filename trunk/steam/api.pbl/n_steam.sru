$PBExportHeader$n_steam.sru
namespace
using LYC.Steam.Proxy.ISteamWebAPIUtil
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

on n_steam.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_steam.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
