$package("sys.qua")
$import(
	"sys.qua.QuaMain"
)
$import("util.dictionary.DictionaryLoader")

sys.qua.QuaOga = function(cfg){
	this.oga = null;
	this.notAutoLoadData = true;
	sys.qua.QuaOga.superclass.constructor.apply(this,[cfg]);
	this.business = [
		{key:'business',text:'医院名称'}
	];
	this.lockColumn = ['阶段','分类'],
	this.requestData = {
		serviceId:this.loadService,
		queryFlag:2,
		DataStandardId:this.DataStandardId
	};
}

Ext.extend(sys.qua.QuaOga, sys.qua.QuaMain, {
	initBusiness:function(){
		this.business.push({key:'Hospital',text:'',CustomIdentify:"Hospital"})	
	},
	openQuaErrorWin:function(d, b, g){
		$import("sys.qua.QuaError");
		var m = this.quaError;
		if(!m){
			m = new sys.qua.QuaError();
			this.quaError = m;
		}
		var i = d.indexOf("@");
		var e,f;
		if(i > -1){
			e = d.substring(0,i);
			f = d.substring(i+1,d.length);
		}
		m.oga = this.oga;
		m.bis = e;
		var w = m.getWin();
		w.setTitle(this.win.title+"("+f+")");
		w.show();
		m.d1.setValue(this.d1.getValue());
		m.d2.setValue(this.d2.getValue());
		m.loadData();
	},
	openQuaOgaWin:function(){
	},
	loadData:function(){
		if(this.grid.el){
			this.grid.el.mask("数据加载中..","x-mask-loading")
		}
		this.requestData.startTime = this.d1.getValue();
		this.requestData.endTime = this.d2.getValue();
		this.requestData.Authororganization = this.oga;
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
	},
	onWinShow:function(){
		this.grid.getView().mainHd.query('div.x-grid3-hd-1')[0].innerHTML = '<a class="x-grid3-hd-btn" href="#"></a>'+'aaa';		
	}
})