//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx\\u_buy_sell.sru"
#line hidden
#line 1 "u_buy_sell"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_buy_sell",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx",null)]
[System.Diagnostics.DebuggerDisplay("",Type="u_buy_sell")]
public class c__u_buy_sell : c__u_basepage
{
	#line hidden
	public c__u_buy_sell.c__r_2 r_2 = null;
	public c__u_buy_sell.c__sle_amount sle_amount = null;
	public c__u_buy_sell.c__pb_buy_sell pb_buy_sell = null;
	public c__u_buy_sell.c__st_action st_action = null;
	public c__u_buy_sell.c__st_1 st_1 = null;
	public c__u_buy_sell.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "is_mode", null, "u_buy_sell", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBString is_mode = Sybase.PowerBuilder.PBString.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "is_symbol", null, "u_buy_sell", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBString is_symbol = Sybase.PowerBuilder.PBString.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "il_shares", null, "u_buy_sell", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBLong il_shares = Sybase.PowerBuilder.PBLong.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "il_holdingid", null, "u_buy_sell", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBLong il_holdingid = Sybase.PowerBuilder.PBLong.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "idc_price", null, "u_buy_sell", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBDouble idc_price = Sybase.PowerBuilder.PBDouble.DefaultValue;
	public new c__u_buy_sell.c__st_message st_message
	{
		get { return (c__u_buy_sell.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_buy_sell.c__st_timestamp st_timestamp
	{
		get { return (c__u_buy_sell.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_buy_sell.c__st_asfo st_asfo
	{
		get { return (c__u_buy_sell.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_buy_sell.c__st_label st_label
	{
		get { return (c__u_buy_sell.c__st_label)base.st_label; }
		set { base.st_label = value; }
	}

	#line 1 "u_buy_sell.of_action(Q)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_action", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual void of_action()
	{
		#line hidden
		Sybase.PowerBuilder.PBString invalid_number = Sybase.PowerBuilder.PBString.DefaultValue;
		c__n_order ln_order = null;
		Sybase.PowerBuilder.PBException e = null;
		try
		{
			try
			{
				#line 5
				if ((Sybase.PowerBuilder.PBBoolean)(is_mode == new Sybase.PowerBuilder.PBString("S")))
				#line hidden
				{
					#line 6
					invalid_number = new Sybase.PowerBuilder.PBString("Please enter a valid number between 1 and ");
					#line hidden
					#line 7
					invalid_number += Sybase.PowerBuilder.WPF.PBSystemFunctions.String((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(il_shares))));
					#line hidden
					#line 8
					if ((Sybase.PowerBuilder.PBBoolean)(!(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNumber(sle_amount.Text))| (Sybase.PowerBuilder.WPF.PBSystemFunctions.Long((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(sle_amount.Text))))> il_shares)))
					#line hidden
					{
						#line 9
						this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), invalid_number);
						#line hidden
						#line 10
						return;
						#line hidden
					}
					#line 12
					this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
					#line hidden
					#line 13
					ln_order = c__stocktrader.GetCurrentApplication().gn_controller.of_sell(il_holdingid, Sybase.PowerBuilder.WPF.PBSystemFunctions.Long((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(sle_amount.Text)))));
					#line hidden
				}
				else
				{
					#line 15
					invalid_number = new Sybase.PowerBuilder.PBString("Please enter a valid number greater than 1");
					#line hidden
					#line 16
					if ((Sybase.PowerBuilder.PBBoolean)(!(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNumber(sle_amount.Text))| (Sybase.PowerBuilder.WPF.PBSystemFunctions.Long((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(sle_amount.Text))))< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))))
					#line hidden
					{
						#line 17
						this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), invalid_number);
						#line hidden
						#line 18
						return;
						#line hidden
					}
					#line 20
					this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
					#line hidden
					#line 21
					ln_order = c__stocktrader.GetCurrentApplication().gn_controller.of_buy(c__stocktrader.GetCurrentApplication().gn_controller.of_get_profile_id(), is_symbol, (Sybase.PowerBuilder.PBDouble)(Sybase.PowerBuilder.WPF.PBSystemFunctions.Long((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(sle_amount.Text))))));
					#line hidden
				}
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
		#line 23
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
		#line hidden
		{
			e = __PB_TEMP_e__temp.E;
			#line 24
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Transaction Failed ")+ e.GetMessage());
			#line hidden
			#line 25
			return;
			#line hidden
		}
		#line 28
		Sybase.PowerBuilder.WPF.PBSystemFunctions.OpenWithParm(ref c__stocktrader.GetCurrentApplication().w_order_alert, (Sybase.PowerBuilder.PBPowerObject)(ln_order));
		#line hidden
		#line 29
		c__stocktrader.GetCurrentApplication().w_stocktrader.ue_display_orders();
		#line hidden
	}

	#line 1 "u_buy_sell.of_initialize(QSLLSM)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_initialize", new string[]{"string", "long", "long", "string", "decimal"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual void of_initialize(Sybase.PowerBuilder.PBString as_buy_sell_flag, Sybase.PowerBuilder.PBLong al_shares, Sybase.PowerBuilder.PBLong al_holdingid, Sybase.PowerBuilder.PBString as_symbol, Sybase.PowerBuilder.PBDecimal adc_price)
	{
		#line hidden
		#line 1
		is_mode = as_buy_sell_flag;
		#line hidden
		#line 2
		il_shares = al_shares;
		#line hidden
		#line 3
		il_holdingid = al_holdingid;
		#line hidden
		#line 4
		is_symbol = as_symbol;
		#line hidden
		#line 5
		idc_price = (Sybase.PowerBuilder.PBDouble)(adc_price);
		#line hidden
		#line 7
		if ((Sybase.PowerBuilder.PBBoolean)(as_buy_sell_flag == new Sybase.PowerBuilder.PBString("B")))
		#line hidden
		{
			#line 8
			pb_buy_sell.Text = new Sybase.PowerBuilder.PBString("Buy");
			#line hidden
			#line 9
			st_action.Text = ((new Sybase.PowerBuilder.PBString("You have requested to buy shares of ")+ as_symbol)+ new Sybase.PowerBuilder.PBString(" which is currently trading at "))+ Sybase.PowerBuilder.WPF.PBSystemFunctions.String((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(adc_price))), new Sybase.PowerBuilder.PBString("$#,###.00"));
			#line hidden
		}
		else
		{
			#line 11
			pb_buy_sell.Text = new Sybase.PowerBuilder.PBString("Sell");
			#line hidden
			#line 12
			st_action.Text = (((((new Sybase.PowerBuilder.PBString("You have requested to sell all or part of your holding #")+ Sybase.PowerBuilder.WPF.PBSystemFunctions.String((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(al_holdingid)))))+ new Sybase.PowerBuilder.PBString(". This holding has a total of "))+ Sybase.PowerBuilder.WPF.PBSystemFunctions.String((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(al_shares)))))+ new Sybase.PowerBuilder.PBString(" shares of stock "))+ as_symbol)+ new Sybase.PowerBuilder.PBString(".  Please input how many shares to sell.");
			#line hidden
		}
		#line 15
		sle_amount.Text = new Sybase.PowerBuilder.PBString("");
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
		this.r_2 = (c__r_2)this.CreateInstance(this, "c__r_2");
		#line hidden
		#line hidden
		this.sle_amount = (c__sle_amount)this.CreateInstance(this, "c__sle_amount");
		#line hidden
		#line hidden
		this.pb_buy_sell = (c__pb_buy_sell)this.CreateInstance(this, "c__pb_buy_sell");
		#line hidden
		#line hidden
		this.st_action = (c__st_action)this.CreateInstance(this, "c__st_action");
		#line hidden
		#line hidden
		this.st_1 = (c__st_1)this.CreateInstance(this, "c__st_1");
		#line hidden
		#line hidden
		this.r_1 = (c__r_1)this.CreateInstance(this, "c__r_1");
		#line hidden
		#line hidden
		icurrent = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.WPF.PBSystemFunctions.UpperBound((Sybase.PowerBuilder.PBAny)(this.Control)));
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.r_2);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.sle_amount);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(3)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.pb_buy_sell);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(4)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.st_action);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(5)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.st_1);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(6)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.r_1);
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
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.r_2);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.sle_amount);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.pb_buy_sell);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.st_action);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.st_1);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_buy_sell.ue_setstate"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_setstate")]
	public override void ue_setstate()
	{
		#line hidden
		#line 1
		base.ue_setstate();
		#line hidden
		#line 1
		this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 2
		sle_amount.Text = new Sybase.PowerBuilder.PBString("");
		#line hidden
	}

	#line 1 "u_buy_sell.st_message"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_message")]
	public new class c__st_message : c__u_basepage.c__st_message
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_message()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.st_timestamp"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_timestamp")]
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


	#line 1 "u_buy_sell.st_asfo"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_asfo")]
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


	#line 1 "u_buy_sell.st_label"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_label")]
	public new class c__st_label : c__u_basepage.c__st_label
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.r_2"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="r_2")]
	public class c__r_2 : Sybase.PowerBuilder.WPF.PBRectangle
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__r_2()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.sle_amount"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="sle_amount")]
	public class c__sle_amount : Sybase.PowerBuilder.WPF.PBSingleLineEdit
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__sle_amount()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.pb_buy_sell"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="pb_buy_sell")]
	public class c__pb_buy_sell : Sybase.PowerBuilder.WPF.PBPictureButton
	{
		#line hidden

		#line 1 "u_buy_sell.pb_buy_sell.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_bnclicked)]
		public override Sybase.PowerBuilder.PBLong clicked()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			Sybase.PowerBuilder.PBMethod.InvokeDynamic(((c__u_buy_sell)(this.Parent)), "of_action", new Sybase.PowerBuilder.PBArgument[0] {}, 0);
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			this.ClickedEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.clicked);

			OnInitialUpdate();
		}

		public c__pb_buy_sell()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.st_action"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_action")]
	public class c__st_action : Sybase.PowerBuilder.WPF.PBStaticText
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_action()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.st_1"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="st_1")]
	public class c__st_1 : Sybase.PowerBuilder.WPF.PBStaticText
	{
		#line hidden

		void _init()
		{

			OnInitialUpdate();
		}

		public c__st_1()
		{
			_init();
		}

	}


	#line 1 "u_buy_sell.r_1"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="r_1")]
	public class c__r_1 : Sybase.PowerBuilder.WPF.PBRectangle
	{
		#line hidden

		void _init()
		{

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

		OnInitialUpdate();
	}

	public c__u_buy_sell()
	{
		_init();
	}

}
 