   M S B A M L   `   `   `  ����=  9PL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PL.EngineerPL.Engineer  5   	   $    PL.Engineer.EngineerListWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PL.Engineer     5   	     � Title$  EngineerListWindow��5	   	   $	��640��6$   $	��800��61    � DataContext 5   	   ��  �� RelativeSource ��  �� Mode$
 Self=� � 	Resources 5
       �� 
TargetType �� Property �� Value��%�   '��       �� 5   
   " ��6   .�5      ��  � ��$ Consolas)�6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��
 ��� �6+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $	��40* �6   �� 5      $
��500* �6   �� 5      $
��Auto �6   .+�5   
   � $��Stretch=�6   $
��auto��6-   $��0��6;   $��Stretch=�6H   $
��auto��6d   	 � ColumnDefinitions	 5      �� 5      �� 5      $	��400 �6#   .+�5      �� $��Level��6   $��0��6$   $��Center=�64   $��Center=�6Q   $��18(�6l   $
��Bold"�6z   �� 5      $��5q�6   $��1��6"   $��1��5!      $��Center=�6'   $��Center=�6D   $
��Bold"�5"      $��18(�6)   ��5   /   �� 
 �� Path$
 EngineerList� �� SelectedValue 5       �� $
 SearchLevelSelectedValue� �� Mode$ TwoWay=�5"   )   5#      �� 5$   
   -   5        $��1��5$      ��6    �� *EngineerList+ �� View 5%      �� 5&      .P�5'      �� 5(      5*      �� 5+   
   -   5        $��1��5.      $��Center=�6   $��Center=�69   $
��Bold"�5/      $��18(�6   ��5+      ��    LevelsCollectionKey �� Source# [   5,      �� $
 SearchLevelSelectedValue�$ TwoWay=�5/      �� 50   
   -   5        $	��Add��50      
�������$��2��62   $��Center=�6?   $��50��6\   $	��100��6h   $��	UltraBold"�6u   6�   51      52      