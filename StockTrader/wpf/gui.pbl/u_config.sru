forward
global type u_config from u_basepage
end type
type r_2 from rectangle within u_config
end type
type sle_host from singlelineedit within u_config
end type
type st_1 from statictext within u_config
end type
type pb_connect from picturebutton within u_config
end type
type r_1 from rectangle within u_config
end type
end forward


global type u_config from u_basepage 
r_2 r_2 
sle_host sle_host 
st_1 st_1 
pb_connect pb_connect 
r_1 r_1 
end type


global u_config u_config

type variables
Private:
constant string INI_FILE = 'StockTrader.ini'
Constant String SUCCESS = 'Successfully connected to '
Constant String FAILURE = 'Failed to connect to '
end variables

forward prototypes
private subroutine of_connect ()
end prototypes

private subroutine of_connect ();try
	setpointer(hourglass!)
	gn_controller.of_connect( sle_host.text )
	SetProfileString( Ini_file, 'connection', 'host', sle_host.text )
	of_showmessage(true, success + ' ' + sle_host.text)
catch (Exception e)
	of_showmessage(true, e.getmessage( ))
end try

end subroutine

on u_config.create
int iCurrent
call super::create
this.r_2=create r_2
this.sle_host=create sle_host
this.st_1=create st_1
this.pb_connect=create pb_connect
this.r_1=create r_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.r_2
this.Control[iCurrent+2]=this.sle_host
this.Control[iCurrent+3]=this.st_1
this.Control[iCurrent+4]=this.pb_connect
this.Control[iCurrent+5]=this.r_1
end on

on u_config.destroy
call super::destroy
destroy(this.r_2)
destroy(this.sle_host)
destroy(this.st_1)
destroy(this.pb_connect)
destroy(this.r_1)
end on

event constructor;call super::constructor;sle_host.text =  ProfileString( Ini_file, 'connection', 'host', 'http://[fill in your host here]')
end event

type st_message from u_basepage`st_message within u_config 
end type



type st_timestamp from u_basepage`st_timestamp within u_config 
end type



type st_asfo from u_basepage`st_asfo within u_config 
end type



type st_label from u_basepage`st_label within u_config 

end type



type r_2 from rectangle within u_config 

end type



type sle_host from singlelineedit within u_config 

end type



type st_1 from statictext within u_config 

end type



type pb_connect from picturebutton within u_config 

end type



event clicked;parent.of_connect( )
end event

type r_1 from rectangle within u_config 

end type

