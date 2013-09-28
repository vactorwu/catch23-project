$styleSheet("ext.ext-all")
$styleSheet("ext.xtheme-gray")
$styleSheet("ext.ext-patch")
$styleSheet("app.desktop.Desktop")
$styleSheet("app.desktop.TaskBar")
$styleSheet("app.desktop.Logon")
$styleSheet("app.desktop.main")

$import(
	"org.ext.ext-base",
	"org.ext.ext-all-debug",
	"org.ext.ext-lang-zh_CN"
);


Ext.BLANK_IMAGE_URL = ClassLoader.appRootOffsetPath +"resources/s.gif"

//fix IE bug for onReady execute twice
__docRdy = false

Ext.onReady(function(){
	if(__docRdy){
		return
	}
	__docRdy = true

	$require([
			"app.desktop.Module",
			"app.desktop.TaskManager",
			"app.viewport.Logon",
			"app.desktop.plugins.LogonWin"
			],
			function(){
				var logon = new app.viewport.Logon({forConfig:true})	
				logon.on("logonSuccess",function(res){
					var body = Ext.get(document.body)
	                body.update("")
					var apps = res.body
					if(res.layoutCss){
						$styleSheet(res.layoutCss)
					}
					if(apps){
						var appModules = []
						var appMenuItems = []
						var size = apps.length
						var myPage = []
						for(var i = 0; i < size; i ++){
							var ap = apps[i]
							if(!ap){
								continue;
							}
							if(ap.type=="index"){
								myPage.push(ap)
							}
							var modules = ap.modules
							if(!modules){
								continue;
							}
							for(var j = 0; j < modules.length; j ++){
								var module = modules[j]
								if(typeof module != "object"){
									continue;
								}
								module.type = "USER"
								appModules.push(module)
								appMenuItems.push({id:module.id,title:module.title,text:module.title,icon:module.icon})
							}
						}
						
						var mainCfg = {
							title:res.title,
							uid:res.userId,
							userPic: res.img,
							uname:res.userName,
							dept:res.dept,
							deptId:res.deptId,
							jobtitle:res.jobtitle,
							jobId:res.jobId,
							regionCode : res.regionCode,
							regionText : res.regionText,
							mapSign:res.mapSign,
							catalog:res.catalog,
							apps:apps,
							modules:appModules,
							myPage:myPage,
							tabnum: res.tabNumber
						}
						eval("globalDomain=res.domain")
						document.title = res.title
						$import(
							"app.viewport.MyDesktop",
							"app.viewport.App"
						)
						mainApp = new app.viewport.App(mainCfg) //不用var为全局变量						
					}
				})
			}
	)//require
	
})