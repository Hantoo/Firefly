   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  %    FireflyGuardian.Views.ShellView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FireflyGuardian.Views     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     D @ModernWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.2.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019     � Title$  FireFly��$
��1920��$
��1740��$	��800��0  *ModernWpf.Controls.Primitives.WindowHelper   UseModernWindowStyle .  � 	Resources    MenuStyling �� 
TargetType �� Property �� Value �� 	Resources  Key �� TargetProperty	 W� To	 g� Duration
 �� Triggers �� Binding �� Mode �� Value N� EnterActions �� ResourceKey N� ExitActions  mouseOverColor �� Color  mouseOverStyle P� Property P� Value��%�  &	        0�� $ OpenMenu��10�� $ 	CloseMenu��1&	 �     &	     7  �� " ��.���  � ��$ 50�� ��W� OpenMenu $ Width�$	 250X�$	 0:0:0.2D�W� 	CloseMenu $ Width�$ 40X�$	 0:0:0.2D�
 ��  �� *
openMenu+$ OneWay=� True �� .���� .��2   �� .���� .��2 �� $
 Gold���� " ��
 P�  �@J�IsMouseOver . . ���  � ��8   .��� ���#373737 � ColumnDefinitions �� $
��auto ��� $��* � � RowDefinitions �� $��* �.+��� -    ��Name$ 	MenuPanel��$��0��$��0�����#1F1F1F �� Orientation$ Vertical=�#��[  ���� *	menuWidth+$ OneWay=�.+��� $��0,0,0,50q�$��Center=�$ 
Horizontal=�.+��� -    ��Name$ 
MenuBurger��$��5,5,5,5q�$	����$��Segoe MDL2 Assets)�$��16(����	#ffffffff��Z� ��Z� �� $��Center=�$��Segoe UI)�$��14(����	#ffffffff$�� ���� -    ��Name$ Pages��$��0,0,0,0q����	#00373700 �� SelectedIndex ��  �� Path$ SelectedPage�$ OneWayToSource=����� .R��� $��40��$��Center=�$ 
Horizontal=�.+���  ��Name$
 Icon��$��
0,10,10,10q�$��Center=�$��Segoe MDL2 Assets)�$��16(����	#ffffffff���� *icon+�� $��Center=�$��Segoe UI)�$��14(����	#ffffffff���� *name+�� -    ��Name$ 
ActiveItem��$��0��$��1��