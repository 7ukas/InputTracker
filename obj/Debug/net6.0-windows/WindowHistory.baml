   M S B A M L   `   `   `  ����G  CInputTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:InputTrackerInputTracker  5   	        InputTracker.WindowHistory   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   &localclr-namespace:InputTracker  5   	     � DataContext  5      #  InputTracker.HistoryViewModel  5   
   .��5      ]� .+�5   
   �� $	��Top=�6    �� Orientation$ 
Horizontal=�6*   $��0 15 0 0q�6C   .+�5      �� $��Start��6   $��DemiBold"�6%   �� 5      $	��End��6   $��DemiBold"�6#   $��	175 0 0 0q�69   �� 5      $��Content Options��6   $��DemiBold"�6/   $��	175 0 0 0q�6E   5      �� 5   
   $	��Top=�6   $ 
Horizontal=�6*   $��0 1 0 0q�6C   .+�5      &   System.Windows.Controls.Calendar  -   5          Name$ StartCalendar��5         SelectedDate 6.   �� *	StartDate+   DisplayDate 6Q   �� *	StartDate+   DisplayDateEnd 5      ��  �� Path$ SelectedDate� �� ElementName$ EndCalendar�� �� UpdateSourceTrigger$ PropertyChanged=�  5      -   5        $ EndCalendar��5      $��20 0 0 0q�6,    6>   �� *	EndDate+ 6_   �� *	EndDate+	   DisplayDateStart	 5      �� $ SelectedDate�$ StartCalendar��$ PropertyChanged=��� 5      $��18 0 0 0q�6   .+�5      �� $��Application��6   $��0 5 0 0q�6Y   
 d� 	IsChecked
 61   �� *ApplicationColumn+6Y   �� 5      $��Window��6   $��0 5 0 0q�6O   
 6,   �� *WindowColumn+6O   �� 5      $��Regular text��6   $��0 5 0 0q�6Z   
 62   �� *RegularTextColumn+6Z   �� 5      $��Raw text��6   $��0 5 0 0q�6R   
 6.   �� *RawTextColumn+6R   �� 5       $��Key strokes��6   $��0 5 0 0q�6X   
 61   �� *KeyStrokesColumn+6X   �� 5!      $��Mouse clicks��6   $��0 5 0 0q�6Z   
 62   �� *MouseClicksColumn+6Z   �� 5"      $��Empty entries��6   $��0 5 0 0q�6V   
 63   �� *EmptyEntries+6V   �� 5%      $ 
Horizontal=�6   $��0 5 0 0q�66   .+�5&      �� $��	Max Rows:��6    $��12(�61   $��0 2 0 0q�6?   �� 5(      -   5         ��Name$ MaxRows��5(      $��22��6/   $��1 2 0 0q�6;   $��5 0 0 0q�5)      $��48��6/   ��5(   M   �� *	MaxRows+��6f   �� *MaxRowsBackground+.��5)   :   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5*      +  %Microsoft.Xaml.Behaviors.EventTrigger  5+         	EventName$ TextChanged��6-   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5,   "   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 68   �� *AdjustMaxRowsCommand+   CommandParameter 6a   �� $
 Text�$ MaxRows��5-      .��5.   2   5/      50      �� 53      $ 
Horizontal=�6   $��0 3 0 0q�66   .+�54      �� -   5         ��Name$ Window��54      $��38��6,    � Cursor$
 Hand��67    
  Search#��[  6i     
HoverEvent#��[ 6�   {�6E   �� *SearchEnabled+ 55        56      $ MouseLeftButtonDown��6-   . 57   "      68   �� *SearchCommand+58      5:      �� 5<      $��10 0 0 0q�6   $��30��6.   $
 Hand��69     Refresh#��[ 6G   #��[ 6i    5=        5>      $ MouseLeftButtonDown��6-   . 5?   "      68   �� *RefreshCommand+5@      5B      5C      5D      5E      ]� 5H   
   $	��Top=�6   $��False��6)   $��0 10 0 0q�6?   .+�5K      �� $
��Left=�6   $ 
Horizontal=�6/   $��	0 -38 1 0q�6H   .+�5L      �� -   5        $ 	StartDate��5L      $��22��6-   $��1 2 0 0q�69   $	��110��6K    �� 
IsReadOnly$
 True��6W   $	��Top=�6i   ��5M      �� $ StartCalendar��$ SelectedDate� �� StringFormat$ 
yyyy-MM-dd��$ PropertyChanged=��� 5O      -   5        $ 	StartTime��5O      $��22��6-   $��1 2 0 0q�69   $��9 0 0 0q�6�   $��59��5P      $	��Top=�6%   ��5O   K   �� *	StartTime+��6f   �� *StartTimeBackground+.��5P   =    5Q        5R      $ TextChanged��6)   . 5S         64   �� *AdjustStartTimeCommand+ 6_   �� $
 Text�$ 	StartTime��5T      .��5U   .   5V      5W      �� 5Z      $
��Left=�6   $ 
Horizontal=�6/   $��
20 -38 1 0q�6H   .+�5[      �� -   5        $ EndDate��5[      $��22��6+   $��1 2 0 0q�67   $	��110��6I   $
 True��6U   $	��Top=�6g   ��5\      �� $ EndCalendar��$ SelectedDate�$ 
yyyy-MM-dd��$ PropertyChanged=��� 5^      -   5        $ EndTime��5^      $��22��6+   $��1 2 0 0q�67   $��9 0 0 0q�6�   $��59��5_      $	��Top=�6%   ��5^   I   �� *	EndTime+��6b   �� *EndTimeBackground+.��5_   =    5`        5a      $ TextChanged��6)   . 5b         64   �� *AdjustEndTimeCommand+ 6]   �� $
 Text�$ EndTime��5c      .��5d   .   5e      5f      �� 5i      $��Right=�6   $ 
Horizontal=�60   $��	0 -30 0 5q�6I   .+�5j      �� $��35��6   $
 Hand��6#     CsvFile#��[ 61   #��[ 6S    5k        5l      $ MouseLeftButtonDown��6)   . 5m         64   �� *GenerateCsvFileCommand+5n      5p      �� 5r      $��35��6   $
 Hand��6#     TxtFile#��[ 61   #��[ 6S    5s        5t      $ MouseLeftButtonDown��6)   . 5u         64   �� *GenerateTxtFileCommand+5v      5x      5y      5z      ]� 5}   
   $��Bottom=�6   $��False��6,   .+�5~      �� $
��Left=�6   $��20��6*     Database#��[ 65   �� 5      $
��Left=�6   $��5 0 0 0q�6D   $��Center=�6U   ��6.   �� *Rows+6U   �� 5�      $
��Left=�6   $��15 0 0 0q�6*   $��20��6<     Console#��[ 6G   �� 5�      $
��Left=�6   $��5 0 0 0q�6L   $��Center=�6]   ��6.   �� *Applications+6]   �� 5�      $
��Left=�6   $��15 0 0 0q�6*   $��20��6<     Keyboard#��[ 6G   �� 5�      $
��Left=�6   $��5 0 0 0q�6J   $��Center=�6[   ��6.   �� *
KeyStrokes+6[   �� 5�      $
��Left=�6   $��15 0 0 0q�6*   $��20��6<    	 Mouse#��[ 6G   �� 5�      $
��Left=�6   $��5 0 0 0q�6K   $��Center=�6\   ��6.   �� *MouseClicks+6\   �� 5�      $��Right=�6   $��0 7 0 0q�6+   $��30��6<    	 	TrashCan2#��[	 6H   #��[ 6l    5�        5�      $ MouseLeftButtonDown��6%   . 5�         60   �� *ClearDatabaseCommand+5�      5�      5�      &   System.Windows.Controls.DataGrid  5�   
   -	   5          Name$ DatabaseDataGrid��5�      $��Bottom=�6-   
�������   RowBackground �#99ccff   AlternatingRowBackground �#c2e0ff$��1q�5�   6      AreRowDetailsFrozen$
 True��6J      HeadersVisibility$ Column=�6e   $   HorizontalScrollBarVisibility$ Visible=�5�      "   VerticalScrollBarVisibility$ Visible=�6;      SelectionMode$ Extended=�6a      MaxColumnWidth$	 512Q�6z   5�      5�      