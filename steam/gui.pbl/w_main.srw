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
type tabpage_2 from UserObject within tab_1
end type
type cb_reset from CommandButton within w_main
end type
type cb_prior from CommandButton within w_main
end type
type cb_next from CommandButton within w_main
end type
type txt_input1 from CommandButton within w_main
end type
type cb_search from System.Windows.Controls.TextBox within w_main
end type
end forward

global type w_main from Window
event ue_init ()
tab_1 tab_1
cb_reset cb_reset
cb_prior cb_prior
cb_next cb_next
txt_input1 txt_input1
cb_search cb_search
end type
global w_main w_main

on w_main.create
this.tab_1 = create tab_1
this.cb_reset = create cb_reset
this.cb_prior = create cb_prior
this.cb_next = create cb_next
this.txt_input1 = create txt_input1
this.cb_search = create cb_search
this.Control[]={this.tab_1,&
this.cb_reset,&
this.cb_prior,&
this.cb_next,&
this.txt_input1,&
this.cb_search}
end on

on w_main.destroy
destroy(this.tab_1)
destroy(this.cb_reset)
destroy(this.cb_prior)
destroy(this.cb_next)
destroy(this.txt_input1)
destroy(this.cb_search)
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
tabpage_2 tabpage_2
end type

on tab_1.create
this.tabpage_1 = create tabpage_1
this.tabpage_2 = create tabpage_2
this.Control[]={this.tabpage_1,&
this.tabpage_2}
end on

on tab_1.destroy
destroy(this.tabpage_1)
destroy(this.tabpage_2)
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

type tabpage_2 from UserObject within tab_1
end type

on tabpage_2.create
end on

on tabpage_2.destroy
end on

type cb_reset from CommandButton within w_main
end type

on cb_reset.create
end on

on cb_reset.destroy
end on

event Clicked;tab_1.tabpage_1.dw_applist.SetFilter("")
tab_1.tabpage_1.dw_applist.filter()
end event

type cb_prior from CommandButton within w_main
end type

on cb_prior.create
end on

on cb_prior.destroy
end on

event Clicked;tab_1.tabpage_1.dw_applist.ScrollNextPage()
end event

type cb_next from CommandButton within w_main
end type

on cb_next.create
end on

on cb_next.destroy
end on

event Clicked;tab_1.tabpage_1.dw_applist.ScrollPriorPage()
end event

type txt_input1 from CommandButton within w_main
end type

on txt_input1.create
end on

on txt_input1.destroy
end on

event Clicked;string ls_query,ls_filter
ls_query = txt_input1.Text
ls_filter = "name like %'"+ ls_query +"'%"
tab_1.tabpage_1.dw_applist.SetFilter(ls_filter)
tab_1.tabpage_1.dw_applist.filter()
end event

type cb_search from System.Windows.Controls.TextBox within w_main
end type
