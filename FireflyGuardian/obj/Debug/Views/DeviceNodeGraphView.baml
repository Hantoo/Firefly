   M S B A M L   `   `   `  ����J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  5      /    )FireflyGuardian.Views.DeviceNodeGraphView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:FireflyGuardian.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      � -   5          �Name$  	container��5      }�.  � ColumnDefinitions 5   
   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��30 �6   .+�5   
   �� -   5         ��Name$ LayoutRootCanvas��5      $��0q�5      }�.  |�. z�5      V� 5      .~�5      �� -   5        st��5   %   T� 5       -   5        	translate��5    )   5!      5$      �� 5%   
   $��1��5&      ���#292929 �� Orientation$ 
Horizontal=�5(      .+�5)      �� -   5         ��Name$ 
moveButton��5*      $��10,0,0,0q�5+      $��	10,0,10,0q�5,      ���#303030$��0q�5.      $	����5/      $��Segoe MDL2 Assets)�50       � ToolTip$ Move Canvas Toggle��52       ^� BetweenShowDelay$	 500��53       ^� InitialShowDelay$
 2000��54      	 ^� ShowDuration$
	 4000��55      ��56      �� 57      .�58      �� 
 �� Property
 � �� �� Value �#6464646>    �� Triggers 5:      �� 5;       �� Binding 6*   �� *canvasDraga+ �� Value True6J   .a�5<   "   �� 
 � �� �#FFB3006F   5=      �� 5>       6*   �� *canvasDraga+	 False6J   .a�5?   "   �� 
 � �� �#6464646F   5@      5B      5D      �� 5F      -   5        $ 
RefreshMap��5G      $��10,0,0,0q�5H      $��	10,0,10,0q�5I      ���#303030$��0q�5K      $	����5L      $��Segoe MDL2 Assets)�5M      $ Refresh Nodes��5O      $	 500��5P      $
 2000��5Q      $
	 4000��5R      ��5S      �� 5T      .�5U      �� 
 � �� �#6464646>   5_      5a      �� 5b      $��	20,0,20,0q�5c      
�������	 �� X1$ 0��5e      	 �� X2$ 0��5f      	 �� Y1$ 5��5g      	 �� Y2$ 25��5h      �� 5i      -   5        $ 	FitToPage��5j      $��0,0,0,0q�5k      ���#3E3E3E$	����5m      $��Segoe MDL2 Assets)�5n      ���#ffffff$ Fit To Page��5q      $	 500��5r      $
 2000��5s      $
	 4000��5t      �� 5u      -   5        $��10,0,0,0q�5v      ���#3E3E3E$	����5x      $��Segoe MDL2 Assets)�5y      ���#ffffff$ Zoom In��5|      $	 500��5}      $
 2000��5~      $
	 4000��5      �� 5�      -	   5        $��10,0,0,0q�5�      ���#3E3E3E$	����5�      $��Segoe MDL2 Assets)�5�      ���#ffffff$ Zoom Out��5�      $	 500��5�      $
 2000��5�      $
	 4000��5�      5�      5�      5�      