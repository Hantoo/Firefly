   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  /    )FireflyGuardian.Views.DeviceNodeGraphView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FireflyGuardian.Views     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .��� -     �Name$  	container��}�.  � ColumnDefinitions �� $��* � � RowDefinitions �� $��* ��� $��30 �.+��� -    ��Name$ LayoutRootCanvas��$��0q�}�.  |�. z�V� .~��� -   st��T� -   	translate���� $��1�����#292929 �� Orientation$ 
Horizontal=�.+��� -    ��Name$ 
moveButton��$��10,0,0,0q�$��	10,0,10,0q����#303030$��0q�$	����$��Segoe MDL2 Assets)� � ToolTip$ Move Canvas Toggle�� ^� BetweenShowDelay$	 500�� ^� InitialShowDelay$
 2000��	 ^� ShowDuration$
	 4000������ .��� 
 �� Property
 � �� �� Value �#646464 �� Triggers ��  �� Binding �� *canvasDraga+ �� Value True.a��� 
 � �� �#FFB300��  �� *canvasDraga+	 False.a��� 
 � �� �#646464�� -   $ 
RefreshMap��$��10,0,0,0q�$��	10,0,10,0q����#303030$��0q�$	����$��Segoe MDL2 Assets)�$ Refresh Nodes��$	 500��$
 2000��$
	 4000������ .��� 
 � �� �#646464�� $��	20,0,20,0q�
�������	 �� X1$ 0��	 �� X2$ 0��	 �� Y1$ 5��	 �� Y2$ 25���� -   $ 	FitToPage��$��0,0,0,0q����#3E3E3E$	����$��Segoe MDL2 Assets)����#ffffff$ Fit To Page��$	 500��$
 2000��$
	 4000���� -   $��10,0,0,0q����#3E3E3E$	����$��Segoe MDL2 Assets)����#ffffff$ Zoom In��$	 500��$
 2000��$
	 4000���� -	   $��10,0,0,0q����#3E3E3E$	����$��Segoe MDL2 Assets)����#ffffff$ Zoom Out��$	 500��$
 2000��$
	 4000��