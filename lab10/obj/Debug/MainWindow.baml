   M S B A M L   `   `   `  ����@  <lab10, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab10lab10  5   	       lab10.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:lab10     5   	     � Title$  
MainWindow��5   	   $	��480��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��4* �6   �� 5      $��3* �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5   #   $��0��6      AutoGenerateColumns .     CanUserSortColumns .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Year��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Year+6L     5      $ Model��6%   $ * 6N    64   �� *Model+6N     5      $ Model��6%   $ * 6T    64   �� *Motor.Model+6T     5      $ Displacement��6%   $ * 6b    6;   �� *Motor.Displacement+6b     5      $ 
HorsePower��6%   $ * 6^    69   �� *Motor.HorsePower+6^   5      �� 5   
   $��0��6   	 �� Orientation$	 
Horizontal=�6%   $��Center=�6>   $��Bottom=�6[   .+�5      �� -   5        $��Delete��5      $��50��69   $	��100��6E   �� 5      -   5        $��Search��5      $��50��6;   $	��100��6G   �� 5      -   5        $��Clear Filters��5      $��50��6H   $	��100��6T   5      �� 5   
   -   5        
 ��Name$
 zadaniaTextBox��5   "   $��1��6   $��0 0 0 10q�6:   �� 5    
   $��1��6   $	 
Horizontal=�6%   $��Center=�6>   $��Bottom=�6[   .+�5!      �� -   5        $��	Zadanie 1��5!      $��50��6<   $��70��6H   �� 5"      -   5        $��	Zadanie 2��5"      $��50��6<   $��70��6H   �� 5#      -   5        $��Zad3 (sort)��5#      $��50��6C   $��70��6O   �� 5$      -	   5        $��Zad3 (find)��5$      $��50��6C   $��70��6O   5%      �� 5&   
   $��	Zadanie 4��6   $
��Left=�6$   $��
10,340,0,0q�6?   $	��Top=�6S   �� 5'   
   -
   5        $��inputComboBox��5'      $
��Left=�6(   $��
75,345,0,0q�6C   $	��Top=�6W   $	��100��6o   �� 5(   
   -   5        $��outputComboBox��5(      $
��Left=�6)   $��275,345,0,0q�6D   $	��Top=�6Y   $	��100��6q   �� 5)   
   -   5        $��filtersTextBox��5)      $
��Left=�6(   $��175,345,0,0q�6C    �� TextWrapping$
 Wrap=�6X   $	��Top=�6l   $	��100��6�   $��22��6�   �� 5*   
   -   5        $��Filter��5*      $��370,270,0,0q�65   $��60��6J   $��22��6U   5+      5,      