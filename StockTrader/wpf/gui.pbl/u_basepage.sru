$PBExportHeader$u_basepage.sru
forward
global type u_basepage from UserObject
end type
type st_message from StaticText within u_basepage
end type
type st_timestamp from StaticText within u_basepage
end type
type st_asfo from StaticText within u_basepage
end type
type st_label from StaticText within u_basepage
end type
end forward

global type u_basepage from UserObject
event ue_setstate ()
st_message st_message
st_timestamp st_timestamp
st_asfo st_asfo
st_label st_label
end type
global u_basepage u_basepage

forward prototypes
protected subroutine of_showmessage (boolean ab_switch, string as_message)
public subroutine of_reset ()
end prototypes

protected subroutine of_showmessage (boolean ab_switch, string as_message);st_message.text = as_message
st_message.visible = ab_switch
return
end subroutine

public subroutine of_reset ();//polymorphic place holder
end subroutine

on u_basepage.create
this.st_message = create st_message
this.st_timestamp = create st_timestamp
this.st_asfo = create st_asfo
this.st_label = create st_label
this.Control[]={this.st_message,&
this.st_timestamp,&
this.st_asfo,&
this.st_label}
end on

on u_basepage.destroy
destroy(this.st_message)
destroy(this.st_timestamp)
destroy(this.st_asfo)
destroy(this.st_label)
end on

event ue_setstate;//establish the state for this page
//extend in descendants as needed
of_reset( )			//each descendent will over ride if necessary to reset page - clear out prior data - when it gets focus
st_timestamp.triggerevent( 'ue_settime')
end event

type st_message from StaticText within u_basepage
end type

on st_message.create
end on

on st_message.destroy
end on

type st_timestamp from StaticText within u_basepage
event ue_settime ()
end type

on st_timestamp.create
end on

on st_timestamp.destroy
end on

event ue_settime;//timestamp the page refresh
//this shows the age of the data displayed
datetime l_timestamp 
l_timestamp = datetime(today( ), now( ))
this.text = string( l_timestamp, 'mm/dd/yyyy hh:mm:ss am/pm')
end event

event constructor;this.event ue_settime( )
end event

type st_asfo from StaticText within u_basepage
end type

on st_asfo.create
end on

on st_asfo.destroy
end on

type st_label from StaticText within u_basepage
end type

on st_label.create
end on

on st_label.destroy
end on
