forward
global type n_ds from datastore
end type
end forward


global type n_ds from datastore 
end type

global n_ds n_ds

event constructor;this.setwsobject( gn_controller.of_get_wsconn( ) )
end event

on n_ds.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_ds.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
