[Sybase.PowerBuilder.PBGroupAttribute("pbglobaldefinitions_steam", Sybase.PowerBuilder.PBGroupType.Unknown,"","", null,"steam")]
[Sybase.PowerBuilder.PBTargetAttribute("e:\\svn\\steam\\steam.pbtx", "c__steam")]
[System.Diagnostics.DebuggerStepThrough]
public class PBGlobalDefinitions_steam
{
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "w_main", null, "w_main", "lyc.steam.gui", "e:\\svn\\steam\\gui.pbl\\gui.pblx",null, null, "w_main")]
	public lyc.steam.gui.c__w_main lyc_steam_gui_w_main = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamapps_getapplist", null, "p_ISteamApps_GetAppList", "lyc.steam.proxy.isteamapps", "e:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamapps_getapplist")]
	[Sybase.PowerBuilder.PBVarDeclNameAttribute("lyc.steam.proxy.isteamapps.p_isteamapps_getapplist","p_isteamapps_getapplist")]
	public lyc.steam.proxy.isteamapps.c__p_isteamapps_getapplist lyc_steam_proxy_isteamapps_p_isteamapps_getapplist = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteameconomy_getassetprices", null, "p_ISteamEconomy_GetAssetPrices", "lyc.steam.proxy.isteameconomy", "e:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteameconomy_getassetprices")]
	[Sybase.PowerBuilder.PBVarDeclNameAttribute("lyc.steam.proxy.isteameconomy.p_isteameconomy_getassetprices","p_isteameconomy_getassetprices")]
	public lyc.steam.proxy.isteameconomy.c__p_isteameconomy_getassetprices lyc_steam_proxy_isteameconomy_p_isteameconomy_getassetprices = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamoauth2_getcaptchagid", null, "p_ISteamOAuth2_GetCaptchaGID", "lyc.steam.proxy.isteamoauth2", "e:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamoauth2_getcaptchagid")]
	[Sybase.PowerBuilder.PBVarDeclNameAttribute("lyc.steam.proxy.isteamoauth2.p_isteamoauth2_getcaptchagid","p_isteamoauth2_getcaptchagid")]
	public lyc.steam.proxy.isteamoauth2.c__p_isteamoauth2_getcaptchagid lyc_steam_proxy_isteamoauth2_p_isteamoauth2_getcaptchagid = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamwebapiutil_getserverinfo", null, "p_ISteamWebAPIUtil_GetServerInfo", "lyc.steam.proxy.isteamwebapiutil", "e:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamwebapiutil_getserverinfo")]
	[Sybase.PowerBuilder.PBVarDeclNameAttribute("lyc.steam.proxy.isteamwebapiutil.p_isteamwebapiutil_getserverinfo","p_isteamwebapiutil_getserverinfo")]
	public lyc.steam.proxy.isteamwebapiutil.c__p_isteamwebapiutil_getserverinfo lyc_steam_proxy_isteamwebapiutil_p_isteamwebapiutil_getserverinfo = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamwebapiutil_getsupportedapilist", null, "p_ISteamWebAPIUtil_GetSupportedAPIList", "lyc.steam.proxy.isteamwebapiutil", "e:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamwebapiutil_getsupportedapilist")]
	[Sybase.PowerBuilder.PBVarDeclNameAttribute("lyc.steam.proxy.isteamwebapiutil.p_isteamwebapiutil_getsupportedapilist","p_isteamwebapiutil_getsupportedapilist")]
	public lyc.steam.proxy.isteamwebapiutil.c__p_isteamwebapiutil_getsupportedapilist lyc_steam_proxy_isteamwebapiutil_p_isteamwebapiutil_getsupportedapilist = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "n_steam", null, "n_steam", "", "e:\\svn\\steam\\api.pbl\\api.pblx",null, null, "n_steam")]
	public c__n_steam n_steam = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "n_logger", null, "n_logger", "", "e:\\svn\\steam\\util.pbl\\util.pblx",null, null, "n_logger")]
	public c__n_logger n_logger = null;

	public static PBGlobalDefinitions_steam _instance = null;

	public static PBGlobalDefinitions_steam GetInstance()
	{
		if (_instance == null)
			_instance = new PBGlobalDefinitions_steam();

		return _instance;
	}
	public static void InitUninitVariables()
	{
		GetInstance().Init();
	}

	public void Init()
	{

	}
} 