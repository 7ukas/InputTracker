namespace InputTracker;

public partial class WindowOverview : UserControl {
    public WindowOverview() {
        InitializeComponent();

        DataContext = new OverviewViewModel() {
            TimerVM =  new TimerViewModel(),
            StatisticsVM = new StatisticsViewModel()
        };
    }
}
