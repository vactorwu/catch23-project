[Sybase.PowerBuilder.PBGroupAttribute("pbglobaldefinitions_custom_dw", Sybase.PowerBuilder.PBGroupType.Unknown,"","", null,"custom_dw")]
[Sybase.PowerBuilder.PBTargetAttribute("d:\\svn\\datawindow_wpfco\\custom_dw.pbtx", "")]
[System.Diagnostics.DebuggerStepThrough]
public class PBGlobalDefinitions_custom_dw
{
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "w_main", null, "w_main", "", "d:\\svn\\datawindow_wpfco\\custom_dw.pbl\\custom_dw.pblx",null, null, "w_main")]
	public c__w_main w_main = null;

	public static PBGlobalDefinitions_custom_dw _instance = null;

	public static PBGlobalDefinitions_custom_dw GetInstance()
	{
		if (_instance == null)
			_instance = new PBGlobalDefinitions_custom_dw();

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