//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl\\u_quotes.sru"
#line hidden
#line 1 "u_quotes"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_quotes",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl",null)]
public class c__u_quotes : c__u_basepage
{
	#line hidden
	public c__u_quotes.c__uo_quotebar uo_quotebar = null;
	public c__u_quotes.c__dw_1 dw_1 = null;
	public c__u_quotes.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "ib_append", null, "u_quotes", false, typeof(Sybase.PowerBuilder.PBBoolean), Sybase.PowerBuilder.PBModifier.Public, "= false")]
	public Sybase.PowerBuilder.PBBoolean ib_append = new Sybase.PowerBuilder.PBBoolean(false);
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "ids_buffer", null, "u_quotes", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public Sybase.PowerBuilder.WinWebDataWindowCommon.PBDataStore ids_buffer = null;
	public new c__u_quotes.c__st_message st_message
	{
		get { return (c__u_quotes.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_quotes.c__st_timestamp st_timestamp
	{
		get { return (c__u_quotes.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_quotes.c__st_asfo st_asfo
	{
		get { return (c__u_quotes.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_quotes.c__st_label st_label
	{
		get { return (c__u_quotes.c__st_label)base.st_label; }
		set { base.st_label = value; }
	}

	#line 1 "u_quotes.ue_set_quote"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_set_quote")]
	public virtual void ue_set_quote(Sybase.PowerBuilder.PBString as_symbol)
	{
		#line hidden
		#line 1
		c__u_quotes_u002e_c__uo_quotebar_of_39875684(uo_quotebar, ref as_symbol);
		#line hidden
	}

	#line 1 "u_quotes.of_get_quote(IS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_get_quote", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBInt of_get_quote(Sybase.PowerBuilder.PBString as_symbol)
	{
		#line hidden
		Sybase.PowerBuilder.PBInt li_colonpos = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBInt li_ctr = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBInt li_commapos = Sybase.PowerBuilder.PBInt.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_symbol_count = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBArray lsymbols = new Sybase.PowerBuilder.PBUnboundedStringArray();
		#line 9
		ib_append = new Sybase.PowerBuilder.PBBoolean(false);
		#line hidden
		#line 10
		li_colonpos = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.Win.PBSystemFunctions.Pos(as_symbol, new Sybase.PowerBuilder.PBString(";")));
		#line hidden
		#line 11
		li_commapos = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.Win.PBSystemFunctions.Pos(as_symbol, new Sybase.PowerBuilder.PBString(",")));
		#line hidden
		#line 12
		if ((Sybase.PowerBuilder.PBBoolean)(((Sybase.PowerBuilder.PBLong)(li_commapos)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)))& ((Sybase.PowerBuilder.PBLong)(li_colonpos)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)))))
		#line hidden
		{
			#line 13
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Can't have both ; and , as symbol separators"));
			#line hidden
			#line 14
			return new Sybase.PowerBuilder.PBInt(-1);
			#line hidden
		}
		#line 16
		this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 17
		if ((Sybase.PowerBuilder.PBBoolean)((li_commapos == new Sybase.PowerBuilder.PBInt(0))& (li_colonpos == new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 17
			return (Sybase.PowerBuilder.PBInt)(dw_1.Retrieve((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(as_symbol)))));
			#line hidden
		}
		#line 18
		if ((Sybase.PowerBuilder.PBBoolean)((Sybase.PowerBuilder.PBLong)(li_commapos)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 19
			ll_symbol_count = of_parsetoarray_3_345252223_3_44230024(this, as_symbol, new Sybase.PowerBuilder.PBString(","), ref lsymbols);
			#line hidden
		}
		#line 21
		if ((Sybase.PowerBuilder.PBBoolean)((Sybase.PowerBuilder.PBLong)(li_colonpos)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 22
			ll_symbol_count = of_parsetoarray_3_345252223_3_44230024(this, as_symbol, new Sybase.PowerBuilder.PBString(";"), ref lsymbols);
			#line hidden
		}
		#line 24
		dw_1.Reset();
		#line hidden
		#line 26
		for (li_ctr = new Sybase.PowerBuilder.PBInt(1);li_ctr <= ((Sybase.PowerBuilder.PBInt)(ll_symbol_count));li_ctr = li_ctr + 1)
		#line hidden
		{
				#line 32
				ids_buffer.Retrieve((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBString)lsymbols[(Sybase.PowerBuilder.PBLong)(li_ctr)])))));
				#line hidden
				#line 33
				ids_buffer.RowsCopy((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), (new Sybase.PowerBuilder.PBDWBufferValue(Sybase.PowerBuilder.PBDWBuffer.Primary)), (Sybase.PowerBuilder.IPBDataWindowControl)(((c__u_quotes.c__dw_1)(Sybase.PowerBuilder.PBSystemFunctions.PBCheckNull(dw_1)))), (Sybase.PowerBuilder.PBLong)(li_ctr)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), 
					(new Sybase.PowerBuilder.PBDWBufferValue(Sybase.PowerBuilder.PBDWBuffer.Primary)));
				#line hidden
		}
		return Sybase.PowerBuilder.PBInt.DefaultValue;
	}

	#line 1 "u_quotes.of_parsetoarray(LSSRS[])"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_parsetoarray", new string[]{"string", "string", "ref string"}, Sybase.PowerBuilder.PBModifier.Private, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	private Sybase.PowerBuilder.PBLong of_parsetoarray_3_345252223(Sybase.PowerBuilder.PBString as_source, Sybase.PowerBuilder.PBString as_delimiter, [Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBString))] ref Sybase.PowerBuilder.PBArray as_array)
	{
		#line hidden
		Sybase.PowerBuilder.PBLong ll_dellen = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_pos = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_count = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_start = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_length = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBString ls_holder = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBLong ll_null = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 56
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.Win.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(as_source))))| Sybase.PowerBuilder.Win.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(as_delimiter))))))
		#line hidden
		{
			#line 58
			ll_null.SetNull();
			#line hidden
			#line 59
			return ll_null;
			#line hidden
		}
		#line 63
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.Win.PBSystemFunctions.Trim(as_source) == new Sybase.PowerBuilder.PBString("")))
		#line hidden
		{
			#line 64
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
			#line hidden
		}
		#line 68
		ll_dellen = Sybase.PowerBuilder.Win.PBSystemFunctions.Len(as_delimiter);
		#line hidden
		#line 70
		ll_pos = Sybase.PowerBuilder.Win.PBSystemFunctions.Pos(Sybase.PowerBuilder.Win.PBSystemFunctions.Upper(as_source), Sybase.PowerBuilder.Win.PBSystemFunctions.Upper(as_delimiter));
		#line hidden
		#line 73
		if ((Sybase.PowerBuilder.PBBoolean)(ll_pos == (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 74
			as_array[as_array.Extend((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = as_source;
			#line hidden
			#line 75
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
			#line hidden
		}
		#line 79
		ll_count = (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
		#line hidden
		#line 80
		ll_start = (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
		#line hidden
		#line 81
		while ( ll_pos> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)) )
		#line hidden
		{
				#line 84
				ll_length = ll_pos- ll_start;
				#line hidden
				#line 85
				ls_holder = Sybase.PowerBuilder.Win.PBSystemFunctions.Mid(as_source, ll_start, ll_length);
				#line hidden
				#line 88
				ll_count++;
				#line hidden
				#line 89
				as_array[as_array.Extend(ll_count)] = ls_holder;
				#line hidden
				#line 92
				ll_start = ll_pos+ ll_dellen;
				#line hidden
				#line 94
				ll_pos = Sybase.PowerBuilder.Win.PBSystemFunctions.Pos(Sybase.PowerBuilder.Win.PBSystemFunctions.Upper(as_source), Sybase.PowerBuilder.Win.PBSystemFunctions.Upper(as_delimiter), ll_start);
				#line hidden
		}

		#line 98
		ls_holder = Sybase.PowerBuilder.Win.PBSystemFunctions.Mid(as_source, ll_start, Sybase.PowerBuilder.Win.PBSystemFunctions.Len(as_source));
		#line hidden
		#line 101
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.Win.PBSystemFunctions.Len(ls_holder)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 102
			ll_count++;
			#line hidden
			#line 103
			as_array[as_array.Extend(ll_count)] = ls_holder;
			#line hidden
		}
		#line 107
		return ll_count;
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
		Sybase.PowerBuilder.PBInt icurrent = Sybase.PowerBuilder.PBInt.DefaultValue;
		#line hidden
		base.create();
		#line hidden
		#line hidden
		this.uo_quotebar = (c__uo_quotebar)this.CreateInstance(this, "c__uo_quotebar");
		#line hidden
		#line hidden
		this.dw_1 = (c__dw_1)this.CreateInstance(this, "c__dw_1");
		#line hidden
		#line hidden
		this.r_1 = (c__r_1)this.CreateInstance(this, "c__r_1");
		#line hidden
		#line hidden
		icurrent = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.Win.PBSystemFunctions.UpperBound((Sybase.PowerBuilder.PBAny)(this.Control)));
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.Win.PBWindowObject)(this.uo_quotebar);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.Win.PBWindowObject)(this.dw_1);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(3)))] = (Sybase.PowerBuilder.Win.PBWindowObject)(this.r_1);
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
		#line hidden
		base.destroy();
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(this.uo_quotebar);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(this.dw_1);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_quotes.ue_setstate"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_setstate")]
	public override void ue_setstate()
	{
		#line hidden
		#line 1
		base.ue_setstate();
		#line hidden
		#line 1
		dw_1.SetWSObject(c__stocktrader.GetCurrentApplication().gn_controller.of_get_wsconn());
		#line hidden
		#line 4
		ids_buffer.SetWSObject(c__stocktrader.GetCurrentApplication().gn_controller.of_get_wsconn());
		#line hidden
	}

	#line 1 "u_quotes.constructor"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("constructor")]
	[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_constructor)]
	public override Sybase.PowerBuilder.PBLong constructor()
	{
		#line hidden
		Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 1
		ancestorreturnvalue = base.constructor();
		#line hidden
		#line 2
		ids_buffer = (Sybase.PowerBuilder.WinWebDataWindowCommon.PBDataStore)this.CreateInstance(typeof(Sybase.PowerBuilder.WinWebDataWindowCommon.PBDataStore));
		#line hidden
		#line 3
		ids_buffer.DataObject = new Sybase.PowerBuilder.PBString("d_quotes");
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
	}

	Sybase.PowerBuilder.PBAny c__u_quotes_u002e_c__uo_quotebar_of_39875684(c__u_quotes.c__uo_quotebar this__object, ref Sybase.PowerBuilder.PBString temp_arg_name_0)
	{
		Sybase.PowerBuilder.PBArgument[] __tempArgument4DynamicCall = new Sybase.PowerBuilder.PBArgument[1] {
			new Sybase.PowerBuilder.PBArgument(temp_arg_name_0, Sybase.PowerBuilder.ParameterStyle.ByValue, typeof(Sybase.PowerBuilder.PBString))
			};
		Sybase.PowerBuilder.PBAny return_value = Sybase.PowerBuilder.PBMethod.InvokeDynamic(this__object, "of_set_quote", ref __tempArgument4DynamicCall);

		if (__tempArgument4DynamicCall[0].Style == Sybase.PowerBuilder.ParameterStyle.ByRef)
			temp_arg_name_0 = (Sybase.PowerBuilder.PBString)__tempArgument4DynamicCall[0].Value;

		return return_value;
	}

	private Sybase.PowerBuilder.PBLong of_parsetoarray_3_345252223_3_44230024(c__u_quotes this__object, Sybase.PowerBuilder.PBString as_source, Sybase.PowerBuilder.PBString as_delimiter, ref Sybase.PowerBuilder.PBArray as_array)
	{
		Sybase.PowerBuilder.PBLong return_value = this__object.of_parsetoarray_3_345252223(as_source, as_delimiter, ref as_array);
		return return_value;
	}


	#line 1 "u_quotes.st_message"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",1189, typeof(Sybase.PowerBuilder.PBInt),
"Y",218, typeof(Sybase.PowerBuilder.PBInt),
"Width",2154, typeof(Sybase.PowerBuilder.PBInt))]
	public new class c__st_message : c__u_basepage.c__st_message
	{
		#line hidden

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(1189);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(218);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(2154);
			#line hidden

			OnInitialUpdate();
		}

		public c__st_message()
		{
			_init();
		}

	}


	#line 1 "u_quotes.st_timestamp"
	#line hidden
	public new class c__st_timestamp : c__u_basepage.c__st_timestamp
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_timestamp()
		{
			_init();
		}

	}


	#line 1 "u_quotes.st_asfo"
	#line hidden
	public new class c__st_asfo : c__u_basepage.c__st_asfo
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_asfo()
		{
			_init();
		}

	}


	#line 1 "u_quotes.st_label"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("Text","Quotes", typeof(Sybase.PowerBuilder.PBString))]
	public new class c__st_label : c__u_basepage.c__st_label
	{
		#line hidden

		void _init()
		{
			Text = new Sybase.PowerBuilder.PBString("Quotes");
			#line hidden

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	#line 1 "u_quotes.uo_quotebar"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",3240, typeof(Sybase.PowerBuilder.PBInt),
"Y",51, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",30, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean))]
	public class c__uo_quotebar : c__u_quotebar
	{
		#line hidden

		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("destroy")]
		public override void destroy()
		{
			#line hidden
			#line hidden
			base.destroy();
			#line hidden
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(3240);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(51);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(30);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__uo_quotebar()
		{
			_init();
		}

	}


	#line 1 "u_quotes.dw_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",26, typeof(Sybase.PowerBuilder.PBInt),
"Y",390, typeof(Sybase.PowerBuilder.PBInt),
"Width",4465, typeof(Sybase.PowerBuilder.PBInt),
"Height",1869, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",50, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"DataObject","d_quotes", typeof(Sybase.PowerBuilder.PBString))]
	public class c__dw_1 : c__u_dw
	{
		#line hidden

		#line 1 "u_quotes.dw_1.retrievestart"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("retrievestart")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_dwnretrievestart)]
		public override Sybase.PowerBuilder.PBLong retrievestart()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			ancestorreturnvalue = base.retrievestart();
			#line hidden
			#line 1
			if ((Sybase.PowerBuilder.PBBoolean)(((c__u_quotes)(Parent)).ib_append == new Sybase.PowerBuilder.PBBoolean(true)))
			#line hidden
			{
				#line 2
				return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2));
				#line hidden
			}
			else
			{
				#line 4
				return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0));
				#line hidden
			}
		}

		#line 1 "u_quotes.dw_1.buttonclicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("buttonclicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_dwnbuttonclicked)]
		public override Sybase.PowerBuilder.PBLong buttonclicked(Sybase.PowerBuilder.PBLong row, Sybase.PowerBuilder.PBLong actionreturncode, Sybase.PowerBuilder.WinWebDataWindowCommon.PBDWObject dwo)
		{
			#line hidden
			c__u_buy_sell l_page = null;
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			ancestorreturnvalue = base.buttonclicked(row, actionreturncode, dwo);
			#line hidden
			#line 3
			l_page = (c__u_buy_sell)(((c__u_basepage)(Sybase.PowerBuilder.PBSystemFunctions.PBCheckNull(c__stocktrader.GetCurrentApplication().w_stocktrader.ue_show_buy_sell()))));
			#line hidden
			#line 4
			l_page.of_initialize(new Sybase.PowerBuilder.PBString("B"), (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)), (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)), this.GetItemString(row, new Sybase.PowerBuilder.PBString("symbol")), (this.GetItemDecimal(row, new Sybase.PowerBuilder.PBString("price"))).ToPBDecimal(-1)
				);
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(26);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(390);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4465);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1869);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(50);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			DataObject = new Sybase.PowerBuilder.PBString("d_quotes");
			#line hidden
			this.RetrieveStartEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.retrievestart);
			this.ButtonClickedEvent += new Sybase.PowerBuilder.WinWebDataWindowCommon.PBM_EventHandler_dwn_lldw(this.buttonclicked);

			OnInitialUpdate();
		}

		public c__dw_1()
		{
			_init();
		}

	}


	#line 1 "u_quotes.r_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"X",7, typeof(Sybase.PowerBuilder.PBInt),
"Y",368, typeof(Sybase.PowerBuilder.PBInt),
"Width",4513, typeof(Sybase.PowerBuilder.PBInt),
"Height",1926, typeof(Sybase.PowerBuilder.PBInt))]
	public class c__r_1 : Sybase.PowerBuilder.Win.PBRectangle
	{
		#line hidden

		void _init()
		{
			LineColor = new Sybase.PowerBuilder.PBLong(33554432);
			#line hidden
			LineThickness = new Sybase.PowerBuilder.PBInt(3);
			#line hidden
			FillColor = new Sybase.PowerBuilder.PBLong(30578299);
			#line hidden
			X = new Sybase.PowerBuilder.PBInt(7);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(368);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4513);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1926);
			#line hidden

			OnInitialUpdate();
		}

		public c__r_1()
		{
			_init();
		}

	}


	void _init()
	{
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.ConstructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.constructor);

		OnInitialUpdate();
	}

	public c__u_quotes()
	{
		_init();
	}

}
 