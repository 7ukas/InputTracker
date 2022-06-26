namespace InputTracker;

[AddINotifyPropertyChangedInterface]
internal class BaseViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
}
