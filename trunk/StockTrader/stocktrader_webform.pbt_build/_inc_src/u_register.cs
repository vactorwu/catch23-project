//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl\\u_register.sru"
#line hidden
#line 1 "u_register"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_register",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\gui.pbl",null)]
public class c__u_register : c__u_basepage
{
	#line hidden
	public c__u_register.c__dw_register dw_register = null;
	public c__u_register.c__pb_save pb_save = null;
	public c__u_register.c__r_2 r_2 = null;
	public c__u_register.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "missing_password", null, "u_register", "Password missing or doesn't match", typeof(Sybase.PowerBuilder.PBString), Sybase.PowerBuilder.PBModifier.Private, "= \"Password missing or doesn't match\"")]
	[Sybase.PowerBuilder.PBConstantAttribute()]
	static protected Sybase.PowerBuilder.PBString missing_password = new Sybase.PowerBuilder.PBString("Password missing or doesn't match");
	public new c__u_register.c__st_message st_message
	{
		get { return (c__u_register.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_register.c__st_timestamp st_timestamp
	{
		get { return (c__u_register.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_register.c__st_asfo st_asfo
	{
		get { return (c__u_register.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_register.c__st_label st_label
	{
		get { return (c__u_register.c__st_label)base.st_label; }
		set { base.st_label = value; }
	}

	#line 1 "u_register.of_update(Q)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_update", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual void of_update()
	{
		#line hidden
		Sybase.PowerBuilder.PBString l_missing = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString l_str = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBLong lrow = (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
		Sybase.PowerBuilder.PBInt lcol = new Sybase.PowerBuilder.PBInt(1);
		Sybase.PowerBuilder.PBException e = null;
		#line 4
		this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 5
		dw_register.AcceptText();
		#line hidden
		#line 7
		if ((Sybase.PowerBuilder.PBBoolean)(this.of_validate_password() == new Sybase.PowerBuilder.PBBoolean(false)))
		#line hidden
		{
			#line 8
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), missing_password);
			#line hidden
			#line 9
			dw_register.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password_confirm"), new Sybase.PowerBuilder.PBString(""));
			#line hidden
			#line 10
			return;
			#line hidden
		}
		#line 13
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"fullname"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 14
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"address"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 15
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"email"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 16
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"creditcard"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 17
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"openingbalance"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 18
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"userid"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 19
		dw_register.FindRequired((new Sybase.PowerBuilder.PBDWBufferValue(Sybase.PowerBuilder.PBDWBuffer.Primary)), ref lrow, ref lcol, ref l_missing, new Sybase.PowerBuilder.PBBoolean(false)
			);
		#line hidden
		#line 20
		if ((Sybase.PowerBuilder.PBBoolean)(lrow != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 21
			l_str += l_missing+ new Sybase.PowerBuilder.PBString(".ValidationMsg");
			#line hidden
			#line 22
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Data Entry Error: ")+ dw_register.Describe(l_str));
			#line hidden
			#line 23
			dw_register.SetColumn(lcol);
			#line hidden
		}
		else
		{
			try
			{
				try
				{
					#line 28
					if ((Sybase.PowerBuilder.PBBoolean)(c__stocktrader.GetCurrentApplication().gn_controller.of_register(dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("userid")), dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password")), dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("fullname")), dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("address")), dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("email")), 
						dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("creditcard")), (dw_register.GetItemDecimal((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("openingbalance"))).ToPBDecimal(-1)) == new Sybase.PowerBuilder.PBBoolean(true)))
					#line hidden
					{
						#line 29
						this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Registration Successful"));
						#line hidden
						#line 30
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"fullname"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 31
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"address"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 32
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"email"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 33
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"creditcard"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 34
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"openingbalance"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 35
						((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"userid"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
						#line hidden
						#line 36
						dw_register.Reset();
						#line hidden
						#line 37
						c__stocktrader.GetCurrentApplication().w_stocktrader.ue_set_page(new Sybase.PowerBuilder.PBString("8"));
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
			#line 39
			catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
			#line hidden
			{
				e = __PB_TEMP_e__temp.E;
				#line 40
				this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("User ID is already in use. Try another User ID"));
				#line hidden
			}
		}
		#line 44
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"fullname"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 45
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"address"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 46
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"email"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 47
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"creditcard"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 48
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"openingbalance"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 49
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_register.Object))[new Sybase.PowerBuilder.PBString(@"userid"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 50
		dw_register.SetFocus();
		#line hidden
	}

	#line 1 "u_register.of_validate_password(B)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_validate_password", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBBoolean of_validate_password()
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_orig = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_copy = Sybase.PowerBuilder.PBString.DefaultValue;
		#line 5
		ls_orig = dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password"));
		#line hidden
		#line 6
		ls_copy = dw_register.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password_confirm"));
		#line hidden
		#line 8
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.Web.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_orig))))| (Sybase.PowerBuilder.Web.PBSystemFunctions.Len(ls_orig)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))))
		#line hidden
		{
			#line 8
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		#line 9
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.Web.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_copy))))| (Sybase.PowerBuilder.Web.PBSystemFunctions.Len(ls_copy)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))))
		#line hidden
		{
			#line 9
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		#line 12
		if ((Sybase.PowerBuilder.PBBoolean)(ls_orig != ls_copy))
		#line hidden
		{
			#line 13
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		else
		{
			#line 15
			return new Sybase.PowerBuilder.PBBoolean(true);
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
		this.dw_register = (c__dw_register)this.CreateInstance(this, "c__dw_register");
		#line hidden
		#line hidden
		this.pb_save = (c__pb_save)this.CreateInstance(this, "c__pb_save");
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
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.dw_register);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.Web.PBWindowObject)(this.pb_save);
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
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.dw_register);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.pb_save);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_2);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Web.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_register.ue_setstate"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_setstate")]
	public override void ue_setstate()
	{
		#line hidden
		#line 1
		base.ue_setstate();
		#line hidden
		#line 1
		dw_register.Reset();
		#line hidden
		#line 2
		dw_register.InsertRow((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)));
		#line hidden
	}

	#line 1 "u_register.st_message"
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


	#line 1 "u_register.st_timestamp"
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


	#line 1 "u_register.st_asfo"
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


	#line 1 "u_register.st_label"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("Text","Register", typeof(Sybase.PowerBuilder.PBString))]
	public new class c__st_label : c__u_basepage.c__st_label
	{
		#line hidden

		void _init()
		{
			Text = new Sybase.PowerBuilder.PBString("Register");
			#line hidden
			this.ID = "st_label";

			OnInitialUpdate();
		}

		public c__st_label()
		{
			_init();
		}

	}


	#line 1 "u_register.dw_register"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",318, typeof(Sybase.PowerBuilder.PBInt),
"Y",810, typeof(Sybase.PowerBuilder.PBInt),
"Width",3994, typeof(Sybase.PowerBuilder.PBInt),
"Height",842, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",10, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"DataObject","d_register", typeof(Sybase.PowerBuilder.PBString))]
	public class c__dw_register : c__u_dw
	{
		#line hidden

		#line 1 "u_register.dw_register.itemerror"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("itemerror")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_dwnitemvalidationerror)]
		public override Sybase.PowerBuilder.PBLong itemerror(Sybase.PowerBuilder.PBLong row, Sybase.PowerBuilder.WinWebDataWindowCommon.PBDWObject dwo, Sybase.PowerBuilder.PBString data)
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			ancestorreturnvalue = base.itemerror(row, dwo, data);
			#line hidden
			#line 1
			return (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
			#line hidden
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(318);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(810);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(3994);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(842);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(10);
			#line hidden
			BringToTop = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			DataObject = new Sybase.PowerBuilder.PBString("d_register");
			#line hidden
			this.ID = "dw_register";
			this.ItemErrorEvent += new Sybase.PowerBuilder.WinWebDataWindowCommon.PBM_EventHandler_dwn_ldws(this.itemerror);

			OnInitialUpdate();
		}

		public c__dw_register()
		{
			_init();
		}

	}


	#line 1 "u_register.pb_save"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("X",3288, typeof(Sybase.PowerBuilder.PBInt),
"Y",1805, typeof(Sybase.PowerBuilder.PBInt),
"Width",380, typeof(Sybase.PowerBuilder.PBInt),
"Height",134, typeof(Sybase.PowerBuilder.PBInt),
"TabOrder",50, typeof(Sybase.PowerBuilder.PBInt),
"BringToTop",true, typeof(Sybase.PowerBuilder.PBBoolean),
"TextSize",-10, typeof(Sybase.PowerBuilder.PBInt),
"Weight",400, typeof(Sybase.PowerBuilder.PBInt),
"FaceName","Trebuchet MS", typeof(Sybase.PowerBuilder.PBString),
"Text","Save", typeof(Sybase.PowerBuilder.PBString),
"Default",true, typeof(Sybase.PowerBuilder.PBBoolean),
"Map3DColors",true, typeof(Sybase.PowerBuilder.PBBoolean),
"PowerTipText","Login to Server", typeof(Sybase.PowerBuilder.PBString),
"TextColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"BackColor",23213090, typeof(Sybase.PowerBuilder.PBLong))]
	public class c__pb_save : Sybase.PowerBuilder.Web.PBPictureButton
	{
		#line hidden

		#line 1 "u_register.pb_save.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_bnclicked)]
		public override Sybase.PowerBuilder.PBLong clicked()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			((c__u_register)(this.Parent)).of_update();
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			X = new Sybase.PowerBuilder.PBInt(3288);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(1805);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(380);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(134);
			#line hidden
			TabOrder = new Sybase.PowerBuilder.PBInt(50);
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
			Text = new Sybase.PowerBuilder.PBString("Save");
			#line hidden
			Default = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			VTextAlign = (new Sybase.PowerBuilder.PBVTextAlignValue(Sybase.PowerBuilder.PBVTextAlign.VCenter));
			#line hidden
			Map3DColors = new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
			PowerTipText = new Sybase.PowerBuilder.PBString("Login to Server");
			#line hidden
			TextColor = new Sybase.PowerBuilder.PBLong(30578299);
			#line hidden
			BackColor = new Sybase.PowerBuilder.PBLong(23213090);
			#line hidden
			this.ID = "pb_save";
			this.ClickedEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.clicked);

			OnInitialUpdate();
		}

		public c__pb_save()
		{
			_init();
		}

	}


	#line 1 "u_register.r_2"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",30578299, typeof(Sybase.PowerBuilder.PBLong),
"X",128, typeof(Sybase.PowerBuilder.PBInt),
"Y",534, typeof(Sybase.PowerBuilder.PBInt),
"Width",4341, typeof(Sybase.PowerBuilder.PBInt),
"Height",1610, typeof(Sybase.PowerBuilder.PBInt))]
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
			X = new Sybase.PowerBuilder.PBInt(128);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(534);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4341);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1610);
			#line hidden

			OnInitialUpdate();
		}

		public c__r_2()
		{
			_init();
		}

	}


	#line 1 "u_register.r_1"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("LineColor",33554432, typeof(Sybase.PowerBuilder.PBLong),
"LineThickness",3, typeof(Sybase.PowerBuilder.PBInt),
"FillColor",1073741824, typeof(Sybase.PowerBuilder.PBLong),
"X",176, typeof(Sybase.PowerBuilder.PBInt),
"Y",573, typeof(Sybase.PowerBuilder.PBInt),
"Width",4268, typeof(Sybase.PowerBuilder.PBInt),
"Height",1507, typeof(Sybase.PowerBuilder.PBInt))]
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
			X = new Sybase.PowerBuilder.PBInt(176);
			#line hidden
			Y = new Sybase.PowerBuilder.PBInt(573);
			#line hidden
			Width = new Sybase.PowerBuilder.PBInt(4268);
			#line hidden
			Height = new Sybase.PowerBuilder.PBInt(1507);
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
		this.ID = "u_register";
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

		OnInitialUpdate();
	}

	public c__u_register()
	{
		_init();
	}

}
 