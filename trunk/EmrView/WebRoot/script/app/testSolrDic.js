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
	
	var createDicField = function(dic) {
			var cls = "util.dictionary.";
			if (!dic.render) {
				cls += "Simple";
			} else {
				cls += dic.render
			}
			cls += "DicFactory"
			$import(cls)
			var factory = eval("(" + cls + ")")
			dic.onlySelectLeaf = dic.onlySelectLeaf || false
			dic.width = dic.width || 800
			var field = factory.createDic(dic)
			field.fieldLabel = dic.alias || "请选择"
			field.name = dic.name
			return field
		}
		var u1 = createDicField({
			id:"mySolrDic",
			render:"Tree",
			alias:"网格地址"
		})
		u1.supportOnKeyQuery = true

	var f = new Ext.FormPanel({
		frame:true,
		labelWidth:80,
		items:[u1]
	})
	var w = new Ext.Window({
		title:"大数据量字典",
		border:false,
		height:120,
		width:960,
		layout:"fit",
		items:f
	})
	w.setPosition([50,50])
	w.show()
	w.getEl().slideIn('b')
	
	
})
