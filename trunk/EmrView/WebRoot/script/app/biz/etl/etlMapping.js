$import("org.ext.ext-base", 
		"org.ext.ext-all",
		"org.ext.ext-patch",
		"org.ext.ext-lang-zh_CN"
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
	
	$import("util.rmi.miniJsonRequestSync")
	util.rmi.miniJsonRequestSync({
		serviceId:"logon",
		uid:"system",
		psw:"123"
	})
	$import("util.schema.SchemaLoader");
	var entryName = "RES_DataSet"
	var re = util.schema.loadSync(entryName);
	if(re.code != 200){
		return;
	}
	var schema = re.schema;
	var items = schema.items;
	for(var i=0;i<items.length;i++){
		var it = items[i];
		it["update"] = "false";
		if(it.id == "DataSetMapping" || it.id == "Parent" || it.id == "Foreignkey"){
			it["display"] = "2"
			delete it["update"];
		}
	}
	$import("app.modules.list.SimpleListView");
	var cfg = {
		title:"etl数据集mapping维护",
		entryName:entryName,
		autoLoadSchema:false,
		width:900,
		height:480,
		actions:[
//			{id:"read",name:"查看"},
//			{id:"create",name:"新建"},
			{id:"update",name:"修改"}
//			{id:"remove",name:"删除"}
		]
	}
	var m = new app.modules.list.SimpleListView(cfg)
	m.initPanel(schema)
	var win = m.getWin();
	win.show();
})
