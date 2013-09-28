$import(
	"util.rmi.miniJsonRequestSync",
	"util.rmi.miniRequestSync",
	"util.rmi.miniRequestAsync"
)


var result = util.rmi.miniJsonRequestSync({
	serviceId:"logon",
	uid:"system",
	psw:"123"
})

//var result = util.rmi.miniJsonRequestSync({
//	serviceId:"testEventService"
//})
var url = "RES_DataStandard.schema"
var result = util.rmi.miniRequestAsync(url,function(a,b,c,d){
	alert(Ext.encode(c))
})
//alert(Ext.encode(result))