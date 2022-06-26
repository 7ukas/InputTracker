namespace InputTracker; 

internal class ApplicationListener {
    /* Sets an event hook function for a range of events */
    [DllImport("user32.dll")]
    private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, _WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

    /* Discards current hook */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    /* Retrieves the window with which the user is currently working */
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    /* Retrieves the id of the thread that created the specified window */
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // Flags
    private const uint WinEventOutOfContext = 0;
    private const uint EventSystemForeground = 3; // The foreground window has changed. 

    public ApplicationListener() { }

    ~ApplicationListener() {
        UnhookWindowsHookEx(IntPtr.Zero);
    }

    // Delegate: callback function
    // Event Handler: application changed
    private delegate void _WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    private _WinEventDelegate _winEvent;
    public event EventHandler<ApplicationChangedArgs> OnApplicationChanged;

    public void HookApplication() {
        _winEvent = new _WinEventDelegate(WinEventProc);
        SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, _winEvent, 0, 0, WinEventOutOfContext);
    }

    // Callback function
    private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
        uint procId = 0;

        GetWindowThreadProcessId(GetForegroundWindow(), out procId);
        Process process = Process.GetProcessById((int)procId);

        // Window title
        string windowTitle = !string.IsNullOrEmpty(process.MainWindowTitle) ?
            process.MainWindowTitle : "";

        // Application title
        ProcessModule module = null;
        string applicationTitle = "";

        try {
            module = process.MainModule;
            string fileDescription = module.FileVersionInfo.FileDescription;

            applicationTitle = !string.IsNullOrEmpty(fileDescription) ?
            fileDescription : StringUtils.RemovePath(module.FileVersionInfo.FileName);
        } catch (Exception) {
        } finally {
            if (string.IsNullOrEmpty(applicationTitle)) {
                applicationTitle = "";
            }
        }

        OnApplicationChanged?.Invoke(this, new ApplicationChangedArgs(applicationTitle, windowTitle));
    }
}
