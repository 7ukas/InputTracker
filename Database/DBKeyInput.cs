namespace InputTracker;

internal class DBKeyInput {
    public Key Key { get; private set; }
    public string RawText { get; private set; }
    public string RegularText { get; private set; }
    public string ShiftText { get; private set; }
}
