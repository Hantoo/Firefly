   M S B A M L   `   `   `  ����J  FFireflyGuardian, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:FireflyGuardian.ViewsFireflyGuardian.Views  5      G(clr-namespace:FireflyGuardian.ViewModelsFireflyGuardian.ViewModels  5      (    "FireflyGuardian.Views.SettingsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:FireflyGuardian.Views     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      7my(clr-namespace:FireflyGuardian.ViewModels     5        � 	Resources  5         WarningHatchBrush � MappingMode � SpreadMethod �� 
StartPoint �� EndPoint � Offset � Color  GoodHatchBrush  ErrorHatchBrush  statusHeading �� 
TargetType �� 	Resources	 �� Triggers,   &FireflyGuardian.ViewModels.statusState
   Warning �� Value �� Binding �� Property �� Value   Good   Error N� EnterActions �� TargetProperty	 W� To g� Duration N� ExitActions�� 5   
   %�  &	        &	     &	 8    &	 T    7   7  7  �� 5      $ Absolute=�6<   $ Repeat=�6S   $	 0,0&�6i   $	 4,8&�6z   .��5      � $ 0Q�6   $ #BE9F3B��6*   � 5      $
 0.55Q�6   $ #BE9F3B��6-   � 5      $
 0.55Q�6   $ #8A7B66��6-   � 5      $ 1Q�6   $ #8A7B66��6*   5      �� 5      $ Absolute=�69   $ Repeat=�6P   $	 0,0&�6f   $	 4,8&�6w   .��5      � $ 0Q�6   $ #57BE3B��6*   � 5      $
 0.55Q�6   $ #46BE3B��6-   � 5      $
 0.55Q�6   $ #1E7227��6-   � 5      $ 1Q�6   $ #0F8015��6*   5      �� 5      $ Absolute=�6:   $ Repeat=�6Q   $	 0,0&�6g   $	 4,8&�6x   .��5      � $ 0Q�6   $ #BE3B3B��6*   � 5      $
 0.55Q�6   $ #BE3B3B��6-   � 5      $
 0.55Q�6   $ #8A7B66��6-   � 5      $ 1Q�6   $ #8A7B66��6*   5       �� 5!      " ��6*    5"      ��	 5#      �� 5$      # Z
 6K    6"   �� *statusStateIndicator+a�5%      �� 5&       � ��8   6;   5(      �� 5)      # Z 6K    6"   �� *statusStateIndicator+a�5*      �� 5+       � ��8  6;   5-      �� 5.      # Z 6K    6"   �� *statusStateIndicator+a�5/      �� 50       � ��8  6;   52      �� 54       6"   �� *
showStatus+ True6A    56      �� 57      .��58   "   �� .��59   &   W� $ 	MaxHeight�5:   )   $ 60X�5;   )   $ 0:0:0.5D�5<   )   5=   #   5>       5@      �� 5A      .��5B   "   �� .��5C   &   W� $ 	MaxHeight�5D   )   $ 0X�5E   )   $ 0:0:0.5D�5F   )   5G   #   5H      5J      5L      5M      .��5O      �  � ColumnDefinitions 5P   
   �� 5Q      $��* �6    � RowDefinitions 5T   
   �� 5U      $��40 �6   �� 5V      $
��auto �6   �� 5W      $��* �6   .+�5Z   
   �� $
��Left=�6   .+�5[      �� -   5         ��Name$ SaveFTP��5\      $��	30,10,0,0q�5]      $��Center=�5^      $��SAVE SETTINGS��5_      ���#fff5`      5a      �� 5b   
   $��1��5c      $��0��5d      #��[ 5e      .+�5f      �� $��Center=�6   .+�5g      �� $
��Bold"�6   ��6.   �� *statusMessage+5h      5i      �� 5j   
   $��2��5k      $��Center=�5l      $��Center=�5m      .+�5n      �� $��Center=�6    �� Orientation$ 
Horizontal=�66   .+�5o      �� $	��200��6   $��Center=�6)   .+�5p      �� $
��Bold"�5q      ���#fff$��FTP Settings��5s      �� 5t      ���#fff$��FTP IP��62   �� 5u      -   5         ��Name$ FTP_IP��5u      ��6.   �� *FTP_IP+ �� Mode$ TwoWay=��� 5v      ���#fff$��FTP USERNAME��62   �� 5w      -   5        $ FTP_User��5w      ��60   �� *
FTP_User+$ TwoWay=��� 5x      ���#fff$��FTP PASSWORD��62   �� 5y      -   5        $ FTP_Pass��5y      ��60   �� *
FTP_Pass+$ TwoWay=�5z      �� 5{      $	��300��5|      $
��50,0q�5}      $��Center=�5~      .+�5      �� $
��Bold"�5�      ���#fff$��Node Diagram Settings��5�      �� 5�      ���#fff$"��Venue Diagram Image Location��62   �� 5�      $ 
Horizontal=�6!   .+�5�      �� -   5        $ NodeDiagramLayoutView��5�      $	��200��5�      ��5�      �� *NodeDiagramLayoutView+$ TwoWay=��� 5�      -   5        $ chooseNewNodeDiagramFile��5�      $	��100��5�      $��Replace File��5�      ���#fff5�      5�      �� 5�      ���#fff$#��Venue Diagram Scale Multipler��62   �� 5�      -   5        $  VenueDiagramScaleMultipler��5�      $	��100��5�      $
��Left=�5�      ��5�      �� *VenueDiagramScaleMultipler+$ TwoWay=��� 5�      $	��300��5�      $��0,30,0,0q�5�      $
��Left=�5�      .+�5�      �� $
��Bold"�5�      ���#fff$��Node Settings��5�      �� 5�      ���#fff$��Global Brightness (0-255)��66   �� 5�      $ 
Horizontal=�6%   .+�5�      �� -   5        $ NodeGlobalBrightness��5�   !   $��80��5�   !   ��5�   !   �� *NodeGlobalBrightness+$ TwoWay=��� 5�      -	   5        $  updateNodeGlobalBrightness��5�   !   $	��180��5�   !   $��Push Global Brightness��5�   !   ���#fff5�   !   5�      5�      5�      5�      5�      5�      5�      