   M S B A M L   `   `   `  ����h  dSybase.PowerBuilder.WPF.Controls, Version=12.0.0.0, Culture=neutral, PublicKeyToken=598c7456a83d557a}Xclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls Sybase.PowerBuilder.WPF.Controls  5   �    stocktrader   u_quotes   6   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=598c7456a83d557aW SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    6U   hpbwpfXclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls    6�   
  J� Uid$	  131��6  1   +Sybase.PowerBuilder.WPF.Controls.UserObject   	BackColor:   4Sybase.PowerBuilder.WPF.Controls.PBWPFColorConverter$ 20790589 6     Border .    BorderStyle$ styleshadowbox=�63     PBHeight$
 2314��6P  $	��578��6`     PBWidth$
 4560��6m  $	��997��6|     PictureMaskColor$ 	536870912 6�     TabTextColor$ 33554432 6�     Text$
 none��6�  .��5      �� .+�5   
   0   *Sybase.PowerBuilder.WPF.Controls.Rectangle  -   5        $	��r_1��5      	   	FillColor$	 30578299 6%   1   +Sybase.PowerBuilder.WPF.Controls.DrawObject
   	LineColor$
 33554432 6:      LineThickness$ 3��6O      PBHeight$
 1926��6a      PBWidth$
 4513��6q      X$ 7��6�      Y$	 368��6�   6�   1   +Sybase.PowerBuilder.WPF.Controls.StaticText  5   
   -   5        $��st_label��5         Text$ Quotes��6+      	BackColor$ 20790589 69      FaceName$ Trebuchet MS��6N      FocusRectangle .     FontCharSet$
 ansi=�6}      	FontPitch$ variable=�6�      PBFontFamily$ swiss=�6�      PBHeight$	 128��6�      PBWidth$
 1232��6�      	TextColor$ 16777215 6�      TextSize$	 -18��6�      Weight$	 700��6�      X$	 117��6	     Y$ 51��6  6    5   
   -   5        $��st_asfo��5      $ 20790589 6*   $ Trebuchet MS��6?    .  $
 ansi=�6n   $ variable=�6�   $ swiss=�6�   $ 80��6�   $	 216��6�   $ as of:��6�   $ 16777215 6�   $	 -10��6�   $	 700��6�   $	 117��6  $	 179��6  6    5   
   -   5        $��st_timestamp��5      $ 20790589 6/   $ Trebuchet MS��6D    .  $
 ansi=�6s   $ variable=�6�   $ swiss=�6�   $ 80��6�   $	 903��6�   $ 16777215 6�   $	 -10��6�   $	 700��6�   $	 311��6�   $	 179��6  6    5   
   -   5        $��
st_message��5      $	 134��6-   $
 2110��6<   $
 1236��6K   $	 227��6T   $��34��6\   
 �� Top$ 57��6h    �� Left$	 271��6x   $	��462��6�       	Alignment$  center=�6�   $ 16777215 6�   $ Trebuchet MS��6�    .  $
 ansi=�6�   $ variable=�6   $ swiss=�6  $ 15780518 6*  $	 -10��6?  !   Visible! .  $	 400��6^  6m    5   
   -   5        $��uo_quotebar��5      "   
BringToTop" . #   TabOrder$# 30��6@   $   X$
$ 3240��6N   %   Y$% 51��6W   $ 20790589 6^   $	 186��6s   $��46��6�   $
 1276��6�   $	��279��6�   $ 	536870912 6�   $ 33554432 6�   $
 none��6�   .��5	      �� .+�5
      4   .Sybase.PowerBuilder.WPF.Controls.PictureButton  -   5        $��uo_quotebar_pb_getquote��5
   &   &   	BackColor$& 23213090 6E   4   .Sybase.PowerBuilder.WPF.Controls.CommandButton'   Default' . (   FaceName$( Trebuchet MS��6i   )   FontCharSet$
) ansi=�6�   *   	FontPitch$* variable=�6�   +   Map3DColors+ . ,   PBFontFamily$, swiss=�6�   -   PBHeight$	- 134��6�   .   PBWidth$	. 380��6�   /   PowerTipText$/ 	Get Quote��6�   0   TabOrder$0 10��6  1   Text$1 	Get Quote��6  2   	TextColor$2 30578299 6&  3   TextSize$	3 -10��6;  4   
VTextAlign$4 vcenter=�6J  5   Weight$	5 400��6_  6   X$	6 856��6l  7   Y$7 19��6t  5   /Sybase.PowerBuilder.WPF.Controls.SingleLineEdit  5      -   5        $��uo_quotebar_sle_symbol��5   '   8   	BackColor$8 15793151 6E   9   BorderStyle=	   7Sybase.PowerBuilder.WPF.Controls.PBBorderStyleConverter$9 stylelowered	 6Z   :   FaceName$: Trebuchet MS��6u   ;   FontCharSet$
; ansi=�6�   <   	FontPitch$< variable=�6�   =   PBFontFamily$= swiss=�6�   >   PBHeight$	> 128��6�   ?   PBWidth$	? 750��6�   @   TabOrder$@ 20��6�   A   TextSize$	A -10��6�   B   Weight$	B 400��6  C   X$C 40��6  D   Y$D 22��6  5      5      1
   +Sybase.PowerBuilder.WPF.Controls.DataWindow
  5   
   -	   5        $
��dw_1��5      E 
  
BringToTopE . F 
  
DataObject$F d_quotes��69   G 
  PBHeight$
G 1869��6O   $	��467��6_   H 
  PBWidth$
H 4465��6l   $	��976��6{   I 
  TabOrder$I 50��6�   J 
  X$J 26��6�   K 
  Y$	K 390��6�   L 
  BorderStyle$L stylelowered	 6�   M 
  
LiveScrollM . N 
  Title$
N none��6�   6�   5      5      