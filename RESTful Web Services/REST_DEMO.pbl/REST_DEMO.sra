forward
global type REST_DEMO from application
end type
global transaction sqlca
global dynamicdescriptionarea sqlda
global dynamicstagingarea sqlsa
global error error
global message message
end forward

global type REST_DEMO from application
string appname = "REST_DEMO"
end type
global REST_DEMO REST_DEMO

on REST_DEMO.create
appname = "REST_DEMO"
message = create message
sqlca = create transaction
sqlda = create dynamicdescriptionarea
sqlsa = create dynamicstagingarea
error = create error
end on

on REST_DEMO.destroy
destroy( sqlca )
destroy( sqlda )
destroy( sqlsa )
destroy( error )
destroy( message )
end on
