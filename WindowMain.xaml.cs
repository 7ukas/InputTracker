namespace InputTracker;

public partial class WindowMain : Window {
    public static new string Title = "Input Tracker";

    private static WindowOverview _windowOverview = new WindowOverview();
    private static WindowLive _windowLive = new WindowLive();

    // Database
    internal static ApplicationInput _input;
    private static WindowDatabase _windowDatabase = new WindowDatabase();
    private static DateTime _lastUpdate = DateTime.Now;

    // Timer
    private DispatcherTimer _timer;

    public WindowMain() {
        InitializeComponent();
        this.PreviewKeyDown += _Window_PreviewKeyDown;
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
    }

    private void _Window_Loaded(object sender, RoutedEventArgs e) {
        grid_Window.Children.Clear();
        grid_Window.Children.Add(_windowOverview);
        grid_Window.Children.Add(_windowLive);
        grid_Window.Children.Add(_windowDatabase);
        grid_Window.Children[1].Visibility = Visibility.Hidden;
        grid_Window.Children[2].Visibility = Visibility.Hidden;

        // Timer
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _LocalTime_Event;
        _timer.Start();
    }

    private void _Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        // Disable keys functions on main window
        if (new Key[] { Key.Tab, Key.Space, Key.Enter }.Contains(e.Key)) {
            e.Handled = true;
        }
    }

    private void _LocalTime_Event(object sender, EventArgs e) {
        DateTime dateTime = DateTime.Now;

        // WindowLive
        _windowLive.textBlock_Date.Text = dateTime.ToString("yyyy, MMMM dd, dddd");
        _windowLive.textBlock_Time.Text = dateTime.ToString("HH:mm:ss");

        // WindowOverview: Periodically update database
        TimeSpan timeSpan = dateTime - _lastUpdate;
        int seconds = (int)timeSpan.TotalSeconds;

        if (seconds > 0) {
            _windowOverview.OnDatabasePassive(StringUtils.FormatLastUpdated(timeSpan));
        }
    }

    public async static void UpdateDatabaseAsync() {
        await Task.Run(() => { DatabaseController.Update(_input); });
        _input = null;
        _windowOverview.OnDatabaseUpdated();
        _lastUpdate = DateTime.Now;
    }

    public static void ClearDatabase() {
        DatabaseController.ClearDatabase();
        _input = null;

        App.LiveSettings.IsTracking = false;
        _windowLive.toggleButton_Tracking.IsChecked = App.LiveSettings.IsTracking;

        // Clear Applications Top 5
        _windowOverview.textBlock_1st_Application.Text = "";
        _windowOverview.textBlock_1st_KeyStrokes.Text = "";
        _windowOverview.textBlock_1st_MouseClicks.Text = "";
        _windowOverview.textBlock_2nd_Application.Text = "";
        _windowOverview.textBlock_2nd_KeyStrokes.Text = "";
        _windowOverview.textBlock_2nd_MouseClicks.Text = "";
        _windowOverview.textBlock_3rd_Application.Text = "";
        _windowOverview.textBlock_3rd_KeyStrokes.Text = "";
        _windowOverview.textBlock_3rd_MouseClicks.Text = "";
        _windowOverview.textBlock_4th_Application.Text = "";
        _windowOverview.textBlock_4th_KeyStrokes.Text = "";
        _windowOverview.textBlock_4th_MouseClicks.Text = "";
        _windowOverview.textBlock_5th_Application.Text = "";
        _windowOverview.textBlock_5th_KeyStrokes.Text = "";
        _windowOverview.textBlock_5th_MouseClicks.Text = "";

        UpdateDatabaseAsync();
    }

    private void _OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e) {
        if (tabItem_Overview == tabControl_Navigation.SelectedItem as TabItem) {
            if (grid_Window.Children.Count > 0) {
                grid_Window.Children[0].Visibility = Visibility.Visible;
                grid_Window.Children[1].Visibility = Visibility.Hidden;
                grid_Window.Children[2].Visibility = Visibility.Hidden;
            }
        } else if (tabItem_Live == tabControl_Navigation.SelectedItem as TabItem) {
            if (grid_Window.Children.Count > 0) {
                grid_Window.Children[0].Visibility = Visibility.Hidden;
                grid_Window.Children[1].Visibility = Visibility.Visible;
                grid_Window.Children[2].Visibility = Visibility.Hidden;
            }
        } else if (tabItem_History == tabControl_Navigation.SelectedItem as TabItem) {
            if (grid_Window.Children.Count > 0) {
                grid_Window.Children[0].Visibility = Visibility.Hidden;
                grid_Window.Children[1].Visibility = Visibility.Hidden;
                grid_Window.Children[2].Visibility = Visibility.Visible;
            }
        }
    }

    private void _Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
        UpdateDatabaseAsync();
    }
}
