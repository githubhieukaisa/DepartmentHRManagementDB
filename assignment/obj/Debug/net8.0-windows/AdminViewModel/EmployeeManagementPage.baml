   M S B A M L   `   `   `  ����  
assignment6    0assignment.AdminViewModel.EmployeeManagementPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � .+�5   
   �� .+�5	      �� -   5        $��formEmployeeManagement��5	      $y�	Collapsed=�67   .+�5
      �� -   5        $��headerOfForm��5
      $��Add new Employee��60   $��20(�6H   $
��Bold"�6V   $��10q�6h   � 5        � ColumnDefinitions  5      �� 5      $
��Auto �6+   �� 5      $��* �6+    � RowDefinitions 5      �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   .+�5      �� $��
Full Name:��6    $��0��62   $��0��6?   $��5q�6O   �� 5      -   5         ��Name$ txtFullName��5      $��0��63   $��1��6@   $��5q�6P   $	��200��6[   �� 5      $��Email:��6    $��1��6.   $��0��6;   $��5q�6K   �� 5      -   5        $ txtEmail��5      $��1��60   $��1��6=   $��5q�6M   $	��200��6X   �� 5      $��	Password:��6    $��2��61   $��0��6>   $��5q�6N   �� 5      -   5        $ txtPassword��5      $��2��63   $��1��6@   $��5q�6P   $	��200��6[   �� 5      $��Role:��6    $��3��6-   $��0��6:   $��5q�6J   �� 5      -   5         ��Name$ cmbRole��5      $��3��60   $��1��6=   $��5q�6M   $	��200��6X    �� DisplayMemberPath$ RoleName��6d    �� SelectedValuePath$ RoleId��6�   �� 5       $��Department:��6    $��4��63   $��0��6@   $��5q�6P   �� 5!      -   5        $ cmbDepartment��5!      $��4��66   $��1��6C   $��5q�6S   $	��200��6^   $ DepartmentName��6j   $ DepartmentId��6�   �� 5"      -   5        $��btnManagement��5"      $	��Add��62   $��5��6@   $��2��6M   $��5q�6a   $	��100��6�   5#      5$      �� 5%       �� Orientation$ 
Horizontal=�6   $
��Left=�62   $��10q�6M   .+�5&      �� -	   5        $ 	txtSearch��5&      $	��200��6-   $��5q�69   �� 5'      -
   5        $��Search��5'      $��5q�6?   5(      �� 5)      $��Employee Management��6   $��20(�63   $
��Bold"�6A   $��10q�6S   &   System.Windows.Controls.DataGrid  5*      -   5          Name$ 
dgEmployee��5*         AutoGenerateColumns$ False��6+   $��10q�6G   $	��200��6S   	   Columns	 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      ,  &System.Windows.Controls.DataGridColumn
   Header$
 FullName��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *
FullName+  5-      $
 Email��6)    68   �� *Email+  5.      $

 Role��6)    67   �� *Role.RoleName+  5/      $
 
Department��6)    6=   �� *Department.DepartmentName+  50      $
 Status��6)    69   �� *Status+52      �� 53      $ 
Horizontal=�6   $��10q�62   .+�54      �� -   5        $��Add Employee��54      $��5q�6J   �� 55      -   5        $��Edit Employee��55      $��5q�6L   �� 56      -   5        $��Deactive Employee��56      $��5q�6R   �� 57      -   5        $��Active Employee��57      $��5q�6P   �� 58      -   5        $��Export to Excel��58      $��5q�6S   59      5:      5;      5<      