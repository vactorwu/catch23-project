   M S B A M L   `   `   `  ����E  AWPF.Themes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:WPF.Themes;assembly=WPF.Themes
WPF.Themes  5       WPF.Themes.Demo   WPF.Themes.Demo.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      ?themes,clr-namespace:WPF.Themes;assembly=WPF.Themes     5      -   5           WindowBackgroundBrush#���   5        � SizeToContent$  Height=�5   	    � Title$ 
WPF.Themes��5	      $	��330��6   $	��732��6(    � WindowStyle$
 None=�64      WPF.Themes.ThemeManager   Theme 5      ��  �� ElementName$ themes�� �� Path$ SelectedItem�.��5
      �� .+�5   
   �� 
�������$��30��6$   
���   �$��0,0,0,1q�6D   ._�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�62   $��Center=�6M   .+�5      �� $��Theme: ��6   $��5,0,0,0q�6+   �� 5      -   5         ��Name$ themes��5      $	��150��6+   $��20��67    �� SelectedIndex$ 0��6n   5      5      5      � 5   
   	 � ColumnDefinitions	 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� $��10,10,10,10q�6   $��1��6.   .+�5      �� -   5        $ comboBox��5   8   $��22��6   $��0,0,0,5q�6'   
 �� DisplayMemberPath$

 Name��6J   $ 0��6c   �� 5      -   5         ��Name$ listBox��5   '   $	��220��6   $

 Name��68   � 5      -   5        $��progressBar1��5   *   $��10��6   $	��100��6>   5      �� 5       $��10,10,10,10q�6   $��0��6.   .+�5!      �� $��Button��6   $��	0,0,0,2.5q�6*   $��22��6=   d� 5"      $��ToggleButton��6   $��0,0,0,5q�66   $��22��6G   �� 5#      $��CheckBox��6    d� 	IsChecked$
 TrueY�6.   �� 5$      $��CheckBox - Three State��6    d� IsThreeState .  6P   Z� �� 5%      $��CheckBox - Disabled��6   {�.  $��0,0,0,5q�6K   	� 5'      $��RadioButton��6   $
 TrueY�64   	� 5(      $��RadioButton��6   	� 5)      $��RadioButton - Disabled��6   {�.  $��0,0,0,5q�6Q   �� 5+      -   5         ��Name$ slider��5+       � Minimum$ 0Q�6)    � Maximum$	 100Q�65    � Value$ 50Q�6C   $��	0,0,0,2.5q�6N   � 5,      -   5        $��testabc��5,   T    � IsIndeterminate . $��22��65   $��	0,0,0,2.5q�6A   6T   � 5-      -   5        $��test1��5-   �   $ 0Q�6   $	 100Q�6*   $��22��6i   $��0,0,0,5q�6u    68   �� $ slider��$ Value�6�   �� 5/      $��TextBox��6   50      �� 52      $��2��6   .@�53      �� $��Tab #1��6   .��54      � .+�56      9�  9� 
IsExpanded . $��Expander��65   $��8,8,8,8q�6G   .��57      R� $��Stretch=�6'   $��Stretch=�6E   $
��Auto��6a   $	��215��6n   .@�58   "   Q� $��TreeViewItem #1��6/   Q� 59   "   $��TreeViewItem #2��6/    Q� 
IsExpanded . .@�5:   &   Q� $��TreeViewItem #3��63   Q� 5;   &   $��TreeViewItem #4��63   5<   #   Q� 5=   "   $��TreeViewItem #5��6/   Q� 5>   "   $��TreeViewItem #6��6/   .@�5?   &   Q� $��TreeViewItem #7��63   .@�5@   *   Q� $��TreeViewItem #8��67   Q� 5A   *   $��TreeViewItem #9��67   5B   '   5C   #   5D      5E      5F      5G      �� 5H      $��Tab #2��6   .��5I      �� $
��Left=�6!   $	��Top=�6<   .+�5J      �� $��Item #1��6$   �� 5K      $��Item #2��6$   �� 5L      $��Item #3��6$   �� 5M      $��Item #4��6$   5N      5O      5Q      5R      5S      5T      