$PBExportHeader$w_main.srw
namespace
namespace LYC.Steam.Gui
using LYC.Steam.Api
using LYC.Steam.Gui
using LYC.Steam.Proxy
using ISteamApps_GetAppList
end namespace

forward
global type w_main from Window
end type
type tab_1 from Tab within w_main
end type
type tabpage_1 from UserObject within tab_1
end type
type dw_applist from DataWindow within tabpage_1
end type
type txt_input1 from System.Windows.Controls.TextBox within w_main
end type
type cb_search from CommandButton within w_main
end type
type cb_reset from CommandButton within w_main
end type
end forward

global type w_main from Window
event ue_init ()
tab_1 tab_1
txt_input1 txt_input1
cb_search cb_search
cb_reset cb_reset
end type
global w_main w_main

on w_main.create
this.tab_1 = create tab_1
this.txt_input1 = create txt_input1
this.cb_search = create cb_search
this.cb_reset = create cb_reset
this.Control[]={this.tab_1,&
this.txt_input1,&
this.cb_search,&
this.cb_reset}
end on

on w_main.destroy
destroy(this.tab_1)
destroy(this.txt_input1)
destroy(this.cb_search)
destroy(this.cb_reset)
end on

event ue_init;long ll_length,i

this.tab_1.tabpage_1.dw_applist.SetTransObject(sqlca)
ISteamApps_GetAppList.applist l_applist = gn_steam.GetAppList()
ll_length = l_applist.apps.Length

for i = 1 to ll_length
	this.tab_1.tabpage_1.dw_applist.Object.appid[i] = l_applist.apps[i].appid
	this.tab_1.tabpage_1.dw_applist.Object.name[i] = l_applist.apps[i].name
next

this.tab_1.tabpage_1.dw_applist.Retrieve()
end event

event Open;trigger event ue_init()
end event

type tab_1 from Tab within w_main
tabpage_1 tabpage_1
end type

on tab_1.create
this.tabpage_1 = create tabpage_1
this.Control[]={this.tabpage_1}
end on

on tab_1.destroy
destroy(this.tabpage_1)
end on

type tabpage_1 from UserObject within tab_1
dw_applist dw_applist
end type

on tabpage_1.create
this.dw_applist = create dw_applist
this.Control[]={this.dw_applist}
end on

on tabpage_1.destroy
destroy(this.dw_applist)
end on

type dw_applist from DataWindow within tabpage_1
end type

on dw_applist.create
end on

on dw_applist.destroy
end on

type txt_input1 from System.Windows.Controls.TextBox within w_main
end type

type cb_search from CommandButton within w_main
end type

on cb_search.create
end on

on cb_search.destroy
end on

event Clicked;string ls_query,ls_filter
ls_query = txt_input1.Text
ls_filter = "name like %'"+ ls_query +"'%"
tab_1.tabpage_1.dw_applist.SetFilter(ls_filter)
tab_1.tabpage_1.dw_applist.filter()
end event

type cb_reset from CommandButton within w_main
end type

on cb_reset.create
end on

on cb_reset.destroy
end on

event Clicked;tab_1.tabpage_1.dw_applist.SetFilter("")
tab_1.tabpage_1.dw_applist.filter()
end event
