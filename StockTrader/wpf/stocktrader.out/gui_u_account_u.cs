//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx\\u_account.sru"
#line hidden
#line 1 "u_account"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("u_account",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\wpf\\gui.pbl\\gui.pblx",null)]
[System.Diagnostics.DebuggerDisplay("",Type="u_account")]
public class c__u_account : c__u_basepage
{
	#line hidden
	public c__u_account.c__dw_profile dw_profile = null;
	public c__u_account.c__dw_account dw_account = null;
	public c__u_account.c__pb_save pb_save = null;
	public c__u_account.c__r_2 r_2 = null;
	public c__u_account.c__r_1 r_1 = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "missing_password", null, "u_account", "Password missing or doesn\t match", typeof(Sybase.PowerBuilder.PBString), Sybase.PowerBuilder.PBModifier.Private, "= 'Password missing or doesn~t match'")]
	[Sybase.PowerBuilder.PBConstantAttribute()]
	static protected Sybase.PowerBuilder.PBString missing_password = new Sybase.PowerBuilder.PBString("Password missing or doesn\t match");
	public new c__u_account.c__st_message st_message
	{
		get { return (c__u_account.c__st_message)base.st_message; }
		set { base.st_message = value; }
	}
	public new c__u_account.c__st_timestamp st_timestamp
	{
		get { return (c__u_account.c__st_timestamp)base.st_timestamp; }
		set { base.st_timestamp = value; }
	}
	public new c__u_account.c__st_asfo st_asfo
	{
		get { return (c__u_account.c__st_asfo)base.st_asfo; }
		set { base.st_asfo = value; }
	}
	public new c__u_account.c__st_label st_label
	{
		get { return (c__u_account.c__st_label)base.st_label; }
		set { base.st_label = value; }
	}

	#line 1 "u_account.of_validate_password(B)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_validate_password", new string[]{}, Sybase.PowerBuilder.PBModifier.Private, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	private Sybase.PowerBuilder.PBBoolean of_validate_password()
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_orig = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_copy = Sybase.PowerBuilder.PBString.DefaultValue;
		#line 4
		ls_orig = dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password"));
		#line hidden
		#line 5
		ls_copy = dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password_confirm"));
		#line hidden
		#line 7
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_orig))))| (Sybase.PowerBuilder.WPF.PBSystemFunctions.Len(ls_orig)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))))
		#line hidden
		{
			#line 7
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		#line 8
		if ((Sybase.PowerBuilder.PBBoolean)(Sybase.PowerBuilder.WPF.PBSystemFunctions.IsNull((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_copy))))| (Sybase.PowerBuilder.WPF.PBSystemFunctions.Len(ls_copy)< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))))
		#line hidden
		{
			#line 8
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		#line 11
		if ((Sybase.PowerBuilder.PBBoolean)(ls_orig != ls_copy))
		#line hidden
		{
			#line 12
			return new Sybase.PowerBuilder.PBBoolean(false);
			#line hidden
		}
		else
		{
			#line 14
			return new Sybase.PowerBuilder.PBBoolean(true);
			#line hidden
		}
	}

	#line 1 "u_account.of_update(Q)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_update", new string[]{}, Sybase.PowerBuilder.PBModifier.Private, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	private void of_update()
	{
		#line hidden
		Sybase.PowerBuilder.PBString l_missing = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString l_str = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBLong lrow = (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1));
		Sybase.PowerBuilder.PBInt lcol = new Sybase.PowerBuilder.PBInt(1);
		Sybase.PowerBuilder.PBException e = null;
		#line 5
		this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 6
		dw_profile.AcceptText();
		#line hidden
		#line 7
		if ((Sybase.PowerBuilder.PBBoolean)(this.of_validate_password() == new Sybase.PowerBuilder.PBBoolean(false)))
		#line hidden
		{
			#line 8
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), missing_password);
			#line hidden
			#line 9
			return;
			#line hidden
		}
		#line 12
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"fullname"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 13
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"address"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 14
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"email"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 15
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"creditcard"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("Yes"));
		#line hidden
		#line 19
		dw_profile.FindRequired((new Sybase.PowerBuilder.PBDWBufferValue(Sybase.PowerBuilder.PBDWBuffer.Primary)), ref lrow, ref lcol, ref l_missing, new Sybase.PowerBuilder.PBBoolean(false)
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
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Data Entry Error: ")+ dw_profile.Describe(l_str));
			#line hidden
			#line 23
			dw_profile.SetColumn(lcol);
			#line hidden
		}
		else
		{
			try
			{
				try
				{
					#line 28
					c__stocktrader.GetCurrentApplication().gn_controller.of_update_profile(dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("address")), dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("creditcard")), dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("email")), dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("fullname")), dw_profile.GetItemString((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password")), 
						c__stocktrader.GetCurrentApplication().gn_controller.of_get_profile_id());
					#line hidden
					#line 31
					this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Changes Saved"));
					#line hidden
					#line 32
					dw_profile.SetColumn(new Sybase.PowerBuilder.PBInt(1));
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
			#line 33
			catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
			#line hidden
			{
				e = __PB_TEMP_e__temp.E;
				#line 34
				this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Service Error: ")+ e.GetMessage());
				#line hidden
			}
		}
		#line 38
		dw_profile.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("password_confirm"), new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 40
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"fullname"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 41
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"address"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 42
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"email"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 43
		((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)((((Sybase.PowerBuilder.PBExtObject)(dw_profile.Object))[new Sybase.PowerBuilder.PBString(@"creditcard"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"edit"), Sybase.PowerBuilder.PBBoolean.False].Value)))[new Sybase.PowerBuilder.PBString(@"required")] = (Sybase.PowerBuilder.PBAny)(new Sybase.PowerBuilder.PBString("No"));
		#line hidden
		#line 44
		dw_profile.SetFocus();
		#line hidden
	}

	#line 1 "u_account.of_reset(Q)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_reset", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public override void of_reset()
	{
		#line hidden
		#line 1
		dw_profile.Reset();
		#line hidden
		#line 2
		dw_account.Reset();
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
		this.dw_profile = (c__dw_profile)this.CreateInstance(this, "c__dw_profile");
		#line hidden
		#line hidden
		this.dw_account = (c__dw_account)this.CreateInstance(this, "c__dw_account");
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
		icurrent = (Sybase.PowerBuilder.PBInt)(Sybase.PowerBuilder.WPF.PBSystemFunctions.UpperBound((Sybase.PowerBuilder.PBAny)(this.Control)));
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.dw_profile);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(2)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.dw_account);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(3)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.pb_save);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(4)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.r_2);
		#line hidden
		#line hidden
		this.Control[this.Control.Extend((Sybase.PowerBuilder.PBLong)(icurrent)+ (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(5)))] = (Sybase.PowerBuilder.WPF.PBWindowObject)(this.r_1);
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
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.dw_profile);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.dw_account);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.pb_save);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.r_2);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(this.r_1);
		#line hidden
	}

	#line 1 "u_account.ue_setstate"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("ue_setstate")]
	public override void ue_setstate()
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_address = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_creditcard = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_email = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBString ls_fullname = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBException e = null;
		#line 1
		base.ue_setstate();
		#line hidden
		#line 2
		dw_account.SetWSObject(c__stocktrader.GetCurrentApplication().gn_controller.of_get_wsconn());
		#line hidden
		#line 3
		dw_account.Retrieve((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(c__stocktrader.GetCurrentApplication().gn_controller.of_get_profile_id()))));
		#line hidden
		try
		{
			try
			{
				#line 8
				c__stocktrader.GetCurrentApplication().gn_controller.of_getprofile(ref ls_address, ref ls_creditcard, ref ls_email, ref ls_fullname, c__stocktrader.GetCurrentApplication().gn_controller.of_get_profile_id()
					);
				#line hidden
				#line 9
				if ((Sybase.PowerBuilder.PBBoolean)(dw_profile.RowCount()< (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1))))
				#line hidden
				{
					#line 9
					dw_profile.InsertRow((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)));
					#line hidden
				}
				#line 10
				dw_profile.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("address"), ls_address);
				#line hidden
				#line 11
				dw_profile.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("creditcard"), ls_creditcard);
				#line hidden
				#line 12
				dw_profile.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("email"), ls_email);
				#line hidden
				#line 13
				dw_profile.SetItem((Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(1)), new Sybase.PowerBuilder.PBString("fullname"), ls_fullname);
				#line hidden
				#line 14
				dw_profile.SetFocus();
				#line hidden
				#line 15
				this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBString(""));
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
		#line 16
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_e__temp)
		#line hidden
		{
			e = __PB_TEMP_e__temp.E;
			#line 17
			this.of_showmessage(new Sybase.PowerBuilder.PBBoolean(true), new Sybase.PowerBuilder.PBString("Failed to get profile ")+ e.GetMessage());
			#line hidden
		}
	}

	#line 1 "u_account.dw_profile"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("DataObject","d_profiledisplay", typeof(Sybase.PowerBuilder.PBString))]
	[System.Diagnostics.DebuggerDisplay("",Type="dw_profile")]
	public class c__dw_profile : c__u_dw
	{
		#line hidden

		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("create")]
		public override void create()
		{
			#line hidden
			#line hidden
			base.create();
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
		}

		void _init()
		{
			DataObject = new Sybase.PowerBuilder.PBString("d_profiledisplay");
			#line hidden
			this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
			this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__dw_profile()
		{
			_init();
		}

	}


	#line 1 "u_account.dw_account"
	#line hidden
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("DataObject","d_account", typeof(Sybase.PowerBuilder.PBString))]
	[System.Diagnostics.DebuggerDisplay("",Type="dw_account")]
	public class c__dw_account : c__u_dw
	{
		#line hidden

		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("create")]
		public override void create()
		{
			#line hidden
			#line hidden
			base.create();
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
		}

		void _init()
		{
			DataObject = new Sybase.PowerBuilder.PBString("d_account");
			#line hidden
			this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
			this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__dw_account()
		{
			_init();
		}

	}


	#line 1 "u_account.pb_save"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="pb_save")]
	public class c__pb_save : Sybase.PowerBuilder.WPF.PBPictureButton
	{
		#line hidden

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

		#line 1 "u_account.pb_save.clicked"
		#line hidden
		[Sybase.PowerBuilder.PBEventAttribute("clicked")]
		[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_bnclicked)]
		public override Sybase.PowerBuilder.PBLong clicked()
		{
			#line hidden
			Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
			#line 1
			((c__u_account)(this.Parent)).of_update();
			#line hidden
			return new Sybase.PowerBuilder.PBLong(0);
		}

		void _init()
		{
			this.CreateEvent -= new Sybase.PowerBuilder.PBEventHandler(this.create);
			this.DestroyEvent -= new Sybase.PowerBuilder.PBEventHandler(this.destroy);
			this.ClickedEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.clicked);

			OnInitialUpdate();
		}

		public c__pb_save()
		{
			_init();
		}

	}


	#line 1 "u_account.r_2"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="r_2")]
	public class c__r_2 : Sybase.PowerBuilder.WPF.PBRectangle
	{
		#line hidden

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

		void _init()
		{
			this.CreateEvent -= new Sybase.PowerBuilder.PBEventHandler(this.create);
			this.DestroyEvent -= new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__r_2()
		{
			_init();
		}

	}


	#line 1 "u_account.r_1"
	#line hidden
	[System.Diagnostics.DebuggerDisplay("",Type="r_1")]
	public class c__r_1 : Sybase.PowerBuilder.WPF.PBRectangle
	{
		#line hidden

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

		void _init()
		{
			this.CreateEvent -= new Sybase.PowerBuilder.PBEventHandler(this.create);
			this.DestroyEvent -= new Sybase.PowerBuilder.PBEventHandler(this.destroy);

			OnInitialUpdate();
		}

		public c__r_1()
		{
			_init();
		}

	}


	#line 1 "u_account.st_message"
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


	#line 1 "u_account.st_timestamp"
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


	#line 1 "u_account.st_asfo"
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


	#line 1 "u_account.st_label"
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


	void _init()
	{
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

		OnInitialUpdate();
	}

	public c__u_account()
	{
		_init();
	}

}
 