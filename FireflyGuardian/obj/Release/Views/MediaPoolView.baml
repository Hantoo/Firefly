   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  G(clr-namespace:FireflyGuardian.ViewModelsFireflyGuardian.ViewModels  )    #FireflyGuardian.Views.MediaPoolView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FireflyGuardian.Views     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     7my(clr-namespace:FireflyGuardian.ViewModels       � 	Resources     WarningHatchBrush � MappingMode � SpreadMethod �� 
StartPoint �� EndPoint � Offset � Color  GoodHatchBrush  ErrorHatchBrush  statusHeading �� 
TargetType �� 	Resources	 �� Triggers,   &FireflyGuardian.ViewModels.statusState
   Warning �� Value �� Binding �� Property �� Value   Good   Error N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions  renameButtonStyle  renameStackPanelStyle�� %�  &	        &	 �     &	 T    &	 �    7   7  7  &	 �    &	 �    �� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #BE9F3B��� $
 0.55Q�$ #BE9F3B��� $
 0.55Q�$ #8A7B66��� $ 1Q�$ #8A7B66���� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #57BE3B��� $
 0.55Q�$ #46BE3B��� $
 0.55Q�$ #1E7227��� $ 1Q�$ #0F8015���� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #BE3B3B��� $
 0.55Q�$ #BE3B3B��� $
 0.55Q�$ #8A7B66��� $ 1Q�$ #8A7B66���� " �� ��	 �� # Z
  �� *statusStateIndicator+a���  � ��8   �� # Z  �� *statusStateIndicator+a���  � ��8  �� # Z  �� *statusStateIndicator+a���  � ��8  ��  �� *
showStatus+ True �� .���� .��W� $ 	MaxHeight�$ 60X�$ 0:0:0.5D� �� .���� .��W� $ 	MaxHeight�$ 0X�$ 0:0:0.5D��� " ��	 ��  �� *renameMediaButton+	 False.a���  � y�$ Visible=���  � ��$
 auto����  �� *renameMediaButton+ True.a���  � y�$ Hidden=���  � ��$ 0����  �� *showMediaUploadButton+ Visible.a���  � y�$ Visible=���  �� *showMediaUploadButton+
 Hidden.a���  � y�$ Hidden=��� " ��	 ��  �� *renameMediaButton+ True.a���  � y�$ Visible=���  �� *renameMediaButton+	 False.a���  � y�$ Hidden=�.���  � ColumnDefinitions �� $��* ��� $
��0.4* � � RowDefinitions �� $��40 ��� $
��auto ��� $��* �.+��� $��2�����#292929 �� Orientation$ 
Horizontal=�.+��� -    ��Name$ SyncLocalToFTP��$��5,0,0,0q����#3E3E3E$��Sync Local Media To FTP�����#ffffff�� -   $ RequestPullToDevices��$��5,0,0,0q����#3E3E3E$��Pull Images To Devices�����#ffffff�� $��1��$��2��$��0��#��[ .+��� $��Center=�.+��� $
��Bold"����� *statusMessage+�� $��2��.���� -    ��Name$ 
mediaSlots��$��Disabled=� �� SelectionMode$ Single=� �� SelectedItem �� *SelectedImageSlot+���� .R��  �Name$ 	wrapPanel������ .R�� $��5q����#1D1D1D �� $��20 ��� $��* ��� $��20 � �� $
��auto ��� $
��auto �.+��� $��0��$��1��$��64��$��64��$��0,20,0,5q����� *image+�� $��0��$��1��$��0,20,0,5q�$
��Left=�$��Bottom=����	#C1000000$��Segoe MDL2 Assets)����#fff���� *image_symbol+�� $��1��$��1��$��64��$��	0,10,0,20q�$��Center=����#fff$��NoWrap=����� *
image_name+�� $��1��$��1��$��64��$��	0,30,0,10q�$��Center=�$��9(����#fff$��NoWrap=����� *slotID+���� " ��.���  � ��$
 Left=���  � ��$ Center=��� -    ��Name$ 
InfoWindow��$��2��$��1��.+��� $��0��$��64��$	��300��$	��300��$��20q����� *SelectedImageSlot.image+�� $��Center=�$
��Bold"����#fff���� *SelectedImageSlot.image_name+�� -   $ replaceImage��$	��300��$��20,10,20,10q�$��Stretch=�$��Replace Image�����#fffy��� *showMediaUploadButton+�� -   $ RenameImage��$	��300��$��
20,0,20,10q�$��Stretch=�$��Rename Slot�����#fff��/   )FireflyGuardian.Views.MultiStyleExtension  *. renameButtonStyle+�� -   $ renameSection��$��Center=�#��[ .+�� .+��� -    ��Name$ mediaSlotName��$	��250��$��5q�$
��Left=�$��Center=��� $��10,0,0,0q�$
��Left=�$��Center=�
�������  J� IsHitTestVisible  .  $�� Enter Media Name...������ " ��.���  � y�$ 	Collapsed=�	 ��  �� *Text+! �� ElementName$! mediaSlotName��  .a���  � y�$ Visible=��� $��Center=�$ 
Horizontal=�.+��� -	   $ renameImageConfirm��$	��300��$��
20,10,5,10q�$��Stretch=�$��Rename�����#fff�� -
   $ renameImageCancel��$	��300��$��
5,10,20,10q�$��Stretch=�$��Cancel�����#fff