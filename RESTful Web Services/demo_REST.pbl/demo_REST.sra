$PBExportHeader$demo_REST.sra
namespace
using getcountryinfo
end namespace

forward
global type demo_REST from Application
end type
global Transaction sqlca
global DynamicDescriptionArea sqlda
global DynamicStagingArea sqlsa
global Error error
global Message message
end forward

global variables
getcountryinfo.geonames g_geonames;
getcountryinfo.geonamesCountry g_geonamescountry;
end variables

global type demo_REST from Application
string AppName = "demo_REST"
end type
global demo_REST demo_REST

on demo_REST.create
sqlca=create Transaction
sqlda=create DynamicDescriptionArea
sqlsa=create DynamicStagingArea
error=create Error
message=create Message
end on

on demo_REST.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on

event Open;Open(w_main)
end event
