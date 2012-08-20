//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\project\\weather_net\\proxy.pbl\\proxy.pblx\\weather_proxy_weatherwebservicesoapclient.sru"
#line hidden
#line 1 "weather_proxy_weatherwebservicesoapclient"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("weather_proxy_weatherwebservicesoapclient",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\project\\weather_net\\proxy.pbl\\proxy.pblx",null,"weather")]
[Sybase.PowerBuilder.PBCaseNameAttribute("weather_proxy_WeatherWebServiceSoapClient")]
[System.Diagnostics.DebuggerDisplay("",Type="weather_proxy_weatherwebservicesoapclient")]
public class c__weather_proxy_weatherwebservicesoapclient : Sybase.PowerBuilder.PBNonVisualObject
{
	#line hidden
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "wcfconnectionobject", null, "weather_proxy_WeatherWebServiceSoapClient", null, null, Sybase.PowerBuilder.PBModifier.Public, "")]
	public PBWCF.WCFConnection wcfconnectionobject = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "m_revision", null, "weather_proxy_WeatherWebServiceSoapClient", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.PBInt m_revision = Sybase.PowerBuilder.PBInt.DefaultValue;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kInstanceVar, "m_service", null, "weather_proxy_WeatherWebServiceSoapClient", null, null, Sybase.PowerBuilder.PBModifier.Private, "")]
	protected Sybase.PowerBuilder.WCFRuntime.Service m_service = null;

	#line 1 "weather_proxy_weatherwebservicesoapclient.getsupportcity(S[]S)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("getsupportcity", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	[return: Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBString))] 
	public virtual Sybase.PowerBuilder.PBArray getsupportcity(Sybase.PowerBuilder.PBString byprovincename)
	{
		#line hidden
		Sybase.PowerBuilder.PBArray svcreturnvalue = new Sybase.PowerBuilder.PBUnboundedStringArray();
		#line 3
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		#line 6
		m_service.setMethodName(new Sybase.PowerBuilder.PBString("getSupportCity"));
		#line hidden
		#line 9
		m_service.removeAllArguments();
		#line hidden
		#line 10
		m_service.addArgument((byprovincename).Value, new Sybase.PowerBuilder.PBString("System.String"), new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBBoolean(false));
		#line hidden
		#line 13
		m_service.setReturnType(new Sybase.PowerBuilder.PBString("string[]"));
		#line hidden
		#line 16
		m_service.Invoke();
		#line hidden
		#line 20
		svcreturnvalue.AssignFrom(m_service.getReturnValue());
		#line hidden
		#line 21
		return svcreturnvalue;
		#line hidden
	}

	#line 1 "weather_proxy_weatherwebservicesoapclient.getsupportprovince(S[])"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("getsupportprovince", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	[return: Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBString))] 
	public virtual Sybase.PowerBuilder.PBArray getsupportprovince()
	{
		#line hidden
		Sybase.PowerBuilder.PBArray svcreturnvalue = new Sybase.PowerBuilder.PBUnboundedStringArray();
		#line 3
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		#line 6
		m_service.setMethodName(new Sybase.PowerBuilder.PBString("getSupportProvince"));
		#line hidden
		#line 9
		m_service.removeAllArguments();
		#line hidden
		#line 12
		m_service.setReturnType(new Sybase.PowerBuilder.PBString("string[]"));
		#line hidden
		#line 15
		m_service.Invoke();
		#line hidden
		#line 19
		svcreturnvalue.AssignFrom(m_service.getReturnValue());
		#line hidden
		#line 20
		return svcreturnvalue;
		#line hidden
	}

	#line 1 "weather_proxy_weatherwebservicesoapclient.getsupportdataset(Csystem+data+dataset.)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("getsupportdataset", new string[]{}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual System.Data.DataSet getsupportdataset()
	{
		#line hidden
		System.Data.DataSet svcreturnvalue = null;
		#line 3
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		#line 6
		m_service.setMethodName(new Sybase.PowerBuilder.PBString("getSupportDataSet"));
		#line hidden
		#line 9
		m_service.removeAllArguments();
		#line hidden
		#line 12
		m_service.setReturnType(new Sybase.PowerBuilder.PBString("System.Data.DataSet"));
		#line hidden
		#line 15
		m_service.Invoke();
		#line hidden
		#line 19
		svcreturnvalue = (System.Data.DataSet)(m_service.getReturnValue());
		#line hidden
		#line 20
		return svcreturnvalue;
		#line hidden
	}

	#line 1 "weather_proxy_weatherwebservicesoapclient.getweatherbycityname(S[]S)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("getweatherbycityname", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	[return: Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBString))] 
	public virtual Sybase.PowerBuilder.PBArray getweatherbycityname(Sybase.PowerBuilder.PBString thecityname)
	{
		#line hidden
		Sybase.PowerBuilder.PBArray svcreturnvalue = new Sybase.PowerBuilder.PBUnboundedStringArray();
		#line 3
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		#line 6
		m_service.setMethodName(new Sybase.PowerBuilder.PBString("getWeatherbyCityName"));
		#line hidden
		#line 9
		m_service.removeAllArguments();
		#line hidden
		#line 10
		m_service.addArgument((thecityname).Value, new Sybase.PowerBuilder.PBString("System.String"), new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBBoolean(false));
		#line hidden
		#line 13
		m_service.setReturnType(new Sybase.PowerBuilder.PBString("string[]"));
		#line hidden
		#line 16
		m_service.Invoke();
		#line hidden
		#line 20
		svcreturnvalue.AssignFrom(m_service.getReturnValue());
		#line hidden
		#line 21
		return svcreturnvalue;
		#line hidden
	}

	#line 1 "weather_proxy_weatherwebservicesoapclient.getweatherbycitynamepro(S[]SS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("getweatherbycitynamepro", new string[]{"string", "string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	[return: Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBString))] 
	public virtual Sybase.PowerBuilder.PBArray getweatherbycitynamepro(Sybase.PowerBuilder.PBString thecityname, Sybase.PowerBuilder.PBString theuserid)
	{
		#line hidden
		Sybase.PowerBuilder.PBArray svcreturnvalue = new Sybase.PowerBuilder.PBUnboundedStringArray();
		#line 3
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		#line 6
		m_service.setMethodName(new Sybase.PowerBuilder.PBString("getWeatherbyCityNamePro"));
		#line hidden
		#line 9
		m_service.removeAllArguments();
		#line hidden
		#line 10
		m_service.addArgument((thecityname).Value, new Sybase.PowerBuilder.PBString("System.String"), new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBBoolean(false));
		#line hidden
		#line 11
		m_service.addArgument((theuserid).Value, new Sybase.PowerBuilder.PBString("System.String"), new Sybase.PowerBuilder.PBBoolean(false), new Sybase.PowerBuilder.PBBoolean(false));
		#line hidden
		#line 14
		m_service.setReturnType(new Sybase.PowerBuilder.PBString("string[]"));
		#line hidden
		#line 17
		m_service.Invoke();
		#line hidden
		#line 21
		svcreturnvalue.AssignFrom(m_service.getReturnValue());
		#line hidden
		#line 22
		return svcreturnvalue;
		#line hidden
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
		;
		#line hidden
	}

	#line 1 "weather_proxy_weatherwebservicesoapclient.constructor"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("constructor")]
	[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_constructor)]
	public override Sybase.PowerBuilder.PBLong constructor()
	{
		#line hidden
		PBWCF.WCFEndpointAddress d_endpoint = null;
		PBWCF.WCFBasicHttpBinding d_binding = null;
		Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 2
		m_service =  new Sybase.PowerBuilder.WCFRuntime.Service();
		#line hidden
		#line 5
		m_revision = new Sybase.PowerBuilder.PBInt(1);
		#line hidden
		#line 6
		m_service.setRevision(m_revision);
		#line hidden
		#line 8
		m_service.setAssemblyName(new Sybase.PowerBuilder.PBString("weather_proxy.dll"));
		#line hidden
		#line 10
		m_service.setClassName(new Sybase.PowerBuilder.PBString("weather_proxy.WeatherWebServiceSoapClient"));
		#line hidden
		#line 12
		m_service.setPrefix(new Sybase.PowerBuilder.PBString(""));
		#line hidden
		#line 14
		wcfconnectionobject =  new PBWCF.WCFConnection();
		#line hidden
		#line 18
		d_endpoint =  new PBWCF.WCFEndpointAddress();
		#line hidden
		#line 19
		d_endpoint.URL = new Sybase.PowerBuilder.PBString("http://www.webxml.com.cn/WebServices/WeatherWebService.asmx");
		#line hidden
		#line 20
		wcfconnectionobject.EndpointAddress = d_endpoint;
		#line hidden
		#line 23
		wcfconnectionobject.Timeout = new Sybase.PowerBuilder.PBString("00:10:00");
		#line hidden
		#line 25
		wcfconnectionobject.BindingType = PBWCF.WCFBindingType.BasicHttpBinding;
		#line hidden
		#line 28
		d_binding =  new PBWCF.WCFBasicHttpBinding();
		#line hidden
		#line 30
		d_binding.TransferMode = PBWCF.WSTransferMode.BUFFERED;
		#line hidden
		#line 31
		d_binding.MessageEncoding = PBWCF.WSMessageEncoding.TEXT;
		#line hidden
		#line 32
		d_binding.TextEncoding = PBWCF.WSTextEncoding.UTF8;
		#line hidden
		#line 33
		d_binding.MaxBufferPoolSize = (long)(Sybase.PowerBuilder.PBLongLong)(new Sybase.PowerBuilder.PBLong(524288));
		#line hidden
		#line 34
		d_binding.MaxBufferSize = new Sybase.PowerBuilder.PBLong(65536);
		#line hidden
		#line 35
		d_binding.MaxReceivedMessageSize = (long)(Sybase.PowerBuilder.PBLongLong)(new Sybase.PowerBuilder.PBLong(65536));
		#line hidden
		#line 36
		d_binding.AllowCookies = new Sybase.PowerBuilder.PBBoolean(false);
		#line hidden
		#line 37
		d_binding.BypassProxyOnLocal = new Sybase.PowerBuilder.PBBoolean(false);
		#line hidden
		#line 38
		d_binding.HostNameComparisonMode = PBWCF.WCFHostNameComparisonMode.STRONGWILDCARD;
		#line hidden
		#line 40
		d_binding.ReaderQuotas =  new PBWCF.WCFReaderQuotas();
		#line hidden
		#line 41
		d_binding.ReaderQuotas.MaxArrayLength = (int)(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(16384));
		#line hidden
		#line 42
		d_binding.ReaderQuotas.MaxBytesPerRead = (int)(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(4096));
		#line hidden
		#line 43
		d_binding.ReaderQuotas.MaxDepth = (int)(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(32));
		#line hidden
		#line 44
		d_binding.ReaderQuotas.MaxNameTableCharCount = (int)(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(16384));
		#line hidden
		#line 45
		d_binding.ReaderQuotas.MaxStringContentLength = (int)(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(8192));
		#line hidden
		#line 47
		d_binding.Security =  new PBWCF.BasicHttpSecurity();
		#line hidden
		#line 48
		d_binding.Security.SecurityMode = PBWCF.BasicHttpSecurityMode.NONE;
		#line hidden
		#line 50
		wcfconnectionobject.BasicHttpBinding = d_binding;
		#line hidden
		#line 53
		wcfconnectionobject.ProxyServer.CredentialType = PBWCF.HttpProxyCredentialType.NONE;
		#line hidden
		#line 54
		wcfconnectionobject.ProxyServer.UseDefaultWebProxy = new Sybase.PowerBuilder.PBBoolean(true);
		#line hidden
		#line 56
		m_service.setConnectionOption(wcfconnectionobject);
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
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

	#line 1 "weather_proxy_weatherwebservicesoapclient.destructor"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destructor")]
	[Sybase.PowerBuilder.PBEventToken(Sybase.PowerBuilder.PBEventTokens.pbm_destructor)]
	public override Sybase.PowerBuilder.PBLong destructor()
	{
		#line hidden
		Sybase.PowerBuilder.PBLong ancestorreturnvalue = Sybase.PowerBuilder.PBLong.DefaultValue;
		#line 2
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(wcfconnectionobject);
		#line hidden
		#line 3
		Sybase.PowerBuilder.WPF.PBSession.CurrentSession.DestroyObject(m_service);
		#line hidden
		return new Sybase.PowerBuilder.PBLong(0);
	}

	void _init()
	{
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.ConstructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.constructor);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.DestructorEvent += new Sybase.PowerBuilder.PBM_EventHandler(this.destructor);
	}

	public c__weather_proxy_weatherwebservicesoapclient()
	{
		_init();
	}


	public static explicit operator c__weather_proxy_weatherwebservicesoapclient(Sybase.PowerBuilder.PBAny v)
	{
		if (v.Value is Sybase.PowerBuilder.PBUnboundedAnyArray)
		{
			Sybase.PowerBuilder.PBUnboundedAnyArray a = (Sybase.PowerBuilder.PBUnboundedAnyArray)v.Value;
			c__weather_proxy_weatherwebservicesoapclient vv = new c__weather_proxy_weatherwebservicesoapclient();
			if (vv.FromUnboundedAnyArray(a) > 0)
			{
				return vv;
			}
			else
			{
				return null;
			}
		}
		else
		{
			return (c__weather_proxy_weatherwebservicesoapclient) v.Value;
		}
	}
}
 