$package("sys.data.element")

$import("app.lang.UIModule",
	"util.dictionary.SimpleDicFactory",
	"sys.data.element.DataElementView",
	"sys.data.element.DataSetView",
	"sys.data.element.DataDicView",
	"app.modules.list.SimpleListView",
	"app.modules.list.SimpleListView")

sys.data.element.DataMain = function(cfg){
	sys.data.element.DataMain.superclass.constructor.apply(this,[cfg]);
}

Ext.extend(sys.data.element.DataMain, app.lang.UIModule, {
	resDataStandard:null,
	initPanel:function(){
		var comb = this.initDataStandardComb();
		var tabPanel = new Ext.TabPanel({
			activeTab:0,
			resizeTabs:true,
			minTabWidth:60,
//			deferredRender:false,
			tabWidth:80,
			items:[
				this.initDataElementPanel()
				,this.initDataSetPanel()
				,this.initDataDicPanel()
			]
		});
		tabPanel.on("beforetabchange",this.onBeforeTabChange,this);
		this.tabPanel = tabPanel;
		var panel = new Ext.Panel({
			tbar:['数据标准:',comb,{
				iconCls:"add",
				handler:this.showDataStandardList,
				scope:this
			}],
			layout:"fit",
			items:[tabPanel]
		})
		this.panel = panel;
		return panel;
	},
	onBeforeTabChange:function(tab,np,cp){
		var m = np._module;
		if(!np.rendered){
			m.resDataStandard = this.resDataStandard;
			if(m instanceof sys.data.element.DataDicView){
				m.dicId = "dictionaries.res."+this.resDataStandard;
			}
			return;
		}
		if(this.resDataStandard != m.resDataStandard){
			m.resDataStandard = this.resDataStandard;
			m.reset();
		}
	},
	initDataStandardComb:function(){
		var dataStandardDicId = "resDataStandard";
		var comb = util.dictionary.SimpleDicFactory.createDic({
			id:dataStandardDicId,
			emptyText:"请选择数据标准..",
			width:200,
			editable:false
		})
		var d = util.dictionary.DictionaryLoader.load({id:'resDataStandard'});
		if(d.items && d.items.length > 0){
			this.resDataStandard = this.resDataStandard || d.items[0]["key"];
			comb.on("afterrender",function(comb){
				if(d.wraper[this.resDataStandard]){
					comb.setValue(d.wraper[this.resDataStandard]);
				}
			},this)
		}
		comb.on({
			select:function(cb,r,index){
				this.resDataStandard = r.data.key;
				var initCnd = ['eq',['$','DataStandardId'],['s',this.resDataStandard]];
//				this.dev.initCnd = initCnd;
//				this.dsv.initCnd = initCnd;
//				this.ddv.initCnd = initCnd;
				var m = this.tabPanel.getActiveTab()._module;
				if(this.resDataStandard != m.resDataStandard){
					m.resDataStandard = this.resDataStandard;
					m.reset();
				}
			},
			scope:this
		});
		this.dataStandardComb = comb;
		return comb;
	},
	initDataElementPanel:function(){
		var dev = new sys.data.element.DataElementView({
			resDataStandard:this.resDataStandard,
			actions:[
				{id:"add",name:"新增"},
				{id:"update",name:"修改"},
				{id:"remove",name:"删除"}
			]
		});
		this.dev = dev;
		var devp = dev.initPanel();
		devp._module = dev;
		devp.title = "数据元"
		return devp;
	},
	initDataSetPanel:function(){
		var dsv = new sys.data.element.DataSetView({
			resDataStandard:this.resDataStandard,
			actions:[
				{id:"addGroup",name:"添加组",iconCls:"common_treat"},
				{id:"add",name:"添加元"},
				{id:"sequence",name:"保存顺序",iconCls:"common_treat"},
				{id:"update",name:"修改"},
				{id:"remove",name:"删除"}
			]
		});
		this.dsv = dsv;
		var dsvp = dsv.initPanel();
		dsvp._module = dsv;
		dsvp.title = "数据集"
		return dsvp;
	},
	initDataDicPanel:function(){
		var ddv = new sys.data.element.DataDicView({
			resDataStandard:this.resDataStandard
		});
		this.ddv = ddv;
		var ddvp = ddv.initPanel();
		ddvp._module = ddv;
		ddvp.title = "数据字典"
		return ddvp;
	},
	showDataStandardList:function(btn){
		if(!this.dsl){
			var dsl = new app.modules.list.SimpleListView({
				title:"数据标准管理",
				entryName:"RES_DataStandard",
				saveServiceId:"dataStandardSave",
				removeServiceId:"dataStandardRemove",
				showButtonOnTop:true,
				width:750,
				actions:[
					{id:"read",name:"查看"},
					{id:"create",name:"新建"},
					{id:"update",name:"修改"}
//					,{id:"remove",name:"删除"}
				]
			});
			dsl.on({
				save:this.doRefresh,
				remove:this.doRefresh,
				scope:this
			});
			this.dsl = dsl;
		}
		var win = this.dsl.getWin();
		win.setPosition(200,100);
		win.modal = true;
		win.show(btn.el);
	},
	doRefresh:function(){
		this.dataStandardComb.store.reload();
	}
})