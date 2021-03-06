$PBExportHeader$steam.sra
namespace
using LYC.Steam.Api
using LYC.Steam.Gui
using LYC.Steam.Proxy
end namespace

forward
global type steam from Application
end type
global Transaction sqlca
global DynamicDescriptionArea sqlda
global DynamicStagingArea sqlsa
global Error error
global Message message
end forward

global variables
n_steam gn_steam
n_logger gn_logger
end variables

global type steam from Application
string AppName = "steam"
end type
global steam steam

on steam.create
sqlca=create Transaction
sqlda=create DynamicDescriptionArea
sqlsa=create DynamicStagingArea
error=create Error
message=create Message
end on

on steam.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on

event Open;gn_steam = create n_steam
open(w_main)
end event
