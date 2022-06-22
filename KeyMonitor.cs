namespace InputTracker;

internal class KeyMonitor : IMonitor<KeyInput> {
    private List<KeyInput> _inputs;

    public KeyMonitor() {
        _inputs = new List<KeyInput>();
    }

    public string LastRawText {
        get { return _inputs.Last().RawText; }
    }

    public string RawText {
        get {
            return string.Join("", _inputs.Select(i => i.RawText).ToList());
        }
    }

    public string LastRegularText {
        get { return _inputs.Last().RegularText; }
    }

    public string RegularText {
        get {
            return string.Join("", _inputs.Select(i => i.RegularText).ToList());
        }
    }

    public void Add(KeyInput input) {
        Key key = input.Key;

        // Backspace
        int regLength = input.RegularText.Length;
        if (key == Key.Back && regLength > 0) {
            input.RegularText = input.RegularText.Remove(regLength - 1);
        }

        _inputs.Add(input);
    }

    public KeyInput Last() {
        return _inputs.Last();
    }

    public int Count() {
        return _inputs.Count();
    }

    public void Remove(KeyInput input) {
        _inputs.Remove(input);
    }

    public void Clear() {
        _inputs.Clear();
    }
}
