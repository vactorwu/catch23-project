$PBExportHeader$w_stocktrader.srw
forward
global type w_stocktrader from Window
end type
type uo_header from u_header within w_stocktrader
end type
end forward

global type w_stocktrader from Window
event ue_set_page (string as_tag)
event ue_get_quote (string as_symbol)
event ue_show_register ()
event type u_basepage ue_show_buy_sell ()
event ue_display_orders ()
uo_header uo_header
end type
global w_stocktrader w_stocktrader

type variables
//page array
constant integer NUMPAGES=11
constant integer WELCOME=1
constant integer HOME = 2
constant integer ACCOUNT = 3
constant integer PORTFOLIO = 4
constant integer ORDERS = 5
constant integer QUOTES= 6
constant integer CONFIG = 7
constant integer LOGIN = 8
constant integer LOGOUT = 9
constant integer REGISTER = 10
constant integer BUY_SELL = 11

u_basepage iu_pages[11]		// can't use constant NUMPAGES var yet !!!
//origin point of user object pages
int i_uo_x=4, i_uo_y=371
end variables

forward prototypes
private function boolean of_is_current_page (integer ai_page)
end prototypes

private function boolean of_is_current_page (integer ai_page);return iu_pages [ ai_page ]. visible
end function

on w_stocktrader.create
this.uo_header = create uo_header
this.Control[]={this.uo_header}
end on

on w_stocktrader.destroy
destroy(this.uo_header)
end on

event ue_set_page;//.NET modification
//There is no SetRedraw(false ) method for .NET - To avoid showing stale data before the new data is loaded from a WCF call
// the .visible = true assignments must be after the calls to ue_setstate

//setup a page when it is invoked by the user clicking on a menu item
//array index,title
int l_index, i

l_index = integer(left(as_tag,1))  //only works for 9 pages@! Change mechanism if supporting more 
//make all pages invisible
for i = 1 to NUMPAGES
	iu_pages[ i ].visible = false
next
IF gn_controller.of_isconnected( ) = false then   //show only welcome and connect pages if not connected
	if l_index = welcome or l_index = config then 
		iu_pages[ l_index ].event ue_setstate( )
		iu_pages[ l_index ].visible = true
		return
	else
		return 
	end if
end if

Choose case l_index 
	//welcome and config pages can show even if not logged in
	case config, welcome
		iu_pages[ l_index ].visible = true
//login is a modal page.  It has two views - if logged in show logout; if not logged in but connected then show login
case login
	if gn_controller.of_is_loggedin( ) = true then //show logout page
		iu_pages[ LOGIN ].visible = false
		iu_pages[ LOGOUT].event ue_setstate( )
		iu_pages[ LOGOUT ].visible = true
	else	//show login page
		iu_pages[ LOGOUT ].visible = false
		iu_pages[ LOGIN].event ue_setstate( )
		iu_pages[ LOGIN ].visible = true
	end if	
//all other pages can only show if logged in
case else
if gn_controller.of_is_loggedin( ) = true then 
	iu_pages[ l_index ].event ue_setstate( )
	iu_pages[ l_index ].visible = true	
end if
END choose
end event

event ue_get_quote;//make the quote page current, invoking the service to get the quote
/* logic - if the quote uo is visible - call the method to get quote - 
					if the quote uo is not visible then
						put the symbol in the quote control sle on the quote page
						make it visible - call the setquote( ) method on the navbar
						post this u_quotebar.pb_getquote  clicked event to repeat the call
*/
if not of_is_current_page( QUOTES) THEN
	//put the symbol in the quote control sle on the quote page 
	iu_pages[Quotes].dynamic event ue_set_quote( as_symbol )	
	//make it visible
	Uo_header.of_setpage ( QUOTES )
END IF
//call retrieve on the quote page
iu_pages[Quotes].dynamic of_get_quote( as_symbol )
end event

event ue_show_register;//special case - this page can only be accessed by clicking button on login page
iu_pages[ LOGIN ].visible = false
iu_pages[ REGISTER ].visible = true
iu_pages[ REGISTER ].event ue_setstate( )
end event

event ue_show_buy_sell;//make the quote page current, invoking the service to get the quote
/* logic - if the quote uo is visible - call the method to get quote - 
					if the quote uo is not visible then
						put the symbol in the quote control sle on the quote page
						make it visible - call the setquote( ) method on the navbar
						post this u_quotebar.pb_getquote  clicked event to repeat the call
*/
int i
if not of_is_current_page( BUY_SELL) THEN
for i = 1 to NUMPAGES
	iu_pages[ i ].visible = false
next
iu_pages[ BUY_SELL] .visible = true
return iu_pages[ BUY_SELL] 	
END IF
end event

event ue_display_orders;//make the quote page current, invoking the service to get the quote
/* logic - if the quote uo is visible - call the method to get quote - 
					if the quote uo is not visible then
						put the symbol in the quote control sle on the quote page
						make it visible - call the setquote( ) method on the navbar
						post this u_quotebar.pb_getquote  clicked event to repeat the call
*/
if not of_is_current_page( ORDERS ) THEN
	Uo_header.of_setpage ( ORDERS )
END IF
end event

event open;//load the pages 
//this is sorta like painter generated code
//the pages will be stateful. when they become visible they will have their previous state
//each menu item has a number in the tag that corresponds to the index in the uo array
this.openuserobject(iu_pages [1], 'u_welcome',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [2], 'u_home',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [3], 'u_account',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [4], 'u_portfolio',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [5], 'u_orders',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [6], 'u_quotes',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [7], 'u_config',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [8], 'u_login',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [9], 'u_logout',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [10], 'u_register',  i_uo_x, i_uo_y)
this.openuserobject(iu_pages [11], 'u_buy_sell',  i_uo_x, i_uo_y)

int i
for i=2 to NUMPAGES
	iu_pages[ i ].visible = false
next
end event

type uo_header from u_header within w_stocktrader
end type

on uo_header.create
call super::create
end on

on uo_header.destroy
call super::destroy
end on
