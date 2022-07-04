namespace InputTracker;

public partial class WindowMain : Window {
    public static new string Title = "Input Tracker";

    private readonly WindowOverview _windowOverview = new WindowOverview();
    private readonly WindowLive _windowLive = new WindowLive();
    private readonly WindowHistory _windowHistory = new WindowHistory();

    public WindowMain() {
        InitializeComponent();

        WindowPanel.Children.Add(_windowOverview);
        WindowPanel.Children.Add(_windowLive);
        WindowPanel.Children.Add(_windowHistory);
        WindowPanel.Children[WindowIndex.Live].Visibility = Visibility.Hidden;
        WindowPanel.Children[WindowIndex.History].Visibility = Visibility.Hidden;

        DataContext = new MainViewModel(WindowPanel);

        this.PreviewKeyDown += _Window_PreviewKeyDown;
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
    }

    private void _Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        // Disable keys functions on main window
        if (new Key[] { Key.Tab, Key.Space, Key.Enter }.Contains(e.Key)) {
            e.Handled = true;
        }
    }
}
