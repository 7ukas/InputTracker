namespace InputTracker;

internal class KeyInputStructure {
    public string RegularText { get { return _regularText.ToString(); } }
    public string RawText { get { return _rawText.ToString(); } }
    public string LastRegularText { get { return _lastInput.RegularText; } }
    public string LastRawText { get { return _lastInput.RawText; } }
    public int Count { get; private set; }

    private Stack<KeyInput> _keyInputs;
    private KeyInput _lastInput;

    private StringBuilder _regularText;
    private StringBuilder _rawText;

    public KeyInputStructure() {
        _keyInputs = new Stack<KeyInput>();

        _regularText = new StringBuilder();
        _rawText = new StringBuilder();
    }

    public void Add(KeyInput keyInput) {
        _keyInputs.Push(keyInput);
        _lastInput = keyInput;

        if (keyInput.IsBackSpace && _regularText.Length > 0) {
           _regularText.Remove(_regularText.Length - 1, 1);
        } else {
            _regularText.Append(keyInput.RegularText);
            _rawText.Append(keyInput.RawText);
        }

        Count++;
    }

    public void RemoveLast() {
        if (Count > 0) {
            KeyInput keyInput = _keyInputs.Pop();

            _lastInput = _keyInputs.Peek();

            _regularText.Remove(
                _regularText.Length - keyInput.RegularText.Length,
                keyInput.RegularText.Length
            );

            _rawText.Remove(
                 _rawText.Length - keyInput.RawText.Length,
                 keyInput.RawText.Length
            );

            Count--;
        }
    }

    public bool Empty() {
        return Count == 0;
    }

    public void Clear() {
        _keyInputs.Clear();
        _lastInput = null;

        _regularText.Clear();
        _rawText.Clear();
    }
}
