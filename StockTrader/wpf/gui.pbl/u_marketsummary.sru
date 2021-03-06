$PBExportHeader$u_marketsummary.sru
forward
global type u_marketsummary from UserObject
end type
type r_1 from Rectangle within u_marketsummary
end type
type dw_losers from DataWindow within u_marketsummary
end type
type dw_gainers from DataWindow within u_marketsummary
end type
type st_volume from StaticText within u_marketsummary
end type
type st_tsia from StaticText within u_marketsummary
end type
type st_5 from StaticText within u_marketsummary
end type
type st_4 from StaticText within u_marketsummary
end type
type st_3 from StaticText within u_marketsummary
end type
type st_2 from StaticText within u_marketsummary
end type
type st_1 from StaticText within u_marketsummary
end type
type r_2 from Rectangle within u_marketsummary
end type
type r_3 from Rectangle within u_marketsummary
end type
type r_4 from Rectangle within u_marketsummary
end type
end forward

global type u_marketsummary from UserObject
r_1 r_1
dw_losers dw_losers
dw_gainers dw_gainers
st_volume st_volume
st_tsia st_tsia
st_5 st_5
st_4 st_4
st_3 st_3
st_2 st_2
st_1 st_1
r_2 r_2
r_3 r_3
r_4 r_4
end type
global u_marketsummary u_marketsummary

forward prototypes
public subroutine of_settsia (decimal ad_tsia)
public subroutine of_setvolumne (double ad_volume)
public function DataWindow of_getgainers ()
public function DataWindow of_getlosers ()
public subroutine of_reset ()
end prototypes

public subroutine of_settsia (decimal ad_tsia);st_tsia.text = string(ad_tsia, '####.00')
end subroutine

public subroutine of_setvolumne (double ad_volume);st_volume.text = string(ad_volume, '#####.00')
end subroutine

public function DataWindow of_getgainers ();return dw_gainers
end function

public function DataWindow of_getlosers ();return dw_LOSERS
end function

public subroutine of_reset ();dw_gainers.Reset( )
dw_losers.Reset( )
st_tsia.Text = ''
st_volume.Text = ''
end subroutine

on u_marketsummary.create
this.r_1 = create r_1
this.dw_losers = create dw_losers
this.dw_gainers = create dw_gainers
this.st_volume = create st_volume
this.st_tsia = create st_tsia
this.st_5 = create st_5
this.st_4 = create st_4
this.st_3 = create st_3
this.st_2 = create st_2
this.st_1 = create st_1
this.r_2 = create r_2
this.r_3 = create r_3
this.r_4 = create r_4
this.Control[]={this.r_1,&
this.dw_losers,&
this.dw_gainers,&
this.st_volume,&
this.st_tsia,&
this.st_5,&
this.st_4,&
this.st_3,&
this.st_2,&
this.st_1,&
this.r_2,&
this.r_3,&
this.r_4}
end on

on u_marketsummary.destroy
destroy(this.r_1)
destroy(this.dw_losers)
destroy(this.dw_gainers)
destroy(this.st_volume)
destroy(this.st_tsia)
destroy(this.st_5)
destroy(this.st_4)
destroy(this.st_3)
destroy(this.st_2)
destroy(this.st_1)
destroy(this.r_2)
destroy(this.r_3)
destroy(this.r_4)
end on

event constructor;//upgrading blew out custom colors so I set em in code
st_2.textcolor=30578299
st_3.textcolor=30578299
st_4.textcolor=30578299
st_5.textcolor=30578299
end event

type r_1 from Rectangle within u_marketsummary
end type

on r_1.create
end on

on r_1.destroy
end on

type dw_losers from DataWindow within u_marketsummary
string DataObject = "d_tops"
end type

on dw_losers.create
end on

on dw_losers.destroy
end on

type dw_gainers from DataWindow within u_marketsummary
string DataObject = "d_tops"
end type

on dw_gainers.create
end on

on dw_gainers.destroy
end on

type st_volume from StaticText within u_marketsummary
end type

on st_volume.create
end on

on st_volume.destroy
end on

type st_tsia from StaticText within u_marketsummary
end type

on st_tsia.create
end on

on st_tsia.destroy
end on

type st_5 from StaticText within u_marketsummary
end type

on st_5.create
end on

on st_5.destroy
end on

type st_4 from StaticText within u_marketsummary
end type

on st_4.create
end on

on st_4.destroy
end on

type st_3 from StaticText within u_marketsummary
end type

on st_3.create
end on

on st_3.destroy
end on

type st_2 from StaticText within u_marketsummary
end type

on st_2.create
end on

on st_2.destroy
end on

type st_1 from StaticText within u_marketsummary
end type

on st_1.create
end on

on st_1.destroy
end on

type r_2 from Rectangle within u_marketsummary
end type

on r_2.create
end on

on r_2.destroy
end on

type r_3 from Rectangle within u_marketsummary
end type

on r_3.create
end on

on r_3.destroy
end on

type r_4 from Rectangle within u_marketsummary
end type

on r_4.create
end on

on r_4.destroy
end on
