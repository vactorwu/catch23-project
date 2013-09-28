$package("app.modules.config")
$styleSheet("util.widgets.FileUploadField.css.fileuploadfield")
$import(
		"app.desktop.Module",
		"util.dictionary.SimpleDicFactory",
		"util.fileUploader.FileUploadField"
)

app.modules.config.PicUploaderConfig = function(cfg){
    this.fileFilter = "png"    //文件后缀名
    this.dirType = "imgDirectory"
    this.pfix = ".jpg"
    this.logonData = [["rhmplogin2.jpg", '登录界面标题(1120×115)'], ["rhmplogin4.jpg", '登录界面背景(1120×388)']]
    this.mainData = [['top_bg.jpg',"主界面标题(1047×76)"]]
    this.home = ClassLoader.stylesheetHome
    app.modules.config.PicUploaderConfig.superclass.constructor.apply(this,[cfg])
}
Ext.extend(app.modules.config.PicUploaderConfig, app.desktop.Module,{
	
	init:function(){
		var cssClass = ["title","body"]
		this.initCssClass()
		for(var i = 0;i<cssClass.length;i++){
			this.initViewCssClass(cssClass[i])
		}
	},
	
    initPanel:function(){
    	var type = new Ext.Panel({
    		border:false,
			fieldLabel:"界面类型选择",
			defaultType:"radio",
			layout:"hbox",
			hideLabels:true,
			defaults:{flex:1,height:20},
			items:[
				{boxLabel:"登录界面",name:"type",inputValue:"logon", checked:true},
				{boxLabel:"主界面",name:"type",inputValue:"main"}
			]
		})
		
    	var pics = new Ext.form.ComboBox({
    		name:'pics',
    		fieldLabel:'界面图片选择',
    		typeAhead: true,
    		triggerAction: 'all',
    		lazyRender:true,
    		mode: 'local',
    		allowBlank:false,
    		width:500,
    		store: new Ext.data.ArrayStore({
        		fields: [
            		'name',
            		'displayText'
        		],
        		data: this.logonData
    		}),
    		valueField: 'name',
    		displayField: 'displayText'
		});
		this.pics = pics
        
		var tabnum = new Ext.form.ComboBox({
    		name:'newtabnum',
    		fieldLabel:'新标签页个数',
    		emptyText:'请选择标签页个数',
    		typeAhead: true,
    		triggerAction: 'all',
    		editable:false,
    		lazyRender:true,
    		mode: 'local',
    		allowBlank:false,
    		width:250,
    		colspan:2,
    		layout:'form',
    		store: new Ext.data.ArrayStore({
        		fields: [
            		'key',
            		'value'
        		],
        		data: [['3','最多3个标签页'],
        			   ['5','最多5个标签页'],
        			   ['8','最多8个标签页'],
        			   ['','无限制']]
    		}),
    		valueField: 'key',
    		displayField: 'value'
		});
		
        var infoFS = new Ext.form.FieldSet({
			title:"图片信息",
			defaultType:"textfield",
			collapsible:false,
			animCollapse:true,
			buttonAlign:"center",
			width:550,
			height:268,
			items:[{   
				  fieldLabel:'文件保存路径',
			      name:'dirType',
				  inputType:'hidden',
				  value:this.dirType
			    },type,pics,{    
				  fieldLabel:'文件名称',
				  name:'fileName',
				  readOnly:true,
				  width: 500,
				  cls:'x-item-disabled'
				},{
		          fieldLabel: '请选择要上传的文件',
		          width:500,
            	  xtype: 'fileuploadfield',
            	  emptyText: '请选择图片...',
            	  name: 'file',
            	  buttonText: '',
            	  buttonCfg: {
                  iconCls: 'folder_image'
            	}
            }],
            buttons:[{text:"上传",handler:this.doUpload,scope:this,iconCls:"green_up",height:20}]
		})
		var viewFS = new Ext.form.FieldSet({
			title:"图片位置",
			collapsible:false,
			animCollapse:true,
			//width:350,
			height:268,
			bodyStyle:"padding:15px 5px",
			style:"margin-right:15px;margin-left:10px;",
			defaults:{xtype:'box',width:'300'},
			items:[{  
    			  height: 44, 
    			  cls:"logon_title"
			    },{
    			  height: 146,
    			  cls:"logon_body"
			    }  
			]
		})
		var confFS = new Ext.form.FieldSet({
			title:"主标签页设置",
			collapsible:false,
			labelAlign:'left',
			buttonAlign:"center",
			labelWidth:100,
			width:900,
			height:150,
			colspan:2,
			style:"margin-right:15px;margin-left:10px;",
			defaults:{border:false},
			layout: {type: 'table', columns : 2},
			items:[{
				xtype:'box',
				style:"margin-left:50px;",
				height:30,
				width:780,
    			colspan:2,
    			cls:"tab_num"
			 },{  
  			   layout: 'form',  
  			   style:"margin-right:70px;margin-left:50px;margin-top:20px;",
               items: [{  
                 xtype: 'textfield',
                 fieldLabel:'当前标签页个数',  
                 readOnly:true,
                 name:'tabnum',
			     width: 250}]
			 },{
			   layout:'form',
			   style:"margin-top:20px",
			   items:tabnum
			 }
		   ],
		   buttons:[{text:'确定',handler:this.changeTabNum,scope:this,colspan:2}]
		})
		
		this.confFS = confFS
		this.infoFS = infoFS
        this.viewFS = viewFS
        this.picForm = new Ext.FormPanel({
        	border:false,
			layout: {type: 'table', columns : 2},
			labelAlign: 'top',
			fileUpload : true,
			defaultType: 'textfield',
			items:[viewFS,infoFS]
        })
        var panel = new Ext.Panel({
        	frame:true,
			items:[this.picForm,confFS]
        })
        type.items.item(0).on("check",this.changeType,this)
        pics.on("select",this.setFileName,this)
        this.confFS.on("afterrender",this.setTabNum,this)
        return panel
    },
    
    changeType:function(com){
    	this.removeSolid()
    	this.pics.clearValue()
    	this.infoFS.items.item(3).reset()
    	var v = com.getGroupValue()
    	this.updateCssClass(v)
    	if(v == 'logon'){
    		this.pics.store.loadData(this.logonData)
    	}
    	if(v == 'main'){
    		this.pics.store.loadData(this.mainData)
    		this.pics.setValue(this.mainData[0][0])
    		this.setFileName(this.pics)
    		this.infoFS.items.itemAt(0).setValue("homeImgDirectory")
    	}
    },
    
    setFileName:function(combox){
    	this.removeSolid()
    	var fileName = combox.getValue()
        var f = this.infoFS.items.item(3)
        if(f){
           f.setValue(fileName)
           this.fileFilter = fileName.substring(fileName.indexOf('.')+1)
 		   if(fileName == "rhmplogin2.jpg" || fileName == "top_bg.jpg"){
 		   	  this.viewFS.items.item(0).addClass("border_line")
 		   }else{
 		   	  this.viewFS.items.item(1).addClass("border_line")
 		   }
        }
    },
    
    setTabNum:function(){
    	var rel = util.rmi.miniJsonRequestSync({
			serviceId:"appConfig",
			cmd:"loadProps"
		})
		var args = rel.json.body
		this.args = args
		var v = args.tabNumber
		var msg = "最多"+v+"个标签页"
		if(!v){
			msg = "无限制"
		}
		this.confFS.items.item(1).items.item(0).setValue(msg)
    },
    
        changeTabNum:function(){
    	var item = this.confFS.items.item(2).items.item(0)
    	var num = item.getValue()
    	var props = []
    	for(var i in this.args){
    		var value = {}
			value['p_key'] = i
			value['p_value'] = this.args[i]
			if(i == 'tabNumber'){
				value['p_value'] = num
			}
			props.push(value)
    	}
    	var rel = util.rmi.miniJsonRequestSync({
			serviceId:"appConfig",
			cmd:"saveProps",
			body:props
		})
		if(rel.code < 300){
			var msg = "最多"+num+"个标签页"
			if(!num){
				msg = "无限制"
			}
			this.confFS.items.item(1).items.item(0).setValue(msg)
			Ext.Msg.alert("提示","修改成功,页面刷新后生效")
		}
    },
    
    doUpload:function(){
		var form = this.picForm.getForm()
		if(!form.isValid()){
		   return
		}
		var f = this.infoFS.items.item(3)
		if(f && !f.getValue()){
			Ext.Msg.alert("错误","请选择上传文件")
		    return
		}
		if(!this.checkFileType()){
			Ext.Msg.alert("错误","请选择[ "+this.fileFilter+" ]格式的文件")
			return;
		}
		
		var con = new Ext.data.Connection();
		form.el.mask("正在上传请稍候...","x-mask-loading")
		con.request({
			url:"*.uploadForm",
			method:"post",
			isUpload:true,
			callback:complete,
			scope:this,
			//form:form.getForm().el
			form:form.el
		})		
		function complete(ops,sucess,response){
			form.el.unmask()
			var code = 200
		    var msg = ""
			if(sucess){			
				var json = {};
				try{
					json = eval("(" + response.responseText + ")")
					code = json["x-response-code"]
				    msg = json["x-response-msg"]
				}catch(e){
					code = 500
				    msg = "uploadException"
				}
				if(code == 200){
				    Ext.MessageBox.confirm("图片上传成功","是否刷新页面",function(id){
				    	if(id == 'yes'){
				    		window.location.reload()
				    	}
				    })
				}else if(code == 401){
				    Ext.MessageBox.alert("错误","上传失败:用户未登陆或登录已过期")				
				}else{
				    Ext.MessageBox.alert("错误",msg)	
				}
			}
		}
	},
	
	checkFileType:function(){
		var form = this.picForm.getForm()
		var f = form.findField("file")
		if(!f){
			return false;
		}
		var filter = this.fileFilter
		if(!filter){
			return true;
		}
		var v = f.getValue()
		var type = v.substring(v.lastIndexOf(".") + 1)	
		type = type.toLowerCase();
		if(typeof filter == "string"){
			return type == filter.toLowerCase();
		}
		if(typeof filter == "object" && filter.length > 0){
			for(var i = 0; i < filter.length; i ++){	
				if(type == filter[i].toLowerCase()){
					return true;
				}
			}
		}
	},
	
	removeSolid:function(){
		this.viewFS.items.item(0).removeClass("border_line")
    	this.viewFS.items.item(1).removeClass("border_line")
	},
	
	initViewCssClass:function(id){
		var cssSelector = ".logon_"+id;
		if(!util.CSSHelper.hasRule(cssSelector)){
			util.CSSHelper.createRule(cssSelector,'background-image:url('+ this.home + 'picview/images/logon_'+id+this.pfix+') !important;')
		}
	},
	
	initCssClass:function(){
		util.CSSHelper.createRule('.border_line','BORDER: #C01919 2px solid;')
		util.CSSHelper.createRule('.tab_num','background-image:url('+ this.home + 'picview/images/main_tab'+this.pfix+') !important;')
	},
	
	updateCssClass:function(id){
		var v = ["title","body"]
		for(var i = 0;i<v.length;i++){
			var cssSelector = ".logon_"+v[i]
			if(util.CSSHelper.hasRule(cssSelector)){
				var value = "url("+ this.home + "picview/images/"+id+"_"+v[i]+this.pfix+")"
				util.CSSHelper.updateRule(cssSelector,'background-image',value)
			}
		}
	}
})
