forward
global type n_holding_totals from nonvisualobject
end type
end forward


global type n_holding_totals from nonvisualobject 
end type

global n_holding_totals n_holding_totals

type variables
long holding_count
double holding_total, gain_total
end variables
on n_holding_totals.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_holding_totals.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
