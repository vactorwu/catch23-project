   M S B A M L   `   `   `  ����h  dSybase.PowerBuilder.WPF.Controls, Version=12.0.0.0, Culture=neutral, PublicKeyToken=598c7456a83d557a}Xclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls Sybase.PowerBuilder.WPF.Controls  5   �    stocktrader   u_marketsummary   6   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=598c7456a83d557aW SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    6V   hpbwpfXclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls    6�   1   +Sybase.PowerBuilder.WPF.Controls.UserObject    	BackColor:   4Sybase.PowerBuilder.WPF.Controls.PBWPFColorConverter$  	134217732 6     PBHeight$
 1926��6&  $	��481��66     PBWidth$
 2359��6C  $	��515��6R     PictureMaskColor$ 	536870912 6^     TabTextColor$ 33554432 6{     Text$
 none��6�  
 J� Uid$ 70��6�  .��5      �� .+�5      0   *Sybase.PowerBuilder.WPF.Controls.Rectangle  -   5        $	��r_1��5         	FillColor$ 
1073741824 6    1   +Sybase.PowerBuilder.WPF.Controls.DrawObject   	LineColor$ 33554432 67   	   	LineStyle$	 transparent=�6L   
   LineThickness$
 3��6d      PBHeight$	 221��6v      PBWidth$
 2352��6�      Y$	 211��6�   5        5      -   5        $	��r_2��5      $ 
1073741824 6    $ 33554432 67   $	 transparent=�6L   $
 3��6d   $	 672��6v   $
 2355��6�   $	 618��6�   5        5      -   5        $	��r_3��5      $ 30578299 6    $ 33554432 65   $
 3��6J   $	 618��6\   $
 1690��6k      X$	 651��6z   $	 653��6�   5        5	      -   5        $	��r_4��5	      $ 30578299 6    $ 33554432 65   $
 3��6J   $	 614��6\   $
 1690��6k   $	 655��6z   $
 1296��6�   5
      1   +Sybase.PowerBuilder.WPF.Controls.StaticText  5      -   5        $
��st_1��5         	Alignment$ right=�6"      	BackColor$ 67108864 64      FaceName$ Trebuchet MS��6I      FocusRectangle .     FontCharSet$
 ansi=�6x      	FontPitch$ variable=�6�      PBFontFamily$ swiss=�6�      PBHeight$ 74��6�      PBWidth$	 497��6�      Text$ Market Summary��6�      	TextColor$ 33554432 6�      TextSize$	 -11��6�      Weight$	 400��6     X$ 40��6     Y$ 61��6  5        5      -   5        $
��st_2��5      $ right=�6"   $ 16777215 64   $ Trebuchet MS��6I    .  $
 ansi=�6x   $ variable=�6�   $ swiss=�6�   $	 173��6�   $	 347��6�   $ Trade Stock Index (TSIA)��6�   $ 33554432 6�   $	 -10��6  $	 400��6  $	 230��6#  $	 224��6+  5        5      -   5        $
��st_3��5      $ right=�6"   $ 67108864 64   $ Trebuchet MS��6I    .  $
 ansi=�6x   $ variable=�6�   $ swiss=�6�   $ 74��6�   $	 497��6�   $ Trading Volume��6�   $ 33554432 6�   $	 -10��6�   $	 400��6  $ 80��6  $	 477��6  5        5      -   5        $
��st_4��5      $ right=�6"   $ 16777215 64   $ Trebuchet MS��6I    .  $
 ansi=�6x   $ variable=�6�   $ swiss=�6�   $ 74��6�   $	 497��6�   $ Top Gainers��6�   $ 33554432 6�   $	 -10��6�   $	 400��6  $ 80��6  $	 630��6  5        5      -	   5        $
��st_5��5      $ right=�6"   $ 67108864 64   $ Trebuchet MS��6I    .  $
 ansi=�6x   $ variable=�6�   $ swiss=�6�   $ 74��6�   $	 497��6�   $ 
Top Losers��6�   $ 33554432 6�   $	 -10��6�   $	 400��6  $ 80��6  $
 1312��6  5        5      -
   5        $��st_tsia��5      $ right=�6%   $ 16777215 67   $ Trebuchet MS��6L    .  $
 ansi=�6{   $ variable=�6�   $ swiss=�6�   $ 74��6�   $	 373��6�   $ 33554432 6�   $ -9��6�   $	 400��6�   $	 695��6  $	 275��6  5        5      -   5        $��	st_volume��5      $ right=�6'   $ 67108864 69   $ Trebuchet MS��6N    .  $
 ansi=�6}   $ variable=�6�   $ swiss=�6�   $ 74��6�   $	 373��6�   $ 33554432 6�   $ -9��6�   $	 400��6�   $	 684��6  $	 483��6  5      1   +Sybase.PowerBuilder.WPF.Controls.DataWindow  5      -   5        $��
dw_gainers��5         Border .     BorderStyle=   7Sybase.PowerBuilder.WPF.Controls.PBBorderStyleConverter$ stylelowered 67       
DataObject$  d_tops��6R   !   PBHeight$	! 586��6f   "   PBVScrollBar" . #   PBWidth$
# 1646��6�   $   TabOrder$$ 10��6�   %   Title$
% none��6�   &   X$	& 673��6�   '   Y$	' 666��6�   5        5      -   5        $��	dw_losers��5       .  $ stylelowered 66   $  d_tops��6Q   (   
LiveScroll( . $	! 586��6w   " . $
# 1646��6�   $$ 20��6�   $
% none��6�   $	& 673��6�   $
' 1299��6�   5      5      5      