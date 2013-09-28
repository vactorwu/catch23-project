$package("app.lang")

$import("app.desktop.Module")

app.lang.EmbedPanel = function(cfg) {
	app.lang.EmbedPanel.superclass.constructor.apply(this, [cfg])
}

Ext.extend(app.lang.EmbedPanel, app.desktop.Module, {
	url:null,		// like 'http://172.16.171.250:8081/CTDS-ETL/'
	port:null,		// like '8081'
	appName:null,	// like 'CTDS-ETL'
	param:null,		// like '?uid=jsaon&age=35'
	initPanel : function() {
		if(!this.url){
			var path = window.location.href;
			if(this.appName){
				path = path.replace(/\/([\w_-]+)\//.exec(path)[1], this.appName);
				this.url = path;
			}
			if(this.port){
				path = path.replace(/:([0-9]+)/.exec(path)[1], this.port);
				this.url = path;
			}
		}
		if(this.url && this.param){
			this.url += this.param;
		}
		var html;
		if(this.url){
			html = "<iframe src="	+ this.url + " width='100%' height='100%' frameborder='no'></iframe>";
		}else{
			html = "missing arg: 'url' or 'appName' or 'port'.";
		}
		var panel = new Ext.Panel({
			frame : false,
			autoScroll : true,
			html : html
		})
		this.panel = panel
		return panel
	}
})











