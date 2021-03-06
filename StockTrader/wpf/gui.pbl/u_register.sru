$PBExportHeader$u_register.sru
forward
global type u_register from u_basepage
end type
type dw_register from u_dw within u_register
end type
type pb_save from PictureButton within u_register
end type
type r_2 from Rectangle within u_register
end type
type r_1 from Rectangle within u_register
end type
end forward

global type u_register from u_basepage
dw_register dw_register
pb_save pb_save
r_2 r_2
r_1 r_1
end type
global u_register u_register

type variables
private:
Constant String MISSING_PASSWORD = "Password missing or doesn't match"
end variables

forward prototypes
public subroutine of_update ()
public function boolean of_validate_password ()
public subroutine of_reset ()
end prototypes

public subroutine of_update ();/*validate required fields
pass parameters to remote method call
display result message
*/
of_showmessage( false,'')
dw_register.accepttext( )
if of_validate_password( ) = false then
		of_showmessage( true, MISSING_PASSWORD)
		dw_register.setitem( 1, 'password_confirm','')
		return
end if
//turn on the required markers to let the findrequired do its magic (nilisnul is set 'Yes' for all columns in the painter)
dw_register.object.fullname.edit.required='Yes'
dw_register.object.address.edit.required='Yes'
dw_register.object.email.edit.required='Yes'
dw_register.object.creditcard.edit.required='Yes'
dw_register.object.openingbalance.edit.required='Yes'
dw_register.object.userid.edit.required='Yes'

string l_missing, l_str
long lrow=1
integer lcol=1
dw_register.findrequired( primary!, lrow, lcol, l_missing, false)
if  lrow <> 0 then
	l_str += l_missing + '.ValidationMsg'
	of_showmessage( true,'Data Entry Error: ' + dw_register.describe(l_str) )
	dw_register.setcolumn( lcol)
else
	//contents of the pwd must match and be present
	//of_validate_password
	try
	if gn_controller.of_register( dw_register.getitemstring(1, 'userid') , dw_register.getitemstring(1, 'password') , dw_register.getitemstring(1, 'fullname'),&
	dw_register.getitemstring(1, 'address'), dw_register.getitemstring(1, 'email'), dw_register.getitemstring(1, 'creditcard'), &
	dw_register.getitemdecimal(1, 'openingbalance')  ) = true then 
		of_showmessage( true,'Registration Successful')
		dw_register.object.fullname.edit.required='No'
		dw_register.object.address.edit.required='No'
		dw_register.object.email.edit.required='No'
		dw_register.object.creditcard.edit.required='No'
		dw_register.object.openingbalance.edit.required='No'
		dw_register.object.userid.edit.required='No'
		dw_register.reset( )
		w_stocktrader.event ue_set_page('8')
	end if
	catch( exception e)
	of_showmessage( true,'User ID is already in use. Try another User ID' )
	end try
end if
//reset the markers to support free navigation
dw_register.object.fullname.edit.required='No'
dw_register.object.address.edit.required='No'
dw_register.object.email.edit.required='No'
dw_register.object.creditcard.edit.required='No'
dw_register.object.openingbalance.edit.required='No'
dw_register.object.userid.edit.required='No'
dw_register.setfocus( )
end subroutine

public function boolean of_validate_password ();//test if user skipped pwd entry and confirm (both are empty) and then if both entered - do both match?
//this snippet works in both pbnative and pb .net
string ls_orig, ls_copy

ls_orig = dw_register.getitemstring( 1, 'password')
ls_copy = dw_register.getitemstring( 1, 'password_confirm')

if isnull(ls_orig) or  len(ls_orig) < 1 then return false
if  isnull (ls_copy) or len (ls_copy) < 1 then return false
//passwords dont match

if ls_orig <> ls_copy then
	return false
else
	return true
end if

/*
	this code doesn't not work in .net - the dw never returns null
	to get it to work in .net I had to use the local variable as above

if isnull(dw_register.getitemstring( 1, 'password')) or len(dw_register.getitemstring( 1, 'password')) < 1 then return false
if  isnull( dw_register.getitemstring( 1, 'password_confirm')) or len (dw_register.getitemstring( 1, 'password_confirm')) < 1 then return false
if dw_register.getitemstring( 1, 'password') <> dw_register.getitemstring( 1, 'password_confirm') then return false
*/
end function

public subroutine of_reset ();dw_register.reset( )
end subroutine

on u_register.create
int iCurrent
call super::create
this.dw_register = create dw_register
this.pb_save = create pb_save
this.r_2 = create r_2
this.r_1 = create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.dw_register
this.Control[iCurrent+2]=this.pb_save
this.Control[iCurrent+3]=this.r_2
this.Control[iCurrent+4]=this.r_1
end on

on u_register.destroy
call super::destroy
destroy(this.dw_register)
destroy(this.pb_save)
destroy(this.r_2)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;dw_register.insertrow( 0 )
end event

type dw_register from u_dw within u_register
string DataObject = "d_register"
end type

on dw_register.create
call super::create
end on

on dw_register.destroy
call super::destroy
end on

event itemerror;call super::itemerror;return 1  //surpress message - we're handling it in the static text
end event

type pb_save from PictureButton within u_register
end type

on pb_save.create
end on

on pb_save.destroy
end on

event clicked;parent.  of_update( )
end event

type r_2 from Rectangle within u_register
end type

on r_2.create
end on

on r_2.destroy
end on

type r_1 from Rectangle within u_register
end type

on r_1.create
end on

on r_1.destroy
end on
