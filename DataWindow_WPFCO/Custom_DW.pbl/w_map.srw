$PBExportHeader$w_map.srw
forward
global type w_map from Window
end type
type vemap1 from VirtualEarthWPFControl.VEMap within w_map
end type
type sle_input from SingleLineEdit within w_map
end type
type cb_search from CommandButton within w_map
end type
end forward

global type w_map from Window
vemap1 vemap1
sle_input sle_input
cb_search cb_search
end type
global w_map w_map

on w_map.create
this.vemap1 = create vemap1
this.sle_input = create sle_input
this.cb_search = create cb_search
this.Control[]={this.vemap1,&
this.sle_input,&
this.cb_search}
end on

on w_map.destroy
destroy(this.vemap1)
destroy(this.sle_input)
destroy(this.cb_search)
end on

type vemap1 from VirtualEarthWPFControl.VEMap within w_map
end type

type sle_input from SingleLineEdit within w_map
end type

on sle_input.create
end on

on sle_input.destroy
end on

type cb_search from CommandButton within w_map
end type

on cb_search.create
end on

on cb_search.destroy
end on

event Clicked;vemap1.Find(sle_input.Text)
end event
