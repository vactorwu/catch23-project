$PBExportHeader$u_portfolio.sru
forward
global type u_portfolio from u_basepage
end type
type uo_1 from u_quotebar within u_portfolio
end type
type dw_1 from u_dw within u_portfolio
end type
type r_2 from Rectangle within u_portfolio
end type
type r_1 from Rectangle within u_portfolio
end type
end forward

global type u_portfolio from u_basepage
uo_1 uo_1
dw_1 dw_1
r_2 r_2
r_1 r_1
end type
global u_portfolio u_portfolio

type variables
integer ii_priorpoint
u_portfolio_piechart iu_portfolio_piechart
end variables

forward prototypes
public subroutine of_reset ()
end prototypes

public subroutine of_reset ();dw_1.Reset( )
end subroutine

on u_portfolio.create
int iCurrent
call super::create
this.uo_1 = create uo_1
this.dw_1 = create dw_1
this.r_2 = create r_2
this.r_1 = create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.uo_1
this.Control[iCurrent+2]=this.dw_1
this.Control[iCurrent+3]=this.r_2
this.Control[iCurrent+4]=this.r_1
end on

on u_portfolio.destroy
call super::destroy
destroy(this.uo_1)
destroy(this.dw_1)
destroy(this.r_2)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;long Lrows, i
iu_portfolio_piechart.visible = false

dw_1.setwsobject( gn_controller.of_get_wsconn( ) )

Lrows=dw_1.retrieve( gn_controller.of_get_profile_id( ) )
iu_portfolio_piechart.of_share( dw_1, true)

decimal ldc_price
for i =1 to Lrows
	ldc_price = gn_controller.of_get_current_price( dw_1.GetItemString( i, 'quoteid'))
	dw_1.SetItem(i, 'current_price_x', ldc_price)
next
end event

event Constructor;OpenUserObject(iu_portfolio_piechart,0,0)
end event

type uo_1 from u_quotebar within u_portfolio
end type

on uo_1.create
call super::create
end on

on uo_1.destroy
call super::destroy
end on

type dw_1 from u_dw within u_portfolio
string DataObject = "d_holdings"
end type

on dw_1.create
call super::create
end on

on dw_1.destroy
call super::destroy
end on

event constructor;call super::constructor;//turn on header click sorting
ib_headersort = true
end event

event buttonclicked;call super::buttonclicked;//show the trade stocks uo
u_buy_sell l_page
l_page = w_stocktrader.event ue_show_buy_sell( )
l_page.of_initialize( 'S', this.getitemnumber( row, 'quantity'), this.getitemnumber(row, 'holdingid'), &
	this.getitemstring(row, 'quoteid'),0)
end event

event clicked;call super::clicked;integer li_point
if row > 0 then
	//implode prior point
    if ii_priorpoint > 0 then iu_portfolio_piechart.dw_1.SetDataPieExplode("gr_1", 1, ii_priorpoint, 0)
	//get and explode current point
	li_point= iu_portfolio_piechart.dw_1.findcategory( 'gr_1',string(this.object.quoteid [ row ]) )
    iu_portfolio_piechart.dw_1.SetDataPieExplode("gr_1", 1, li_point, 30)
	ii_priorpoint = li_point  //set current to prior for next round
	
//get and set the chart title values
	iu_portfolio_piechart.dw_1.object.gr_1.title =	&
		string(this.object.quoteid [ row ])  + '  ' + string(this.Object.purchase_basis [ row ], '$###,###.00')
//set the uo position relative to the click point
	iu_portfolio_piechart.x = parent.PointerX( )
	iu_portfolio_piechart.y = parent.PointerY( )
	iu_portfolio_piechart.visible = true
else
	iu_portfolio_piechart.visible = false
end	 if
end event

type r_2 from Rectangle within u_portfolio
end type

on r_2.create
end on

on r_2.destroy
end on

type r_1 from Rectangle within u_portfolio
end type

on r_1.create
end on

on r_1.destroy
end on
