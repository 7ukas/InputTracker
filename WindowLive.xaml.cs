using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace InputTracker {
    public partial class WindowLive : UserControl {
        // Listeners
        private InputDeviceListener _inputListener;
        private ApplicationListener _appListener;

        // Monitors
        private ApplicationMonitor _appMonitor;
        private KeyMonitor _keyMonitor;
        private MouseMonitor _mouseMonitor;

        // Applications
        private string _lastAppTitle = WindowMain.Title;
        private string _lastAppWindowTitle = WindowMain.Title;

        // Log
        private StringBuilder _log = new StringBuilder();
        private int _logCharLimit = 10000;

        // Count
        private int _entries = 0;
        private int _keyStrokes = 0;
        private int _mouseClicks = 0;

        public WindowLive() {
            InitializeComponent();
        }

        private void _Window_Loaded(object sender, RoutedEventArgs e) {
            // Settings
            toggleButton_Tracking.IsChecked = App.LiveSettings.IsTracking;

            if (App.LiveSettings.Style == "Regular") {
                comboBoxItem_Regular.IsSelected = true;
                comboBoxItem_Raw.IsSelected = false;
            } else if (App.LiveSettings.Style == "Raw") {
                comboBoxItem_Regular.IsSelected = false;
                comboBoxItem_Raw.IsSelected = true;
            }

            // Listeners, Events, Hooks
            _inputListener = new InputDeviceListener();
            _inputListener.OnKeyPressed += _Listener_OnKeyPressed;
            _inputListener.OnButtonClicked += _Listener_OnButtonClicked;
            _inputListener.HookKeyboard();
            _inputListener.HookMouse();

            _appListener = new ApplicationListener();
            _appListener.OnApplicationChanged += _Listener_OnApplicationChanged;
            _appListener.HookApplication();

            // Monitors
            _appMonitor = new ApplicationMonitor();
            _keyMonitor = new KeyMonitor();
            _mouseMonitor = new MouseMonitor();
        }

        private void _OnTrackingChecked(object sender, EventArgs e) {
            App.LiveSettings.IsTracking = true;
        }

        private void _OnTrackingUnchecked(object sender, EventArgs e) {
            App.LiveSettings.IsTracking = false;
        }

        private void _OnStyleChanged(object sender, EventArgs e) {
            if (_keyMonitor == null || _appMonitor == null) 
                return;

            if (comboBoxItem_Regular.IsSelected) {
                _log = new StringBuilder(StringUtils.ShrinkLog(_appMonitor.AllRegularLog, _logCharLimit));
                textBox_Log.Text = _log.ToString();
                App.LiveSettings.Style = "Regular";
            } else {
                _log = new StringBuilder(StringUtils.ShrinkLog(_appMonitor.AllRawLog, _logCharLimit));
                textBox_Log.Text = _log.ToString();
                App.LiveSettings.Style = "Raw";
            }
        }

        private void _OnArrowPressed(object sender, EventArgs e) {
            textBox_Log.ScrollToEnd();

            // Update image
            image_Arrow.Source = new BitmapImage(new Uri(@"/resources/images/arrow_active.png", UriKind.Relative));
        }

        private void _OnArrowUnpressed(object sender, EventArgs e) {
            // Update image
            image_Arrow.Source = new BitmapImage(new Uri(@"/resources/images/arrow.png", UriKind.Relative));
        }

        private void _OnTrashCanPressed(object sender, EventArgs e) {
            _appMonitor.Clear();

            // Clear text log
            textBox_Live.Text = "";
            textBox_Log.Text = "";
            _log.Clear();

            // Reset input count 
            _entries = 0;
            _keyStrokes = 0;
            _mouseClicks = 0;

            textBlock_Entries.Text = "0";
            textBlock_KeyStrokes.Text = "0";
            textBlock_MouseClicks.Text = "0";

            // Update image
            image_TrashCan.Source = new BitmapImage(new Uri(@"/resources/images/trashCan_active.png", UriKind.Relative));
        }

        private void _OnTrashCanUnpressed(object sender, EventArgs e) {
            // Update image
            image_TrashCan.Source = new BitmapImage(new Uri(@"/resources/images/trashCan.png", UriKind.Relative));
        }

        private void _Listener_OnKeyPressed(object sender, KeyPressedArgs e) {
            if (toggleButton_Tracking.IsChecked == false || 
                !App.KeyboardKeys.ContainsKey(e.KeyPressed)) {
                return;
            }
               
            // Reset live log once length reaches 84
            if (textBox_Live.Text.Length > 84) {
                textBox_Live.Text = "";
            }

            // Image animation - Raising meaningless event
            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = Button.MouseDownEvent;
            image_Keyboard.RaiseEvent(args);

            // Add pressed key
            _keyMonitor.Add(new KeyInput(e.KeyPressed, e.Modifiers));

            textBox_Live.Text += comboBoxItem_Regular.IsSelected ? 
                _keyMonitor.LastRegularText : _keyMonitor.LastRawText;
            textBlock_KeyStrokes.Text = StringUtils.FormatCount(++_keyStrokes);
        }

        private void _Listener_OnButtonClicked(object sender, ButtonClickedArgs e) {
            if (toggleButton_Tracking.IsChecked == false)
                return;

            // Add clicked mouse button
            _mouseMonitor.Add(e.ButtonClicked);
            textBlock_MouseClicks.Text = StringUtils.FormatCount(++_mouseClicks);

            // Image animation - Raising meaningless event
            MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            args.RoutedEvent = Button.MouseDownEvent;
            image_Mouse.RaiseEvent(args);
        }

        private void _Listener_OnApplicationChanged(object sender, ApplicationChangedArgs e) {
            if (toggleButton_Tracking.IsChecked == false) {
                return;
            }

            // Add previous application and inputs that came with it
            ApplicationInput input = new ApplicationInput(DateTime.Now, _lastAppTitle, _lastAppWindowTitle, _keyMonitor, _mouseMonitor);
            WindowMain.input = input;
            _appMonitor.Add(input);

            // Update database
            WindowMain.UpdateDatabase();

            // Update last application's title
            _lastAppTitle = e.ApplicationTitle;
            _lastAppWindowTitle = e.WindowTitle;

            // Set new "Monitor" objects
            _keyMonitor = new KeyMonitor();
            _mouseMonitor = new MouseMonitor();

            // Remove early entries from live log (char limit: 10,000)
            _log.Append(comboBoxItem_Regular.IsSelected ?
                _appMonitor.LastRegularLog : _appMonitor.LastRawLog);
            _log = new StringBuilder(StringUtils.ShrinkLog(_log.ToString(), _logCharLimit));
            textBox_Log.Text = _log.ToString();

            textBlock_Entries.Text = StringUtils.FormatCount(++_entries);
        }

        public ApplicationInput TryGetLastApplication() {
            return null;
        }
    }
}
