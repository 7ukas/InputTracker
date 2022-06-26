namespace InputTracker;

internal class RelayCommand : ICommand {
    public event EventHandler CanExecuteChanged = (sender, e) => { };
    private Action<object> _action;

    public RelayCommand(Action<object> action) {
        _action = action;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) {
        _action(parameter);
    }
}
