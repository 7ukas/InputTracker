   M S B A M L   `   `   `  ����G  CInputTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:InputTrackerInputTracker  5   	       InputTracker.WindowMain   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ,localclr-namespace:InputTracker     5   	   $	��615��5   	   $	��680��6   $	��615��6(   $	��680��65     � Title$  Input Tracker��5   	   .��5      � $��20q�6      TabMenuGridStyle#��[  6    � ColumnDefinitions 5	   
   �� 5
      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   d� $��0��6   $��0��6$   $��OVERVIEW��64     TabMenuButtonStyle#��[ 6w    d� 	IsChecked 6G   �� *IsOverviewWindowDisplayed+6w   d� 5   
   $��0��6   $��1��6$   $
��LIVE��64   #��[ 6o    6C   �� *IsLiveWindowDisplayed+6o   d� 5   
   $��0��6   $��2��6$   $��HISTORY��64   #��[ 6u    6F   �� *IsHistoryWindowDisplayed+6u   � 5   
   -   5         �Name$ 
WindowGrid��5      $��1��6#   $��0��60   $��3��6@   
�������6T   5      5      