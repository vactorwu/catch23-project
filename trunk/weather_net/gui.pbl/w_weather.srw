$PBExportHeader$w_weather.srw
forward
global type w_weather from Window
end type
type dw_province from DataWindow within w_weather
end type
type hpb_connect from HProgressBar within w_weather
end type
type dw_city from DataWindow within w_weather
end type
type dw_weather from DataWindow within w_weather
end type
end forward

global type w_weather from Window
dw_province dw_province
hpb_connect hpb_connect
dw_city dw_city
dw_weather dw_weather
end type
global w_weather w_weather

on w_weather.create
this.dw_province = create dw_province
this.hpb_connect = create hpb_connect
this.dw_city = create dw_city
this.dw_weather = create dw_weather
this.Control[]={this.dw_province,&
this.hpb_connect,&
this.dw_city,&
this.dw_weather}
end on

on w_weather.destroy
destroy(this.dw_province)
destroy(this.hpb_connect)
destroy(this.dw_city)
destroy(this.dw_weather)
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

type hpb_connect from HProgressBar within w_weather
end type

on hpb_connect.create
end on

on hpb_connect.destroy
end on

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
