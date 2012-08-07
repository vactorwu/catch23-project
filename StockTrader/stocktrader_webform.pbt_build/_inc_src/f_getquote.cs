//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\dwo.pbl\\f_getquote.srf"
#line hidden
#line 1 "f_getquote"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("f_getquote",Sybase.PowerBuilder.PBGroupType.FunctionObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\dwo.pbl",null)]
public class c__f_getquote : Sybase.PowerBuilder.PBFunction_Object
{
	#line hidden

	#line 1 "f_getquote(MS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("f_getquote", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public static Sybase.PowerBuilder.PBDecimal f_getquote(Sybase.PowerBuilder.PBString as_symbol)
	{
		#line hidden
		Sybase.PowerBuilder.PBException e = null;
		try
		{
			try
			{
				#line 2
				return c__stocktrader.GetCurrentApplication().gn_controller.of_get_current_price(as_symbol);
				#line hidden
			}
			catch (System.DivideByZeroException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0001);
				throw new System.Exception();
			}
			catch (System.NullReferenceException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0002);
				throw new System.Exception();
			}
			catch (System.IndexOutOfRangeException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0003);
				throw new System.Exception();
			}
		}
		#line 3
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
		#line hidden
		{
			e = __PB_TEMP_e__temp.E;
		}
		return Sybase.PowerBuilder.PBDecimal.DefaultValue;
	}
}
 