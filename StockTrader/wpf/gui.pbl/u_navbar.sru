forward
global type u_navbar from userobject
end type
type st_login from st_navitem within u_navbar
end type
type st_config from st_navitem within u_navbar
end type
type st_quotes from st_navitem within u_navbar
end type
type st_orders from st_navitem within u_navbar
end type
type st_portfolio from st_navitem within u_navbar
end type
type st_account from st_navitem within u_navbar
end type
type st_home from st_navitem within u_navbar
end type
type st_welcome from st_navitem within u_navbar
end type
end forward


global type u_navbar from userobject 

st_login st_login 
st_config st_config 
st_quotes st_quotes 
st_orders st_orders 
st_portfolio st_portfolio 
st_account st_account 
st_home st_home 
st_welcome st_welcome 
end type


global u_navbar u_navbar

forward prototypes
public subroutine of_reset_button_colors ()
public subroutine of_setpage (integer ai_page)
end prototypes

public subroutine of_reset_button_colors ();//walk the control array - resetting colors of all buttons to neutral (unselected)
int j, i
i = upperbound( this.control)
statictext l_label
for j = 1 to i
	if typeof(control [j]) = statictext! then
		l_label = control [j]
		l_label.backcolor = 20790589
		l_label.textcolor = 30578299
	end if
next

end subroutine

public subroutine of_setpage (integer ai_page);/*
constant integer WELCOME=1
constant integer HOME = 2
constant integer ACCOUNT = 3
constant integer PORTFOLIO = 4
constant integer ORDERS = 5
constant integer QUOTES= 6
constant integer CONFIG = 7
constant integer LOGIN = 8
*/
choose case ai_page
	case 1
		st_welcome.triggerevent(clicked!)		
	case 2
		st_home.triggerevent(clicked!)	
	case 3
		st_account.triggerevent(clicked!)	
	case 4
		st_portfolio.triggerevent(clicked!)	
	case 5
		st_orders.triggerevent(clicked!)	
	case 6
		st_quotes.triggerevent(clicked!)	
	case 7
		st_config.triggerevent(clicked!)	
	case 8
		st_login.triggerevent(clicked!)	
end choose
end subroutine

on u_navbar.create
this.st_login=create st_login
this.st_config=create st_config
this.st_quotes=create st_quotes
this.st_orders=create st_orders
this.st_portfolio=create st_portfolio
this.st_account=create st_account
this.st_home=create st_home
this.st_welcome=create st_welcome
this.Control[]={this.st_login,&
this.st_config,&
this.st_quotes,&
this.st_orders,&
this.st_portfolio,&
this.st_account,&
this.st_home,&
this.st_welcome}
end on

on u_navbar.destroy
destroy(this.st_login)
destroy(this.st_config)
destroy(this.st_quotes)
destroy(this.st_orders)
destroy(this.st_portfolio)
destroy(this.st_account)
destroy(this.st_home)
destroy(this.st_welcome)
end on

type st_login from st_navitem within u_navbar 

end type



type st_config from st_navitem within u_navbar 

end type



type st_quotes from st_navitem within u_navbar 

end type



type st_orders from st_navitem within u_navbar 

end type



type st_portfolio from st_navitem within u_navbar 

end type



type st_account from st_navitem within u_navbar 

end type



type st_home from st_navitem within u_navbar 

end type



type st_welcome from st_navitem within u_navbar 

end type

