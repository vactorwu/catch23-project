   M S B A M L   `   `   `  ����h  dSybase.PowerBuilder.WPF.Controls, Version=12.0.0.0, Culture=neutral, PublicKeyToken=598c7456a83d557a}Xclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls Sybase.PowerBuilder.WPF.Controls  5   �    stocktrader   u_orders   6   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=598c7456a83d557aW SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    6U   hpbwpfXclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls    6�   
  J� Uid$  85��6  1   +Sybase.PowerBuilder.WPF.Controls.UserObject   	BackColor:   4Sybase.PowerBuilder.WPF.Controls.PBWPFColorConverter$ 20790589 6     Border .    BorderStyle$ styleshadowbox=�62     PBHeight$
 2314��6O  $	��578��6_     PBWidth$
 4560��6l  $	��997��6{     PictureMaskColor$ 	536870912 6�     TabTextColor$ 33554432 6�     Text$
 none��6�  .��5      �� .+�5   
   0   *Sybase.PowerBuilder.WPF.Controls.Rectangle  -   5        $	��r_1��5      	   	FillColor$	 30578299 6%   1   +Sybase.PowerBuilder.WPF.Controls.DrawObject
   	LineColor$
 33554432 6:      LineThickness$ 3��6O      PBHeight$
 1926��6a      PBWidth$
 4513��6q      X$ 26��6�      Y$	 323��6�   6�   1   +Sybase.PowerBuilder.WPF.Controls.StaticText  5   
   -   5        $��st_label��5         Text$ Orders��6+      	BackColor$ 20790589 69      FaceName$ Trebuchet MS��6N      FocusRectangle .     FontCharSet$
 ansi=�6}      	FontPitch$ variable=�6�      PBFontFamily$ swiss=�6�      PBHeight$	 128��6�      PBWidth$
 1232��6�      	TextColor$ 16777215 6�      TextSize$	 -18��6�      Weight$	 700��6�      X$	 117��6	     Y$ 51��6  6    5   
   -   5        $��st_asfo��5      $ 20790589 6*   $ Trebuchet MS��6?    .  $
 ansi=�6n   $ variable=�6�   $ swiss=�6�   $ 80��6�   $	 216��6�   $ as of:��6�   $ 16777215 6�   $	 -10��6�   $	 700��6�   $	 117��6  $	 179��6  6    5   
   -   5        $��st_timestamp��5      $ 20790589 6/   $ Trebuchet MS��6D    .  $
 ansi=�6s   $ variable=�6�   $ swiss=�6�   $ 80��6�   $	 903��6�   $ 16777215 6�   $	 -10��6�   $	 700��6�   $	 311��6�   $	 179��6  6    5   
   -   5        $��
st_message��5         	Alignment$ center=�6-   $ 16777215 6@   $ Trebuchet MS��6U    .  $
 ansi=�6�   $ variable=�6�   $ swiss=�6�   $	 138��6�   $
 2790��6�   $ 15780518 6�   $	 -10��6�      Visible .  $	 400��6  $	 929��6   $	 352��6(  62    5   
   -   5        $
��uo_1��5          
BringToTop  . !   TabOrder$! 30��69   "   X$
" 3240��6G   #   Y$# 51��6P   $ 20790589 6W   $	 186��6l   $��46��6{   $
 1276��6�   $	��279��6�   $ 	536870912 6�   $ 33554432 6�   $
 none��6�   .��5	      �� .+�5
      4   .Sybase.PowerBuilder.WPF.Controls.PictureButton  -   5        $��uo_1_pb_getquote��5
   &   $   	BackColor$$ 23213090 6>   4   .Sybase.PowerBuilder.WPF.Controls.CommandButton%   Default% . &   FaceName$& Trebuchet MS��6b   '   FontCharSet$
' ansi=�6z   (   	FontPitch$( variable=�6�   )   Map3DColors) . *   PBFontFamily$* swiss=�6�   +   PBHeight$	+ 134��6�   ,   PBWidth$	, 380��6�   -   PowerTipText$- 	Get Quote��6�   .   TabOrder$. 10��6   /   Text$/ 	Get Quote��6  0   	TextColor$0 30578299 6  1   TextSize$	1 -10��64  2   
VTextAlign$2 vcenter=�6C  3   Weight$	3 400��6X  4   X$	4 856��6e  5   Y$5 19��6m  5   /Sybase.PowerBuilder.WPF.Controls.SingleLineEdit  5      -   5        $��uo_1_sle_symbol��5   '   6   	BackColor$6 15793151 6>   7   BorderStyle=	   7Sybase.PowerBuilder.WPF.Controls.PBBorderStyleConverter$7 stylelowered	 6S   8   FaceName$8 Trebuchet MS��6n   9   FontCharSet$
9 ansi=�6�   :   	FontPitch$: variable=�6�   ;   PBFontFamily$; swiss=�6�   <   PBHeight$	< 128��6�   =   PBWidth$	= 750��6�   >   TabOrder$> 20��6�   ?   TextSize$	? -10��6�   @   Weight$	@ 400��6�   A   X$A 40��6
  B   Y$B 22��6  5      5      1
   +Sybase.PowerBuilder.WPF.Controls.DataWindow
  5   
   -	   5        $
��dw_1��5      C 
  
BringToTopC . D 
  
DataObject$D d_orders��69   E 
  PBHeight$
E 1869��6O   $	��467��6_   F 
  PBWidth$
F 4465��6l   $	��976��6{   G 
  TabOrder$G 60��6�   H 
  X$H 44��6�   I 
  Y$	I 346��6�   J 
  BorderStyle$J stylelowered	 6�   K 
  
LiveScrollK . L 
  Title$
L none��6�   6�   5      5      