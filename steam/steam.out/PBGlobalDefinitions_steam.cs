[Sybase.PowerBuilder.PBGroupAttribute("pbglobaldefinitions_steam", Sybase.PowerBuilder.PBGroupType.Unknown,"","", null,"steam")]
[Sybase.PowerBuilder.PBTargetAttribute("d:\\svn\\steam\\steam.pbtx", "c__steam")]
[System.Diagnostics.DebuggerStepThrough]
public class PBGlobalDefinitions_steam
{
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamwebapiutil_getserverinfo", null, "p_ISteamWebAPIUtil_GetServerInfo", "lyc.steam.proxy.isteamwebapiutil", "d:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamwebapiutil_getserverinfo")]
	public lyc.steam.proxy.isteamwebapiutil.c__p_isteamwebapiutil_getserverinfo lyc_steam_proxy_isteamwebapiutil_p_isteamwebapiutil_getserverinfo = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "p_isteamwebapiutil_getsupportedapilist", null, "p_ISteamWebAPIUtil_GetSupportedAPIList", "lyc.steam.proxy.isteamwebapiutil", "d:\\svn\\steam\\proxy.pbl\\proxy.pblx",null, null, "p_isteamwebapiutil_getsupportedapilist")]
	public lyc.steam.proxy.isteamwebapiutil.c__p_isteamwebapiutil_getsupportedapilist lyc_steam_proxy_isteamwebapiutil_p_isteamwebapiutil_getsupportedapilist = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "n_steam", null, "n_steam", "", "d:\\svn\\steam\\api.pbl\\api.pblx",null, null, "n_steam")]
	public c__n_steam n_steam = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "w_main", null, "w_main", "lyc.steam.gui", "d:\\svn\\steam\\gui.pbl\\gui.pblx",null, null, "w_main")]
	public lyc.steam.gui.c__w_main lyc_steam_gui_w_main = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "n_logger", null, "n_logger", "", "d:\\svn\\steam\\util.pbl\\util.pblx",null, null, "n_logger")]
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