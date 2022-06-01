# Input Tracker
![](https://imgur.com/v4JWS4W.png)

## About
"Input Tracker" monitors and stores data of locally accessed Applications (processes) and Keyboard/Mouse inputs within them. 
It provides live overview of mentioned inputs, some general statistics and ability to fetch tracked data from the database.
With the help of SQLite, data is stored in to locally based database (located: bin/Debug/net5.0-windows/database/).

## Built using
* [WPF](https://github.com/dotnet/wpf)
* [SQLite3](https://www.sqlite.org/index.html)
* [Dapper](https://dapperlib.github.io/Dapper/)
* [Dragablz](https://dragablz.net/)
* [MaterialDesignColors](http://materialdesigninxaml.net/)
* [ModernUI.WPF](https://github.com/firstfloorsoftware/mui)

# Menu
### Overview
* Amount of unique applications, keyboard keys and mouse buttons pressed over certain period of time.
* Most active applications ordered by keys/buttons amounts.
* Last time database was updated with new records (can also update manually).

### Live
* Current local time.
* Tracking switch to turn on/off tracking of your inputs.
* Ability to choose text style between "Raw" (all key inputs) and "Regular" (visible by naked-eye).
* Live log of accessed applications and keys pressed within them.
* Keyboard and Mouse icons to notify once press or click happens.

### History
* Calendar to choose in what time frame data should be displayed.
* Content options to choose what columns of data to display/hide also includes options for empty (key-less) records and maximum number of lines.
* Data table of used applications and data collected within them such as it's process and window names, text, key strokes, mouse clicks.
* Export of selected data to CSV and TXT files.
* Ability to remove of all existing data from the database.
