forward
global type u_buy_sell from u_basepage
end type
type r_2 from rectangle within u_buy_sell
end type
type sle_amount from singlelineedit within u_buy_sell
end type
type pb_buy_sell from picturebutton within u_buy_sell
end type
type st_action from statictext within u_buy_sell
end type
type st_1 from statictext within u_buy_sell
end type
type r_1 from rectangle within u_buy_sell
end type
end forward


global type u_buy_sell from u_basepage 
r_2 r_2 
sle_amount sle_amount 
pb_buy_sell pb_buy_sell 
st_action st_action 
st_1 st_1 
r_1 r_1 
end type


global u_buy_sell u_buy_sell

type variables
private:
string is_mode, is_symbol
long il_shares, il_holdingid 
double idc_price
end variables

forward prototypes
public subroutine of_action ()
public subroutine of_initialize (string as_buy_sell_flag, long al_shares, long al_holdingid, string as_symbol, decimal adc_price)
end prototypes

public subroutine of_action ();string INVALID_NUMBER 
n_order ln_order
//have a valid #; based on the action type call the buy or sell method on the proxy; open the order window; than refresh the holdings page to show the change
	try
	if is_mode = 'S' then
		INVALID_NUMBER = 'Please enter a valid number between 1 and '
		INVALID_NUMBER += string(il_shares)
		if not isnumber( sle_amount.text) or long( sle_amount.text) > il_shares then
			of_showmessage( true, invalid_number )
			return
		end if
		of_showmessage( FALSE, '')
		ln_order = gn_controller.of_sell( il_holdingid, long( sle_amount.text) )
	else
		INVALID_NUMBER = 'Please enter a valid number greater than 1'
		if not isnumber( sle_amount.text) or long( sle_amount.text) < 1 then
			of_showmessage( true, invalid_number )
			return
		end if
		of_showmessage( FALSE, '')
		ln_order = gn_controller.of_buy( gn_controller.of_get_profile_id( ) , is_symbol, long( sle_amount.text) )
	end if
catch ( exception e)
	of_showmessage( true, 'Transaction Failed ' + e.getmessage( ) )
	return
end try

openwithparm( w_order_alert, ln_order )
w_stocktrader.event ue_display_orders( )	
end subroutine

public subroutine of_initialize (string as_buy_sell_flag, long al_shares, long al_holdingid, string as_symbol, decimal adc_price);is_mode = as_buy_sell_flag
il_shares = al_shares
il_holdingid = al_holdingid
is_symbol = as_symbol
idc_price = adc_price
//expecting 'B' or 'S'
if as_buy_sell_flag = 'B' then
	pb_buy_sell.text ='Buy'
	st_action.text = 'You have requested to buy shares of ' + as_symbol + ' which is currently trading at ' +  string(adc_price, '$#,###.00')
else
	pb_buy_sell.text ='Sell'
	st_action.text = 'You have requested to sell all or part of your holding #' + string( al_holdingid) + &
		'. This holding has a total of ' + string(al_shares) + ' shares of stock ' + as_symbol + '.  Please input how many shares to sell.'
end if
sle_amount.text = ''

end subroutine

on u_buy_sell.create
int iCurrent
call super::create
this.r_2=create r_2
this.sle_amount=create sle_amount
this.pb_buy_sell=create pb_buy_sell
this.st_action=create st_action
this.st_1=create st_1
this.r_1=create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.r_2
this.Control[iCurrent+2]=this.sle_amount
this.Control[iCurrent+3]=this.pb_buy_sell
this.Control[iCurrent+4]=this.st_action
this.Control[iCurrent+5]=this.st_1
this.Control[iCurrent+6]=this.r_1
end on

on u_buy_sell.destroy
call super::destroy
destroy(this.r_2)
destroy(this.sle_amount)
destroy(this.pb_buy_sell)
destroy(this.st_action)
destroy(this.st_1)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;of_showmessage( false, '')
sle_amount.text=''
end event

type st_message from u_basepage`st_message within u_buy_sell 
end type



type st_timestamp from u_basepage`st_timestamp within u_buy_sell 
end type



type st_asfo from u_basepage`st_asfo within u_buy_sell 
end type



type st_label from u_basepage`st_label within u_buy_sell 

end type



type r_2 from rectangle within u_buy_sell 

end type



type sle_amount from singlelineedit within u_buy_sell 

end type



type pb_buy_sell from picturebutton within u_buy_sell 

end type



event clicked;parent. dynamic of_action( )
end event

type st_action from statictext within u_buy_sell 

end type



type st_1 from statictext within u_buy_sell 

end type



type r_1 from rectangle within u_buy_sell 

end type

