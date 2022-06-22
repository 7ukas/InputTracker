namespace InputTracker;

internal class InputDeviceListener {
    /* Sets the hook for low-level keyboard monitoring */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    /* Sets the hook for low-level mouse monitoring */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    /* Passes the hook information to the next hook */
    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    /* Discards current hook */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    /* ... */
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    /* Retrieves the window with which the user is currently working */
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    /* Retrieves the id of the thread that created the specified window */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // Hooks (IntPtr), IDs
    private IntPtr _keyboardHook = IntPtr.Zero;
    private IntPtr _mouseHook = IntPtr.Zero;
    private const int _KeyboardHookId = 13;
    private const int _MouseHookId = 14;

    // Keyboard flags: WM_KEYDOWN, WM_SYSKEYDOWN
    // Mouse flags: WM_LBUTTONDOWN, WM_RBUTTONDOWN, WM_MBUTTONDOWN
    private List<IntPtr> _WMKeys = new() { (IntPtr)0x0100, (IntPtr)0x0104 };
    private List<IntPtr> _WMButtons = new() { (IntPtr)0x0201, (IntPtr)0x0204, (IntPtr)0x0207 };

    // Data
    private List<Key> _keys;
    private List<MouseButton> _buttons;
    private List<Dictionary<string, bool>> _modifiers;
    Dictionary<string, bool> _modifiersMap = new Dictionary<string, bool>{
            {"Shift", false}, {"CapsLock", false}
    };

    // Delegates: keyboard/mouse callbacks
    // Event Handlers: key/button click
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
    private LowLevelKeyboardProc _keyboardProc;
    private LowLevelMouseProc _mouseProc;
    public event EventHandler<KeyPressedArgs> OnKeyPressed;
    public event EventHandler<ButtonClickedArgs> OnButtonClicked;

    public InputDeviceListener() {
        _keyboardProc = KeyboardHookCallback;
        _mouseProc = MouseHookCallback;

        _keys = new List<Key>();
        _buttons = new List<MouseButton>();
        _modifiers = new List<Dictionary<string, bool>>();
    }

    ~InputDeviceListener() {
        UnhookWindowsHookEx(_keyboardHook);
        UnhookWindowsHookEx(_mouseHook);
    }

    public void HookKeyboard() {
        /* 
         * 13               id of LowLevelKeyboardProc hook procedure 
         * _keyboardProc    LowLevelKeyboardProc callback function
         * hMod             current process module
         * 0                hook procedure is associated with all existing threads 
         *                      running in the same desktop */

        IntPtr hMod = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
        _keyboardHook = SetWindowsHookEx(_KeyboardHookId, _keyboardProc, hMod, 0);
    }

    public void HookMouse() {
        IntPtr hMod = GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
        _mouseHook = SetWindowsHookEx(_MouseHookId, _mouseProc, hMod, 0);
    }

    private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0 && _WMKeys.Contains(wParam)) {
            Key virtualKey = KeyInterop.KeyFromVirtualKey(Marshal.ReadInt32(lParam));

            _modifiersMap["Shift"] = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            _modifiersMap["CapsLock"] = Keyboard.IsKeyToggled(Key.CapsLock);

            OnKeyPressed?.Invoke(this, new KeyPressedArgs(virtualKey, _modifiersMap));

            _keys.Add(virtualKey);
            _modifiers.Add(new Dictionary<string, bool>(_modifiersMap));
        }

        return CallNextHookEx(_keyboardHook, nCode, wParam, lParam);
    }

    private IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode >= 0 && _WMButtons.Contains(wParam)) {
            MouseButton button;

            if (wParam == _WMButtons[0])
                button = MouseButton.Left;
            else if (wParam == _WMButtons[1])
                button = MouseButton.Right;
            else
                button = MouseButton.Middle;

            _buttons.Add(button);

            OnButtonClicked?.Invoke(this, new ButtonClickedArgs(button));
        }

        return CallNextHookEx(_mouseHook, nCode, wParam, lParam);
    }
}
