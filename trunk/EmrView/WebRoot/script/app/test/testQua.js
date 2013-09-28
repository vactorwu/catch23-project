$styleSheet("ext.ext-all")
$styleSheet("ext.ext-patch")
$styleSheet("app.desktop.Desktop")

$import("org.ext.ext-base",
		"org.ext.ext-all",
		"org.ext.ext-patch",
		"org.ext.ext-lang-zh_CN"
)

Ext.BLANK_IMAGE_URL = ClassLoader.appRootOffsetPath +"resources/s.gif"

Ext.onReady(function(){
	Ext.QuickTips.init();
	$import("util.rmi.miniJsonRequestSync");
	util.rmi.miniJsonRequestSync({
		serviceId : "logon",
		uid : "system",
		psw : "123"
	});

	$import("sys.qua.QuaMain");
//	$import("sys.qua.QuaError");
//	$import("sys.qua.QuaOga");
	var grid = new sys.qua.QuaMain({
		
	});

	  var win = new Ext.Window({
	  	title: '数据质控', 
	  	layout:'fit', 
	  	items:grid.initPanel()
	  });
	  win.show();
})