forward
global type n_order from nonvisualobject
end type
end forward


global type n_order from nonvisualobject 
end type

global n_order n_order

type variables
datetime completiondate, opendate
decimal orderfee  
decimal price //don't need this
double quantity
long orderid
string orderstatus, ordertype, symbol
end variables
on n_order.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_order.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
