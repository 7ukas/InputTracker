namespace InputTracker;

internal class KeyInput {
    private Key _key = 0;
    private Dictionary<string, bool> _toggleKeys;
    private Dictionary<string, bool> _modifierKeys;
    private string _rawText;
    private string _regularText;

    public KeyInput(Key key, Dictionary<string, bool> toggleKeys, Dictionary<string, bool> modifierKeys) {
        if (App.KeyboardKeys.ContainsKey(key)) {
            _key = key;
        }

        _toggleKeys = new Dictionary<string, bool>(toggleKeys);
        _modifierKeys = new Dictionary<string, bool>(modifierKeys);

        _rawText = App.KeyboardKeys[_key].RawText;
        _regularText = App.KeyboardKeys[_key].RegularText;
        
        // Adjust regular text case depending on Shift/CapsLock status
        if (_regularText.Length > 0) {
            bool keyIsLetter = _regularText.Length == 1 && char.IsLetter(_regularText[0]);

            if (_IsShiftDown) {
                if (!_IsCapsLockToggled && keyIsLetter || !keyIsLetter) {
                    _regularText = App.KeyboardKeys[_key].ShiftText;
                }
            } else if (_IsCapsLockToggled && !_IsShiftDown && keyIsLetter) {
                _regularText = App.KeyboardKeys[_key].ShiftText;
            }
        }
    }

    public Key Key {
        get { return _key; }
        set { _key = value; }
    }

    public string RawText {
        get { return _rawText; }
        set { _rawText = value; }
    }

    public string RegularText {
        get { return _regularText; }
        set { _regularText = value; }
    }

    private bool _IsShiftDown {
        get {
            bool shiftOn = false;
            _modifierKeys.TryGetValue("Shift", out shiftOn);

            return shiftOn;
        }
    }

    private bool _IsCapsLockToggled {
        get {
            bool capsLockOn = false;
            _toggleKeys.TryGetValue("CapsLock", out capsLockOn);

            return capsLockOn;
        }
    }
}
