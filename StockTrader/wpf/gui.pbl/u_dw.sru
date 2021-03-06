forward
global type u_dw from datawindow
end type
end forward


global type u_dw from datawindow 

end type

global u_dw u_dw

type variables
//controls column header click sort
//when designing dwo to work with this service
//header name must be same as column name and be suffixed by _t for this to work
boolean ib_headersort=false
string is_sortcolumn, is_sortorder
end variables
on u_dw.create
end on

on u_dw.destroy
end on

event clicked;//////////////////////////////////////////////////////////////////////////////
//
//	Function:  		pfc_clicked
//
//	Access:    		Public
//
//	Arguments:  	(passed arguments of dw Clicked event)
//  xpos (integer)  		 	The x-position of the mouse
//  ypos (integer)  			The y-position of the mouse
//  row (long) 				The row number clicked on, if any
//	 dwo (dwobject)   		The dwobject clicked on )
//	 cntl_clicked (boolean) Indicates that the user pressed Ctnl/Click
//
//	Returns:   		Integer
//   					 1 if it succeeds.
//						 0 if no action was required.
//						-1 if an error occurs.
//
//	Description:  	Causes sorting when the user clicks on the header
//					  	of a datawindow, emulating the WIN95 style.
//					  	Click causes new sort (alternately Asc/Desc if same column).
//
//////////////////////////////////////////////////////////////////////////////
//
//	Revision History
//
//	Version
//	5.0   Initial version
//
//////////////////////////////////////////////////////////////////////////////
//
/*
 * Open Source PowerBuilder Foundation Class Libraries
 *
 * Copyright (c) 2004-2005, All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted in accordance with the GNU Lesser General
 * Public License Version 2.1, February 1999
 *
 * http://www.gnu.org/copyleft/lesser.html
 *
 * ====================================================================
 *
 * This software consists of voluntary contributions made by many
 * individuals and was originally based on software copyright (c) 
 * 1996-2004 Sybase, Inc. http://www.sybase.com.  For more
 * information on the Open Source PowerBuilder Foundation Class
 * Libraries see http://pfc.codexchange.sybase.com
*/
//
//////////////////////////////////////////////////////////////////////////////

string	ls_headername
string 	ls_colname
integer	li_rc
integer	li_suffixlen
integer	li_headerlen
string	ls_sortstring

// Validate the dwo reference.
IF IsNull(dwo) OR NOT IsValid(dwo) THEN	 
	Return -1
END IF 

// Check if the service is set to sort on column headers.
IF NOT ib_headersort THEN Return 0

// Only valid on header column.
If dwo.Name = 'datawindow' THEN Return 0
IF dwo.Band <> "header" THEN Return 0

// Get column header information.
ls_headername = dwo.Name
li_headerlen = Len(ls_headername)
li_suffixlen = 2//Len(is_defaultheadersuffix)

// Extract the columname from the header label 
// (by taking out the header suffix).
IF Right(ls_headername, li_suffixlen) <> '_t' THEN 
	// Cannot determine the column name from the header.	
	Return -1
END IF 	
ls_colname = Left (ls_headername, li_headerlen - li_suffixlen)

// Validate the column name.
If IsNull(ls_colname) or Len(Trim(ls_colname))=0 Then 
	Return -1
End If

// Check the previous sort click.
IF is_sortcolumn = ls_colname THEN	
	// Second sort click on the same column, reverse sort order.
	IF is_sortorder = " A" THEN 	
		is_sortorder = " D"
	ELSE
		is_sortorder = " A"
	END IF 
ELSE
	// Clicked on a different column.
	is_sortcolumn = ls_colname
	is_sortorder = " A" 
END IF 

// Build the sort string.
	ls_sortstring = is_sortcolumn + is_sortorder

// Perform the SetSort operation (check the rc).
li_rc = this.SetSort (ls_sortstring) 
If li_rc < 0 Then Return li_rc

// Perform the actual Sort operation (check the rc).
li_rc = this.Sort()
If li_rc < 0 Then Return li_rc	
	
Return 1

end event
