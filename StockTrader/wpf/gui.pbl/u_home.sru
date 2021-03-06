$PBExportHeader$u_home.sru
forward
global type u_home from u_basepage
end type
type uo_summary from u_marketsummary within u_home
end type
type dw_account from u_dw within u_home
end type
type r_1 from Rectangle within u_home
end type
type r_2 from Rectangle within u_home
end type
end forward

global type u_home from u_basepage
uo_summary uo_summary
dw_account dw_account
r_1 r_1
r_2 r_2
end type
global u_home u_home

forward prototypes
public subroutine of_reset ()
end prototypes

public subroutine of_reset ();dw_account.Reset( )
uo_summary.of_reset( )
end subroutine

on u_home.create
int iCurrent
call super::create
this.uo_summary = create uo_summary
this.dw_account = create dw_account
this.r_1 = create r_1
this.r_2 = create r_2
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.uo_summary
this.Control[iCurrent+2]=this.dw_account
this.Control[iCurrent+3]=this.r_1
this.Control[iCurrent+4]=this.r_2
end on

on u_home.destroy
call super::destroy
destroy(this.uo_summary)
destroy(this.dw_account)
destroy(this.r_1)
destroy(this.r_2)
end on

event ue_setstate;call super::ue_setstate;n_holding_totals ln_totals
try
	//market summary
	gn_controller.of_getmarketsummary( )
	//put into dw buffer
	uo_summary.of_settsia ( gn_controller.of_gettsia( ) )
	uo_summary.of_setvolumne( gn_controller.of_getvolume( ) )

	gn_controller.of_settops( uo_summary.of_getgainers( ), true)
	gn_controller.of_settops( uo_summary.of_getlosers( ), false)
	
long Lrows
	//user account summary
dw_account.setwsobject( gn_controller.of_get_wsconn( ) )
Lrows = dw_account.retrieve(gn_controller.of_get_profile_id( ))
ln_totals = gn_controller.of_getholdingtotals( gn_controller.of_get_profile_id( ) )
dw_account.setitem( 1, 'holding_count', ln_totals.holding_count  )
dw_account.setitem( 1, 'holding_total', ln_totals.holding_total   )
catch (exception e)

end try
end event

type uo_summary from u_marketsummary within u_home
end type

on uo_summary.create
call super::create
end on

on uo_summary.destroy
call super::destroy
end on

type dw_account from u_dw within u_home
string DataObject = "d_user_statistics"
end type

on dw_account.create
call super::create
end on

on dw_account.destroy
call super::destroy
end on

type r_1 from Rectangle within u_home
end type

on r_1.create
end on

on r_1.destroy
end on

type r_2 from Rectangle within u_home
end type

on r_2.create
end on

on r_2.destroy
end on
