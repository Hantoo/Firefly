   M S B A M L   `   `   `  ����J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  5      )    #FireflyGuardian.Views.DashboardView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      8
Syncfusion!http://schemas.syncfusion.com/wpf     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:FireflyGuardian.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      D @ModernWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.2.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   5	      W SXamlRadialProgressBar.DotNet, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null.xrpb#http://XamlRadialProgressBar/DotNet  5
      .��5      �   � ColumnDefinitions  5   
   �� 5      $��20 �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��70 �6   �� 5      $
��auto �6   �� 5      $	��420 �6   $	��420��6)   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   �� $��0��5      $��1��5       $��3��5!      $��Center=�5"      $��Center=�5#       �� Orientation$ 
Horizontal=�5$      .+�5%      �� $��Center=�5&      $��Center=�5'      $
��16pt(�5(      $
��Bold"�5)      ���#fff��5+      �� *welcomeMessage+ �� Mode$ OneWay=�5,      � 5.   
   $��1��5/      $��1��50      $��3��51      $��25q�52      ���#282828.+�54      �� $��30q�6   $��Center=�6%   .+�55      �� $��Center=�6   $ 
Horizontal=�68   .+�56      � $��Center=�6   .+�57      �� $��Center=�58      $��Segoe MDL2 Assets)�59      $	��150(�5:      $	����5<      ��5;      �� *
iconColour+$ OneWay=�5<      �� 5=      $��	0,-25,0,0q�5>      $��Center=�5?      $��Segoe MDL2 Assets)�5@      $	��160(�5A      ��5B      �� *
iconColour+$ OneWay=���5C      �� *icon+$ OneWay=�5D      �� 5E      $��50,0,0,0q�5F      $��Center=�5G      $ Vertical=�5H      .+�5I      �� $��50(�5J      $��ExtraExpanded&�5K      $��Black"�5L      ��5M      �� *
iconColour+$ OneWay=���5N      �� *
SystemText+$ OneWay=��� 5O      
���������6;   �� *	subText+$ OneWay=�5P      5Q      5R      5S      �� 5U   
   $��2��5V      $��2��5W      $��1��5X      $	��420��5Y      $	��375��5Z      $	��575��5[      $��	20,0,20,0q�5\      $��Stretch=�5]      $	��Top=�5^      ���#282828.+�5`      �� $��
50,25,50,0q�5a      $��Center=�5b      $��Center=�5c      .+�5d      �� $��0,0,0,30q�5e      $��Center=�5f      $��Center=�5g      $
��16pt(�5h      $
��Bold"�5i      ���#fff$��Next Heart Beat��5k      � 5l      .+�5m      -  'XamlRadialProgressBar.RadialProgressBar  $	��200��5n      $	��200��5o      ���#FF980E � Maximum$ 10Q�5q       � Minimum$ 0Q�5r       � Value 5s      �� *heartUpdateTimeRef+$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�5t      �� 5u      $��
0,-200,0,0q�5v      $��Center=�5w      $��Center=�5x      $��76(�5y      $
��Bold"�5z      ���#fff��5|      �� *heartUpdateTimeRefReverse+$ OneWay=��� 5}      $��0,30,0,0q�5~      $��Center=�5      $��Center=�5�      $
��12pt(�5�      ���#A2A2A2$����A Heartbeat is sent out to devices every 10 seconds to check that they are still connected to the network and functioning correctly. You can check which devices are working in the Device Management tab.��5�       �� TextAlignment$ Center=�5�      $
��Wrap=�5�      5�      5�      �� 5�   
   $��2��5�      $��2��5�      $��2��5�      $	��375��5�      $	��420��5�      $	��575��5�      $	��420��5�      $��	20,0,20,0q�5�      $��Stretch=�5�      $	��Top=�5�      ���#282828.+�5�      �� $��
50,25,50,0q�5�      $��Center=�5�      $��Center=�5�      .+�5�      �� $��0,0,0,30q�5�      $��Center=�5�      $��Center=�5�      $
��16pt(�5�      $
��Bold"�5�      ���#fff$��Next Global Update��5�      � 5�      .+�5�        $	��200��5�      $	��200��5�      ���#FF980E$ 60Q�5�      $ 0Q�5�       5�      �� *globalUpdateTimeRef+$ OneWay=�$ PropertyChanged=�5�      �� 5�      $��
0,-200,0,0q�5�      $��Center=�5�      $��Center=�5�      $��76(�5�      $
��Bold"�5�      ���#fff��5�      �� *globalUpdateTimeRefReversed+$ OneWay=��� 5�      $��0,30,0,0q�5�      $��Center=�5�      $��Center=�5�      $
��12pt(�5�      ���#A2A2A2$����A Global Update is pushed to all devices every 60 seconds. This updates the devices with any important changes such as new default images. When an update occurs the software also runs for any routines that are scheduled.��5�      $ Center=�5�      $
��Wrap=�5�      5�      5�      �� 5�   
   $��2��5�      $��2��5�      $��3��5�      $	��375��5�      $	��420��5�      $	��575��5�      $	��420��5�      $��	20,0,20,0q�5�      $��Stretch=�5�      $	��Top=�5�      ���#282828.+�5�      �� $��
50,25,50,0q�5�      $��Center=�5�      $��Center=�5�      .+�5�      �� $��0,0,0,30q�5�      $��Center=�5�      $��Center=�5�      $
��16pt(�5�      $
��Bold"�5�      ���#fff$��Devices On Network��5�      �� 5�      $��0,50,0,0q�5�      $��Center=�5�      $��Center=�5�      $��76(�5�      $
��Bold"�5�      ���#fff��5�      �� *devicesOnNetwork+$ OneWay=��� 5�      $��0,80,0,0q�5�      $��Center=�5�      $��Bottom=�5�      $
��12pt(�5�      ���#A2A2A2$ Center=�5�      $
��Wrap=�5�      ��5�      �� *devicesOnNetworkMessage+$ OneWay=�5�      5�      5�      5�      5�      