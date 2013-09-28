$package("sys.qua")
$import(
	"sys.qua.QuaMain"
)
$import("util.dictionary.DictionaryLoader")

sys.qua.QuaError = function(cfg){
	this.oga = null;
	this.bis = null;
	this.notAutoLoadData = true;
	this.hideSuccessCol = true;
	sys.qua.QuaError.superclass.constructor.apply(this,[cfg]);
	this.business = [
		{key:'business',text:'错误数据'}
	];
	this.numbType = [
		{key:'Success',text:'正确数'},
		{key:'Fail',text:'错误数'}
	],
	this.lockColumn = ['阶段','分类'],
	this.requestData = {
		serviceId:this.loadService,
		queryFlag:3,
		DataStandardId:this.DataStandardId
	};
}

Ext.extend(sys.qua.QuaError, sys.qua.QuaMain, {
	initBusiness:function(){
		this.business.push({key:'Hospital',text:'',CustomIdentify:"Hospital"})	
	},
	cellClick:function(grid,rowIndex,columnIndex,e){
	},
	loadData:function(){
		if(this.grid.el){
			this.grid.el.mask("数据加载中..","x-mask-loading")
		}
		this.requestData.startTime = this.d1.getValue();
		this.requestData.endTime = this.d2.getValue();
		this.requestData.Authororganization = this.oga;
		this.requestData.RecordClassifying = this.bis;
		this.store.load();
	},
	getToolBar:function(){
		var toolbars = ['开始时间:'];
		var d1 = new Ext.form.DateField({width:120});
		var dt = new Date().add(Date.DAY, -1);
		d1.setValue(dt);
		this.d1 = d1;
		var d2 = new Ext.form.DateField({width:120});
		d2.setValue(new Date())
		this.d2 = d2;
		toolbars.push(d1);
		toolbars.push('结束时间:');
		toolbars.push(d2);
		toolbars.push({
			text:'查询',
			iconCls:'query',
			handler:this.loadData,
			scope:this
		});
		return toolbars;
	}
})