   M S B A M L   `   `   `  ����J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  5      %    FireflyGuardian.Views.ShellView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:FireflyGuardian.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      D @ModernWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.2.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   5        � Title$  FireFly��5	      $
��1920��5
      $
��1740��5      $	��800��5      0  *ModernWpf.Controls.Primitives.WindowHelper   UseModernWindowStyle .  � 	Resources 5         MenuStyling �� 
TargetType �� Property �� Value �� 	Resources  Key �� TargetProperty	 W� To	 g� Duration
 �� Triggers �� Binding �� Mode �� Value N� EnterActions �� ResourceKey N� ExitActions  mouseOverColor �� Color  mouseOverStyle P� Property P� Value��%  &	        0�� 5$   "   $ OpenMenu��61   10�� 5+   "   $ 	CloseMenu��61   1&	     &	 )    7  �� 5   
   " ��6$   .�5      ��  � ��$ 50��6&    5      ��W� 5      OpenMenu $ Width�5      $	 250X�5      $	 0:0:0.2D�5      W� 5      	CloseMenu $ Width�5      $ 40X�5      $	 0:0:0.2D�5      5      
 5      �� 5        6   �� *
openMenu+$ OneWay=� True6H    5!      �� 5"      .��5#      �� .��5$   "   2  5%      5&       5(      �� 5)      .��5*      �� .��5+   "   2 5,      5-      5/      5I      �� 5K   
   $
 Gold��61   �� 5L   
   " ��6'   
 5M      P� 5N       �@J�IsMouseOver . . �5O      ��  � ��8   63   5P      5R      5V      .��5X      � ���#373737 � ColumnDefinitions 5Y   
   �� 5Z      $
��auto �6   �� 5[      $��* �6    � RowDefinitions 5]   
   �� 5^      $��* �6   .+�5a   
   �� -   5         ��Name$ 	MenuPanel��5b      $��0��5c      $��0��5d      ���#1F1F1F �� Orientation$ Vertical=�5g      #��[  5h      ��5e      �� *	menuWidth+$ OneWay=�.+�5i      �� $��0,0,0,50q�5j      $��Center=�5k      $ 
Horizontal=�5l      .+�5n      �� -   5         ��Name$ 
MenuBurger��5o      $��5,5,5,5q�5p      $	����5s      $��Segoe MDL2 Assets)�5t      $��16(�5u      ���	#ffffffff��5q      Z� ��5r      Z� 5v      �� 5w      $��Center=�5x      $��Segoe UI)�5y      $��14(�5z      ���	#ffffffff$�� ��5|      5}      �� 5�      -   5         ��Name$ Pages��5�      $��0,0,0,0q�5�      ���	#00373700 �� SelectedIndex 5�      ��  �� Path$ SelectedPage�$ OneWayToSource=���5�      �� 5�      .R�5�      �� $��40��5�      $��Center=�5�      $ 
Horizontal=�5�      .+�5�      ��  ��Name$
 Icon��5�   !   $��
0,10,10,10q�5�   !   $��Center=�5�   !   $��Segoe MDL2 Assets)�5�   !   $��16(�5�   !   ���	#ffffffff��5�   !   �� *icon+�� 5�      $��Center=�5�   !   $��Segoe UI)�5�   !   $��14(�5�   !   ���	#ffffffff��5�   !   �� *name+5�      5�      5�      5�      �� 5�   
   -   5         ��Name$ 
ActiveItem��5�      $��0��5�      $��1��5�      5�      5�      