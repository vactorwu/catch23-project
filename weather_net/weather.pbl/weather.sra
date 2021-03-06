$PBExportHeader$weather.sra
forward
global type weather from Application
end type
global Transaction sqlca
global DynamicDescriptionArea sqlda
global DynamicStagingArea sqlsa
global Error error
global Message message
end forward

global type weather from Application
string AppName = "weather"
end type
global weather weather

on weather.create
sqlca=create Transaction
sqlda=create DynamicDescriptionArea
sqlsa=create DynamicStagingArea
error=create Error
message=create Message
end on

on weather.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on

event Open;open(w_weather)
end event
