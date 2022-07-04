namespace InputTracker;

internal class MainViewModel : BaseViewModel {
    public ICommand DisplayWindowCommand { get; set; }

    public bool IsOverviewWindowDisplayed { get; set; } = true;
    public bool IsLiveWindowDisplayed { get; set; } = false;
    public bool IsHistoryWindowDisplayed { get; set; } = false;

    private readonly Grid _windowPanel;

    public MainViewModel() {
        DisplayWindowCommand = new RelayCommand(DisplayWindow);
    }

    public MainViewModel(Grid windowPanel) : this() {
        _windowPanel = windowPanel;
    }

    public void DisplayWindow(object obj) {
        string title = (string)obj;
        
        if (string.Equals(title, "Overview", StringComparison.OrdinalIgnoreCase)) {
            _windowPanel.Children[WindowIndex.Overview].Visibility = Visibility.Visible;
            _windowPanel.Children[WindowIndex.Live].Visibility = Visibility.Collapsed;
            _windowPanel.Children[WindowIndex.History].Visibility = Visibility.Collapsed;

            IsOverviewWindowDisplayed = true;
            IsLiveWindowDisplayed = false;
            IsHistoryWindowDisplayed = false;
        } else if (string.Equals(title, "Live", StringComparison.OrdinalIgnoreCase)) {
            _windowPanel.Children[WindowIndex.Overview].Visibility = Visibility.Collapsed;
            _windowPanel.Children[WindowIndex.Live].Visibility = Visibility.Visible;
            _windowPanel.Children[WindowIndex.History].Visibility = Visibility.Collapsed;

            IsOverviewWindowDisplayed = false;
            IsLiveWindowDisplayed = true;
            IsHistoryWindowDisplayed = false;
        } else if (string.Equals(title, "History", StringComparison.OrdinalIgnoreCase)) {
            _windowPanel.Children[WindowIndex.Overview].Visibility = Visibility.Collapsed;
            _windowPanel.Children[WindowIndex.Live].Visibility = Visibility.Collapsed;
            _windowPanel.Children[WindowIndex.History].Visibility = Visibility.Visible;

            IsOverviewWindowDisplayed = false;
            IsLiveWindowDisplayed = false;
            IsHistoryWindowDisplayed = true;
        }
    }

}
