//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl\\u_config.sru"
#line hidden
#line 1 "u_config"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_config",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl",null)]
public class c__u_config : c__u_basepage
{
	#line hidden
	public c__u_config.c__r_2 r_2 = null;
	public c__u_config.c__sle_host sle_host = null;
	public c__u_config.c__st_1 st_1 = null;
	public c__u_config.c__pb_connect pb_connect = null;
	public c__u_config.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "ini_file", null, "u_config", "StockTrader.ini", typeof(Sybase.PowerBuilder.PBString), Sybase.PowerBuilder.PBModifier.Private, "= 'StockTrader.ini'")]
	[Sybase.PowerBuilder.PBConstantAttribute()]
	static protected Sybase.PowerBuilder.PBString ini_file = new Sybase.PowerBuilder.PBString("StockTrader.ini");
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "success", null, "u_config", "Successfully connected to ", typeof(Sybase.PowerBuilder.PBString), Sybase.PowerBuilder.PBModifier.Private, "= 'Successfully connected to '")]
	[Sybase.PowerBuilder.PBConstantAttribute()]
	static protected Sybase.PowerBuilder.PBString success = new Sybase.PowerBuilder.PBString("Successfully connected to ");
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "failure", null, "u_config", "Failed to connect to ", typeof(Sybase.PowerBuilder.PBString), Sybase.PowerBuilder.PBModifier.Private, "= 'Failed to connect to '")]
	[Sybase.PowerBuilder.PBConstantAttribute()]
	static protected Sybase.PowerBuilder.PBString failure = new Sybase.PowerBuilder.PBString("Failed to connect to ");
	public new c__u_config.c__st_message st_message
	{
		get { return (c__u_config.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_config.c__st_timestamp st_timestamp
	{
		get { return (c__u_config.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_config.c__st_asfo st_asfo
	{
		get { return (c__u_config.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_config.c__st_label st_label
	{
		get { return (c__u_config.c__st_label)base.st_label; }
		set { base.st_label = value; }
	}

	#line 1 "u_config.of_connect(Q)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_connect", new string[]{}, Sybase.PowerBuilder.PBModifier.Private, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	private void of_connect()
	{
		#line hidden
		Sybase.PowerBuilder.PBException e = null;
		try
		{
			try
			{
				#line 2
				Sybase.PowerBuilder.Web.PBSystemFunctions.SetPointer((new Sybase.PowerBuilder.PBPointerValue(Sybase.PowerBuilder.PBPointer.HourGlass)));
				#line hidden
				#line 3
				c__stocktrader.GetCurrentApplication().gn_controller.of_connect(sle_host.Text);
				#line hidden
				#line 4
				Sybase.PowerBuilder.Web.PBSystemFunctions.SetProfileString(ini_file, new Sybase.PowerBuilder.PBString("connection"), new Sybase.PowerBuilder.PBString("host"), sle_host.Text);
				#line hidden
				#line 5
				this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), (success+ new Sybase.PowerBuilder.PBString(" "))+ sle_host.Text);
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
		#line 6
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
		#line hidden
		{
			e = __PB_TEMP_e__temp.E;
			#line 7
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), e.GetMessage());
			#line hidden
		}
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
		this.r_2 = (c__r_2)this.CreateInstance(this, "c__r_2");
		#line hidden
		#line hidden
		this.sle_host = (c__sle_host)this.CreateInstance(this, "c__sle_host");
		#line hidden
		#line hidden
		this.st_1 = (c__st_1)this.CreateInstance(this, "c__st_1");
		#line hidden
		#line hidden
		this.pb_connect = (c__pb_connect)this.CreateInstance(this, "c__pb_connect");
		#line hidden
		#line hidden
		this.r_1 = (c__r_1)this.CreateInstance(this, "c__r_1");
		#line hidden
		#line hidden
		icurrent = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.Web.PBSystemFunctions.UpperBound((Sybase.PowerBuilder.PBAny)(this.Control)));
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.r_2);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.sle_host);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(3)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.st_1);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(4)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.pb_connect);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(5)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.r_1);
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
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_2);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.sle_host);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.st_1);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.pb_connect);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_config.constructor"
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
		sle_host.Text = Sybase.PowerBuilder.Web.PBSystemFunctions.ProfileString(ini_file, new Sybase.PowerBuilder.PBString("connection"), new Sybase.PowerBuilder.PBString("host"), new Sybase.PowerBuilder.PBString("http://[fill in your host here]"));
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
	}

	#line 1 "u_config.st_message"
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


	#line 1 "u_config.st_timestamp"
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


	#line 1 "u_config.st_asfo"
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


	#line 1 "u_config.st_label"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("Text","Cofiguration", typeof(Sybase.PowerBuilder.PBString))]
	public new class c__st_label : c__u_basepage.c__st_label
	{
		#line hidden

		void _init()
		{
			Text = new Sybase.PowerBuilder.PBString("Cofiguration");
			#line hidden
			this.ID = "st_label";

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	#line 1 "u_config.r_2"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"X",110, typeof(Sybase.PowerBuilder.PBInt),
"Y",570, typeof(Sybase.PowerBuilder.PBInt),
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
			Y = new Sybase.PowerBuilder.PBInt(570);
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


	#line 1 "u_config.sle_host"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",987, typeof(Sybase.PowerBuilder.PBInt),
"Y",1744, typeof(Sybase.PowerBuilder.PBInt),
"Width",2820, typeof(Sybase.PowerBuilder.PBInt),
"Height",118, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",10, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"TextSize",-12, typeof(Sybase.PowerBuilder.PBInt),
"Weight",400, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"TextColor",33554432, typeof(Sybase.PowerBuilder.PBLong))]
	public class c__sle_host : Sybase.PowerBuilder.Web.PBSingleLineEdit
	{
		#line hidden

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(987);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(1744);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(2820);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(118);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(10);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			TextSize = new Sybase.PowerBuilder.PBInt(-12);
			#line hidden
			Weight = new Sybase.PowerBuilder.PBInt(400);
			#line hidden
			FontCharSet = (new Sybase.PowerBuilder.PBFontCharSetValue(Sybase.PowerBuilder.PBFontCharSet.Ansi));
			#line hidden
			FontPitch = (new Sybase.PowerBuilder.PBFontPitchValue(Sybase.PowerBuilder.PBFontPitch.Variable));
			#line hidden
			FontFamily = (new Sybase.PowerBuilder.PBFontFamilyValue(Sybase.PowerBuilder.PBFontFamily.Swiss));
			#line hidden
			FaceName = new Sybase.PowerBuilder.PBString("Trebuchet MS");
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(33554432);
			#line hidden
			BorderStyle = (new Sybase.PowerBuilder.PBBorderStyleValue(Sybase.PowerBuilder.PBBorderStyle.StyleLowered));
			#line hidden
			this.ID = "sle_host";

			OnInitialUpdate();
		}

		public c__sle_host()
		{
			_init();
		}

	}


	#line 1 "u_config.st_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",230, typeof(Sybase.PowerBuilder.PBInt),
"Y",1773, typeof(Sybase.PowerBuilder.PBInt),
"Width",735, typeof(Sybase.PowerBuilder.PBInt),
"Height",83, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"TextSize",-10, typeof(Sybase.PowerBuilder.PBInt),
"Weight",400, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"TextColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"BackColor",16777215, typeof(Sybase.PowerBuilder.PBLong),
"Text","Address for Remote Host:", typeof(Sybase.PowerBuilder.PBString),
"FocusRectangle",false, typeof(Sybase.PowerBuilder.PBBoolean))]
	public class c__st_1 : Sybase.PowerBuilder.Web.PBStaticText
	{
		#line hidden

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(230);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(1773);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(735);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(83);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			TextSize = new Sybase.PowerBuilder.PBInt(-10);
			#line hidden
			Weight = new Sybase.PowerBuilder.PBInt(400);
			#line hidden
			FontCharSet = (new Sybase.PowerBuilder.PBFontCharSetValue(Sybase.PowerBuilder.PBFontCharSet.Ansi));
			#line hidden
			FontPitch = (new Sybase.PowerBuilder.PBFontPitchValue(Sybase.PowerBuilder.PBFontPitch.Variable));
			#line hidden
			FontFamily = (new Sybase.PowerBuilder.PBFontFamilyValue(Sybase.PowerBuilder.PBFontFamily.Swiss));
			#line hidden
			FaceName = new Sybase.PowerBuilder.PBString("Trebuchet MS");
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(33554432);
			#line hidden
			BackColor = new Sybase.PowerBuilder.PBLong(16777215);
			#line hidden
			Text = new Sybase.PowerBuilder.PBString("Address for Remote Host:");
			#line hidden
			FocusRectangle = new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
			this.ID = "st_1";

			OnInitialUpdate();
		}

		public c__st_1()
		{
			_init();
		}

	}


	#line 1 "u_config.pb_connect"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",3851, typeof(Sybase.PowerBuilder.PBInt),
"Y",1738, typeof(Sybase.PowerBuilder.PBInt),
"Width",380, typeof(Sybase.PowerBuilder.PBInt),
"Height",134, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",20, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"TextSize",-10, typeof(Sybase.PowerBuilder.PBInt),
"Weight",400, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"Text","Connect", typeof(Sybase.PowerBuilder.PBString),
"Default",true, typeof(Sybase.PowerBuilder.PBBoolean),
"Map3DColors",true, typeof(Sybase.PowerBuilder.PBBoolean),
"PowerTipText","Get Quote", typeof(Sybase.PowerBuilder.PBString),
"TextColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"BackColor",23213090, typeof(Sybase.PowerBuilder.PBLong))]
	public class c__pb_connect : Sybase.PowerBuilder.Web.PBPictureButton
	{
		#line hidden

		#line 1 "u_config.pb_connect.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_bnclicked)]
		public override Sybase.PowerBuilder.PBLong clicked()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			((c__u_config)(this.Parent)).of_connect();
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(3851);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(1738);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(380);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(134);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(20);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			TextSize = new Sybase.PowerBuilder.PBInt(-10);
			#line hidden
			Weight = new Sybase.PowerBuilder.PBInt(400);
			#line hidden
			FontCharSet = (new Sybase.PowerBuilder.PBFontCharSetValue(Sybase.PowerBuilder.PBFontCharSet.Ansi));
			#line hidden
			FontPitch = (new Sybase.PowerBuilder.PBFontPitchValue(Sybase.PowerBuilder.PBFontPitch.Variable));
			#line hidden
			FontFamily = (new Sybase.PowerBuilder.PBFontFamilyValue(Sybase.PowerBuilder.PBFontFamily.Swiss));
			#line hidden
			FaceName = new Sybase.PowerBuilder.PBString("Trebuchet MS");
			#line hidden
			Text = new Sybase.PowerBuilder.PBString("Connect");
			#line hidden
			Default = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			VTextAlign = (new Sybase.PowerBuilder.PBVTextAlignValue(Sybase.PowerBuilder.PBVTextAlign.VCenter));
			#line hidden
			Map3DColors = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			PowerTipText = new Sybase.PowerBuilder.PBString("Get Quote");
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(30578299);
			#line hidden
			BackColor = new Sybase.PowerBuilder.PBLong(23213090);
			#line hidden
			this.ID = "pb_connect";
			this.ClickedEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.clicked);

			OnInitialUpdate();
		}

		public c__pb_connect()
		{
			_init();
		}

	}


	#line 1 "u_config.r_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",1073741824, typeof(Sybase.PowerBuilder.PBLong),
"X",157, typeof(Sybase.PowerBuilder.PBInt),
"Y",595, typeof(Sybase.PowerBuilder.PBInt),
"Width",4268, typeof(Sybase.PowerBuilder.PBInt),
"Height",1354, typeof(Sybase.PowerBuilder.PBInt))]
	public class c__r_1 : Sybase.PowerBuilder.Web.PBRectangle
	{
		#line hidden

		void _init()
		{
			LineColor = new Sybase.PowerBuilder.PBLong(33554432);
			#line hidden
			LineThickness = new Sybase.PowerBuilder.PBInt(3);
			#line hidden
			FillColor = new Sybase.PowerBuilder.PBLong(1073741824);
			#line hidden
			X = new Sybase.PowerBuilder.PBInt(157);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(595);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4268);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1354);
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
		this.ID = "u_config";
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.ConstructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.constructor);

		OnInitialUpdate();
	}

	public c__u_config()
	{
		_init();
	}

}
 