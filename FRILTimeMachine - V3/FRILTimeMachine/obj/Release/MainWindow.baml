   M S B A M L   `   `   `  ����   FRILTimeMachine     FRILTimeMachine.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     T PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f;Controls,http://metro.mahapps.com/winfx/xaml/controls  -     � Title$  FRIL Time Machine��$	��569��$��1005.8�� � WindowState$ 	Maximized=�(  "MahApps.Metro.Controls.MetroWindow   	GlowBrush$ Black�� � 	Resources  �� MergedDictionaries �� Source��  �� $ /Resources/Icons.xaml?�%    .��� .+��� .@��� $��Race Management��.���  � RowDefinitions �� $
��Auto ��� $��* �.+��� $��0��� $��1�� �� $
��Auto ��� $��* �.+�� .+��  �� $
��Auto ��� $��* � � ColumnDefinitions �� $��* ��� $��* �$	��200���� $��* �.+��� $��0��$��2�� �� Orientation$ 
Horizontal=�$��Center=�$	��500��.+��� -   $��MinTimeLabel��$��00��$��90(��� $��:��$��90(��� -   $��SecTimeLabel��$��00��$��90(��� $��.��$��90(��� -   $��MiliTimeLabel��$	��000��$��90(��� $��0��$��0��$��Round��$��90(�$��Center=�� $��1��$��0�� �� $��* ��� $
��Auto �.+��� -   $��RoundNoLabel��$��0��$��0��$��0��$��90(�$��Center=��� $��1��$ 
Horizontal=�$��Center=�.+��� -   $��
PrevButton��$��Prev. Round��$��5q��� -   $��
NextButton��$��
Next Round��$��5q��� $��1��$��1��.+��� $��Images/FRILLogo.png��$	��170��$��Center=��� $��1��$��3��$ 
Horizontal=�$��Center=�.+��� $ Vertical=�$��10q�$��90��.+��� -   	 ��Name$	 startBtn��$	��120��$	��Top=�   MetroCircleButtonStyle#���   $��Center=�$	��120��
������.���� $ Vertical=�.+��� $��60��$��70����!� 
 j� Stretch$

 Fill=�  appbar_timer_play !� Visual# [ �� $��START��$��Center=�$��16(��� $ Vertical=�$��10q�$��90��.+��� -	   $	 stopBtn��$	��120��$	��Top=�#���   $��Center=�$	��120��
�������.���� $ Vertical=�.+��� $��60��$��70����!� $

 Fill=�  appbar_timer_stop# [ �� $
��STOP��$��Center=�$��16(��� $��1��.+��� .+��� $��Online Racers��$��Center=�$
��Bold"�$��14(�� -
   $��PilotsPanel�� � Orientation$ 
Horizontal=�$��Center=�.+��� -   $��	Pilot1Box��$��Pilot 1��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot1LapsListBox��$��5q�$	��150��$	��200�� �� View �� .P��� $
��Time�� �� Width$	 110�� �� DisplayMemberBinding �� *	LapTime+�� $
��Type��$	 100�� �� *	EventName+�� -   $��	Pilot2Box��$��Pilot 2��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot2LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��Time��$	 110�� �� *	LapTime+�� $
��Type��$	 100�� �� *	EventName+�� -   $��	Pilot3Box��$��Pilot 3��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot3LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��Time��$	 110�� �� *	LapTime+�� $
��Type��$	 100�� �� *	EventName+�� -   $��	Pilot4Box��$��Pilot 4��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot4LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��Time��$	 110�� �� *	LapTime+�� $
��Type��$	 100�� �� *	EventName+�� -   $��	Pilot5Box��$��Pilot 5��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot5LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��time��$	 110�� �� *	LapTime+�� $
��type��$	 100�� �� *	EventName+�� -   $��	Pilot6Box��$��Pilot 6��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot6LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��time��$	 110�� �� *	LapTime+�� $
��type��$	 100�� �� *	EventName+�� -   $��	Pilot7Box��$��Pilot 7��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot7LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��time��$	 110�� �� *	LapTime+�� $
��type��$	 100�� �� *	EventName+�� -   $��	Pilot8Box��$��Pilot 8��.���  �� $��* ��� $
��Auto �.+��� $��0��.+��� -   $��Pilot8LapsListBox��$��5q�$	��150��$	��200�� �� .P��� $
��time��$	 110�� �� *	LapTime+�� $
��type��$	 100�� �� *	EventName+