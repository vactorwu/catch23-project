forward
global type u_logout from u_basepage
end type
type pb_logout from picturebutton within u_logout
end type
type r_2 from rectangle within u_logout
end type
type r_1 from rectangle within u_logout
end type
end forward


global type u_logout from u_basepage 
pb_logout pb_logout 
r_2 r_2 
r_1 r_1 
end type


global u_logout u_logout

type variables
private constant string SUCCESS='You are now logged out'
end variables

forward prototypes
public subroutine of_logout ()
end prototypes

public subroutine of_logout ();gn_controller.of_logout( )
st_message.text = SUCCESS
st_message.visible = true
w_stocktrader lw_win
lw_win = getparent( )
lw_win.Uo_header.of_setpage (  lw_win.login )

end subroutine

on u_logout.create
int iCurrent
call super::create
this.pb_logout=create pb_logout
this.r_2=create r_2
this.r_1=create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.pb_logout
this.Control[iCurrent+2]=this.r_2
this.Control[iCurrent+3]=this.r_1
end on

on u_logout.destroy
call super::destroy
destroy(this.pb_logout)
destroy(this.r_2)
destroy(this.r_1)
end on

event ue_setstate;call super::ue_setstate;st_message.visible = false
end event

type st_timestamp from u_basepage`st_timestamp within u_logout 
end type



type st_asfo from u_basepage`st_asfo within u_logout 
end type



type st_label from u_basepage`st_label within u_logout 

end type



type pb_logout from picturebutton within u_logout 

end type



event clicked;parent. dynamic of_logout( )
end event

type r_2 from rectangle within u_logout 

end type



type r_1 from rectangle within u_logout 

end type

