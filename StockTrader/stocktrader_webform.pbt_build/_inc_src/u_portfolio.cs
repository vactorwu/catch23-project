//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl\\u_portfolio.sru"
#line hidden
#line 1 "u_portfolio"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_portfolio",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl",null)]
public class c__u_portfolio : c__u_basepage
{
	#line hidden
	public c__u_portfolio.c__uo_1 uo_1 = null;
	public c__u_portfolio.c__dw_1 dw_1 = null;
	public c__u_portfolio.c__r_2 r_2 = null;
	public c__u_portfolio.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "ii_priorpoint", null, "u_portfolio", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public Sybase.PowerBuilder.PBInt ii_priorpoint = Sybase.PowerBuilder.PBInt.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "iu_portfolio_piechart", null, "u_portfolio", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public c__u_portfolio_piechart iu_portfolio_piechart = null;
	public new c__u_portfolio.c__st_message st_message
	{
		get { return (c__u_portfolio.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_portfolio.c__st_timestamp st_timestamp
	{
		get { return (c__u_portfolio.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_portfolio.c__st_asfo st_asfo
	{
		get { return (c__u_portfolio.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_portfolio.c__st_label st_label
	{
		get { return (c__u_portfolio.c__st_label)base.st_label; }
		set { base.st_label = value; }
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
		this.uo_1 = (c__uo_1)this.CreateInstance(this, "c__uo_1");
		#line hidden
		#line hidden
		this.dw_1 = (c__dw_1)this.CreateInstance(this, "c__dw_1");
		#line hidden
		#line hidden
		this.r_2 = (c__r_2)this.CreateInstance(this, "c__r_2");
		#line hidden
		#line hidden
		this.r_1 = (c__r_1)this.CreateInstance(this, "c__r_1");
		#line hidden
		#line hidden
		icurrent = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.Web.PBSystemFunctions.UpperBound((Sybase.PowerBuilder.PBAny)(this.Control)));
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.uo_1);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.dw_1);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(3)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.r_2);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(4)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.r_1);
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
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.uo_1);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.dw_1);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_2);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_portfolio.ue_setstate"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_setstate")]
	public override void ue_setstate()
	{
		#line hidden
		Sybase.PowerBuilder.PBLong lrows = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong i = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBLong j = Sybase.PowerBuilder.PBLong.DefaultValue;
		Sybase.PowerBuilder.PBDecimal ldc_price = new Sybase.PowerBuilder.PBDecimal(0m);
		#line 1
		base.ue_setstate();
		#line hidden
		#line 1
		iu_portfolio_piechart.Visible = new Sybase.PowerBuilder.PBBoolean(false);
		#line hidden
		#line 5
		dw_1.SetWSObject(c__stocktrader.GetCurrentApplication().gn_controller.of_get_wsconn());
		#line hidden
		#line 7
		lrows = dw_1.Retrieve((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(c__stocktrader.GetCurrentApplication().gn_controller.of_get_profile_id()))));
		#line hidden
		#line 9
		for (i = ((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)));i <= lrows;i = i + 1)
		#line hidden
		{
				#line 10
				ldc_price.AssignFrom(c__stocktrader.GetCurrentApplication().gn_controller.of_get_current_price(dw_1.GetItemString(i, new Sybase.PowerBuilder.PBString("quoteid"))));
				#line hidden
				#line 11
				dw_1.SetItem(i, new Sybase.PowerBuilder.PBString("current_price_x"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ldc_price))));
				#line hidden
		}
		#line 14
		iu_portfolio_piechart.of_share((Sybase.PowerBuilder.Web.PBDataWindow)(dw_1), new Sybase.PowerBuilder.PBBoolean(true));
		#line hidden
	}

	#line 1 "u_portfolio.constructor"
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
		#line 1
		this.OpenUserObject(ref iu_portfolio_piechart, new Sybase.PowerBuilder.PBInt(0), new Sybase.PowerBuilder.PBInt(0));
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
	}

	#line 1 "u_portfolio.st_message"
	#line hidden
	public new class c__st_message : c__u_basepage.c__st_message
	{
		#line hidden

		void _init()
		{
			this.ID = "st_message";

			OnInitialUpdate();
		}

		public c__st_message()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.st_timestamp"
	#line hidden
	public new class c__st_timestamp : c__u_basepage.c__st_timestamp
	{
		#line hidden

		void _init()
		{
			this.ID = "st_timestamp";

			OnInitialUpdate();
		}

		public c__st_timestamp()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.st_asfo"
	#line hidden
	public new class c__st_asfo : c__u_basepage.c__st_asfo
	{
		#line hidden

		void _init()
		{
			this.ID = "st_asfo";

			OnInitialUpdate();
		}

		public c__st_asfo()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.st_label"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("Text","Portfolio Information", typeof(Sybase.PowerBuilder.PBString))]
	public new class c__st_label : c__u_basepage.c__st_label
	{
		#line hidden

		void _init()
		{
			Text = new Sybase.PowerBuilder.PBString("Portfolio Information");
			#line hidden
			this.ID = "st_label";

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.uo_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",3240, typeof(Sybase.PowerBuilder.PBInt),
"Y",51, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",30, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean))]
	public class c__uo_1 : c__u_quotebar
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
			this.ID = "uo_1";
			this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__uo_1()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.dw_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",26, typeof(Sybase.PowerBuilder.PBInt),
"Y",390, typeof(Sybase.PowerBuilder.PBInt),
"Width",4465, typeof(Sybase.PowerBuilder.PBInt),
"Height",1869, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",40, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"DataObject","d_holdings", typeof(Sybase.PowerBuilder.PBString))]
	public class c__dw_1 : c__u_dw
	{
		#line hidden

		#line 1 "u_portfolio.dw_1.constructor"
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
			ib_headersort = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		#line 1 "u_portfolio.dw_1.buttonclicked"
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
			l_page.of_initialize(new Sybase.PowerBuilder.PBString("S"), (Sybase.PowerBuilder.PBLong)(this.GetItemNumber(row, new Sybase.PowerBuilder.PBString("quantity"))), (Sybase.PowerBuilder.PBLong)(this.GetItemNumber(row, new Sybase.PowerBuilder.PBString("holdingid"))), this.GetItemString(row, new Sybase.PowerBuilder.PBString("quoteid")), (Sybase.PowerBuilder.PBDecimal)(new Sybase.PowerBuilder.PBInt(0))
				);
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		#line 1 "u_portfolio.dw_1.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_dwnlbuttonclk)]
		public override Sybase.PowerBuilder.PBLong clicked(Sybase.PowerBuilder.PBInt xpos, Sybase.PowerBuilder.PBInt ypos, Sybase.PowerBuilder.PBLong row, Sybase.PowerBuilder.WinWebDataWindowCommon.PBDWObject dwo)
		{
			#line hidden
			Sybase.PowerBuilder.PBString ls_symbol = Sybase.PowerBuilder.PBString.DefaultValue;
			Sybase.PowerBuilder.PBInt li_point = Sybase.PowerBuilder.PBInt.DefaultValue;
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			ancestorreturnvalue = base.clicked(xpos, ypos, row, dwo);
			#line hidden
			#line 3
			if ((Sybase.PowerBuilder.PBBoolean)(row> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
			#line hidden
			{
				#line 4
				if ((Sybase.PowerBuilder.PBBoolean)((((Sybase.PowerBuilder.PBExtObject)(dwo))[new Sybase.PowerBuilder.PBString(@"name"), Sybase.PowerBuilder.PBBoolean.True]) == (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("b_sell"))))
				#line hidden
				{
					#line 4
					return new Sybase.PowerBuilder.PBLong(0);
					#line hidden
				}
				#line 6
				((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(((c__u_portfolio)(Parent)).iu_portfolio_piechart.dw_1.Object))[new Sybase.PowerBuilder.PBString(@"gr_1"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"title")] = (Sybase.PowerBuilder.PBAny)((Sybase.PowerBuilder.Web.PBSystemFunctions.String(((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBExtObject)this.Object)[
					new Sybase.PowerBuilder.PBString(@"quoteid"), 
					new Sybase.PowerBuilder.PBUInt(1), 
					new Sybase.PowerBuilder.PBUnboundedLongArray(new Sybase.PowerBuilder.PBLong[1]{(Sybase.PowerBuilder.PBLong)(row)}) ,
					Sybase.PowerBuilder.PBBoolean.True
				])))+ new Sybase.PowerBuilder.PBString("  "))+ Sybase.PowerBuilder.Web.PBSystemFunctions.String(((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBExtObject)this.Object)[
						new Sybase.PowerBuilder.PBString(@"purchase_basis"), 
						new Sybase.PowerBuilder.PBUInt(1), 
						new Sybase.PowerBuilder.PBUnboundedLongArray(new Sybase.PowerBuilder.PBLong[1]{(Sybase.PowerBuilder.PBLong)(row)}) ,
						Sybase.PowerBuilder.PBBoolean.True
					])), new Sybase.PowerBuilder.PBString("$###,###.00")));
				#line hidden
				#line 9
				if ((Sybase.PowerBuilder.PBBoolean)((Sybase.PowerBuilder.PBLong)(((c__u_portfolio)(Parent)).ii_priorpoint)> (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
				#line hidden
				{
					#line 9
					((c__u_portfolio)(Parent)).iu_portfolio_piechart.dw_1.SetDataPieExplode(new Sybase.PowerBuilder.PBString("gr_1"), new Sybase.PowerBuilder.PBInt(1), ((c__u_portfolio)(Parent)).ii_priorpoint, new Sybase.PowerBuilder.PBInt(0));
					#line hidden
				}
				#line 11
				ls_symbol = (new Sybase.PowerBuilder.PBString("symbol=\"")+ this.GetItemString(row, new Sybase.PowerBuilder.PBString("quoteid")))+ new Sybase.PowerBuilder.PBString("\"");
				#line hidden
				#line 12
				li_point = ((c__u_portfolio)(Parent)).iu_portfolio_piechart.dw_1.FindCategory(new Sybase.PowerBuilder.PBString("gr_1"), Sybase.PowerBuilder.Web.PBSystemFunctions.String(((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBExtObject)this.Object)[
					new Sybase.PowerBuilder.PBString(@"quoteid"), 
					new Sybase.PowerBuilder.PBUInt(1), 
					new Sybase.PowerBuilder.PBUnboundedLongArray(new Sybase.PowerBuilder.PBLong[1]{(Sybase.PowerBuilder.PBLong)(row)}) ,
					Sybase.PowerBuilder.PBBoolean.True
				]))));
				#line hidden
				#line 14
				((c__u_portfolio)(Parent)).iu_portfolio_piechart.dw_1.SetDataPieExplode(new Sybase.PowerBuilder.PBString("gr_1"), new Sybase.PowerBuilder.PBInt(1), li_point, new Sybase.PowerBuilder.PBInt(30));
				#line hidden
				#line 15
				((c__u_portfolio)(Parent)).ii_priorpoint = li_point;
				#line hidden
				#line 18
				((c__u_portfolio)(Parent)).iu_portfolio_piechart.X = ((c__u_portfolio)(this.Parent)).PointerX();
				#line hidden
				#line 19
				((c__u_portfolio)(Parent)).iu_portfolio_piechart.Y = ((c__u_portfolio)(this.Parent)).PointerY();
				#line hidden
				#line 20
				((c__u_portfolio)(Parent)).iu_portfolio_piechart.Visible = new Sybase.PowerBuilder.PBBoolean(true);
				#line hidden
			}
			else
			{
				#line 22
				((c__u_portfolio)(Parent)).iu_portfolio_piechart.Visible = new Sybase.PowerBuilder.PBBoolean(false);
				#line hidden
			}
			#line 24
			return new Sybase.PowerBuilder.PBLong(0);
			#line hidden
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
			TabOrder = new Sybase.PowerBuilder.PBInt(40);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			DataObject = new Sybase.PowerBuilder.PBString("d_holdings");
			#line hidden
			this.ID = "dw_1";
			this.ConstructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.constructor);
			this.ButtonClickedEvent += new Sybase.PowerBuilder.WinWebDataWindowCommon.PBM_EventHandler_dwn_lldw(this.buttonclicked);
			this.ClickedEvent += new Sybase.PowerBuilder.WinWebDataWindowCommon.PBM_EventHandler_dwn_iildw(this.clicked);

			OnInitialUpdate();
		}

		public c__dw_1()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.r_2"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"X",110, typeof(Sybase.PowerBuilder.PBInt),
"Y",525, typeof(Sybase.PowerBuilder.PBInt),
"Width",4341, typeof(Sybase.PowerBuilder.PBInt),
"Height",1507, typeof(Sybase.PowerBuilder.PBInt))]
	public class c__r_2 : Sybase.PowerBuilder.Web.PBRectangle
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
			X = new Sybase.PowerBuilder.PBInt(110);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(525);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4341);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1507);
			#line hidden

			OnInitialUpdate();
		}

		public c__r_2()
		{
			_init();
		}

	}


	#line 1 "u_portfolio.r_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"X",7, typeof(Sybase.PowerBuilder.PBInt),
"Y",368, typeof(Sybase.PowerBuilder.PBInt),
"Width",4513, typeof(Sybase.PowerBuilder.PBInt),
"Height",1926, typeof(Sybase.PowerBuilder.PBInt))]
	public class c__r_1 : Sybase.PowerBuilder.Web.PBRectangle
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
		this.ID = "u_portfolio";
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.ConstructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.constructor);

		OnInitialUpdate();
	}

	public c__u_portfolio()
	{
		_init();
	}

}
 