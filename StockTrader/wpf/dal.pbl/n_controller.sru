$PBExportHeader$n_controller.sru
namespace
using fundtraderproxy
end namespace

forward
global type n_controller from NonVisualObject
end type
end forward

global type n_controller from NonVisualObject
end type
global n_controller n_controller

type variables
private:
boolean ib_connected, ib_loggedin		/* these control gui views on/off */
string is_host								/* used for proxy and ws dwo calls */

/*  These are the standard services */
//wcf proxy reference
fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices i_service
//DataWindow WS connection object
wsconnection i_wsconn
//error logging object
n_logger i_logger
//Standard exception thrown in response to received System.Exception
n_exception i_ex

/*  These are the value objects returned by proxy calls*/
		//need this for UID info
accountdatabean i_account					//returned by login,  getaccountdata, register
accountprofiledatabean i_profile			//returned by getaccountprofiledata & updateaccountprofile

holdingdatabean i_holding  					//returned by get holding
holdingdatabean i_holdings [  ]				//returned by get holdingS 

marketsummarydatabeanws i_marketsummary  //returned by getmarketsummary

orderdatabean	i_order							//returned by sell & sellenhanced
orderdatabean	i_orders [ ]						//returned as ANY by getOrders
quotedatabean i_quote							//returned by getQuote

datastore ids_holdings		//in Classic arrays of beans are not a valid datatype-proxy sees them as ANY 
							// but dwos can handle them

/* Value objects for passing between the Controller and View layers - non proxy types */
n_holding_totals i_totals	//a value object for data retrieval
n_order in_order				//a value object for passing to gui tier
end variables

forward prototypes
public function boolean of_isconnected ()
public function boolean of_is_loggedin ()
public subroutine of_logout ()
public function boolean of_connect (string as_url) throws Exception
public function WSConnection of_get_wsconn ()
public function boolean of_login (string as_uid, string as_pwd) throws Exception
public subroutine of_getmarketsummary () throws Exception
public function decimal of_get_current_price (string as_symbol)
public function string of_get_profile_id ()
public function boolean of_update_profile (readonly string as_address, readonly string as_creditcard, readonly string as_email, readonly string as_fullname, readonly string as_password, string as_userid) throws Exception
public function decimal of_gettsia () throws Exception
public function double of_getvolume () throws Exception
public function integer of_settops (DataWindow a_buffer, boolean ab_gainer)
public function n_holding_totals of_getholdingtotals (string as_id)
public subroutine of_getprofile (ref string as_address, ref string as_creditcard, ref string as_email, ref string as_fullname, string as_uid) throws Exception
public function boolean of_register (readonly string as_userid, readonly string as_password, readonly string as_fullname, readonly string as_address, readonly string as_email, readonly string as_creditcard, readonly decimal ad_openbalance) throws Exception
public function n_order of_sell (long al_holdingid, long al_quantity) throws Exception
public function n_order of_buy (string as_uid, string as_symbol, double ad_quantity) throws Exception
end prototypes

public function boolean of_isconnected ();return ib_connected
end function

public function boolean of_is_loggedin ();return ib_loggedin
end function

public subroutine of_logout ();//destroys account state object
//sets ib_loggedin to false
i_service.logout( i_account.profileid )
destroy i_account
ib_loggedin = false
end subroutine

public function boolean of_connect (string as_url) throws Exception;/*
Attempt to establish a connection to the app server at the user entered uri
The boolean ib_connect controls GUI access to login page
The wsconnection object points the ws dwos to the same server as the proxy
*/

int retcode

ib_connected = false
ib_loggedin = false

if not isvalid( i_service) then
	i_service = create fundtraderproxy_TradeServicesClient_BasicHttpBinding_ITradeServices
end if
//dynamically set the endpoint url with the user supplied value - replace the one hardcoded in the proxy by the code generator
	PBWCF.WCFEndpointAddress d_endpoint
	d_endpoint = create PBWCF.WCFEndpointAddress
	d_endpoint.URL = as_url
	i_service.wcfConnectionObject.EndpointAddress = d_endpoint

if not isvalid (i_wsconn) then 
	i_wsconn = create wsconnection
end if
i_wsconn.endpoint =  as_url		//dynamically set the endpoint for the ws dwos
try
	i_service .isonline( )
catch (System.Exception e)
	ib_connected = false
	i_ex.setmessage( 'Unable to connect to ' + as_url, this.classname( ) )
	throw i_ex
end try
ib_connected = true
return true
end function

public function WSConnection of_get_wsconn ();//used by datastore and datacontrol setWSobject( ) method calls to point retrieve to runtime assigned URI
return this.i_wsconn
end function

public function boolean of_login (string as_uid, string as_pwd) throws Exception;//wraps a call to the proxy login method
//ib_logged_in controls GUI access to all data pages
//if successful, stores the user profile in the i_account value object
try
	i_account = i_service.login( as_uid, as_pwd )
catch (System.Exception e)
	i_ex.setmessage(  'Remote exception ' + e.Message , this.classname( ))
	throw i_ex
end try
if isvalid (i_account ) then
	ib_loggedin = true
	return true
else
	return false
end if
end function

public subroutine of_getmarketsummary () throws Exception;//wraps a call to the proxy get market summary method
try
	i_marketsummary = i_service.getmarketsummary( )
catch (System.Exception e)
	i_ex.setmessage(  'Remote exception ' + e.message , 'of_getmarketsummary')
	throw i_ex
end try
end subroutine

public function decimal of_get_current_price (string as_symbol);//wraps a call to the proxy getquote method
//returns a decimal holding the quote price value
try
	i_quote = i_service.getquote( as_symbol )
	return i_quote.price
catch (System.Exception e)
	i_ex.setmessage( e.message, this.classname() )
end try
return -1
end function

public function string of_get_profile_id ();//returns the profile id from the i_account value object
return i_account.profileid
end function

public function boolean of_update_profile (readonly string as_address, readonly string as_creditcard, readonly string as_email, readonly string as_fullname, readonly string as_password, string as_userid) throws Exception;//wraps a call to the proxy updateaccountprofile method
//make sure we have a bean for the 1st call
if not isvalid (i_profile) then i_profile = CREATE accountprofiledatabean
//bean may be dirty from last call - but all params are required so all values will be current

//this reference is only here for debug purposes - to see the echoed returned values from the proxy call
accountprofiledatabean l_profile

i_profile.address = as_address
i_profile.creditcard = as_creditcard
i_profile.email = as_email
i_profile.fullname = as_fullname
i_profile.password = as_password
i_profile.userid = as_userid
try					//the call echoes the bean return
	L_profile	= i_service.updateaccountprofile( i_profile )
catch (System.Exception e)
	i_ex.setmessage( e.message, 'of_update_profile( )')
	throw i_ex
end try
destroy l_profile
return true
end function

public function decimal of_gettsia () throws Exception;//call of_GetMarketSummary( ) before calling this method
//returns tsia value after getmarketsummary has been called
if isvalid( i_marketsummary ) then 
	return i_marketsummary.tsia
else
	i_ex.setmessage(  'Market Summary Object Not valid  ' +  'of_gettsia')
	throw i_ex
end if
end function

public function double of_getvolume () throws Exception;if isvalid( i_marketsummary ) then 
	return i_marketsummary.volume
else
	i_ex.setmessage(  'Market Summary Object Not valid  ' +  'of_getvolume')
	throw i_ex
end if
end function

public function integer of_settops (DataWindow a_buffer, boolean ab_gainer);//populates a the contents of a passed datawindow buffer - with either gainers or losers
//the dwo was defined manually via external RS definition

quotedatabean l_beans[ ]	//method call returns an array of beans
integer l_ctr, i, row
if ab_gainer = true then
	l_beans = i_marketsummary.topgainers
else
	l_beans = i_marketsummary.toplosers
end if
l_ctr = upperbound( l_beans)
if l_ctr > 0 then
	for i = 1 to l_ctr
		row = a_buffer.insertrow( 0 )
		a_buffer.setitem( row, 'symbol', l_beans [ i ].companyname)
		a_buffer.setitem( row, 'price', l_beans [ i ].price)
		a_buffer.setitem( row, 'change', l_beans [ i ].change)
	next
end if
return l_ctr
end function

public function n_holding_totals of_getholdingtotals (string as_id);//called from the home page - uses a ws dwo inside a datstore to get totals
long Lrows, i
decimal ldc_price
Double l_current_value					//temp fix - until engineering supplies fix to getitem from summary band
ids_holdings.dataobject = 'd_holding_totals'
ids_holdings.setwsobject( this.of_get_wsconn( )  )
Lrows = ids_holdings.retrieve(  as_id )

for i =1 to Lrows
	ldc_price = this.of_get_current_price( ids_holdings.GetItemString( i, 'quoteid'))
	ids_holdings.SetItem(i, 'current_price', ldc_price)
	l_current_value += ids_holdings.GetItemNumber( i, 'current_value')		//temp fix - until engineering supplies fix to getitem from summary band
next
i_totals.holding_count =Lrows				// awaiting engineering fix ids_holdings.getitemnumber( 0, 'holding_count')
i_totals.holding_total = l_current_value	//awaiting engineering fix ids_holdings.getitemnumber( 0, 'holding_total')
											//i_totals.gain_total =  ids_holdings.getitemnumber( 0, 'gain_total')
return i_totals
end function

public subroutine of_getprofile (ref string as_address, ref string as_creditcard, ref string as_email, ref string as_fullname, string as_uid) throws Exception;//values are returned by reference from local state - no need to call to get these
//sets REF parameters on successful call
try
	i_profile = i_service.getaccountprofiledata( as_uid )
	as_address 		= i_profile.address
	as_creditcard 	= i_profile.creditcard
	as_email 		= i_profile.email
	as_fullname	= i_profile.fullname
	return
catch (System.Exception e)
	i_ex.setmessage(  'Remote exception ' + e.message , 'of_getprofile for ' + as_uid )
	throw i_ex
end try
end subroutine

public function boolean of_register (readonly string as_userid, readonly string as_password, readonly string as_fullname, readonly string as_address, readonly string as_email, readonly string as_creditcard, readonly decimal ad_openbalance) throws Exception;//wraps a call to proxy register method
try
	i_account = i_service.register( as_userid, as_password, as_fullname, as_address, as_email, as_creditcard, ad_openbalance )
	return true
catch (System.Exception e)
	i_ex.setmessage(  'Remote exception ' + e.message , this.classname( ))
	throw i_ex
end try
if isvalid (i_account ) then
	return true
else
	return false
end if
end function

public function n_order of_sell (long al_holdingid, long al_quantity) throws Exception;//wraps a call to the proxy sellenhanced method - returns data in a non proxy specific value object (CCUO)
//this approach is to keep proxy objects out of the GUI - this allows tier independence - proxy change will not directly impact the GUI
if not isvalid( in_order) then in_order = create n_order
try
i_order	= i_service.sellenhanced( this.of_get_profile_id( ) , al_holdingid, al_quantity )
in_order.completiondate = i_order.completiondate
in_order.opendate	= i_order.opendate
in_order.orderfee  	= i_order.orderfee
in_order.price 		= i_order.price
in_order.quantity		= i_order.quantity
in_order.orderid		= i_order.orderid
in_order.orderstatus	= i_order.orderstatus
in_order.ordertype	= i_order.ordertype
in_order.symbol		= i_order.symbol
return in_order		
catch (System.Exception e)
	i_ex.setmessage( e.message, 'of_sell')
	throw i_ex
end try
end function

public function n_order of_buy (string as_uid, string as_symbol, double ad_quantity) throws Exception;//wraps ws buy method call , returns a non proxy PB custom type
if not isvalid( in_order) then in_order = create n_order
try 
	i_order = i_service.buy( as_uid, as_symbol, ad_quantity, 0)
	in_order.completiondate = i_order.completiondate
	in_order.opendate	= i_order.opendate
	in_order.orderfee  	= i_order.orderfee
	in_order.price 		= i_order.price
	in_order.quantity		= i_order.quantity
	in_order.orderid		= i_order.orderid
	in_order.orderstatus	= i_order.orderstatus
	in_order.ordertype	= i_order.ordertype
	in_order.symbol		= i_order.symbol
return in_order		
catch (System.Exception e)
	i_ex.setmessage( e.message, 'of_buy')
	throw i_ex
end try
end function

on n_controller.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_controller.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event constructor;//instantiate helper objects
i_ex = CREATE n_exception
i_logger = CREATE n_logger
//register the logger with the exception
i_ex.of_setlogger( i_logger )
ids_holdings = CREATE datastore
//value object for home page total return data
i_totals = create n_holding_totals
end event
