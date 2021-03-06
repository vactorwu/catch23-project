$PBExportHeader$u_welcome.sru
forward
global type u_welcome from u_basepage
end type
type r_3 from Rectangle within u_welcome
end type
type r_2 from Rectangle within u_welcome
end type
type dw_1 from DataWindow within u_welcome
end type
type rte_1 from RichTextEdit within u_welcome
end type
type st_credits from StaticText within u_welcome
end type
type mle_1 from MultiLineEdit within u_welcome
end type
end forward

global type u_welcome from u_basepage
r_3 r_3
r_2 r_2
dw_1 dw_1
rte_1 rte_1
st_credits st_credits
mle_1 mle_1
end type
global u_welcome u_welcome

on u_welcome.create
int iCurrent
call super::create
this.r_3 = create r_3
this.r_2 = create r_2
this.dw_1 = create dw_1
this.rte_1 = create rte_1
this.st_credits = create st_credits
this.mle_1 = create mle_1
iCurrent=UpperBound(this.Control)
this.Control[iCurrent+1]=this.r_3
this.Control[iCurrent+2]=this.r_2
this.Control[iCurrent+3]=this.dw_1
this.Control[iCurrent+4]=this.rte_1
this.Control[iCurrent+5]=this.st_credits
this.Control[iCurrent+6]=this.mle_1
end on

on u_welcome.destroy
call super::destroy
destroy(this.r_3)
destroy(this.r_2)
destroy(this.dw_1)
destroy(this.rte_1)
destroy(this.st_credits)
destroy(this.mle_1)
end on

type r_3 from Rectangle within u_welcome
end type

on r_3.create
end on

on r_3.destroy
end on

type r_2 from Rectangle within u_welcome
end type

on r_2.create
end on

on r_2.destroy
end on

type dw_1 from DataWindow within u_welcome
string DataObject = "d_welcome"
end type

on dw_1.create
end on

on dw_1.destroy
end on

type rte_1 from RichTextEdit within u_welcome
end type

on rte_1.create
end on

on rte_1.destroy
end on

event Constructor;this.InsertDocument('images\WelcomeText.rtf', true)
end event

type st_credits from StaticText within u_welcome
end type

on st_credits.create
end on

on st_credits.destroy
end on

event Clicked;if mle_1.Visible = true then
	mle_1.Visible = false
else
	mle_1.Visible=true
end	 if
end event

type mle_1 from MultiLineEdit within u_welcome
end type

on mle_1.create
end on

on mle_1.destroy
end on

event Constructor;Text = 'Crafted for Sybase Inc. by~r~n'
Text += 'Yakov Werde of eLearnIT LLC~r~n'
Text += 'http://www.eLearnITonline.com~r~n'
Text += 'With assistance from the Sybase team~r~n'
Text += 'Sue Dunnell~r~n'
Text += 'Dave Fish~r~n'
Text += 'Lisa Hopkins~r~n'
Text += 'John Strano~r~n'
Text += 'Melinda Wilson~r~n'
Text += 'Special Thanks to~r~n'
Text += 'Don Clayton of Intertech Consulting Inc'
end event
