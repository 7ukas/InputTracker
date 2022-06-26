namespace InputTracker;

public partial class WindowLive : UserControl {
    public WindowLive() {
        InitializeComponent();

        DataContext = new LiveViewModel() {
            TimerVM = new TimerViewModel(),
            InputVM = new InputViewModel(KeyboardImage, MouseImage)
        };
    }
}
