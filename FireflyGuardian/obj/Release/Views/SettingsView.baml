   M S B A M L   `   `   `  ���� J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  G(clr-namespace:FireflyGuardian.ViewModelsFireflyGuardian.ViewModels  (    "FireflyGuardian.Views.SettingsView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:FireflyGuardian.Views     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     7my(clr-namespace:FireflyGuardian.ViewModels       � 	Resources     WarningHatchBrush � MappingMode � SpreadMethod �� 
StartPoint �� EndPoint � Offset � Color  GoodHatchBrush  ErrorHatchBrush  statusHeading �� 
TargetType �� 	Resources	 �� Triggers,   &FireflyGuardian.ViewModels.statusState
   Warning �� Value �� Binding �� Property �� Value   Good   Error N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions�� %�  &	        &	 �     &	 T    &	 �    7   7  7  �� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #BE9F3B��� $
 0.55Q�$ #BE9F3B��� $
 0.55Q�$ #8A7B66��� $ 1Q�$ #8A7B66���� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #57BE3B��� $
 0.55Q�$ #46BE3B��� $
 0.55Q�$ #1E7227��� $ 1Q�$ #0F8015���� $ Absolute=�$ Repeat=�$	 0,0&�$	 4,8&�.��� $ 0Q�$ #BE3B3B��� $
 0.55Q�$ #BE3B3B��� $
 0.55Q�$ #8A7B66��� $ 1Q�$ #8A7B66���� " �� ��	 �� # Z
  �� *statusStateIndicator+a���  � ��8   �� # Z  �� *statusStateIndicator+a���  � ��8  �� # Z  �� *statusStateIndicator+a���  � ��8  ��  �� *
showStatus+ True �� .���� .��W� $ 	MaxHeight�$ 60X�$ 0:0:0.5D� �� .���� .��W� $ 	MaxHeight�$ 0X�$ 0:0:0.5D�.���  � ColumnDefinitions �� $��* � � RowDefinitions �� $��40 ��� $
��auto ��� $��* �.+��� $
��Left=�.+��� -    ��Name$ SaveFTP��$��	30,10,0,0q�$��Center=�$��SAVE SETTINGS�����#fff�� $��1��$��0��#��[ .+��� $��Center=�.+��� $
��Bold"����� *statusMessage+�� $��2��$��Center=�$��Center=�.+��� $��Center=� �� Orientation$ 
Horizontal=�.+��� $	��200��$��Center=�.+��� $
��Bold"����#fff$��FTP Settings���� ���#fff$��FTP IP���� -    ��Name$ FTP_IP������ *FTP_IP+ �� Mode$ TwoWay=��� ���#fff$��FTP USERNAME���� -   $ FTP_User������ *
FTP_User+$ TwoWay=��� ���#fff$��FTP PASSWORD���� -   $ FTP_Pass������ *
FTP_Pass+$ TwoWay=��� $	��300��$
��50,0q�$��Center=�.+��� $
��Bold"����#fff$��Node Diagram Settings���� ���#fff$"��Venue Diagram Image Location���� $ 
Horizontal=�.+��� -   $ NodeDiagramLayoutView��$	��200������ *NodeDiagramLayoutView+$ TwoWay=��� -   $ chooseNewNodeDiagramFile��$	��100��$��Replace File�����#fff�� ���#fff$#��Venue Diagram Scale Multipler���� -   $  VenueDiagramScaleMultipler��$	��100��$
��Left=����� *VenueDiagramScaleMultipler+$ TwoWay=��� $	��300��$��0,30,0,0q�$
��Left=�.+��� $
��Bold"����#fff$��Node Settings���� ���#fff$��Global Brightness (0-255)���� $ 
Horizontal=�.+��� -   $ NodeGlobalBrightness��$��80������ *NodeGlobalBrightness+$ TwoWay=��� -	   $  updateNodeGlobalBrightness��$	��180��$��Push Global Brightness�����#fff