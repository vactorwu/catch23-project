$package("app.modules.config.person")
$import()

app.modules.config.person.PersonExcelParse = function(cfg) {
	this.width = 600
	this.height = 400
	Ext.apply(this, cfg)
	this.init()
	//app.modules.config.person.PersonExcelParse.constructor.apply(this, [cfg]);
}
Ext.extend(app.modules.config.person.PersonExcelParse, Ext.util.Observable, {
	init:function(){
		this.initPanel();
	},
	initPanel : function() {
		this.errorType = {0:"文件加载",3:"性名",4:"性别",5:"出生日期",6:"身份证号",8:"执业证书编码",9:"执业级别",
						10:"执业类别",11:"执业科目",12:"执业机构",13:"任职资格"}
		var store = new Ext.data.ArrayStore({
		    fields: [
		 	   {name: 'errorType'},
		 	   {name: 'errorInfo'}
		    ]
   		});
   		this.store=store
   		this.freshData(this.result)
		var grid = new Ext.grid.GridPanel({
     		store: store,
	        columns: [ 
	        	{
	                id       :'errorType',
	                header   : '错误类别', 
	                width    : 160, 
	                sortable : true, 
	                dataIndex: 'errorType',
	                hidden   :true
	            },
	            {
	                id       :'errorInfo',
	                header   : '错误信息概要', 
	                width    : 160, 
	                sortable : true, 
	                dataIndex: 'errorInfo'
	            },
	           {
					header : "查看详细错误信息",
					width : 120,
					dataIndex : "list",
					renderer : function(value, metadata, record, rowIndex, colIndex) {
						var str = '<button style="width:80px;" title="查看详细" onclick="doList()"/>查看详细</button>';
						return str;
					}
				}
	        ],
     		 autoExpandColumn: 'errorInfo'
   		 });
   		var a = this
		doList = function(){
			a.doList()
		}
   		this.grid=grid
   		return grid;
	},
	getWin : function() {
		var win = this.win
		if (!win) {
			win = new Ext.Window({
				title : "解析错误信息",
				width : this.width,
				height : this.height,
				items : this.grid,
				iconCls : 'icon-form',
				bodyBorder : false,
				closeAction : 'hide',
				maximizable: true,
				layout : "fit",
				modal : true
				})
			this.win=win
		}
		return win;
	},
	doList:function(){
		var r = this.grid.getSelectionModel().getSelected()
		var type=r.get("errorType")
		var store = new Ext.data.ArrayStore({
		    fields: [
		 	   {name: 'errorDetail'}
		    ]
   		});
   		var detailList=this.result.json.errorInfo[type]
   		var data=[]
   		for(var i=0;i<detailList.length;i++){
   			var item=[]
   			item.push(detailList[i])
			data.push(item)
		}
   		store.loadData(data)
		var detailGrid = new Ext.grid.GridPanel({
     		store: store,
	        columns: [ 
	        	{
	                id       :'errorDetail',
	                header   : '详细错误信息', 
	                width    : 160, 
	                sortable : true, 
	                dataIndex: 'errorDetail'
	            }
	        ],
     		 autoExpandColumn: 'errorDetail'
   		 });
		var	detailWin = new Ext.Window({
				title : this.errorType[r.get("errorType")]+":错误信息",
				width : 500,
				height : 400,
				items : detailGrid,
				iconCls : 'icon-form',
				bodyBorder : false,
				closeAction : 'close',
				maximizable: true,
				layout : "fit",
				modal : true
				})
		detailWin.show()
	},
	freshData:function(result){
		this.result=result
		var errorList=result.json.errorInfo
   		var errData=[]
   		for(var e in errorList){
   			var item=[]
   			item.push(e)
   			if(e==0){
   				item.push("["+this.errorType[e]+"]错误")
   			}else{
	   			item.push("["+this.errorType[e]+"]一列数据不符合上传要求，请检查")
   			}
			errData.push(item)
		}
   		this.store.loadData(errData)
	}
})