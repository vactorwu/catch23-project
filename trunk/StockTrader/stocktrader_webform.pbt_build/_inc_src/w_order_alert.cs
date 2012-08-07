//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl\\w_order_alert.srw"
#line hidden
#line 1 "w_order_alert"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("w_order_alert",Sybase.PowerBuilder.PBGroupType.Window,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl",null)]
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("Width",3006, typeof(Sybase.PowerBuilder.PBInt),
"Height",1446, typeof(Sybase.PowerBuilder.PBInt),
"TitleBar",true, typeof(Sybase.PowerBuilder.PBBoolean),
"Title","Order Alert", typeof(Sybase.PowerBuilder.PBString),
"ControlMenu",true, typeof(Sybase.PowerBuilder.PBBoolean),
"MinBox",true, typeof(Sybase.PowerBuilder.PBBoolean),
"MaxBox",true, typeof(Sybase.PowerBuilder.PBBoolean),
"Resizable",true, typeof(Sybase.PowerBuilder.PBBoolean),
"BackColor",20790589, typeof(Sybase.PowerBuilder.PBLong),
"Icon","AppIcon!", typeof(Sybase.PowerBuilder.PBString),
"Center",true, typeof(Sybase.PowerBuilder.PBBoolean))]
public class c__w_order_alert : Sybase.PowerBuilder.Web.PBWindow
{
	#line hidden
	public c__w_order_alert.c__dw_result dw_result = null;
	public c__w_order_alert.c__pb_ok pb_ok = null;
	public c__w_order_alert.c__st_label st_label = null;

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
		#line hidden
		this.dw_result = (c__dw_result)this.CreateInstance(this, "c__dw_result");
		#line hidden
		#line hidden
		this.pb_ok = (c__pb_ok)this.CreateInstance(this, "c__pb_ok");
		#line hidden
		#line hidden
		this.st_label = (c__st_label)this.CreateInstance(this, "c__st_label");
		#line hidden
		#line hidden
		this.Control.AssignFrom((Sybase.PowerBuilder.PBArray)( new Sybase.PowerBuilder.PBBoundedArray(typeof(Sybase.PowerBuilder.Web.PBDragObject),  new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2]{1, 3},false), new Sybase.PowerBuilder.Web.PBDragObject[3]{
		(Sybase.PowerBuilder.Web.PBDragObject)(this.dw_result),(Sybase.PowerBuilder.Web.PBDragObject)(this.pb_ok),(Sybase.PowerBuilder.Web.PBDragObject)(this.st_label) })));
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.dw_result);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.pb_ok);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.st_label);
		#line hidden
	}

	#line 1 "w_order_alert.open"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("open")]
	[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_open)]
	public override Sybase.PowerBuilder.PBLong open()
	{
		#line hidden
		c__n_order ln_order = null;
		Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 1
		dw_result.Reset();
		#line hidden
		#line 2
		dw_result.InsertRow((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)));
		#line hidden
		#line 4
		ln_order = (c__n_order)(((Sybase.PowerBuilder.PBPowerObject)(Sybase.PowerBuilder.PBSystemFunctions.PBCheckNull(c__stocktrader.GetCurrentApplication().message.PowerObjectParm))));
		#line hidden
		#line 5
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("price"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.price))));
		#line hidden
		#line 6
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("orderid"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.orderid))));
		#line hidden
		#line 7
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("orderstatus"), ln_order.orderstatus);
		#line hidden
		#line 8
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("opendate"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.opendate))));
		#line hidden
		#line 9
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("completiondate"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.completiondate))));
		#line hidden
		#line 10
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("orderfee"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.orderfee))));
		#line hidden
		#line 11
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("ordertype"), ln_order.ordertype);
		#line hidden
		#line 12
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("symbol"), ln_order.symbol);
		#line hidden
		#line 13
		dw_result.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("quantity"), (Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ln_order.quantity))));
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
	}

	#line 1 "w_order_alert.dw_result"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",110, typeof(Sybase.PowerBuilder.PBInt),
"Y",291, typeof(Sybase.PowerBuilder.PBInt),
"Width",2750, typeof(Sybase.PowerBuilder.PBInt),
"Height",781, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",10, typeof(Sybase.PowerBuilder.PBInt),
"DataObject","d_order_display", typeof(Sybase.PowerBuilder.PBString))]
	public class c__dw_result : c__u_dw
	{
		#line hidden

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(110);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(291);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(2750);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(781);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(10);
			#line hidden
			DataObject = new Sybase.PowerBuilder.PBString("d_order_display");
			#line hidden
			this.ID = "dw_result";

			OnInitialUpdate();
		}

		public c__dw_result()
		{
			_init();
		}

	}


	#line 1 "w_order_alert.pb_ok"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",1408, typeof(Sybase.PowerBuilder.PBInt),
"Y",1146, typeof(Sybase.PowerBuilder.PBInt),
"Width",355, typeof(Sybase.PowerBuilder.PBInt),
"Height",144, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",10, typeof(Sybase.PowerBuilder.PBInt),
"TextSize",-12, typeof(Sybase.PowerBuilder.PBInt),
"Weight",700, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"Text","OK", typeof(Sybase.PowerBuilder.PBString),
"OriginalSize",true, typeof(Sybase.PowerBuilder.PBBoolean),
"TextColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"BackColor",27021878, typeof(Sybase.PowerBuilder.PBLong))]
	public class c__pb_ok : Sybase.PowerBuilder.Web.PBPictureButton
	{
		#line hidden

		#line 1 "w_order_alert.pb_ok.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_bnclicked)]
		public override Sybase.PowerBuilder.PBLong clicked()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			Sybase.PowerBuilder.Web.PBSystemFunctions.Close((Sybase.PowerBuilder.Web.PBWindow)(((c__w_order_alert)(this.Parent))));
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(1408);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(1146);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(355);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(144);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(10);
			#line hidden
			TextSize = new Sybase.PowerBuilder.PBInt(-12);
			#line hidden
			Weight = new Sybase.PowerBuilder.PBInt(700);
			#line hidden
			FontCharSet = (new Sybase.PowerBuilder.PBFontCharSetValue(Sybase.PowerBuilder.PBFontCharSet.Ansi));
			#line hidden
			FontPitch = (new Sybase.PowerBuilder.PBFontPitchValue(Sybase.PowerBuilder.PBFontPitch.Variable));
			#line hidden
			FontFamily = (new Sybase.PowerBuilder.PBFontFamilyValue(Sybase.PowerBuilder.PBFontFamily.Swiss));
			#line hidden
			FaceName = new Sybase.PowerBuilder.PBString("Trebuchet MS");
			#line hidden
			Text = new Sybase.PowerBuilder.PBString("OK");
			#line hidden
			OriginalSize = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			VTextAlign = (new Sybase.PowerBuilder.PBVTextAlignValue(Sybase.PowerBuilder.PBVTextAlign.VCenter));
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(30578299);
			#line hidden
			BackColor = new Sybase.PowerBuilder.PBLong(27021878);
			#line hidden
			this.ID = "pb_ok";
			this.ClickedEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.clicked);

			OnInitialUpdate();
		}

		public c__pb_ok()
		{
			_init();
		}

	}


	#line 1 "w_order_alert.st_label"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",117, typeof(Sybase.PowerBuilder.PBInt),
"Y",51, typeof(Sybase.PowerBuilder.PBInt),
"Width",1232, typeof(Sybase.PowerBuilder.PBInt),
"Height",128, typeof(Sybase.PowerBuilder.PBInt),
"TextSize",-18, typeof(Sybase.PowerBuilder.PBInt),
"Weight",700, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"TextColor",16777215, typeof(Sybase.PowerBuilder.PBLong),
"BackColor",20790589, typeof(Sybase.PowerBuilder.PBLong),
"Text","Order Alert", typeof(Sybase.PowerBuilder.PBString),
"FocusRectangle",false, typeof(Sybase.PowerBuilder.PBBoolean))]
	public class c__st_label : Sybase.PowerBuilder.Web.PBStaticText
	{
		#line hidden

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(117);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(51);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(1232);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(128);
			#line hidden
			TextSize = new Sybase.PowerBuilder.PBInt(-18);
			#line hidden
			Weight = new Sybase.PowerBuilder.PBInt(700);
			#line hidden
			FontCharSet = (new Sybase.PowerBuilder.PBFontCharSetValue(Sybase.PowerBuilder.PBFontCharSet.Ansi));
			#line hidden
			FontPitch = (new Sybase.PowerBuilder.PBFontPitchValue(Sybase.PowerBuilder.PBFontPitch.Variable));
			#line hidden
			FontFamily = (new Sybase.PowerBuilder.PBFontFamilyValue(Sybase.PowerBuilder.PBFontFamily.Swiss));
			#line hidden
			FaceName = new Sybase.PowerBuilder.PBString("Trebuchet MS");
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(16777215);
			#line hidden
			BackColor = new Sybase.PowerBuilder.PBLong(20790589);
			#line hidden
			Text = new Sybase.PowerBuilder.PBString("Order Alert");
			#line hidden
			FocusRectangle = new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
			this.ID = "st_label";

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	void _init()
	{
		Width = new Sybase.PowerBuilder.PBInt(3006);
		#line hidden
		Height = new Sybase.PowerBuilder.PBInt(1446);
		#line hidden
		TitleBar = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		Title = new Sybase.PowerBuilder.PBString("Order Alert");
		#line hidden
		ControlMenu = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		MinBox = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		MaxBox = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		Resizable = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		BackColor = new Sybase.PowerBuilder.PBLong(20790589);
		#line hidden
		Icon = new Sybase.PowerBuilder.PBString("AppIcon!");
		#line hidden
		Center = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.OpenEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.open);
	}

	public c__w_order_alert()
	{
		_init();
	}

}
 