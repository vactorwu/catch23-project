$PBExportHeader$u_orders.sru
forward
global type u_orders from u_basepage
end type
type uo_1 from u_quotebar within u_orders
end type
type dw_1 from u_dw within u_orders
end type
type r_1 from Rectangle within u_orders
end type
end forward

global type u_orders from u_basepage
uo_1 uo_1
dw_1 dw_1
r_1 r_1
end type
global u_orders u_orders

forward prototypes
public subroutine of_reset ()
end prototypes

public subroutine of_reset ();dw_1.Reset()
end subroutine

on u_orders.create
int iCurrent
call super::create
this.uo_1 = create uo_1
this.dw_1 = create dw_1
this.r_1 = create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.uo_1
this.Control[iCurrent+2]=this.dw_1
this.Control[iCurrent+3]=this.r_1
end on

on u_orders.destroy
call super::destroy
destroy(this.uo_1)
destroy(this.dw_1)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;dw_1.setwsobject( gn_controller.of_get_wsconn( ) )
dw_1.retrieve( gn_controller.of_get_profile_id( ) )
end event

type uo_1 from u_quotebar within u_orders
end type

on uo_1.create
call super::create
end on

on uo_1.destroy
call super::destroy
end on

type dw_1 from u_dw within u_orders
string DataObject = "d_orders"
end type

on dw_1.create
call super::create
end on

on dw_1.destroy
call super::destroy
end on

event constructor;call super::constructor;//enable header click sorting
ib_headersort = true
end event

type r_1 from Rectangle within u_orders
end type

on r_1.create
end on

on r_1.destroy
end on
