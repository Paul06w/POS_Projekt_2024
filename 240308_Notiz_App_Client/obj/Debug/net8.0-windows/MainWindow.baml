   M S B A M L   `   `   `  ����R  N240308_Notiz_App_Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:_240308_Notiz_App_Client_240308_Notiz_App_Client  5   	   )    #_240308_Notiz_App_Client.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:_240308_Notiz_App_Client     5   	     � Title$  	Notiz App��5   	   $	��450��6   $	��800��6(    � Icon$ 	/icon.png��64    � 	Resources 5
         NoMouseOverButtonTemplate �� 
TargetType �� Triggers P� Property P� Value �� Value �� Property��%2  &	        �� 5   
   " ��5      .R�5      �� #��z��6   #��z��5      #��z��5      ._�5      �� #��z��6#   #��z��5      5       5      P� 5       � {�$ False��5      . �5      �� # Z�5       � ���� 5      # Z
�5       � ��5      5      5      .��5       � 	 � RowDefinitions	 5!   
   �� 5"      $
��0.2* �6   �� 5#      $��* �6   
 � ColumnDefinitions
 5'   
   �� 5(      $��* �6   .+�5+   
   �� -   5        $��0��5+      $��0��6   $��Center=�6.   $��Center=�6I   $��
Neue Notiz��6f   6�   �� 5,   
   -   5         ��Name$ ScrollViewNotizen��5,   4   $��1��6   $��0��6$   $
��Auto=�6O   $
��Auto=�6t   .��5-      � -   5         �Name$ WrapPanelNotizen��5-       � Orientation$ 
Horizontal=�62   $	��700��6K   5.      50      51      