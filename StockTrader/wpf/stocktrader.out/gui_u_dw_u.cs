//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx\\u_dw.sru"
#line hidden
#line 1 "u_dw"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_dw",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx",null)]
[System.Diagnostics.DebuggerDisplay("",Type="u_dw")]
public class c__u_dw : Sybase.PowerBuilder.WPF.PBDataWindow
{
	#line hidden
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "ib_headersort", null, "u_dw", false, typeof(Sybase.PowerBuilder.PBBoolean), Sybase.PowerBuilder.PBModifier.Public, "= false")]
	public Sybase.PowerBuilder.PBBoolean ib_headersort = new Sybase.PowerBuilder.PBBoolean(false);
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "is_sortcolumn", null, "u_dw", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public Sybase.PowerBuilder.PBString is_sortcolumn = Sybase.PowerBuilder.PBString.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "is_sortorder", null, "u_dw", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public Sybase.PowerBuilder.PBString is_sortorder = Sybase.PowerBuilder.PBString.DefaultValue;

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
	}

	#line 1 "u_dw.clicked"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("clicked")]
	[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_dwnlbuttonclk)]
	public override Sybase.PowerBuilder.PBLong clicked(Sybase.PowerBuilder.PBInt xpos, Sybase.PowerBuilder.PBInt ypos, Sybase.PowerBuilder.PBLong row, Sybase.PowerBuilder.WPF.PBDWObject dwo)
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_headername = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_colname = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBInt li_rc = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBInt li_suffixlen = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBInt li_headerlen = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBString ls_sortstring = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 62
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)((new Sybase.PowerBuilder.PBAny(((Sybase.PowerBuilder.WPF.PBDWObject)(Sybase.PowerBuilder.PBSystemFunctions.PBCheckNull(dwo)))))))| !(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsValid((Sybase.PowerBuilder.PBPowerObject)(dwo)))))
		#line hidden
		{
			#line 63
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-1));
			#line hidden
		}
		#line 67
		if (!(ib_headersort))
		#line hidden
		{
			#line 67
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
			#line hidden
		}
		#line 70
		if ((Sybase.PowerBuilder.PBBoolean)((((Sybase.PowerBuilder.PBExtObject)(dwo))[new Sybase.PowerBuilder.PBString(@"name"), Sybase.PowerBuilder.PBBoolean.True]) == (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("datawindow"))))
		#line hidden
		{
			#line 70
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
			#line hidden
		}
		#line 71
		if ((Sybase.PowerBuilder.PBBoolean)((((Sybase.PowerBuilder.PBExtObject)(dwo))[new Sybase.PowerBuilder.PBString(@"band"), Sybase.PowerBuilder.PBBoolean.True]) != (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("header"))))
		#line hidden
		{
			#line 71
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
			#line hidden
		}
		#line 74
		ls_headername = (Sybase.PowerBuilder.PBString)((((Sybase.PowerBuilder.PBExtObject)(dwo))[new Sybase.PowerBuilder.PBString(@"name"), Sybase.PowerBuilder.PBBoolean.True]));
		#line hidden
		#line 75
		li_headerlen = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.WPF.PBSystemFunctions.Len(ls_headername));
		#line hidden
		#line 76
		li_suffixlen = new Sybase.PowerBuilder.PBInt(2);
		#line hidden
		#line 80
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.WPF.PBSystemFunctions.Right(ls_headername, (Sybase.PowerBuilder.PBLong)(li_suffixlen)) != new Sybase.PowerBuilder.PBString("_t")))
		#line hidden
		{
			#line 82
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-1));
			#line hidden
		}
		#line 84
		ls_colname = Sybase.PowerBuilder.WPF.PBSystemFunctions.Left(ls_headername, (Sybase.PowerBuilder.PBLong)(li_headerlen)- (Sybase.PowerBuilder.PBLong)(li_suffixlen));
		#line hidden
		#line 87
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_colname))))| (Sybase.PowerBuilder.WPF.PBSystemFunctions.Len(Sybase.PowerBuilder.WPF.PBSystemFunctions.Trim(ls_colname)) == (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)))))
		#line hidden
		{
			#line 88
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-1));
			#line hidden
		}
		#line 92
		if ((Sybase.PowerBuilder.PBBoolean)(is_sortcolumn == ls_colname))
		#line hidden
		{
			#line 94
			if ((Sybase.PowerBuilder.PBBoolean)(is_sortorder == new Sybase.PowerBuilder.PBString(" A")))
			#line hidden
			{
				#line 95
				is_sortorder = new Sybase.PowerBuilder.PBString(" D");
				#line hidden
			}
			else
			{
				#line 97
				is_sortorder = new Sybase.PowerBuilder.PBString(" A");
				#line hidden
			}
		}
		else
		{
			#line 101
			is_sortcolumn = ls_colname;
			#line hidden
			#line 102
			is_sortorder = new Sybase.PowerBuilder.PBString(" A");
			#line hidden
		}
		#line 106
		ls_sortstring = is_sortcolumn+ is_sortorder;
		#line hidden
		#line 109
		li_rc = this.SetSort(ls_sortstring);
		#line hidden
		#line 110
		if ((Sybase.PowerBuilder.PBBoolean)((Sybase.PowerBuilder.PBLong)(li_rc)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 110
			return (Sybase.PowerBuilder.PBLong)(li_rc);
			#line hidden
		}
		#line 113
		li_rc = this.Sort();
		#line hidden
		#line 114
		if ((Sybase.PowerBuilder.PBBoolean)((Sybase.PowerBuilder.PBLong)(li_rc)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 114
			return (Sybase.PowerBuilder.PBLong)(li_rc);
			#line hidden
		}
		#line 116
		return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
		#line hidden
	}

	void _init()
	{
		this.CreateEvent -= new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent -= new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.ClickedEvent += new Sybase.PowerBuilder.WPF.PBM_EventHandler_dwn_iildw(this.clicked);

		OnInitialUpdate();
	}

	public c__u_dw()
	{
		_init();
	}

}
 