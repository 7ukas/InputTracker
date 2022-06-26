namespace InputTracker;

public partial class WindowMain : Window {
    public static new string Title = "Input Tracker";

    private static WindowOverview _windowOverview = new WindowOverview();
    private static WindowLive _windowLive = new WindowLive();

    // Database
    private static WindowHistory _windowHistory = new WindowHistory();

    public WindowMain() {
        //InitializeComponent();
        this.PreviewKeyDown += _Window_PreviewKeyDown;
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
    }

    private void _Window_Loaded(object sender, RoutedEventArgs e) {
        grid_Window.Children.Clear();
        grid_Window.Children.Add(_windowOverview);
        grid_Window.Children.Add(_windowLive);
        grid_Window.Children.Add(_windowHistory);
        grid_Window.Children[1].Visibility = Visibility.Hidden;
        grid_Window.Children[2].Visibility = Visibility.Hidden;
    }

    private void _Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        // Disable keys functions on main window
        if (new Key[] { Key.Tab, Key.Space, Key.Enter }.Contains(e.Key)) {
            e.Handled = true;
        }
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
        //UpdateDatabaseAsync();
    }
}
