   M S B A M L   `   `   `  ����G  CInputTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:InputTrackerInputTracker  5   	       InputTracker.WindowLive   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   &localclr-namespace:InputTracker  5   	   .��5      ]� .+�5	   
   ]� $	��Top=�6   $��False��6)   $��0 15 0 0q�6?   .+�5
      �� $
��Left=�6   $��35��6*      Calendar#��[  65   �� 5      $
��Left=�6   $��DemiBold"�6K   $��8 3 0 0q�6a   $��Center=�6r   ��6.   �� *CurrentDate+6r   d� 5      $��Right=�6   $��10 0 0 0q�62   $��20��6D   $��68��6P     
HoverEvent#��[ 6[     d� 	IsChecked  6   �� *
IsTracking+ �� Mode$ TwoWay=���5      �� 5       �� 
TargetType" d�6&   .R�5      ��  ��Name$ B1��6!   ���#E74C3C �� CornerRadius$ 10��6B    �� Padding$ 2q�6T   ._�5      �� $ B2��6%   
�������$��17��6D   $ 10��6O   $
��Left=�6a   ._�5   "   �� 5      5       �� Triggers 5      P� 5       P� Property �@d�	IsChecked P� Value$
 TrueY�6;   . �5   "   �� 	 �� 
TargetName$	 B2��6)   
 �� Property
 � �� �� Value$ Right=�6X   �� 5   "   $	 B1��6)   
 � �� �#2ECC716O   5      5      5      �� 5      $��Right=�6   $��Tracking��6/   $��DemiBold"�6?   $��Center=�6U   5      ]� 5!   
   $	��Top=�6   $��False��6)   $��0 5 0 0q�6?   .+�5"      �� $
��Left=�6    	 Clock#��[ 6*   $��34��6J   �� 5#      $
��Left=�6   $��DemiBold"�6K   $��8 3 0 0q�6a   $��Center=�6r   ��6.   �� *CurrentTime+6r   �� 5$      $��Right=�6   $��10 0 0 0q�6.   $��22��6@   $��87��6L   ��6W   �� *Styles+ �� SelectedItem 6v   �� *SelectedStyle+$ TwoWay=�*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5%      +  %Microsoft.Xaml.Behaviors.EventTrigger  5&         	EventName$ SelectionChanged��6%   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5'      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *ChangeTextStyleCommand+5(      5*      �� 5+      $��Right=�6   $��Style��6/   $��DemiBold"�6<   $��Center=�6R   5,      ]� 5/   
   $	��Top=�6   $��False��6)   $��0 35 0 0q�6?   .+�50      �� $
��Left=�6   $��0 0 0 5q�6*   $��35��6;     Application#��[ 6F   �� 51      $
��Left=�6   $��Application��6.   $��DemiBold"�6A   $��8 0 0 5q�6W   $��Center=�6h   �� 52      -   5         ��Name$ KeyboardImage��52   +   $��Right=�6   $��5 0 0 0q�6B   $��30��6S     Keyboard#��[ 6^     OnKeyPressedEvent#��[ 6�   $��Center=�6�   �� 53      -   5        $ 
MouseImage��53   +   $��Right=�6   $��27��6?    	 Mouse#��[ 6J     OnMouseButtonClickedEvent#��[ 6j   $��Center=�6�   54      ]� 57   
   $��Bottom=�6   $��False��6,   .+�58      �� $
��Left=�6   $��0 0 5 0q�6*   $��20��6;     Database#��[ 6F   �� 59      $
��Left=�6   $��0 0 15 0q�6G   $��Center=�6Y   ��6.   �� *	Entries+6Y   �� 5:      $
��Left=�6   $��0 0 5 0q�6*   $��20��6;   #��[ 6F   �� 5;      $
��Left=�6   $��0 0 15 0q�6J   $��Center=�6\   ��6.   �� *
KeyStrokes+6\   �� 5<      $
��Left=�6   $��0 0 5 0q�6*   $��20��6;   #��[ 6F   �� 5=      $
��Left=�6   $��0 0 15 0q�6K   $��Center=�6]   ��6.   �� *MouseClicks+6]   �� 5?      $��Right=�6   $��5 7 0 0q�6+   $��30��6<    � Cursor$
 Hand��6H    		 Arrow#��[	 6V   #��[ 6v    5@        5A      $ MouseLeftButtonDown��6%   . 5B      4  .Microsoft.Xaml.Behaviors.Core.CallMethodAction     
MethodName$ ScrollToEnd��6-      TargetObject 6F   ��  �� ElementName$ LiveLogTextBox��$ OneWay=�8  2Microsoft.Xaml.Behaviors.Core.ChangePropertyAction  5C         PropertyName$ Source��61    
 ArrowActive   Value# [
 6G   5D        5E      $ MouseLeftButtonUp��6%   . 5F        $ Source��61   # [	 6G   5G      5I      �� 5J      $��Right=�6   $��0 7 0 0q�6+   $��30��6<   $
 Hand��6H     TrashCan#��[ 6V   #��[ 6y    5K        5L      $ MouseLeftButtonDown��6%   . 5M        $ Source��61     TrashCanActive# [ 6G     5N       60   �� *ClearLogCommand+5O        5P      $ MouseLeftButtonUp��6%   . 5Q        $ Source��61   # [ 6G   5R       J� InputBindings 5T      d� 5U       d� Gesture$ 	LeftClicka�6#    �� Command 67   �� *ClearLogCommand+5W      5X      �� 5[   
   $	��Top=�6    �� 
IsReadOnly$
 True��6E   $��0 0 0 10q�6W   $��2 2 0 0q�6i   $��24��6{     LogTextBoxStyle#��[ 6�   ��6'   �� *BriefLiveLog+6�   �� 5\   
   -   5         ��Name$ LiveLogTextBox��5\   *   $��Bottom=�6   $��2 2 0 0q�6[   $
 True��6m    �� TextWrapping$
 Wrap=�6   $��Disabled=�5]      $
��Auto=�6H   #��[ 6x   ��5\   B   �� *	LiveLog+.��5]   �    5^        5_      $ TextChanged��6!   . 5`        $ ScrollToEnd��6)   5a      .��5b   &   5c      5f      5g      