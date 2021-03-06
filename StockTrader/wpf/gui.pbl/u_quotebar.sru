$PBExportHeader$u_quotebar.sru
forward
global type u_quotebar from UserObject
end type
type sle_symbol from SingleLineEdit within u_quotebar
end type
type pb_getquote from PictureButton within u_quotebar
end type
end forward

global type u_quotebar from UserObject
sle_symbol sle_symbol
pb_getquote pb_getquote
end type
global u_quotebar u_quotebar

forward prototypes
public subroutine of_set_quote (string as_symbol)
end prototypes

public subroutine of_set_quote (string as_symbol);sle_symbol.text =  as_symbol
end subroutine

on u_quotebar.create
this.sle_symbol = create sle_symbol
this.pb_getquote = create pb_getquote
this.Control[]={this.sle_symbol,&
this.pb_getquote}
end on

on u_quotebar.destroy
destroy(this.sle_symbol)
destroy(this.pb_getquote)
end on

type sle_symbol from SingleLineEdit within u_quotebar
end type

on sle_symbol.create
end on

on sle_symbol.destroy
end on

type pb_getquote from PictureButton within u_quotebar
end type

on pb_getquote.create
end on

on pb_getquote.destroy
end on

event clicked;//functionality implemented in a window service 
//get the base window & call the event that manages the service
if Len( sle_symbol.Text) = 0 then return		//nothing entered, then do nothing
window basewindow
powerobject lpo
lpo=this
Do
	lpo=lpo.getparent()
loop until lpo.typeof( ) = window!
basewindow = lpo

basewindow.dynamic event ue_get_quote( sle_symbol.text  )
/* logic - if the quote uo is visible - call the method to get quote - 
					if the quote uo is not visible then
						put the symbol in the quote control sle on the quote page
						make it visible - call the setquote( ) method on the navbar
						post this u_quotebar.pb_getquote  clicked event to repeat the call
*/
end event
