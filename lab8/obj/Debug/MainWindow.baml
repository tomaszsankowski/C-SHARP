   M S B A M L   `   `   `  ����?  ;lab8, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab8lab8  5   	       lab8.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:lab8     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      �� 5      $
��auto �6   .+�5   
   o� $��0��6   $��2��6   .@�5      m� $
��File��6   .@�5      m� -   5        $
��Open��5      6F   m� 5      -   5        $��Close��5      6H   m� 5      -   5        $
��Exit��5      6F   5      5      R� 5   
   -   5         R�Name$ treeView��5      $��0��6%   $��1��65   6B   �� 5   
   $��1��6   $��1��6'   $
��Auto=�64   $
��Auto=�6Y   .��5      �� -   5         ��Name$ fileContentTextBlock��5      $
��Wrap=�66   5      �� 5!   
   $��2��6   $��1��6!   .@�5"      �� .��5#      �� -   5        $ openedStatusBarTextBlock��5#      5$      5%      �� 5'   
   $��2��6   $��0��6!   .@�5(      �� .��5)      �� -   5        $ statusBarTextBlock��5)      5*      5+      5,      5-      