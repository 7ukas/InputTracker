namespace InputTracker;

internal static class DBKeyInputDictionary {
    private static Dictionary<Key, DBKeyInput> _DatabaseKeyInputs =
            DatabaseController.GetKeyboardKeys()
            .ToDictionary(x => x.Key, x => x);

    public static bool ContainsKey(Key key) {
        return _DatabaseKeyInputs.ContainsKey(key);
    }

    public static DBKeyInput TryGetKeyInput(Key key, out DBKeyInput databaseKeyInput) {
        _DatabaseKeyInputs.TryGetValue(key, out databaseKeyInput);
        return databaseKeyInput;
    }
}
