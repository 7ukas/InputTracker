namespace InputTracker;

internal class ApplicationInputStructure {
    public string RegularLog { get { return _regularLog.ToString(); } }
    public string RawLog { get { return _rawLog.ToString(); } }
    public string LastRegularLog { get { return _lastInput.RegularLog; } }
    public string LastRawLog { get { return _lastInput.RegularLog; } }
    public int Count { get; private set; }

    private Stack<ApplicationInput> _applicationInputs;
    private ApplicationInput _lastInput;

    private StringBuilder _regularLog;
    private StringBuilder _rawLog;

    public ApplicationInputStructure() {
        _applicationInputs = new Stack<ApplicationInput>();

        _regularLog = new StringBuilder();
        _rawLog = new StringBuilder();
    }

    public void Add(ApplicationInput applicationInput) {
        _applicationInputs.Push(applicationInput);
        _lastInput = applicationInput;

        _regularLog.Append(_lastInput.RegularLog);
        _rawLog.Append(_lastInput.RawLog);

        Count++;
    }

    public void RemoveLast() {
        if (Count > 0) {
            ApplicationInput applicationInput = _applicationInputs.Pop();

            _lastInput = _applicationInputs.Peek();

            _regularLog.Remove(
                _regularLog.Length - _lastInput.RegularLog.Length,
                _lastInput.RegularLog.Length
            );

            _rawLog.Remove(
                _rawLog.Length - _lastInput.RawLog.Length,
                _lastInput.RawLog.Length
            );

            Count--;
        }
    }

    public bool Empty() {
        return Count == 0;
    }

    public void Clear() {
        _applicationInputs.Clear();
        _lastInput = null;

        _regularLog.Clear();
        _rawLog.Clear();
    }
}
