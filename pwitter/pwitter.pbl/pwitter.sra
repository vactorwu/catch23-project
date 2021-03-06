$PBExportHeader$pwitter.sra
forward
global type pwitter from Application
end type
global Transaction sqlca
global DynamicDescriptionArea sqlda
global DynamicStagingArea sqlsa
global Error error
global Message message
end forward

global type pwitter from Application
string AppName = "pwitter"
end type
global pwitter pwitter

on pwitter.create
sqlca=create Transaction
sqlda=create DynamicDescriptionArea
sqlsa=create DynamicStagingArea
error=create Error
message=create Message
end on

on pwitter.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on

event Open;open(w_main)
end event
