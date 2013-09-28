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
			dic.width = dic.width || 320
			var field = factory.createDic(dic)
			field.fieldLabel = dic.alias || "请选择"
			field.name = dic.name
			return field
		}
		var u1 = createDicField({
			id:"manageUnit2",
			render:"Tree",
			alias:"管辖机构(原始)"
		})
		var u2 = createDicField({
			id:"manageUnit",
			render:"Tree",
			alias:"管辖机构(只显示到中心层级)",
			lengthLimit:9
		})
		var u3 = createDicField({
			id:"manageUnit",
			render:"Tree",
			alias:"管辖机构(只显示到城区层级)",
			lengthLimit:6
		})
		var u4 = createDicField({
			id:"manageUnit",
			render:"Tree",
			alias:"管辖机构(过滤掉城区那一层级)",
			layerConfig:{length:4,layer:1}
		})
		var u5 = createDicField({
			id:"manageUnit",
			render:"Tree",
			alias:"管辖机构(过滤掉城区、中心两层)",
			layerConfig:{length:4,layer:2}
		})
		var u6 = createDicField({
			id:"manageUnit",
			render:"Tree",
			alias:"管辖机构(过滤掉中心那一层级)",
			layerConfig:{length:6,layer:1}
		})
		var u7 = createDicField({
			id:"chis.boolType",
			alias:"域字典"
		})

	var f = new Ext.FormPanel({
		frame:true,
		labelWidth:200,
		items:[u1,u2,u3,u4,u5,u6,u7]
	})
	var w = new Ext.Window({
		title:"字典层数控制以及层的过滤",
		border:false,
		height:220,
		width:560,
		layout:"fit",
		items:f
	})
	w.show()
	w.getEl().slideIn('b')
	
	
})
