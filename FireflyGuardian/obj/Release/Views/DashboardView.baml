   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  )    #FireflyGuardian.Views.DashboardView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     8
Syncfusion!http://schemas.syncfusion.com/wpf     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FireflyGuardian.Views     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     D @ModernWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.2.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   W SXamlRadialProgressBar.DotNet, Version=1.0.3.0, Culture=neutral, PublicKeyToken=null.xrpb#http://XamlRadialProgressBar/DotNet  .���   � ColumnDefinitions  �� $��20 ��� $��* ��� $��* ��� $��* ��� $��20 � � RowDefinitions �� $��70 ��� $
��auto ��� $	��420 �$	��420���� $��* ��� $��20 �.+��� $��0��$��1��$��3��$��Center=�$��Center=� �� Orientation$ 
Horizontal=�.+��� $��Center=�$��Center=�$
��16pt(�$
��Bold"����#fff���� *welcomeMessage+ �� Mode$ OneWay=�� $��1��$��1��$��3��$��25q����#282828.+��� $��30q�$��Center=�.+��� $��Center=�$ 
Horizontal=�.+�� $��Center=�.+��� $��Center=�$��Segoe MDL2 Assets)�$	��150(�$	�������� *
iconColour+$ OneWay=��� $��	0,-25,0,0q�$��Center=�$��Segoe MDL2 Assets)�$	��160(����� *
iconColour+$ OneWay=����� *icon+$ OneWay=��� $��50,0,0,0q�$��Center=�$ Vertical=�.+��� $��50(�$��ExtraExpanded&�$��Black"����� *
iconColour+$ OneWay=����� *
SystemText+$ OneWay=��� 
����������� *	subText+$ OneWay=��� $��2��$��2��$��1��$	��420��$	��375��$	��575��$��	20,0,20,0q�$��Stretch=�$	��Top=����#282828.+��� $��
50,25,50,0q�$��Center=�$��Center=�.+��� $��0,0,0,30q�$��Center=�$��Center=�$
��16pt(�$
��Bold"����#fff$��Next Heart Beat��� .+�-  'XamlRadialProgressBar.RadialProgressBar  $	��200��$	��200�����#FF980E � Maximum$ 10Q� � Minimum$ 0Q� � Value �� *heartUpdateTimeRef+$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��
0,-200,0,0q�$��Center=�$��Center=�$��76(�$
��Bold"����#fff���� *heartUpdateTimeRefReverse+$ OneWay=��� $��0,30,0,0q�$��Center=�$��Center=�$
��12pt(����#A2A2A2$����A Heartbeat is sent out to devices every 10 seconds to check that they are still connected to the network and functioning correctly. You can check which devices are working in the Device Management tab.�� �� TextAlignment$ Center=�$
��Wrap=��� $��2��$��2��$��2��$	��375��$	��420��$	��575��$	��420��$��	20,0,20,0q�$��Stretch=�$	��Top=����#282828.+��� $��
50,25,50,0q�$��Center=�$��Center=�.+��� $��0,0,0,30q�$��Center=�$��Center=�$
��16pt(�$
��Bold"����#fff$��Next Global Update��� .+�  $	��200��$	��200�����#FF980E$ 60Q�$ 0Q� �� *globalUpdateTimeRef+$ OneWay=�$ PropertyChanged=��� $��
0,-200,0,0q�$��Center=�$��Center=�$��76(�$
��Bold"����#fff���� *globalUpdateTimeRefReversed+$ OneWay=��� $��0,30,0,0q�$��Center=�$��Center=�$
��12pt(����#A2A2A2$����A Global Update is pushed to all devices every 60 seconds. This updates the devices with any important changes such as new default images. When an update occurs the software also runs for any routines that are scheduled.��$ Center=�$
��Wrap=��� $��2��$��2��$��3��$	��375��$	��420��$	��575��$	��420��$��	20,0,20,0q�$��Stretch=�$	��Top=����#282828.+��� $��
50,25,50,0q�$��Center=�$��Center=�.+��� $��0,0,0,30q�$��Center=�$��Center=�$
��16pt(�$
��Bold"����#fff$��Devices On Network���� $��0,50,0,0q�$��Center=�$��Center=�$��76(�$
��Bold"����#fff���� *devicesOnNetwork+$ OneWay=��� $��0,80,0,0q�$��Center=�$��Bottom=�$
��12pt(����#A2A2A2$ Center=�$
��Wrap=����� *devicesOnNetworkMessage+$ OneWay=�