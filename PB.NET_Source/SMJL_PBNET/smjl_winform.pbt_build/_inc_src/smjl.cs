//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\project\\smjl\\smjl.pbl\\smjl.sra"
#line hidden
#line 1 "smjl"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("smjl",Sybase.PowerBuilder.PBGroupType.Application,"","c:\\project\\smjl\\smjl.pbl",null,"smjl")]
[Sybase.PowerBuilder.PBFieldInfoCollectionAttribute("AppName","smjl", typeof(Sybase.PowerBuilder.PBString))]
public class c__smjl : Sybase.PowerBuilder.PBApplication
{
	#line hidden
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "sqlca", null, "smjl", "", "",null, null, "sqlca")]
	public Sybase.PowerBuilder.PBTransaction sqlca = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "sqlda", null, "smjl", "", "",null, null, "sqlda")]
	public Sybase.PowerBuilder.PBDynamicDescriptionArea sqlda = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "sqlsa", null, "smjl", "", "",null, null, "sqlsa")]
	public Sybase.PowerBuilder.PBDynamicStagingArea sqlsa = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "error", null, "smjl", "", "",null, null, "error")]
	public Sybase.PowerBuilder.PBError error = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "message", null, "smjl", "", "",null, null, "message")]
	public Sybase.PowerBuilder.PBMessage message = null;
	[Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBLong), new int[2]{1,66})] 
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "gl_int", null, "smjl", "", "",null, null, "gl_int [ 66 ] = { 19 , 37 , 54 , 69 , 81 , 91 , 97 , 100 , 99 , 95 , 87 , 76 , 62 , 46 , 28 , 10 , - 10 , - 28 , - 46 , - 62 , - 76 , - 87 , - 95 , - 99 , - 100 , - 97 , - 91 , - 81 , - 69 , - 54 , - 37 , - 19 , 0 , 19 , 37 , 54 , 69 , 81 , 91 , 97 , 100 , 99 , 95 , 87 , 76 , 62 , 46 , 28 , 10 , - 10 , - 28 , - 46 , - 62 , - 76 , - 87 , - 95 , - 99 , - 100 , - 97 , - 91 , - 81 , - 69 , - 54 , - 37 , - 19 , 0 }")]
	public Sybase.PowerBuilder.PBArray gl_int;
	[Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBLong), new int[2]{1,66})] 
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "gl_sta", null, "smjl", "", "",null, null, "gl_sta [ 66 ] = { 27 , 52 , 73 , 89 , 98 , 100 , 94 , 82 , 63 , 40 , 14 , - 14 , - 40 , - 63 , - 82 , - 94 , - 100 , - 98 , - 89 , - 73 , - 52 , - 27 , 0 , 27 , 52 , 73 , 89 , 98 , 100 , 94 , 82 , 63 , 40 , 14 , - 14 , - 40 , - 63 , - 82 , - 94 , - 100 , - 98 , - 89 , - 73 , - 52 , - 27 , 0 , 27 , 52 , 73 , 89 , 98 , 100 , 94 , 82 , 63 , 40 , 14 , - 14 , - 40 , - 63 , - 82 , - 94 , - 100 , - 98 , - 89 , - 73 }")]
	public Sybase.PowerBuilder.PBArray gl_sta;
	[Sybase.PowerBuilder.PBArrayAttribute(typeof(Sybase.PowerBuilder.PBLong), new int[2]{1,66})] 
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "gl_mod", null, "smjl", "", "",null, null, "gl_mod [ 66 ] = { 22 , 43 , 62 , 78 , 90 , 97 , 100 , 97 , 90 , 78 , 62 , 43 , 22 , 0 , - 22 , - 43 , - 62 , - 78 , - 90 , - 97 , - 100 , - 97 , - 90 , - 78 , - 62 , - 43 , - 22 , 0 , 22 , 43 , 62 , 78 , 90 , 97 , 100 , 97 , 90 , 78 , 62 , 43 , 22 , 0 , - 22 , - 43 , - 62 , - 78 , - 90 , - 97 , - 100 , - 97 , - 90 , - 78 , - 62 , - 43 , - 22 , 0 , 22 , 43 , 62 , 78 , 90 , 97 , 100 , 97 , 90 , 78 }")]
	public Sybase.PowerBuilder.PBArray gl_mod;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGlobalVar, "gl_dayrange_show", null, "smjl", "", "",30, typeof(Sybase.PowerBuilder.PBInt), "gl_dayrange_show = 30")]
	public Sybase.PowerBuilder.PBLong gl_dayrange_show = (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(30));
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "smjl", null, "smjl", "", "",null, null, "smjl")]
	[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
	public c__smjl smjl = null;
	[Sybase.PowerBuilder.PBVariableAttribute(Sybase.PowerBuilder.VariableTypeKind.kGroupVar, "w_smjl", null, "w_smjl", "", "",null, null, "w_smjl")]
	public c__w_smjl w_smjl = null;

	public static c__smjl GetCurrentApplication()
	{
		c__smjl currApp = Sybase.PowerBuilder.PBApplication.CurrentApplication as c__smjl;
		if ((currApp != null )&&(currApp.smjl == null))
		{
			currApp.smjl = currApp;
			
		}

		return currApp;
	}

	[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
	public override Sybase.PowerBuilder.PBError Error
	{
		get { return error; }
	}

	[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
	public override Sybase.PowerBuilder.PBMessage Message
	{
		get { return message; }
	}
	
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
		#line hidden
		AppName = new Sybase.PowerBuilder.PBString("smjl");
		#line hidden
		#line hidden
		message = (Sybase.PowerBuilder.PBMessage)this.CreateInstance(typeof(Sybase.PowerBuilder.PBMessage));
		#line hidden
		#line hidden
		sqlca = (Sybase.PowerBuilder.PBTransaction)this.CreateInstance(typeof(Sybase.PowerBuilder.PBTransaction));
		#line hidden
		#line hidden
		sqlda = (Sybase.PowerBuilder.PBDynamicDescriptionArea)this.CreateInstance(typeof(Sybase.PowerBuilder.PBDynamicDescriptionArea));
		#line hidden
		#line hidden
		sqlsa = (Sybase.PowerBuilder.PBDynamicStagingArea)this.CreateInstance(typeof(Sybase.PowerBuilder.PBDynamicStagingArea));
		#line hidden
		#line hidden
		error = (Sybase.PowerBuilder.PBError)this.CreateInstance(typeof(Sybase.PowerBuilder.PBError));
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(sqlca);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(sqlda);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(sqlsa);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(error);
		#line hidden
		#line hidden
		Sybase.PowerBuilder.Win.PBSession.CurrentSession.DestroyObject(message);
		#line hidden
	}

	#line 1 "smjl.open"
	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("open")]
	public override void open(Sybase.PowerBuilder.PBString commandline)
	{
		#line hidden
		#line 1
		Sybase.PowerBuilder.Win.PBSystemFunctions.Open(ref c__smjl.GetCurrentApplication().w_smjl);
		#line hidden
	}

	void _init()
	{
		AppName = new Sybase.PowerBuilder.PBString("smjl");
		#line hidden
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);
		this.OpenEvent += new Sybase.PowerBuilder.PBOpenHandler(this.open);
	}

	public override void InitUnInitedVariables()
	{
		base.InitUnInitedVariables();
		gl_int = (Sybase.PowerBuilder.PBBoundedLongArray)(( new Sybase.PowerBuilder.PBBoundedLongArray( new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2]{1, 66},false), new Sybase.PowerBuilder.PBLong[66]{
		(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(19)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(37)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(54)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(69)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(81)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(91)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(99)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(95)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(87)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(76)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(46)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(28)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(10)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-10)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-28)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-46)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-76)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-87)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-95)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-99)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-91)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-81)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-69)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-54)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-37)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-19)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(19)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(37)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(54)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(69)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(81)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(91)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(99)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(95)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(87)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(76)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(46)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(28)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(10)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-10)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-28)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-46)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-76)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-87)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-95)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-99)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-91)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-81)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-69)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-54)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-37)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-19)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)) })).ToBounded(typeof(Sybase.PowerBuilder.PBLong), new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2] {1,66}, false)));
		gl_sta = (Sybase.PowerBuilder.PBBoundedLongArray)(( new Sybase.PowerBuilder.PBBoundedLongArray( new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2]{1, 66},false), new Sybase.PowerBuilder.PBLong[66]{
		(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(27)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(52)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(73)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-73)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-52)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-27)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(27)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(52)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(73)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-73)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-52)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-27)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(27)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(52)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(73)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-14)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-40)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-63)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-82)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-94)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-98)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-89)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-73)) })).ToBounded(typeof(Sybase.PowerBuilder.PBLong), new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2] {1,66}, false)));
		gl_mod = (Sybase.PowerBuilder.PBBoundedLongArray)(( new Sybase.PowerBuilder.PBBoundedLongArray( new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2]{1, 66},false), new Sybase.PowerBuilder.PBLong[66]{
		(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(-22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(22)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(43)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(62)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(100)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(97)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(90)),(Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(78)) })).ToBounded(typeof(Sybase.PowerBuilder.PBLong), new Sybase.PowerBuilder.PBArray.ArrayBounds(new int[2] {1,66}, false)));
	}


	public c__smjl()
	{
		_init();
	}

}
 