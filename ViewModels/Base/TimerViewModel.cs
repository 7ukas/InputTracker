namespace InputTracker;

internal class TimerViewModel : BaseViewModel {
    public string CurrentDate { get; private set; }
    public string CurrentTime { get; private set; }
    public string LastUpdated { get; private set; }

    private DispatcherTimer _timer = new DispatcherTimer();
    private const int _intervalMilliseconds = 600;
    private double _lastUpdatedSeconds = 0;

    private ApplicationListener _applicationListener;

    public TimerViewModel() {
        _timer.Interval = TimeSpan.FromMilliseconds(_intervalMilliseconds);
        _timer.Tick += _UpdateTimeEvent;
        _timer.Start();

        _applicationListener = new ApplicationListener();
        _applicationListener.OnApplicationChanged += _OnApplicationChangedEvent;
        _applicationListener.HookApplication();
    }

    private void _UpdateTimeEvent(object sender, EventArgs e) {
        DateTime now = DateTime.Now;

        CurrentDate = now.ToString("yyyy, MMMM dd, dddd");
        CurrentTime = now.ToString("HH:mm:ss");

        _lastUpdatedSeconds += ((double)_intervalMilliseconds / 1000.0);
        LastUpdated = StringUtils.FormatLastUpdated(TimeSpan.FromSeconds(_lastUpdatedSeconds));
    }

    private void _OnApplicationChangedEvent(object sender, ApplicationChangedArgs e) {
        _lastUpdatedSeconds = 0.0;
        LastUpdated = StringUtils.FormatLastUpdated(TimeSpan.FromSeconds(_lastUpdatedSeconds));
    }
}
