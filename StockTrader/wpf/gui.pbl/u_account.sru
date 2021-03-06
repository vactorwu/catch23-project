$PBExportHeader$u_account.sru
forward
global type u_account from u_basepage
end type
type dw_profile from u_dw within u_account
end type
type dw_account from u_dw within u_account
end type
type pb_save from PictureButton within u_account
end type
type r_2 from Rectangle within u_account
end type
type r_1 from Rectangle within u_account
end type
end forward

global type u_account from u_basepage
dw_profile dw_profile
dw_account dw_account
pb_save pb_save
r_2 r_2
r_1 r_1
end type
global u_account u_account

type variables
private:
Constant String MISSING_PASSWORD = 'Password missing or doesn~t match'
end variables

forward prototypes
private function boolean of_validate_password ()
private subroutine of_update ()
public subroutine of_reset ()
end prototypes

private function boolean of_validate_password ();//test if user skipped pwd entry and confirm (both are empty) and then if both entered - do both match?
//this snippet works in both pbnative and pb .net
string ls_orig, ls_copy
ls_orig = dw_profile.getitemstring( 1, 'password')
ls_copy = dw_profile.getitemstring( 1, 'password_confirm')

if isnull(ls_orig) or  len(ls_orig) < 1 then return false
if  isnull (ls_copy) or len (ls_copy) < 1 then return false
//passwords dont match

if ls_orig <> ls_copy then
	return false
else
	return true
end if

/*
	This snippet works as expected in pbnative  
	BUT
	this snippet doesn't work in pb .net Winform
	IT RETURNS TRUE IF THE USE SKIPS BOTH SLE
	to get it to work in .net and pbnative I had to use the local variable as above
	
if isnull(dw_profile.getitemstring( 1, 'password')) or len(dw_profile.getitemstring( 1, 'password')) < 1 then return false
if  isnull( dw_profile.getitemstring( 1, 'password_confirm')) or len (dw_profile.getitemstring( 1, 'password_confirm')) < 1 then return false
if dw_profile.getitemstring( 1, 'password') <> dw_profile.getitemstring( 1, 'password_confirm') then return false
*/
end function

private subroutine of_update ();/*validate required fields
pass parameters to remote method call
display result message
*/
of_showmessage( false,'')
dw_profile.accepttext( )
if of_validate_password( ) = false then
		of_showmessage( true, MISSING_PASSWORD)
		return
end if
//turn on the required markers to let the findrequired do its magic (nilisnul is set 'Yes' for all in the painter)
dw_profile.object.fullname.edit.required='Yes'
dw_profile.object.address.edit.required='Yes'
dw_profile.object.email.edit.required='Yes'
dw_profile.object.creditcard.edit.required='Yes'
string l_missing, l_str
long lrow=1
integer lcol=1
dw_profile.findrequired( primary!, lrow, lcol, l_missing, false)
if  lrow <> 0 then
	l_str += l_missing + '.ValidationMsg'
	of_showmessage( true,'Data Entry Error: ' + dw_profile.describe(l_str) )
	dw_profile.setcolumn( lcol)
else
	//contents of the pwd must match and be present
	//of_validate_password
	try
	gn_controller.of_update_profile( dw_profile.getitemstring(1, 'address'), dw_profile.getitemstring(1, 'creditcard'), &
	dw_profile.getitemstring(1, 'email'), dw_profile.getitemstring(1, 'fullname'), dw_profile.getitemstring(1, 'password') , &
	gn_controller.of_get_profile_id( )  )
	of_showmessage( true,'Changes Saved')
	dw_profile.setcolumn( 1)
	catch( exception e)
	of_showmessage( true,'Service Error: ' + e.getmessage( ) )
	end try
end if
//reset confirm
dw_profile.setitem( 1, 'password_confirm','')
//reset the markers to support free navigation
dw_profile.object.fullname.edit.required='No'
dw_profile.object.address.edit.required='No'
dw_profile.object.email.edit.required='No'
dw_profile.object.creditcard.edit.required='No'
dw_profile.setfocus( )
end subroutine

public subroutine of_reset ();dw_profile.Reset( )
dw_account.Reset( )
end subroutine

on u_account.create
int iCurrent
call super::create
this.dw_profile = create dw_profile
this.dw_account = create dw_account
this.pb_save = create pb_save
this.r_2 = create r_2
this.r_1 = create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.dw_profile
this.Control[iCurrent+2]=this.dw_account
this.Control[iCurrent+3]=this.pb_save
this.Control[iCurrent+4]=this.r_2
this.Control[iCurrent+5]=this.r_1
end on

on u_account.destroy
call super::destroy
destroy(this.dw_profile)
destroy(this.dw_account)
destroy(this.pb_save)
destroy(this.r_2)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;//get account into
dw_account.setwsobject( gn_controller.of_get_wsconn( ) )
dw_account.retrieve( gn_controller.of_get_profile_id( ) )

string ls_address, ls_creditcard, ls_email, ls_fullname
try
											//parameters are passed by reference
	gn_controller.of_getprofile( ls_address, ls_creditcard, ls_email, ls_fullname,  gn_controller.of_get_profile_id( ) )
	if dw_profile.rowcount( ) < 1 then dw_profile.insertrow( 0 )
	dw_profile.setitem( 1, 'address', ls_address)
	dw_profile.setitem( 1, 'creditcard', ls_creditcard)
	dw_profile.setitem( 1, 'email', ls_email)
	dw_profile.setitem( 1, 'fullname', ls_fullname)
	dw_profile.setfocus()
this. of_showmessage(false, '')		//hide the message
catch (exception e)
	this. of_showmessage(true, 'Failed to get profile ' + e.getmessage( ))
end try
end event

type dw_profile from u_dw within u_account
string DataObject = "d_profiledisplay"
end type

on dw_profile.create
call super::create
end on

on dw_profile.destroy
call super::destroy
end on

type dw_account from u_dw within u_account
string DataObject = "d_account"
end type

on dw_account.create
call super::create
end on

on dw_account.destroy
call super::destroy
end on

type pb_save from PictureButton within u_account
end type

on pb_save.create
end on

on pb_save.destroy
end on

event clicked;parent.  of_update( )
end event

type r_2 from Rectangle within u_account
end type

on r_2.create
end on

on r_2.destroy
end on

type r_1 from Rectangle within u_account
end type

on r_1.create
end on

on r_1.destroy
end on
