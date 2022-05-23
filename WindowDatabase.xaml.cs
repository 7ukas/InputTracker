using InputTracker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InputTracker {
    public partial class WindowDatabase : UserControl {
        // "CheckBox" Indexes
        private const int _ApplicationIndex = 1;
        private const int _WindowIndex = 2;
        private const int _RawTextIndex = 3;
        private const int _RegularTextIndex = 4;
        private const int _KeyStrokesIndex = 5;
        private const int _MouseClicksIndex = 6;

        // Date and Time "TextBox" values
        private IFormatProvider enUS; 
        private string _prevStartTimeText = "00:00:01";
        private string _prevEndTimeText = "23:59:59";
        private string _prevMaxRowsText = "10000";

        // "DataGrid" data
        private List<DBInput> _inputs;
        private FileGenerator _fileGenerator; // for .csv and .txt exports

        public WindowDatabase() {
            InitializeComponent();
        }

        private void _Window_Loaded(object sender, RoutedEventArgs e) {
            _OnRefreshClicked(null, null);
        }

        private void _OnSearch(object sender, EventArgs e) {
            // Set Start, End dates and Max rows
            DateTime start = Convert.ToDateTime($"{textBox_StartDate.Text} {textBox_StartTime.Text}");
            DateTime end = Convert.ToDateTime($"{textBox_EndDate.Text} {textBox_EndTime.Text}");
            int maxRows = int.Parse(textBox_MaxRows.Text);

            App.DatabaseSettings.StartDate = calendar_Start.DisplayDate;
            App.DatabaseSettings.StartTime = textBox_StartTime.Text;
            App.DatabaseSettings.EndDate = calendar_End.DisplayDate;
            App.DatabaseSettings.EndTime = textBox_EndTime.Text;
            App.DatabaseSettings.MaxRows = maxRows;

            // Empty Entries; Get required inputs to show.
            if ((bool)!checkBox_EmptyEntries.IsChecked) {
                _inputs = DatabaseController.GetInputs(start, end, maxRows)
                    .Where(i => i.RegularText.Length > 0).ToList();
                App.DatabaseSettings.DisplayEmptyEntries = false;
            } else {
                _inputs = DatabaseController.GetInputs(start, end, maxRows);
                App.DatabaseSettings.DisplayEmptyEntries = true;
            }

            // Set up DataGrid and FileGenerator
            dataGrid_Inputs.ItemsSource = _inputs;
            _fileGenerator = new FileGenerator(_inputs);

            // Counts of present inputs
            int keyStrokes = 0;
            int mouseClicks = 0;
            foreach (DBInput input in _inputs) {
                keyStrokes += input.KeyStrokes;
                mouseClicks += input.MouseClicks;
            }

            textBlock_Rows.Text = StringUtils.FormatCount(_inputs.Count);
            textBlock_Applications.Text = StringUtils.FormatCount(_inputs.GroupBy(x => x.Application).Select(x => x.Key).Count());
            textBlock_KeyStrokes.Text = StringUtils.FormatCount(keyStrokes);
            textBlock_MouseClicks.Text = StringUtils.FormatCount(mouseClicks);

            // Content Options: Applications, Window, Raw text, Regular text, Key strokes, Mouse clicks
            if ((bool)!checkBox_Application.IsChecked) {
                dataGrid_Inputs.Columns[_ApplicationIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayApplications = false;
            } else App.DatabaseSettings.DisplayApplications = true;

            if ((bool)!checkBox_Window.IsChecked) {
                dataGrid_Inputs.Columns[_WindowIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayWindows = false;
            } else App.DatabaseSettings.DisplayWindows = true;

            if ((bool)!checkBox_RawText.IsChecked) {
                dataGrid_Inputs.Columns[_RawTextIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayRawText = false;
            } else App.DatabaseSettings.DisplayRawText = true;

            if ((bool)!checkBox_RegularText.IsChecked) {
                dataGrid_Inputs.Columns[_RegularTextIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayRegularText = false;
            } else App.DatabaseSettings.DisplayRegularText = true;

            if ((bool)!checkBox_KeyStrokes.IsChecked) {
                dataGrid_Inputs.Columns[_KeyStrokesIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayKeyStrokes = false;
            } else App.DatabaseSettings.DisplayKeyStrokes = true;

            if ((bool)!checkBox_MouseClicks.IsChecked) {
                dataGrid_Inputs.Columns[_MouseClicksIndex].Visibility = Visibility.Collapsed;
                App.DatabaseSettings.DisplayMouseClicks = false;
            } else App.DatabaseSettings.DisplayMouseClicks = true;
        }

        private void _OnApplicationColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_ApplicationIndex].Visibility =
                (bool)checkBox_Application.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnWindowColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_WindowIndex].Visibility =
                (bool)checkBox_Window.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnRawTextColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_RawTextIndex].Visibility =
                (bool)checkBox_RawText.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnRegularTextColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_RegularTextIndex].Visibility =
                (bool)checkBox_RegularText.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnKeyStrokesColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_KeyStrokesIndex].Visibility =
                (bool)checkBox_KeyStrokes.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnMouseClicksColumnChanged(object sender, EventArgs e) {
            if (dataGrid_Inputs.Columns.Count > 0) {
                dataGrid_Inputs.Columns[_MouseClicksIndex].Visibility = 
                (bool)checkBox_MouseClicks.IsChecked ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _OnMaxRowsChanged(object sender, TextChangedEventArgs e) {
            // In case illegal char was typed
            TextChange change = e.Changes.Last();
            int maxLen = 6;

            if (change.AddedLength > 0) {
                char added = textBox_MaxRows.Text
                    .Substring(change.Offset, change.AddedLength).Last();

                if (!char.IsDigit(added)) {
                    textBox_MaxRows.Text = _prevMaxRowsText;
                }
            }

            if (textBox_MaxRows.Text.Length > maxLen) {
                textBox_MaxRows.Text = _prevMaxRowsText;
            }

            int maxRows;
            int.TryParse(textBox_MaxRows.Text, out maxRows);

            if (maxRows <= 100000) {
                _prevMaxRowsText = textBox_MaxRows.Text;
                textBox_MaxRows.Background = Brushes.White; // White (Default)
            } else {
                textBox_MaxRows.Background = (Brush)new BrushConverter().ConvertFrom("#E74C3C"); // Red
            }

            if (image_Search is not null) {
                image_Search.IsEnabled = _AreValuesValidForSearch();
            }
        }

        private void _OnStartDateChanged(object sender, SelectionChangedEventArgs e) {
            DateTime selectedDate = DateTime.Parse(e.AddedItems[0].ToString());

            calendar_End.DisplayDateStart = selectedDate;
            if (selectedDate > calendar_End.SelectedDate) {
                calendar_End.SelectedDate = selectedDate;
            }
        }

        private void _OnStartTimeChanged(object sender, TextChangedEventArgs e) {
            // In case illegal char was typed
            TextChange change = e.Changes.Last();
            int maxLen = 8;

            if (change.AddedLength > 0) {
                char added = textBox_StartTime.Text.Substring(change.Offset, change.AddedLength).Last();
                if (!char.IsDigit(added) && added != ':') {
                    textBox_StartTime.Text = _prevStartTimeText;
                }
            }

            if (textBox_StartTime.Text.Length > maxLen) {
                textBox_StartTime.Text = _prevStartTimeText;
            }

            DateTime time;
            DateTime.TryParseExact(textBox_StartTime.Text, "HH:mm:ss", 
                enUS, DateTimeStyles.None, out time);

            if (time.ToString().Substring(11) != "00:00:00") {
                _prevStartTimeText = textBox_StartTime.Text;
                textBox_StartTime.Background = Brushes.White; // White (Default)
            } else {
                textBox_StartTime.Background = (Brush)new BrushConverter().ConvertFrom("#E74C3C"); // Red
            }

            if (image_Search is not null) {
                image_Search.IsEnabled = _AreValuesValidForSearch();
            }
        }

        private void _OnEndTimeChanged(object sender, TextChangedEventArgs e) {
            // if wrong char added
            TextChange change = e.Changes.Last();
            int maxLen = 8;

            if (change.AddedLength > 0) {
                char added = textBox_EndTime.Text.Substring(change.Offset, change.AddedLength).Last();
                if (!char.IsDigit(added) && added != ':') {
                    textBox_EndTime.Text = _prevEndTimeText;
                }
            }

            if (textBox_EndTime.Text.Length > maxLen) {
                textBox_EndTime.Text = _prevEndTimeText;
            }

            DateTime time;
            DateTime.TryParseExact(textBox_EndTime.Text, "HH:mm:ss",
                enUS, DateTimeStyles.None, out time);

            if (time.ToString().Substring(11) != "00:00:00") {
                _prevEndTimeText = textBox_EndTime.Text;
                textBox_EndTime.Background = Brushes.White; // White (Default)
            } else {
                textBox_EndTime.Background = (Brush)new BrushConverter().ConvertFrom("#E74C3C"); // Red
            }

            if (image_Search is not null) {
                image_Search.IsEnabled = _AreValuesValidForSearch();
            }
        }

        private void _OnRefreshClicked(object sender, EventArgs e) {
            // Set setting to default values
            App.DatabaseSettings.ResetToDefault();

            calendar_Start.SelectedDate = App.DatabaseSettings.StartDate;
            calendar_Start.DisplayDate = App.DatabaseSettings.StartDate;
            textBox_StartTime.Text = App.DatabaseSettings.StartTime;

            calendar_End.SelectedDate = App.DatabaseSettings.EndDate;
            calendar_End.DisplayDate = App.DatabaseSettings.EndDate;
            textBox_EndTime.Text = App.DatabaseSettings.EndTime;

            checkBox_Application.IsChecked = App.DatabaseSettings.DisplayApplications;
            checkBox_Window.IsChecked = App.DatabaseSettings.DisplayWindows;
            checkBox_RawText.IsChecked = App.DatabaseSettings.DisplayRawText;
            checkBox_RegularText.IsChecked = App.DatabaseSettings.DisplayRegularText;
            checkBox_KeyStrokes.IsChecked = App.DatabaseSettings.DisplayKeyStrokes;
            checkBox_MouseClicks.IsChecked = App.DatabaseSettings.DisplayMouseClicks;
            checkBox_EmptyEntries.IsChecked = App.DatabaseSettings.DisplayEmptyEntries;
            textBox_MaxRows.Text = $"{App.DatabaseSettings.MaxRows}";

            // Empty DataGrid
            dataGrid_Inputs.ItemsSource = null;
            _fileGenerator = null;

            textBlock_Rows.Text = "0";
            textBlock_Applications.Text = "0";
            textBlock_KeyStrokes.Text = "0";
            textBlock_MouseClicks.Text = "0";
        }

        private void _OnCsvFileClicked(object sender, EventArgs e) {
            if (dataGrid_Inputs.HasItems) {
                _fileGenerator.GenerateCSV(
                    checkBox_Application.IsChecked == true, checkBox_Window.IsChecked == true, 
                    checkBox_RawText.IsChecked == true, checkBox_RegularText.IsChecked == true,
                    checkBox_KeyStrokes.IsChecked == true, checkBox_MouseClicks.IsChecked == true
                );
            }
        }

        private void _OnTxtFileClicked(object sender, EventArgs e) {
            if (dataGrid_Inputs.HasItems) {
                _fileGenerator.GenerateTXT(
                    checkBox_Application.IsChecked == true, checkBox_Window.IsChecked == true,
                    checkBox_RawText.IsChecked == true, checkBox_RegularText.IsChecked == true,
                    checkBox_KeyStrokes.IsChecked == true, checkBox_MouseClicks.IsChecked == true
                );
            }
        }

        private void _OnTrashCanEmptied(object sender, EventArgs e) {
            DateTime start = Convert.ToDateTime($"{textBox_StartDate.Text} {textBox_StartTime.Text}");
            DateTime end = Convert.ToDateTime($"{textBox_EndDate.Text} {textBox_EndTime.Text}");
            string startStr = start.ToString("yyyy-MM-dd HH:mm:ss");
            string endStr = end.ToString("yyyy-MM-dd HH:mm:ss");

            // Warn before proceeding data removal from database
            if (MessageBox.Show(
                $"Are you sure you want to REMOVE ALL EXISTING DATA from the database?",
                WindowMain.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                WindowMain.ClearDatabase();
                _OnRefreshClicked(null, null);
            }
        }

        private bool _AreValuesValidForSearch() {
            return new[] {textBox_StartTime.Background, 
                          textBox_EndTime.Background, 
                          textBox_MaxRows.Background }
            .All(bg => bg.ToString() == "#FFFFFFFF");
        }
    }
}
