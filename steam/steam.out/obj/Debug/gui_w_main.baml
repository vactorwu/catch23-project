   M S B A M L   `   `   `  ����h  dSybase.PowerBuilder.WPF.Controls, Version=12.5.0.0, Culture=neutral, PublicKeyToken=598c7456a83d557a}Xclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls Sybase.PowerBuilder.WPF.Controls  5   �   	 steam  LYC.Steam.Gui.w_main   6   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    6I   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6�   jpbwpfXclr-namespace:Sybase.PowerBuilder.WPF.Controls;assembly=Sybase.PowerBuilder.WPF.Controls     6�   Ssyntaxeditor:http://schemas.actiprosoftware.com/winfx/xaml/syntaxeditor     6�  -   'Sybase.PowerBuilder.WPF.Controls.Window    PBTitle$  Steam��6)     PBHeight$
 2000��69     PBWidth$
 2500��6I     MinBox .    MaxBox .    TitleBar .    ControlMenu .    Center .    	Resizable . 
	 J� Uid$	 19��6�  $	��500��6�  $	��547��6�  .��5      � .+�5   
   *   $Sybase.PowerBuilder.WPF.Controls.Tab  -   5        $��tab_1��5   O   $	��437��6   $
��Left=�6!   $��	12,12,0,0q�6<   
   PBHeight$

 1748��6\      PBWidth$
 2290��6l      TabOrder$ 20��6{   $	��Top=�6�   $	��501��6�      X$ 55��6�      Y$ 48��6�   .@�5      .   (Sybase.PowerBuilder.WPF.Controls.TabPage  -   5        $��	tabpage_1��5   4      	BackColor:   4Sybase.PowerBuilder.WPF.Controls.PBWPFColorConverter$ Button Face 6      PictureMaskColor$ Transparent 6E      TabBackColor$ Button Face 6d      TabTextColor$ Window Text 6      Text$ 程序列表��6�   .��5      1   +Sybase.PowerBuilder.WPF.Controls.DataWindow  -   5        $��
dw_applist��5   p   $	��152��6#   $
��Left=�60      
LiveScroll . $��	12,39,0,0q�6]      TabOrder$ 10��6�   $	��Top=�6�   $	��393��6�      PBHeight$	 608��6�      PBWidth$
 1797��6�      X$ 55��6�      Y$	 156��6�      
DataObject$ 	d_applist��6�      PBHScrollBar .    PBVScrollBar . 6  5        5      -   5        $��	tabpage_2��5   +   $ Button Face 6   $ Transparent 6<   $ Button Face 6[   $ Window Text 6v   $ 程序详情��6�   5	      5
      4   .Sybase.PowerBuilder.WPF.Controls.CommandButton  5      -   5        $��	cb_search��5   S   $��23��6   $
��Left=�6$   $��
320,49,0,0q�6?      TabOrder$ 40��6d      Text$ 搜索��6r      TextSize$	 -10��6|   $	��Top=�6�   $��47��6�       PBHeight$  92��6�   !   PBWidth$	! 215��6�   "   X$
" 1463��6�   #   Y$	# 196��6�     5   
   -   5        $��cb_reset��5   Y   $��23��6   $
��Left=�6*   $��
373,49,0,0q�6E   $  92��6i   $	! 215��6w   $ 40��6�   $ 重置��6�   $	 -10��6�   $	��Top=�6�   $��47��6�   $
" 1705��6�   $	# 196��6�     5      -   5        $��cb_prior��5   S   $��28��6   $
��Left=�6$   $��
27,236,0,0q�6?   $ 50��6c   $ 	上一页��6q   $	 -10��6|   $	��Top=�6�   $��88��6�   $	  112��6�   $	! 402��6�   $	" 123��6�   $	# 944��6�     5      -   5        $��cb_next��5   U   $��28��6   $��Right=�6$   $��0,236,105,0q�6@   $ 60��6d   $ 	下一页��6r   $	 -10��6}   $	��Top=�6�   $��88��6�   $	  112��6�   $	! 402��6�   $
" 1518��6�   $	# 944��6�   �� 5      -	   5        $��
txt_input1��5   G   $��23��6   $
��Left=�6   $��
194,49,0,0q�63   $��30��6Y   $	��Top=�6g   $	��120��6   5      5      