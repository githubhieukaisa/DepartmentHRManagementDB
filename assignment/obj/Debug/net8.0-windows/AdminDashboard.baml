   M S B A M L   `   `   `  ����  
assignment    assignment.AdminDashboard   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  AdminDashboard��5   	   $	��450��6    $	��800��6-   .��5      � .+�5	   
   �� .+�5
      �  � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $��* �6'   �� 5      $��* �6'   �� 5      $��* �6'   .+�5      �� -   5        $��Quản lý nhân viên��5      $��0��65   6E   �� 5      -   5        $��Quản lý phòng ban��5      $��1��65   6E   �� 5      -   5        $��Quản lý vai trò��5      $��2��63   6C   �� 5      -   5        $��Đăng xuất��5      $��3��6-   6=   5      �� 5      $��Admin Dashboard��6   $��24(�6/   $
��Bold"�6=   $��10q�6O   $
��Left=�6[   �� 5      $,��&Quản lý nhân viên theo phòng ban��6   $��16(�6@   $��10q�6N   $
��Left=�6Z   � 5       5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� $��Chọn phòng ban:��6   $��0��63   $��Center=�6C   $��10q�6^   �� 5      -   5         ��Name$ cbDepartment��5      $��1��61   $��10q�6A   $��200px��6M   $
��Left=�6[    �� DisplayMemberPath$ DepartmentName��5        �� SelectedValuePath$ DepartmentId��6>   6�   5!      �� 5"      .+�5#      �  5$      �� 5%      $
��Auto �6+   �� 5&      $��* �6+    � RowDefinitions 5(      �� 5)      $
��Auto �6(   �� 5*      $
��Auto �6(   .+�5,      �� $��Mô tả phòng ban:��6    $��0��68   $��0��6E   $��10q�6V   6d   �� 5-      -   5         ��Name$ tbDescription��5-      $��0��65   $��1��6B   $��10q�6R   $��300px��6^   $
��Left=�6l   ${�False��6�   �� 5.      $��Quản lý:��6    $��1��60   $��0��6=   $��10q�6M   6[   �� 5/      -   5        $ 	tbManager��5/      $��1��61   $��1��6>   $��10q�6N   $��300px��6Z   $
��Left=�6h   ${�False��6�   50      &   System.Windows.Controls.DataGrid  51      -   5        $��
dgEmployee��51         AutoGenerateColumns$ False��6-      Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn	   Header$	 FullName��6-   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6?   �� *
FullName+6^     54      $	 Email��6-   
 6<   �� *Email+6X     55      $
	 Role��6-   
 6;   �� *Role.RoleName+6_     56      $	 
Department��6-   
 6A   �� *Department.DepartmentName+6q     57      $	 Status��6-   
 6=   �� *Status+6Z   59      5:      5;      5<      5=      