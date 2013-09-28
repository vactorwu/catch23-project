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
//		var u1 = createDicField({
//			id:"user01",
////			id:"resDataGroup",
////			id:"manageUnit",
////			filter:['and',
////				['eq',['$map',['s','GroupType']],['s','H']],
////				['eq',['$map',['s','DataStandardId']],['s','V2011']]
////			],
////			lengthLimit:9,
////			limitNodeToLeaf:false,
////			layerStartLength:9,
////			layer:1,
//			keyNotUniquely:true,
//			render:"Tree",
//			alias:"用户"
//		})
		var u2 = createDicField({
			id:"manageUnit2",
			alias:"用户",
			render:"Tree"
		})
		u2.store.on("beforeload",function(st){

		})
//		u1.on("select",function(f){
//			alert(Ext.encode(f.getValue()))
//		});
		u2.on("select",function(f){
//			alert(Ext.encode(f.getValue()))
		});
//		u1.tree.on("expandnode",function(node){
//			alert(node.text)	
//		})
//		var u2 = createDicField({
//			id:"unit",
//			render:"Tree",
//			alias:"管辖机构Tree"
//		})

	var f = new Ext.FormPanel({
		frame:true,
		labelWidth:200,
		items:[
			u2
//			,
//			new Ext.form.NumberField({
//				
//			})
//			,u2
		]
	})
	var w = new Ext.Window({
		title:"TableDic",
		border:false,
		height:220,
		width:560,
		layout:"fit",
		items:f,
		buttons:[{
			text:"okay",
			handler:function(){
				var node = u1.tree.getSelectionModel().getSelectedNode();
				alert(node.attributes["b.manaUnitId"])
//				alert(u1.getValue())
//				var st = u1.getStore();
//				st.proxy.conn.url = st.url+"?filter=['eq',['$map',['s','key']],['s','11']]";
//				alert(st.url+"="+st.proxy.conn.url)
//				st.load();
			},
			scope:this
		}]
	})
	w.show()
	w.getEl().slideIn('b')
	
	
})
