namespace InputTracker;

internal class KeyPressedArgs : EventArgs {
    public Key KeyPressed { get; private set; }
    public Dictionary<string, bool> ToggleKeys { get; private set; }
    public Dictionary<string, bool> ModifierKeys { get; private set; }

    public KeyPressedArgs(Key key, Dictionary<string, bool> toggleKeys, Dictionary<string, bool> modifierKeys) {
        KeyPressed = key;
        ToggleKeys = new Dictionary<string, bool>(toggleKeys);
        ModifierKeys = new Dictionary<string, bool>(modifierKeys);
    }
}
