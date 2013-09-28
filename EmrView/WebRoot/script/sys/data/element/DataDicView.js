$package("sys.data.element")
$styleSheet("sys.data.element.go")
$import("util.dictionary.DictionaryBuilder")

sys.data.element.DataDicView = function(cfg) {
	sys.data.element.DataDicView.superclass.constructor.apply(this, [cfg])
}
Ext.extend(sys.data.element.DataDicView, util.dictionary.DictionaryBuilder, {
	initPanel:function(){
		this.dicId = "dictionaries.res."+this.resDataStandard;
		var panel=sys.data.element.DataDicView.superclass.initPanel.apply(this);
		return panel;
	},
	processDicRemove:function(){
		var fields = this.getFields();
		var id = fields.dicId.getValue()
		if(id == ""){
			return;
		}
		var result = this.saveToServer("removeDic4Meta",{key:id})
		if(result.code == 200){
			var node = this.selectedNode
			if(node){
				var next = node.nextSibling || node.previousSibling
				node.parentNode.removeChild(node)
				this.form.getForm().reset()
				if(next){
					this.onTreeClick(next)
					next.ensureVisible()
				}
			}
		}
	},
	reset:function(){
		this.tree.getLoader().url = "dictionaries.res."+this.resDataStandard+".dic"
		this.tree.getLoader().load(this.tree.getRootNode());
	}
});
