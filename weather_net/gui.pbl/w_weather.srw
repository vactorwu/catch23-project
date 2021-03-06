$PBExportHeader$w_weather.srw
forward
global type w_weather from Window
end type
type dw_province from DataWindow within w_weather
end type
type dw_city from DataWindow within w_weather
end type
type dw_weather from DataWindow within w_weather
end type
type cb_1 from CommandButton within w_weather
end type
type hpb_retrieve from HProgressBar within w_weather
end type
type cb_2 from CommandButton within w_weather
end type
type l_grid from System.Windows.Controls.Grid within w_weather
end type
type cb_3 from CommandButton within w_weather
end type
type htb_1 from HTrackBar within w_weather
end type
end forward

global type w_weather from Window
dw_province dw_province
dw_city dw_city
dw_weather dw_weather
cb_1 cb_1
hpb_retrieve hpb_retrieve
cb_2 cb_2
l_grid l_grid
cb_3 cb_3
htb_1 htb_1
end type
global w_weather w_weather

on w_weather.create
this.dw_province = create dw_province
this.dw_city = create dw_city
this.dw_weather = create dw_weather
this.cb_1 = create cb_1
this.hpb_retrieve = create hpb_retrieve
this.cb_2 = create cb_2
this.l_grid = create l_grid
this.cb_3 = create cb_3
this.htb_1 = create htb_1
this.Control[]={this.dw_province,&
this.dw_city,&
this.dw_weather,&
this.cb_1,&
this.hpb_retrieve,&
this.cb_2,&
this.l_grid,&
this.cb_3,&
this.htb_1}
end on

on w_weather.destroy
destroy(this.dw_province)
destroy(this.dw_city)
destroy(this.dw_weather)
destroy(this.cb_1)
destroy(this.hpb_retrieve)
destroy(this.cb_2)
destroy(this.l_grid)
destroy(this.cb_3)
destroy(this.htb_1)
end on

event Open;weather_proxy_WeatherWebServiceSoapClient l_proxy 

l_proxy = create weather_proxy_WeatherWebServiceSoapClient
dw_province.Retrieve()
end event

type dw_province from DataWindow within w_weather
end type

on dw_province.create
end on

on dw_province.destroy
end on

event Clicked;string ls_province
ls_province = dw_province.object.returnvalue[row]
dw_city.retrieve(ls_province)
end event

type dw_city from DataWindow within w_weather
end type

on dw_city.create
end on

on dw_city.destroy
end on

event Clicked;string ls_city,ls_citycode
ls_city = dw_city.object.returnvalue[row]
ls_citycode = gf_getcitycode(ls_city)
dw_weather.retrieve(ls_citycode)
end event

type dw_weather from DataWindow within w_weather
end type

on dw_weather.create
end on

on dw_weather.destroy
end on

type cb_1 from CommandButton within w_weather
end type

on cb_1.create
end on

on cb_1.destroy
end on

event Clicked;close(w_weather)
end event

type hpb_retrieve from HProgressBar within w_weather
end type

on hpb_retrieve.create
end on

on hpb_retrieve.destroy
end on

type cb_2 from CommandButton within w_weather
end type

on cb_2.create
end on

on cb_2.destroy
end on

event Clicked;dw_weather.Retrieve("苏州")
end event

type l_grid from System.Windows.Controls.Grid within w_weather
end type

type cb_3 from CommandButton within w_weather
end type

on cb_3.create
end on

on cb_3.destroy
end on

event Clicked;System.Windows.Controls.Calendar l_cal
l_cal = create System.Windows.Controls.Calendar()

//messagebox("画布容器内控件数",String(l_grid.Children.Capacity))
l_grid.Children.Add(l_cal)
l_grid.RegisterName("l_cal",l_cal)
System.Windows.Controls.Canvas.SetLeft(l_cal,0)
System.Windows.Controls.Canvas.Settop(l_cal,500)
//messagebox("画布容器内控件数",String(l_grid.Children.Capacity))

messagebox("",String(dw_weather.Object.ReturnValue[1]))
end event

type htb_1 from HTrackBar within w_weather
end type

on htb_1.create
end on

on htb_1.destroy
end on
