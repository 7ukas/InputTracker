namespace InputTracker;

public partial class WindowMain : Window {
    public static new string Title = "Input Tracker";

    // Windows
    private readonly WindowOverview _windowOverview = new WindowOverview();
    private readonly WindowLive _windowLive = new WindowLive();
    private readonly WindowHistory _windowHistory = new WindowHistory();

    public WindowMain() {
        InitializeComponent();

        // Adding windows to Grid, setting visibility
        WindowGrid.Children.Add(_windowOverview);
        WindowGrid.Children.Add(_windowLive);
        WindowGrid.Children.Add(_windowHistory);
        WindowGrid.Children[WindowIndex.Live].Visibility = Visibility.Hidden;
        WindowGrid.Children[WindowIndex.History].Visibility = Visibility.Hidden;

        DataContext = new MainViewModel(WindowGrid);

        // Disable keys functions on main window
        this.PreviewKeyDown += _Window_PreviewKeyDown;
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
    }

    private void _Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        if (new Key[] { Key.Tab, Key.Space, Key.Enter }.Contains(e.Key)) {
            e.Handled = true;
        }
    }
}
