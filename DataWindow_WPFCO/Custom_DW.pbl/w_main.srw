$PBExportHeader$w_main.srw
forward
global type w_main from Window
end type
type dw_1 from DataWindow within w_main
end type
type cb_query from CommandButton within w_main
end type
type cb_exit from CommandButton within w_main
end type
type cb_1 from CommandButton within w_main
end type
type raddropdownbutton1 from Telerik.Windows.Controls.RadDropDownButton within w_main
end type
end forward

global type w_main from Window
dw_1 dw_1
cb_query cb_query
cb_exit cb_exit
cb_1 cb_1
raddropdownbutton1 raddropdownbutton1
end type
global w_main w_main

type variables
long il_rowcount
end variables

on w_main.create
this.dw_1 = create dw_1
this.cb_query = create cb_query
this.cb_exit = create cb_exit
this.cb_1 = create cb_1
this.raddropdownbutton1 = create raddropdownbutton1
this.Control[]={this.dw_1,&
this.cb_query,&
this.cb_exit,&
this.cb_1,&
this.raddropdownbutton1}
end on

on w_main.destroy
destroy(this.dw_1)
destroy(this.cb_query)
destroy(this.cb_exit)
destroy(this.cb_1)
destroy(this.raddropdownbutton1)
end on

type dw_1 from DataWindow within w_main
end type

on dw_1.create
end on

on dw_1.destroy
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

type cb_1 from CommandButton within w_main
end type

on cb_1.create
end on

on cb_1.destroy
end on

event Clicked;open(w_map)
end event

type raddropdownbutton1 from Telerik.Windows.Controls.RadDropDownButton within w_main
end type
