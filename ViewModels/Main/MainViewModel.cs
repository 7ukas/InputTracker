namespace InputTracker;

internal class MainViewModel : BaseViewModel {
    // Commands
    public ICommand DisplayWindowCommand { get; set; }

    // Windows visibility properties
    public bool IsOverviewWindowDisplayed { get; set; } = true;
    public bool IsLiveWindowDisplayed { get; set; } = false;
    public bool IsHistoryWindowDisplayed { get; set; } = false;

    private readonly Grid _windowGrid;

    public MainViewModel() {
        DisplayWindowCommand = new RelayCommand(DisplayWindow);
    }

    public MainViewModel(Grid windowGrid) : this() {
        _windowGrid = windowGrid;
    }

    public void DisplayWindow(object obj) {
        string title = (string)obj;
        
        if (string.Equals(title, "Overview", StringComparison.OrdinalIgnoreCase)) {
            _windowGrid.Children[WindowIndex.Overview].Visibility = Visibility.Visible;
            _windowGrid.Children[WindowIndex.Live].Visibility = Visibility.Collapsed;
            _windowGrid.Children[WindowIndex.History].Visibility = Visibility.Collapsed;

            IsOverviewWindowDisplayed = true;
            IsLiveWindowDisplayed = false;
            IsHistoryWindowDisplayed = false;
        } else if (string.Equals(title, "Live", StringComparison.OrdinalIgnoreCase)) {
            _windowGrid.Children[WindowIndex.Overview].Visibility = Visibility.Collapsed;
            _windowGrid.Children[WindowIndex.Live].Visibility = Visibility.Visible;
            _windowGrid.Children[WindowIndex.History].Visibility = Visibility.Collapsed;

            IsOverviewWindowDisplayed = false;
            IsLiveWindowDisplayed = true;
            IsHistoryWindowDisplayed = false;
        } else if (string.Equals(title, "History", StringComparison.OrdinalIgnoreCase)) {
            _windowGrid.Children[WindowIndex.Overview].Visibility = Visibility.Collapsed;
            _windowGrid.Children[WindowIndex.Live].Visibility = Visibility.Collapsed;
            _windowGrid.Children[WindowIndex.History].Visibility = Visibility.Visible;

            IsOverviewWindowDisplayed = false;
            IsLiveWindowDisplayed = false;
            IsHistoryWindowDisplayed = true;
        }
    }

}
