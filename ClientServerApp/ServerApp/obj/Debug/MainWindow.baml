   M S B A M L   `   `   `  ����D  @ServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ServerApp	ServerApp  5   
       ServerApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   )localclr-namespace:ServerApp     5   
     � Title$  
MainWindow��5   	    � WindowState$ 	Maximized=�6    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5	      $. (pack://application:,,,/Styles/Style.xaml?�6%   %    5      .��5      � $��Stretch=�6   $��Stretch=�6)    � RowDefinitions 5   
   �� 5      $	��15* �6   �� 5      $	��85* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $	��20* �6#   �� 5      $��4* �6#   �� 5      $	��38* �6#   �� 5      $	��38* �6#    5      �� 5      $��* �6    �� 5      $��* �6    .+�5       �� $��0��6   $��0��6(   $��2��65   $��3��6F   $��Server��6Z   $��Center=�6h   $��30(�6�   $��Center=�6�   �� 5"      -   5        $��buttonConnect��5#      $��0��5"      $��3��6"   $��Connect��62   $��32��6D   $
��Left=�6P   {�. $��50,0,0,0q�6|   $��Center=�5#   &   $��88��6A   6L   �� 5$      -   5        $��buttonDisConnect��5%      $��1��5$      $��
Disconnect��6"   $��32��67   $
��Left=�6C   {�.  $��50,0,0,0q�6p   $��3��5%   )   $��Center=�69   $��88��6T   6_   5&      � 5(   
   $��1��6   .+�5)      9�  9� 
IsExpanded . $��Stretch=�6)   $��Stretch=�6E   ��5*      � 5+      $��Stretch=�6   $��Stretch=�69   .+�5,      �� $��Center=�6$   Recieved Client Message6Z   5-      .��5/      �� $
��Auto=�6   $
��Auto��6B   .��50      � $��Stretch=�6   $��Stretch=�69   .+�51      �� -   5        $��RichTextBoxCommand��51   J   $
��Bold"�6&    �� 
IsReadOnly . $
��Auto=�6�   $��Stretch=�6�   6�   52      53      54      55      56      57      