   M S B A M L   `   `   `  ����=  9PL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PL.EngineerPL.Engineer  5   	   #    PL.Engineer.AddUpdateEngineer   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PL.Engineer     5   	     � Title$  AddUpdateEngineer��5	   	   $	��500��6#   $	��460��60   
������� � DataContext 5   	   �� *CurrentEngineer+ �� RelativeSource �� *Self+ �� Mode$ TwoWay=� � 	Resources 5
       �� 
TargetType �� Property �� Value��%b  '��       '�� t     '�� U    �� 5   
   " ��6   .�5      ��  � ��$ Consolas)�6+   �� 5       � ��$
 Bold"�6+   5      �� 5   
   " ��6   .�5      ��  � ��$ 15(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 1��6,   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   5      �� 5   
   " ��6   .�5      ��  � ��$ 2��6,   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � ��$	 120��6&   �� 5       �@��TextWrapping$
 Wrap=�6-   5      5      .��5      �  � RowDefinitions 5   
   �� 5       $��* �6   �� 5!      $��* �6   �� 5"      $��* �6   �� 5#      $��* �6   �� 5$      $��* �6   �� 5%      $��* �6   �� 5&      $��* �6   �� 5'      $��2* �6   	 � ColumnDefinitions	 5)   
   �� 5*      $��* �6   �� 5+      $��4* �6   �� 5,      $��4* �6   �� 5-      $��* �6   .+�5/   
   �� $��Id��6   $��0��6   �� 50   
   $��0��52   
   ��51   
   �� *Id+$ TwoWay=�
 �� NotifyOnValidationError$

 true�� �� ValidatesOnExceptions$
 true��52   
   �� 53   
   $
��Name��6   $��1��6   �� 54   
   $��1��6   ��55      �� *Name+$ TwoWay=�$

 true��$
 true���� 56   
   $��Email��6   $��2��6    �� 57   
   $��2��6   ��58      �� *Email+$ TwoWay=�$

 true��$
 true���� 59   
   $��Level��6   $��3��6    �� 5:   
   $��Center=�6   $��Center=�61   $	��120��6L   $��3��5=   0   $��2��6=   ��5;      ��    LevelsCollectionKey �� Source# [   �� SelectedValue 5<      �� *Level+$ TwoWay=�$

 true��$
 true��5=   =   �� 5>   
   $
��Cost��6   $��4��6   �� 5?   
   $��4��6   ��5@      �� *Cost+$ TwoWay=�$

 true��$
 true���� 5A   
   $
��Task��6   $��5��6   �� 5B   
   $��5��6   ��5C      �� *Task+$ TwoWay=�$

 true��$
 true���� 5D   
   -   5        $	��ADD��5D      $��6��5E   +   $��0��68   $��4��6H   $��Center=�5F   
   $��Center=�6'   $	��120��5G   
   $��50��6   
�������$��20(�66   5J      5K      