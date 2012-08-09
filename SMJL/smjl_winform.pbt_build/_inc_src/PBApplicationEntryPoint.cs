public class PBApplicationEntryPoint
{

	[System.Diagnostics.DebuggerStepThrough]
	[System.STAThread]
	static void Main()
	{
		c__smjl.ApplicationName = "smjl";
		Sybase.PowerBuilder.PBSessionBase.HasPBExtensions = false;
		Sybase.PowerBuilder.PBSessionBase.MainAssembly = System.Reflection.Assembly.GetExecutingAssembly();
		Sybase.PowerBuilder.Win.PBSession session = Sybase.PowerBuilder.Win.PBSession.CreateSession(
			typeof(c__smjl), 
			@"smjl.pbd");
		c__smjl.GetCurrentApplication().smjl = c__smjl.GetCurrentApplication();
		session.RunWinForm();
	}
}
 