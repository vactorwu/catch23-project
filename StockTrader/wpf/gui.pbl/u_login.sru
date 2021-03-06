$PBExportHeader$u_login.sru
forward
global type u_login from u_basepage
end type
type st_1 from StaticText within u_login
end type
type sle_uid from SingleLineEdit within u_login
end type
type pb_login from PictureButton within u_login
end type
type st_2 from StaticText within u_login
end type
type sle_password from SingleLineEdit within u_login
end type
type pb_register from PictureButton within u_login
end type
type r_2 from Rectangle within u_login
end type
type r_1 from Rectangle within u_login
end type
end forward

global type u_login from u_basepage
st_1 st_1
sle_uid sle_uid
pb_login pb_login
st_2 st_2
sle_password sle_password
pb_register pb_register
r_2 r_2
r_1 r_1
end type
global u_login u_login

type variables
private:
constant string SUCCESS='Login Succeed'
constant string FAILURE = 'Login Failed:  Check user id / password'
end variables

forward prototypes
public subroutine of_login ()
public subroutine of_register ()
end prototypes

public subroutine of_login ();w_stocktrader lw_win
setpointer( hourglass!)
try
if gn_controller.of_login( sle_uid.text, sle_password.text) = true then
	st_message.visible = false
	//set focus to home window
	lw_win = getparent( )
	lw_win.Uo_header.of_setpage (  lw_win.home )
else
	of_showmessage(true, failure)
end if
catch (exception e)
	of_showmessage(true, e.getmessage( ))
end try
end subroutine

public subroutine of_register ();w_stocktrader lw_win
st_message.visible = false
lw_win = getparent( )
lw_win.triggerevent( 'ue_show_register' )
end subroutine

on u_login.create
int iCurrent
call super::create
this.st_1 = create st_1
this.sle_uid = create sle_uid
this.pb_login = create pb_login
this.st_2 = create st_2
this.sle_password = create sle_password
this.pb_register = create pb_register
this.r_2 = create r_2
this.r_1 = create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.st_1
this.Control[iCurrent+2]=this.sle_uid
this.Control[iCurrent+3]=this.pb_login
this.Control[iCurrent+4]=this.st_2
this.Control[iCurrent+5]=this.sle_password
this.Control[iCurrent+6]=this.pb_register
this.Control[iCurrent+7]=this.r_2
this.Control[iCurrent+8]=this.r_1
end on

on u_login.destroy
call super::destroy
destroy(this.st_1)
destroy(this.sle_uid)
destroy(this.pb_login)
destroy(this.st_2)
destroy(this.sle_password)
destroy(this.pb_register)
destroy(this.r_2)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;//reinitialize after logout 
sle_uid.text=''
sle_password.text=''
end event

type st_1 from StaticText within u_login
end type

on st_1.create
end on

on st_1.destroy
end on

type sle_uid from SingleLineEdit within u_login
end type

on sle_uid.create
end on

on sle_uid.destroy
end on

type pb_login from PictureButton within u_login
end type

on pb_login.create
end on

on pb_login.destroy
end on

event clicked;parent. of_login( )
end event

type st_2 from StaticText within u_login
end type

on st_2.create
end on

on st_2.destroy
end on

type sle_password from SingleLineEdit within u_login
end type

on sle_password.create
end on

on sle_password.destroy
end on

type pb_register from PictureButton within u_login
end type

on pb_register.create
end on

on pb_register.destroy
end on

event clicked;parent.of_register( )
end event

type r_2 from Rectangle within u_login
end type

on r_2.create
end on

on r_2.destroy
end on

type r_1 from Rectangle within u_login
end type

on r_1.create
end on

on r_1.destroy
end on
