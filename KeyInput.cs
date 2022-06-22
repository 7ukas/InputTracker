namespace InputTracker;

internal class KeyInput {
    private Key _key = 0;
    private Dictionary<string, bool> _modifiers;
    private string _rawText;
    private string _regularText;

    public KeyInput(Key key, Dictionary<string, bool> modifiers) {
        if (App.KeyboardKeys.ContainsKey(key)) {
            _key = key;
        }

        _modifiers = new Dictionary<string, bool>(modifiers);

        _rawText = App.KeyboardKeys[_key].RawText;
        _regularText = App.KeyboardKeys[_key].RegularText;
        
        // Adjust regular text depending on Shift/CapsLock status
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
            bool value = false;
            _modifiers.TryGetValue("Shift", out value);

            return value;
        }
    }

    private bool _IsCapsLockToggled {
        get {
            bool value = false;
            _modifiers.TryGetValue("CapsLock", out value);

            return value;
        }
    }
}
