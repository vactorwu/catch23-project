//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\soapclasses.pbl\\soapexception.sru"
#line hidden
#line 1 "soapexception"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("soapexception",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\users\\r. yakov werde\\documents\\sybase\\referenceapp\\stocktrader\\soapclasses.pbl","pbwsclient120.pbx")]
public class c__soapexception : Sybase.PowerBuilder.PBRuntimeError, System.IDisposable
{
	#line hidden
	private static string _dllPath = @"pbwsclient120.pbx";
	private static Sybase.PowerBuilder.PBNI.PBExtension _dll;
	private static Sybase.PowerBuilder.PBNI.PBNativeClass _class;
	protected Sybase.PowerBuilder.PBNI.PBNativeObject _object;

	private bool __disposed = false;

	public override void Dispose(bool disposing)
	{
		if (!__disposed)
		{
			if (disposing && (_object != null))
			{
				_object.Dispose();
			}

			base.Dispose(disposing);
			__disposed = true;
		}
	}

	[Sybase.PowerBuilder.PBFunctionAttribute("getfaultcode", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPbniFunction)]
	public virtual Sybase.PowerBuilder.PBString getfaultcode()
	{
		Sybase.PowerBuilder.IPBValue[] __PBNIInteralArgs = new Sybase.PowerBuilder.IPBValue[0];
		System.Type[] __PBNIInteralArgTypes = new System.Type[0];
		bool[] __PBNIInteralArgsByRef = new bool[0];
		Sybase.PowerBuilder.IPBValue __PBNIInteralReturn = new Sybase.PowerBuilder.PBString();
		_object.Invoke(0, __PBNIInteralArgs, __PBNIInteralArgTypes, __PBNIInteralArgsByRef, ref __PBNIInteralReturn, typeof(Sybase.PowerBuilder.PBString));

		return (Sybase.PowerBuilder.PBString)__PBNIInteralReturn;
	}

	[Sybase.PowerBuilder.PBFunctionAttribute("getfaultstring", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPbniFunction)]
	public virtual Sybase.PowerBuilder.PBString getfaultstring()
	{
		Sybase.PowerBuilder.IPBValue[] __PBNIInteralArgs = new Sybase.PowerBuilder.IPBValue[0];
		System.Type[] __PBNIInteralArgTypes = new System.Type[0];
		bool[] __PBNIInteralArgsByRef = new bool[0];
		Sybase.PowerBuilder.IPBValue __PBNIInteralReturn = new Sybase.PowerBuilder.PBString();
		_object.Invoke(1, __PBNIInteralArgs, __PBNIInteralArgTypes, __PBNIInteralArgsByRef, ref __PBNIInteralReturn, typeof(Sybase.PowerBuilder.PBString));

		return (Sybase.PowerBuilder.PBString)__PBNIInteralReturn;
	}

	[Sybase.PowerBuilder.PBFunctionAttribute("getdetailmessage", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPbniFunction)]
	public virtual Sybase.PowerBuilder.PBString getdetailmessage()
	{
		Sybase.PowerBuilder.IPBValue[] __PBNIInteralArgs = new Sybase.PowerBuilder.IPBValue[0];
		System.Type[] __PBNIInteralArgTypes = new System.Type[0];
		bool[] __PBNIInteralArgsByRef = new bool[0];
		Sybase.PowerBuilder.IPBValue __PBNIInteralReturn = new Sybase.PowerBuilder.PBString();
		_object.Invoke(2, __PBNIInteralArgs, __PBNIInteralArgTypes, __PBNIInteralArgsByRef, ref __PBNIInteralReturn, typeof(Sybase.PowerBuilder.PBString));

		return (Sybase.PowerBuilder.PBString)__PBNIInteralReturn;
	}

	[Sybase.PowerBuilder.PBFunctionAttribute("setdetails", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPbniFunction)]
	public virtual Sybase.PowerBuilder.PBString setdetails(Sybase.PowerBuilder.PBString details)
	{
		Sybase.PowerBuilder.IPBValue[] __PBNIInteralArgs = new Sybase.PowerBuilder.IPBValue[1];
		System.Type[] __PBNIInteralArgTypes = new System.Type[1];
		bool[] __PBNIInteralArgsByRef = new bool[1];
		__PBNIInteralArgs[0] = details;
		if ((object)details == null)
			__PBNIInteralArgTypes[0] = typeof(Sybase.PowerBuilder.PBString);
		else
			__PBNIInteralArgTypes[0] = details.GetType();
		__PBNIInteralArgsByRef[0] = false;
		Sybase.PowerBuilder.IPBValue __PBNIInteralReturn = new Sybase.PowerBuilder.PBString();
		_object.Invoke(3, __PBNIInteralArgs, __PBNIInteralArgTypes, __PBNIInteralArgsByRef, ref __PBNIInteralReturn, typeof(Sybase.PowerBuilder.PBString));

		return (Sybase.PowerBuilder.PBString)__PBNIInteralReturn;
	}


	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
		#line hidden
		base.create();
		#line hidden
		#line hidden
		this.TriggerEvent(new Sybase.PowerBuilder.PBString("constructor"));
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
		#line hidden
		this.TriggerEvent(new Sybase.PowerBuilder.PBString("destructor"));
		#line hidden
		#line hidden
		base.destroy();
		#line hidden
	}

	void _init()
	{
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
	}

	public c__soapexception()
	{
		_init();
		Session = Sybase.PowerBuilder.PBSessionBase.GetCurrentSession();
		_object = _class.CreateObject(Session);
	}


	static c__soapexception()
	{
		Sybase.PowerBuilder.IPBSession session = Sybase.PowerBuilder.PBSessionBase.GetCurrentSession();
		_dll = Sybase.PowerBuilder.PBNI.PBExtension.LoadExtension(session, _dllPath);
		_class = _dll.LoadClass(session, typeof(c__soapexception), "soapexception", true);
	}

}

public class c__soapexceptionE : Sybase.PowerBuilder.PBRuntimeErrorE
{
	public new c__soapexception E;
	public c__soapexceptionE(c__soapexception e) : base(e)
	{
		E = e;
	}
}
 