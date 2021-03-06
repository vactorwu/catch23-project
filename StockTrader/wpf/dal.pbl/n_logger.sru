forward
global type n_logger from nonvisualobject
end type
end forward


global type n_logger from nonvisualobject 
end type

global n_logger n_logger

type variables
private constant string IS_LOGFILE='FundTraderErrors.log'
end variables
forward prototypes
public function integer of_log (string as_source, string as_message)
end prototypes

public function integer of_log (string as_source, string as_message);/*
Function Name: of_log
Author:  Yakov
Created: 2009/06/09
Purpose:  Log important error and application event messages to a text file

Modification History
-----------------------------
*/
//if a parameter is null or empty return -1
IF as_source ='' or as_message = '' or isnull(as_source) or isnull(as_message) THEN
	return -1
END IF
integer li_filenumber, li_chars
String ls_message

//open the file for append
li_filenumber = FileOpen( is_logfile, linemode!,write!, lockwrite!, append!, EncodingUTF8!)
//failed to open
If li_filenumber = -1 then return -2

//build the timestamp
ls_message = '***' + string(today( ) , 'dddd, mmmm dd yyyy hh:mm:ss') + '***'
//append the object 
ls_message += ' Object:'+ as_source
//append the message
ls_message += ' Message:' + as_message

li_chars = FileWriteEx( li_filenumber, ls_message)
FileClose( li_filenumber )
return li_chars

end function

on n_logger.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_logger.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on
