$import("org.ext.ext-base", 
		"org.ext.ext-all", 
		"org.ext.ext-patch",
		"org.ext.ext-lang-zh_CN",
		"app.desktop.Module",
		"app.desktop.TaskManager", 
		"app.desktop.plugins.Logon",
		"app.viewport.Desktop", 
		"app.viewport.App"
);

$styleSheet("ext.ext-all")
$styleSheet("ext.ext-patch")
$styleSheet("app.desktop.Desktop")

Ext.BLANK_IMAGE_URL = ClassLoader.appRootOffsetPath + "resources/s.gif"

var __docRdy = false
Ext.onReady(function() {
	if (__docRdy) {
		return
	}
	__docRdy = true
	Ext.QuickTips.init();
	$import("util.rmi.miniJsonRequestSync")
	util.rmi.miniJsonRequestSync({
		serviceId:"logon",
		uid:"system",
		rid:"system",
		psw:"123"
	})
	
		if(!this.list){
			var cfg = {
					entryName:"BASE_Project",
					actions:[
					        {id:"read",name:"read"},
					        {id:"create",name:"create"},
					        {id:"print",name:"print"}
					         ]
				}
				$import("app.modules.list.EditorListView")
				var m = new app.modules.list.EditorListView(cfg);
			this.list = m
		}
		var win = this.list.getWin();
		win.show();
		win.maximize();
	
})
