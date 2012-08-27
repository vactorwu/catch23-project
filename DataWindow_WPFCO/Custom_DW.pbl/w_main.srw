$PBExportHeader$w_main.srw
forward
global type w_main from Window
end type
type cb_query from CommandButton within w_main
end type
type cb_exit from CommandButton within w_main
end type
type dw_1 from DataWindow within w_main
end type
type st_1 from StaticText within w_main
end type
end forward

global type w_main from Window
cb_query cb_query
cb_exit cb_exit
dw_1 dw_1
st_1 st_1
end type
global w_main w_main

type variables
long il_rowcount

public:
string ypmc
end variables

on w_main.create
this.cb_query = create cb_query
this.cb_exit = create cb_exit
this.dw_1 = create dw_1
this.st_1 = create st_1
this.Control[]={this.cb_query,&
this.cb_exit,&
this.dw_1,&
this.st_1}
end on

on w_main.destroy
destroy(this.cb_query)
destroy(this.cb_exit)
destroy(this.dw_1)
destroy(this.st_1)
end on

type cb_query from CommandButton within w_main
end type

on cb_query.create
end on

on cb_query.destroy
end on

event Clicked;dw_1.SetTransObject(sqlca)
il_rowcount = dw_1.Retrieve()
end event

type cb_exit from CommandButton within w_main
end type

on cb_exit.create
end on

on cb_exit.destroy
end on

event Clicked;Close(w_main)
end event

type dw_1 from DataWindow within w_main
end type

on dw_1.create
end on

on dw_1.destroy
end on

event Clicked;ypmc = dw_1.Object.ypmc[row]
end event

type st_1 from StaticText within w_main
end type

on st_1.create
end on

on st_1.destroy
end on
