//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "d:\\svn\\weather_net\\control.pbl\\control.pblx\\gf_getcitycode.srf"
#line hidden
#line 1 "gf_getcitycode"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("gf_getcitycode",Sybase.PowerBuilder.PBGroupType.FunctionObject,"","d:\\svn\\weather_net\\control.pbl\\control.pblx",null,"weather")]
[System.Diagnostics.DebuggerDisplay("",Type="gf_getcitycode")]
public class c__gf_getcitycode : Sybase.PowerBuilder.PBFunction_Object
{
	#line hidden

	#line 1 "gf_getcitycode(SS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("gf_getcitycode", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public static Sybase.PowerBuilder.PBString gf_getcitycode(Sybase.PowerBuilder.PBString as_city)
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_citycode = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_start = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_end = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 3
		ll_start = Sybase.PowerBuilder.WPF.PBSystemFunctions.Pos(as_city, new Sybase.PowerBuilder.PBString("("));
		#line hidden
		#line 4
		ll_end = Sybase.PowerBuilder.WPF.PBSystemFunctions.Pos(as_city, new Sybase.PowerBuilder.PBString(")"));
		#line hidden
		#line 5
		ls_citycode = Sybase.PowerBuilder.WPF.PBSystemFunctions.Mid(as_city, ll_start+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), (ll_end- ll_start)- (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)));
		#line hidden
		#line 6
		return ls_citycode;
		#line hidden
	}
}
 