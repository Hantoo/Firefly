   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  G(clr-namespace:FireflyGuardian.ViewModelsFireflyGuardian.ViewModels  -    'FireflyGuardian.Views.DeviceNetworkView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^ ZCaliburn.Micro.Platform, Version=3.2.0.0, Culture=neutral, PublicKeyToken=8e5891231f2ed21f(calhttp://www.caliburnproject.org  :d2http://schemas.microsoft.com/expression/blend/2008  /local#clr-namespace:FireflyGuardian.Views  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  1my(clr-namespace:FireflyGuardian.ViewModels    � 	Resources     
HatchBrush � MappingMode � SpreadMethod �� 
StartPoint �� EndPoint � Offset � Color  statusHeading �� 	Resources �� Triggers	 �� Binding
 �� Value N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions �� DataType/   )FireflyGuardian.Views.DeviceNodeGraphView�� %�  &	        &	 �     (�� b    *, +)�� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #BE9F3B��� $
 0.55Q�$ #BE9F3B��� $
 0.55Q�$ #8A7B66��� $ 1Q�$ #8A7B66����  �� �� 	 �� *statusShown+
 True �� .���� .��W� $ 	MaxHeight�$ 60X�$ 0:0:0.5D� �� .���� .��W� $ 	MaxHeight�$ 0X�$ 0:0:0.5D��� "  .R�  .���  � RowDefinitions �� $��40 ��� $
��auto ��� $��* �.+�� ���#292929 � ColumnDefinitions �� $
��auto ��� $��* ��� $
��auto �.+���  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ RefreshDevices��$��5,0,0,0q����#3E3E3E$��Poll Connected Devices�����#ffffff�� -   $ AddNewDevice��$��5,0,0,0q����#3E3E3E$��Add New Device�����#ffffff�� -   $ recalculateRoutes��$��45,0,0,0q����#3E3E3E$��Recalculate Routes�����#ffffff�� -   $ Evacuate��$��5,0,0,0q�$��Evacuate�����#ffffff���� *evacButtonColour+ �� Mode$ OneWay=��� $��2��$��Right=�$ 
Horizontal=�.+��� $	��130��$ 
Horizontal=�.+��� $��Center=�$��Next Heartbeat:��
��������� $��Center=����	#FFFFBA4F���� *heartUpdateTimeRef+$ OneWay=��� $	��160��$ 
Horizontal=�.+��� $��Center=�$��Next Global Update:��
��������� $��Center=����	#FFFFBA4F���� *globalUpdateTimeRef+$ OneWay=��� $��1��$��0��#��[  #��[ .+��� $��Center=�.+��� $��20��$��Center=�$
��Bold"����#000 �� TextAlignment$ Center=�����  �� ElementName$ statusStackPanel�� �� Path$ ActualWidth����� *
statusText+$ OneWay=�� $��2�� �� $��* ��� $��5 ��� $��* � .+��  �� $
��0.4* ��� $
��0.6* � �� $��30 ��� $��* �.+��� $��0��$��0��$
��20,0q�$��Center=�$��Segoe UI)�$��18(�$
��Bold"����#ffffff$��Active Devices���� $��1��
������  �� SelectionMode$ Single=����� *
deviceList+$ OneWay=� �� SelectedIndex �� *selectedDeviceIndex+$ TwoWay=�����  �� 
TargetType" ��.���  �� Property � �� �� Value �#252525���� .R��� $ 
Horizontal=�.+��� $��
10,10,0,10q�$��Center=�$��Center=�$��Segoe MDL2 Assets)�$��16(����	#FFCB4525���� " ��.���  � ��$	 �� �� 	 �� *hasHeartBeat+$ OneWay=�
 True.a���  � ��$	 ���� $��20,10,20,10q�$��Center=�$��Center=�$��Segoe MDL2 Assets)�$��16(����	#FF4AE720$	��������  �� 	 �� *isExit+
 True.a���  � y�$ Visible=��� 	 �� *isExit+	
 False.a���  � y�$ Hidden=��� $��
0,20,20,20q�$��Center=�$��Segoe UI)�$��14(����#ffffff���� *
deviceName+�� $��1��$��1��$
��20,0q����� " ��.���  � y�$ 	Collapsed=� �� 0   *FireflyGuardian.ViewModels.infoWindowShown   deviceInfomation#
 Z 	 �� *
infoWindow+.a���  � y�$ Visible=�.+��� $ 
Horizontal=�.+��� ���	#3C000000$ Vertical=�.+���  �� Padding$
 10,5q�$��16(�$��SemiBold"����#ffffff$��Name:���� -     ��Name$  SelectedDevice_deviceName��$	��150��$ 10q����	#3C000000$��Light"����#ffffff�� $ Vertical=�.+��� $
 10,5q�$��16(�$��SemiBold"����#ffffff$��IP Address:���� -   $  SelectedDevice_deviceIP��$	��150��$ 10q����	#3C000000$��Light"����#ffffff�� ���	#3C000000$ Vertical=�.+��� $
 10,5q�$��16(�$��SemiBold"����#ffffff$��
Device ID:���� -   $  SelectedDevice_deviceID��$��1��$��3��$	��100��$ 10q����	#3C000000$��Light"����#ffffff�� $ Vertical=�.+��� $
 10,5q�$��16(�$��SemiBold"����#ffffff$��Is Exit���� -   $  SelectedDevice_isExit��$��1��$��3��$��80��$ 10q����	#3C000000$��Light"����#ffffff�� $ 
Horizontal=�.+��� -	   $ markNodeAsExit��$��20,10q�$��Toggle Exit��$��Light"����#ffffff�� -
   $ DeactivateNode��$��Toggle Node Activation��$��Light"����#ffffff�� $ 
Horizontal=�.+��� .+��� -   $ saveSelectedDevice��$	��250��$��
20,0,20,10q�$��Center=�$��Update Device Infomation�����#fff�� -   $ editSelectedDevice��$	��250��$��
20,0,20,10q�$��Center=�$��Edit Device Infomation�����#fff�� $ 
Horizontal=�.+��� $��0,0,10,0q�.+��� $��16(�$��SemiBold"����#ffffff$��Connections To:���� $ 
Horizontal=�.+��� ���� $ 
deviceList� �� *selectedComboBoxItem+$ TwoWay=����� .R��� $ 
Horizontal=�.+��� ���� *
deviceName+�� -   $ AddToAdjacencyList��$��+�����#fff�� -   ! ��Name$! SelectedDeviceConnections������ .R��� $ 
Horizontal=�.+��� ���#F4F4F4���� *
deviceName+�� $��30,0,0,0q�$��5q�  Caliburn.Micro.Message"   Attach$=" 7[Event Click] = [Action RemoveConnection($datacontext)]�����#622323$	����$��Segoe MDL2 Assets)����#fff# � DataContext# �� *
deviceID+
$ � Tag$ �� *
deviceID+�� $
��10,0q�.+��� $ Vertical=�.+��� $ 	10,0,10,0q�$��16(�$��SemiBold"����#ffffff$��Active Image Slot:���� -   $$  SelectedDevice_activeImageSlot��$��1��$��3��$��Light"����#ffffff�� $ Vertical=�.+��� $ 	10,0,10,0q�$��16(�$��SemiBold"����#ffffff$��Default Image Slot:���� -    �� *defaultImageSlot+$ TwoWay=����� .R��� $ 
Horizontal=�.+��� $��30��$��0,0,10,0q�$��Center=�$��Center=����� *slotID+�� ���� *image+�� �� $
��10,0q�.+��� .+��� $��4��$��1��$ 0,0,50,0q�$��16(�$��SemiBold"����#ffffff$��Device Location XY:���� $��5��$��1��$ 
Horizontal=�.+��� -   % ��Name$% deviceLocationX���� -   $% deviceLocationZ���� $��1��$��1��$
��20,0q����� " ��.���  � y�$ 	Collapsed=� �� &   
editDevice#
 Z& 	 �� *
infoWindow+.a���  � y�$ Visible=�.+��� $
��Bold"����#fff$��Edit Device���� $��Medium"����#fff$��Device Name:���� -   $% editDeviceName���� $ 0,30,0,0q�$��Medium"����#fff$��Device Orientation:���� -   ' ��Name$' Orientation�� �� *Orientation+$ TwoWay=�.@��� .���� 	0 Degrees�� .���� -90 / 270 Degrees�� .���� 180 Degrees�� .���� 90 / -270 Degrees���� .R��� $ 
Horizontal=�.+��� ���� *
deviceName+�� -   $ RemoveDeviceFromSystem��$��	0,20,0,20q�$��10q����#622323$��Delete Device From System�����#ffffff�� $��20q�$ 
Horizontal=�.+��� -   $ UpdateEditedDevice��$��5,0,0,0q����#3E3E3E$��Update Device�����#ffffff�� -   $ CancelEditingDevice��$��5,0,0,0q����#622323$��Cancel�����#ffffff�� $��1��$��1��$
��20,0q����� " ��.���  � y�$ 	Collapsed=� �� (   	addDevice#
 Z( 	 �� *
infoWindow+.a���  � y�$ Visible=�.+��� $
��Bold"����#fff$��
Add Device���� $��Medium"����#fff$��
Device ID:���� -   $% newdeviceID���� $��20q�$ 
Horizontal=�.+��� -   $ AddNewDeviceToDeviceList��$��5,0,0,0q����#3E3E3E$��Add New Device�����#ffffff�� -   $ CancelAddingNewDevice��$��5,0,0,0q����#622323$��Cancel�����#ffffff�� $��1��$��5��$��Stretch=��� -   $��
NodeLayout��$��2��.��  