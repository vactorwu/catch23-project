forward
global type st_navitem from statictext
end type
end forward


global type st_navitem from statictext 

end type

global st_navitem st_navitem

type variables
//a reference to the navigation bar which contains this button
private u_navbar iu_navbar
end variables

on st_navitem.create
end on

on st_navitem.destroy
end on

event constructor;//register the bar - to enable message sending
iu_navbar = this.getparent( )
end event

event clicked;//unmark all - mark selected
iu_navbar.of_reset_button_colors( )
this.backcolor = 15793151
this.textcolor = 255

//signal the window to change the current control
//functionality implemented in a window service 
//get the base window & call the event that manages the service
window basewindow
powerobject lpo
lpo=this
Do
	lpo=lpo.getparent()
loop until lpo.typeof( ) = window!
basewindow = lpo

//the name of the user object is u_ + the value in the tag
basewindow.dynamic event ue_set_page( this.tag )
end event
