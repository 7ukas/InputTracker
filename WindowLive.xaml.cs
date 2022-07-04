namespace InputTracker;

public partial class WindowLive : UserControl {
    public WindowLive() {
        InitializeComponent();

        DataContext = new LiveViewModel(KeyboardImage, MouseImage);
    }
}
