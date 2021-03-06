$PBExportHeader$n_logger.sru
namespace
using System.IO.Log
using System.IO
using System.Text
end namespace

forward
global type n_logger from NonVisualObject
end type
end forward

global type n_logger from NonVisualObject
end type
global n_logger n_logger

type variables
private:
string path = ".\steam.log"
string logContainer = "MyContainer"
long containerSize = 1024 * 1024
LogRecordSequence i_log
end variables

forward prototypes
public function integer append (string as_log)
end prototypes

public function integer append (string as_log);//system.text.encoding enc = system.text.encoding.unicode
//byte array[]
//array[] = enc.getbytes(as_log)
//system.arraysegment<byte> list
//system.arraysegment<byte> segments[]
//
//segments[] = create system.arraysegment<byte>[1]
//segments[0] = create system.arraysegment<byte>(array)
//list = system.array.asreadonly<system.arraysegment<byte>>(segments[])
//i_log.append(list,sequencenumber.invalid,sequencenumber.invalid,recordappendoptions.forceflush!);
//
//return 1
return 1
end function

on n_logger.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_logger.destroy
TriggerEvent( this, "destructor" )
call super::destroy
end on

event Constructor;i_log = create LogRecordSequence(path,System.IO.FileMode.CreateNew!,System.IO.FileAccess.ReadWrite!,System.IO.FileShare.None!)
i_log.LogStore.Extents.Add(logContainer,containerSize)
end event
