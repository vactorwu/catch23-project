forward
global type n_exception from exception
end type
end forward


global type n_exception from exception 
end type

global n_exception n_exception

type variables
private n_logger i_logger
end variables
forward prototypes
public subroutine of_setlogger (n_logger a_logger)
public subroutine setmessage (string newmessage, string a_classname)
end prototypes

public subroutine of_setlogger (n_logger a_logger);i_logger = a_logger
end subroutine

public subroutine setmessage (string newmessage, string a_classname);i_logger.of_log( a_classname, newmessage )
setmessage( newmessage )

end subroutine

on n_exception.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_exception.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
