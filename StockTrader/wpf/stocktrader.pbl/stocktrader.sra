forward
global type stocktrader from application
end type
global transaction sqlca
global dynamicdescriptionarea sqlda
global dynamicstagingarea sqlsa
global error error
global message message
end forward

global variables
n_controller gn_controller
end variables


global type stocktrader from application 
string appname = "stocktrader"

end type

global stocktrader stocktrader

on stocktrader.create
appname="stocktrader"
message=create message
sqlca=create transaction
sqlda=create dynamicdescriptionarea
sqlsa=create dynamicstagingarea
error=create error
end on

on stocktrader.destroy
destroy(sqlca)
destroy(sqlda)
destroy(sqlsa)
destroy(error)
destroy(message)
end on

event open;//instantiate the controller layer
gn_controller = CREATE n_controller
open(w_stocktrader)
end event

event close;destroy gn_controller
end event
