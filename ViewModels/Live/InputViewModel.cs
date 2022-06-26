namespace InputTracker;

internal class InputViewModel : BaseViewModel {
    // Commands
    public ICommand ChangeTextStyleCommand { get; init; }
    public ICommand ClearLogCommand { get; init; }

    // Properties
    public ObservableCollection<string> Styles { get; init; }

    public bool IsTracking { get; set; } = false;
    public string SelectedStyle { get; set; } = "Regular";
    public string BriefLiveLog { get; set; } = "";
    public string LiveLog { get; set; } = "";
    public string Entries { get; set; } = "0";
    public string KeyStrokes { get; set; } = "0";
    public string MouseClicks { get; set; } = "0";

    // Listeners
    private readonly InputDeviceListener _inputListener;
    private readonly ApplicationListener _applicationListener;

    // Monitors
    private readonly ApplicationInputStructure _applicationInputStructure;
    private KeyInputStructure _keyInputStructure;
    private ButtonInputStructure _buttonInputStructure;

    // Applications
    private string _lastApplicationTitle = WindowMain.Title;
    private string _lastApplicationWindowTitle = WindowMain.Title;

    // Activity Images
    private readonly Image _keyboardImage;
    private readonly Image _mouseImage;

    // Log
    private int _logCharLimit = 10000;
    private string _liveRegularLog = "";
    private string _liveRawLog = "";

    // Count
    private int _entries = 0;
    private int _keyStrokes = 0;
    private int _mouseClicks = 0;

    public InputViewModel(Image keyboardImage, Image mouseImage) {
        ChangeTextStyleCommand = new RelayCommand(ChangeTextStyle);
        ClearLogCommand = new RelayCommand(ClearLog);

        Styles = new ObservableCollection<string>() {
            "Regular",
            "Raw"
        };

        _inputListener = new InputDeviceListener();
        _inputListener.OnKeyPressed += _OnKeyPressedEvent;
        _inputListener.OnButtonClicked += _OnButtonClickedEvent;
        _inputListener.HookKeyboard();
        _inputListener.HookMouse();

        _applicationListener = new ApplicationListener();
        _applicationListener.OnApplicationChanged += _OnApplicationChangedEvent;
        _applicationListener.HookApplication();

        _applicationInputStructure = new ApplicationInputStructure();
        _keyInputStructure = new KeyInputStructure();
        _buttonInputStructure = new ButtonInputStructure();

        _keyboardImage = keyboardImage;
        _mouseImage = mouseImage;
    }

    public void ChangeTextStyle(object obj) {
        if (SelectedStyle == "Regular") {
            LiveLog = _liveRegularLog;
        } else if (SelectedStyle == "Raw") {
            LiveLog = _liveRawLog;
        }
    }

    public void ClearLog(object obj) {
        _applicationInputStructure.Clear();

        BriefLiveLog = LiveLog = _liveRegularLog = _liveRawLog = "";
        Entries = KeyStrokes = MouseClicks = "0";
        _entries = _keyStrokes = _mouseClicks = 0;
    }

    private void _OnKeyPressedEvent(object sender, KeyPressedArgs e) {
        /* Exit event if:
         * - Tracking is off
         * - Database available keys table doesn't contain given key 
         * - Any of modifier keys being held */
        if (!IsTracking || !DBKeyInputDictionary.ContainsKey(e.KeyPressed) || _AnyModifierKeysDown(e)) {
            return;
        }

        // Reset live log once length reaches 84
        if (BriefLiveLog.Length > 84) {
            BriefLiveLog = "";
        }

        // Add pressed key
        _keyInputStructure.Add(new KeyInput(e.KeyPressed, e.ToggleKeys, e.ModifierKeys));

        BriefLiveLog += SelectedStyle == "Regular" ?
            _keyInputStructure.LastRegularText : _keyInputStructure.LastRawText;
        KeyStrokes = StringUtils.FormatCount(++_keyStrokes);

        // Image animation - Raising meaningless event
        MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
        args.RoutedEvent = Button.MouseDownEvent;
        _keyboardImage.RaiseEvent(args);
    }

    private bool _AnyModifierKeysDown(KeyPressedArgs e) {
        Key[] modifierKeys = {
            Key.LeftShift, Key.RightShift,
            Key.LeftAlt, Key.RightAlt,
            Key.LeftCtrl, Key.RightCtrl
        };
        Key key = e.KeyPressed;

        return modifierKeys.Any(m => {
            if (key.Equals(m)) {
                bool modifierDown = false;
                string keyName = m.ToString().Replace("Left", "").Replace("Right", "");

                e.ModifierKeys.TryGetValue(keyName, out modifierDown);
                return modifierDown;
            } else return false;
        });
    }

    private void _OnButtonClickedEvent(object sender, ButtonClickedArgs e) {
        if (!IsTracking) {
            return;
        }

        // Add clicked mouse button
        _buttonInputStructure.Add(e.ButtonClicked);
        MouseClicks = StringUtils.FormatCount(++_mouseClicks);

        // Image animation - Raising meaningless event
        MouseButtonEventArgs args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
        args.RoutedEvent = Button.MouseDownEvent;
        _mouseImage.RaiseEvent(args);
    }

    private void _OnApplicationChangedEvent(object sender, ApplicationChangedArgs e) {
        if (!IsTracking) {
            return;
        }

        // Add previous application and inputs that came with it
        ApplicationInput input = new ApplicationInput(
            DateTime.Now, _lastApplicationTitle, _lastApplicationWindowTitle, 
            _keyInputStructure, _buttonInputStructure
        );
        _applicationInputStructure.Add(input);

        // Update database
        DatabaseController.UpdateDatabaseAsync(input);

        // Update last application's title
        _lastApplicationTitle = e.ApplicationTitle;
        _lastApplicationWindowTitle = e.WindowTitle;

        // Set new "Monitor" objects
        _keyInputStructure = new KeyInputStructure();
        _buttonInputStructure = new ButtonInputStructure();

        // Remove early entries from live log (char limit: 10,000)
        _liveRegularLog = StringUtils.ShrinkLog(_applicationInputStructure.RegularLog, _logCharLimit);
        _liveRawLog = StringUtils.ShrinkLog(_applicationInputStructure.RawLog, _logCharLimit);
        LiveLog = SelectedStyle == "Regular" ? _liveRegularLog : _liveRawLog;

        Entries = StringUtils.FormatCount(++_entries);
    }
}
