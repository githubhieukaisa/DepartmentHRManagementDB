   M S B A M L   `   `   `  ����E  Aassignment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:assignment.EmployeeUseControlassignment.EmployeeUseControl  5   	   /    )assignment.EmployeeUseControl.ViewPartner   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:assignment.EmployeeUseControl     5   	     � Title$  ViewPartner��5   	   $	��450��6   $	��800��6*   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $/��)Danh sách Partner liên quan đến Task��6   $
��Bold"�5      $��18(�6"   $��0,0,0,10q�60   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 
dgPartners��5      $��1��5         AutoGenerateColumns$ False��5         CanUserAddRows$ False��5         
IsReadOnly$
 True��5         SelectionMode$ Single=�5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 6Y   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *Employee.EmployeeId+6Y     5      $
 Tên��6%   $	 * 6X   
 62   �� *Employee.FullName+6X     5      $
 Role��6%   $	 * 6U   
 63   �� *Role.RoleName+6U     5      $ AssignAt��6%   $	 * 6V   
 67   �� *
AssignedAt+6V   5      �� 5    
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5!      �� -   5         ��Name$ cbTeamMember��5!      $	��250��6-   $��5q�69   �� 5"      -   5         ��Name$ btnAdd��5"      $��Thêm��6%   $��80��64   $��5q�6?   6J   �� 5#      -   5        $ 	btnDelete��5#      $
��Xóa��6(   $��80��66   $��5q�6A   6L   5$      5%      5&      