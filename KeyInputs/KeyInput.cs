namespace InputTracker;

internal class KeyInput {
    public Key Key { get; init; } = Key.None;
    public bool IsBackSpace { get; init; } = false;
    public string RegularText { get; init; } = "";
    public string RawText { get; init; } = "";

    private readonly DBKeyInput _databaseKeyInput;
    private readonly Dictionary<string, bool> _toggleKeys;
    private readonly Dictionary<string, bool> _modifierKeys;

    public KeyInput(Key key, Dictionary<string, bool> toggleKeys, Dictionary<string, bool> modifierKeys) {
        DBKeyInputDictionary.TryGetKeyInput(key, out _databaseKeyInput);

        if (_databaseKeyInput != null) {
            Key = key;

            IsBackSpace = key == Key.Back;

            _toggleKeys = new Dictionary<string, bool>(toggleKeys);
            _modifierKeys = new Dictionary<string, bool>(modifierKeys);

            RegularText = !_ApplyTextOnShift() ? _databaseKeyInput.RegularText : _databaseKeyInput.ShiftText;
            RawText = _databaseKeyInput.RawText;
        }
    }

    private bool _ApplyTextOnShift() {
        bool keyIsLetter = _databaseKeyInput.RegularText.Length == 1 &&
                char.IsLetter(_databaseKeyInput.RegularText[0]);

        bool shiftDown;
        _modifierKeys.TryGetValue("Shift", out shiftDown);

        bool capsLockToggled;
        _toggleKeys.TryGetValue("CapsLock", out capsLockToggled);

        if (shiftDown) {
            if (!capsLockToggled && keyIsLetter || !keyIsLetter) {
                return true;
            }
        } else if (capsLockToggled && !shiftDown && keyIsLetter) {
            return true;
        }

        return false;
    }
}
