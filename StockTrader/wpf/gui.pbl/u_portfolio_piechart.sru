$PBExportHeader$u_portfolio_piechart.sru
forward
global type u_portfolio_piechart from UserObject
end type
type dw_1 from u_dw within u_portfolio_piechart
end type
end forward

global type u_portfolio_piechart from UserObject
dw_1 dw_1
end type
global u_portfolio_piechart u_portfolio_piechart

forward prototypes
public function integer of_share (DataWindow adw_share, boolean ab_switch)
end prototypes

public function integer of_share (DataWindow adw_share, boolean ab_switch);if ab_switch = true then
	return adw_share.ShareData( dw_1 )
else
	return adw_share.ShareDataOff( )
end	if
end function

on u_portfolio_piechart.create
this.dw_1 = create dw_1
this.Control[]={this.dw_1}
end on

on u_portfolio_piechart.destroy
destroy(this.dw_1)
end on

type dw_1 from u_dw within u_portfolio_piechart
string DataObject = "d_portfolio_piechart"
end type

on dw_1.create
call super::create
end on

on dw_1.destroy
call super::destroy
end on
