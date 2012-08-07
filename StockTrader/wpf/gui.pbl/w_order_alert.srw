forward
global type w_order_alert from window
end type
type dw_result from u_dw within w_order_alert
end type
type pb_ok from picturebutton within w_order_alert
end type
type st_label from statictext within w_order_alert
end type
end forward


global type w_order_alert from window 

dw_result dw_result 
pb_ok pb_ok 
st_label st_label 
end type


global w_order_alert w_order_alert

on w_order_alert.create
this.dw_result=create dw_result
this.pb_ok=create pb_ok
this.st_label=create st_label
this.Control[]={this.dw_result,&
this.pb_ok,&
this.st_label}
end on

on w_order_alert.destroy
destroy(this.dw_result)
destroy(this.pb_ok)
destroy(this.st_label)
end on

event open;dw_result.reset( )
dw_result.insertrow( 0 )
n_order ln_order
ln_order = message.powerobjectparm
dw_result.setitem( 1, 'price', ln_order.price)
dw_result.setitem( 1, 'orderid', ln_order.orderid)
dw_result.setitem( 1, 'orderstatus', ln_order.orderstatus)
dw_result.setitem( 1, 'opendate', ln_order.opendate)
dw_result.setitem( 1, 'completiondate', ln_order.completiondate)
dw_result.setitem( 1, 'orderfee', ln_order.orderfee)
dw_result.setitem( 1, 'ordertype', ln_order.ordertype)
dw_result.setitem( 1, 'symbol', ln_order.symbol)
dw_result.setitem( 1, 'quantity', ln_order.quantity)
end event

type dw_result from u_dw within w_order_alert 
string dataobject = "d_order_display"

end type



type pb_ok from picturebutton within w_order_alert 

end type



event clicked;close(parent)
end event

type st_label from statictext within w_order_alert 

end type

