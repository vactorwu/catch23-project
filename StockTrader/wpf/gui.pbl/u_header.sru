$PBExportHeader$u_header.sru
forward
global type u_header from UserObject
end type
type uo_navbar from u_navbar within u_header
end type
type st_2 from StaticText within u_header
end type
type st_1 from StaticText within u_header
end type
type p_1 from Picture within u_header
end type
end forward

global type u_header from UserObject
uo_navbar uo_navbar
st_2 st_2
st_1 st_1
p_1 p_1
end type
global u_header u_header

forward prototypes
public subroutine of_setpage (integer ai_page)
end prototypes

public subroutine of_setpage (integer ai_page);uo_navbar.of_setpage( ai_page )
end subroutine

on u_header.create
this.uo_navbar = create uo_navbar
this.st_2 = create st_2
this.st_1 = create st_1
this.p_1 = create p_1
this.Control[]={this.uo_navbar,&
this.st_2,&
this.st_1,&
this.p_1}
end on

on u_header.destroy
destroy(this.uo_navbar)
destroy(this.st_2)
destroy(this.st_1)
destroy(this.p_1)
end on

type uo_navbar from u_navbar within u_header
end type

on uo_navbar.create
call super::create
end on

on uo_navbar.destroy
call super::destroy
end on

type st_2 from StaticText within u_header
end type

on st_2.create
end on

on st_2.destroy
end on

type st_1 from StaticText within u_header
end type

on st_1.create
end on

on st_1.destroy
end on

type p_1 from Picture within u_header
end type

on p_1.create
end on

on p_1.destroy
end on
