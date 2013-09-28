$package("app.modules.config.unit")
$import(
	"app.modules.form.TableFormView"
)
app.modules.config.unit.CustomTableForm = function(cfg){
	app.modules.config.unit.CustomTableForm.superclass.constructor.apply(this,[cfg])
}
Ext.extend(app.modules.config.unit.CustomTableForm, app.modules.form.TableFormView ,{
	doNew:function(){
		this.op = "create"
		if(this.data){
			this.data = {}
		}
		if(!this.schema){
			return;
		}
		var form = this.form.getForm();
		form.reset();
		var items = this.schema.items
		var items = this.schema.items
		var n = items.length
		for(var i = 0; i < n; i ++){
			var it = items[i]
			var f = form.findField(it.id)
			if(f){
				if(!(arguments[0] == 1)){	// whether set defaultValue, it will be setted when there is no args.
					var dv = it.defaultValue;
					if(dv){
						if((it.type == 'date' || it.xtype=='datefield') && typeof dv == 'string' && dv.length > 10){
							dv = dv.substring(0,10);
						}
						f.setValue(dv);
					}
				}
				if(it.isVisible == "true"){
					f.setVisible(false)
				}
				if(it.update == "false" && !it.fixed && !it.evalOnServer){
					f.enable();
				}
				f.validate();
			}		
		}
		this.setKeyReadOnly(false)
		this.fireEvent("doNew",this.form)
		this.focusFieldAfter(-1,800)
		var f = form.findField("organizType")
		if(f && !f.hasListener("select")){
		    f.on("select",this.changeType,this)		    
		}
		this.panel.doLayout()
	},
	initFormData:function(data){
		app.modules.config.unit.CustomTableForm.superclass.initFormData.call(this,data)
		var form = this.form.getForm()
		var pf = form.findField("organizType")
		if(pf){
			this.changeStatus(pf)
		}
	}
});