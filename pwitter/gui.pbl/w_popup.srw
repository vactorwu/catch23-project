$PBExportHeader$w_popup.srw
forward
global type w_popup from Window
end type
type webbrowser1 from System.Windows.Controls.WebBrowser within w_popup
end type
end forward

global type w_popup from Window
webbrowser1 webbrowser1
end type
global w_popup w_popup

on w_popup.create
this.webbrowser1 = create webbrowser1
this.Control[]={this.webbrowser1}
end on

on w_popup.destroy
destroy(this.webbrowser1)
end on

type webbrowser1 from System.Windows.Controls.WebBrowser within w_popup
end type
