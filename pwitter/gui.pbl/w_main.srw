$PBExportHeader$w_main.srw
forward
global type w_main from Window
end type
type grid1 from System.Windows.Controls.Grid within w_main
end type
end forward

global type w_main from Window
grid1 grid1
end type
global w_main w_main

on w_main.create
this.grid1 = create grid1
this.Control[]={this.grid1}
end on

on w_main.destroy
destroy(this.grid1)
end on

type grid1 from System.Windows.Controls.Grid within w_main
end type
