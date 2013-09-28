$package("app.modules.config.unit")
$import(
	"app.modules.common",
	"org.ext.ux.treegrid.TreeGrid",
	"app.modules.list.SimpleListView"
)

app.modules.config.unit.UnitTreeGrid = function(cfg){
	Ext.apply(this,app.modules.common)
	app.modules.config.unit.UnitTreeGrid.superclass.constructor.apply(this,[cfg])
}

Ext.extend(app.modules.config.unit.UnitTreeGrid, app.modules.list.SimpleListView,{
	initPanel:function(schema){
		var items = schema.items
		this.store = this.getStore(items)
        this.grid = new Ext.ux.tree.TreeGrid({ 
           	width: 500,
            height: 300,
            animate : true, 
            enableDD : true, 
            rootVisible : false, 
            containerScroll : true, 
            enableSort:false,         
            columns :this.getCM(items)
        }); 
    	this.grid.on("render",this.onReady,this)
    	return this.grid
	},
	
	onReady:function(){
		var root = new Ext.tree.AsyncTreeNode({
    			children:this.store
    		})
    	this.grid.setRootNode(root)
	},
	
	getStore:function(items){
		var result = util.rmi.miniJsonRequestSync({
			serviceId : this.listServiceId,
			cmd : this.cmd
		})
		var data = {}
		if(result.code == 200){
			data = result.json.body
		}
		return data
	}
})