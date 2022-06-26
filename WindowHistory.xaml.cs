namespace InputTracker;

public partial class WindowHistory : UserControl {
    public WindowHistory() {
        InitializeComponent();

        DataContext = new HistoryViewModel(DatabaseDataGrid);
    }
}
