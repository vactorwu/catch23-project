   M S B A M L   `   `   `  ����h  dSybase.PowerBuilder.WPF.Controls, Version=12.0.0.0, Culture=neutral, PublicKeyToken=598c7456a83d557a}Xclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls Sybase.PowerBuilder.WPF.Controls  5   �    stocktrader   w_order_alert   6   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=598c7456a83d557aW SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    6Q   hpbwpfXclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls    6�   -   'Sybase.PowerBuilder.WPF.Controls.Window    	BackColor:   4Sybase.PowerBuilder.WPF.Controls.PBWPFColorConverter$  20790589 6  ���	#ff3d3d3d   Center .    ControlMenu .    MaxBox .    MinBox .    PBHeight$
 1446��6p     PBIcon$ AppIcon!��6�     PBTitle$ Order Alert��6�     PBWidth$
 3006��6�  	   	Resizable	 . 
   TitleBar
 . .��5      �� -   5        $��PBClientArea��5      .+�5   
   1   +Sybase.PowerBuilder.WPF.Controls.StaticText  -   5        $��st_label��5         	BackColor$ 20790589 6+      FaceName$ Trebuchet MS��6@      FocusRectangle .     FontCharSet$
 ansi=�6o      	FontPitch$ variable=�6�      PBFontFamily$ swiss=�6�      PBHeight$	 128��6�      PBWidth$
 1232��6�      Text$ Order Alert��6�      	TextColor$ 16777215 6�      TextSize$	 -18��6�      Weight$	 700��6     X$	 117��6     Y$ 51��6  6  4   .Sybase.PowerBuilder.WPF.Controls.PictureButton  5   
   -   5        $��pb_ok��5         	BackColor$ 27021878 6+   4   .Sybase.PowerBuilder.WPF.Controls.CommandButton   FaceName$ Trebuchet MS��6@      FontCharSet$
 ansi=�6X      	FontPitch$ variable=�6k      OriginalSize .    PBFontFamily$ swiss=�6�      PBHeight$	 144��6�       PBWidth$	  355��6�   !   TabOrder$! 10��6�   "   Text$" OK��6�   #   	TextColor$# 30578299 6�   $   TextSize$	$ -12��6�   %   
VTextAlign$% vcenter=�6  &   Weight$	& 700��6  '   X$
' 1408��6$  (   Y$
( 1146��6-  68  1   +Sybase.PowerBuilder.WPF.Controls.DataWindow  5   
   -   5        $��	dw_result��5      )   
DataObject$) d_order_display��6,   *   PBHeight$	* 781��6I   $	��195��6X   +   PBWidth$
+ 2750��6e   $	��601��6t   ,   TabOrder$, 10��6�   -   X$	- 110��6�   .   Y$	. 291��6�   /   BorderStyle=   7Sybase.PowerBuilder.WPF.Controls.PBBorderStyleConverter$/ stylelowered 6�   0   
LiveScroll0 . 1   Title$
1 none��6�   6�   5      5      