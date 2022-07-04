﻿namespace InputTracker;

internal class OverviewViewModel : BaseViewModel {
    // Commands
    public ICommand CustomizeStatisticsCommand { get; init; }
    public ICommand SortApplicationsTableCommand { get; init; }

    // Static property
    public static event PropertyChangedEventHandler StaticPropertyChanged;
    public static void NotifyStaticPropertyChanged([CallerMemberName] string propertyName = null) {
        StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
    }

    public static string LastUpdated {
        get { return _lastUpdated; }
        set {
            _lastUpdated = value;
            NotifyStaticPropertyChanged();
        }
    }

    // Properties
    public ObservableCollection<BitmapImage> BitmapImages { get; set; }
    public ObservableCollection<string> ApplicationsTableTitles { get; set; }
    public ObservableCollection<string> ApplicationsTableKeyStrokes { get; set; }
    public ObservableCollection<string> ApplicationsTableMouseClicks { get; set; }
    public ObservableCollection<string> CustomStatistics { get; set; }
    public ObservableCollection<string> YearStatistics { get; set; }
    public ObservableCollection<string> MonthStatistics { get; set; }
    public ObservableCollection<string> WeekStatistics { get; set; }
    private bool _IsTitleOrderAscending { get; set; } = true;
    private bool _IsKeyStrokesOrderAscending { get; set; } = false;
    private bool _IsMouseClicksOrderAscending { get; set; } = false;
    public string SelectedStatisticsTimeSpan { get; set; } = "Today";

    // Local variables
    private static string _lastUpdated = "";
    private ApplicationListener _applicationListener;
    private List<DBStatistics> _dbStatistics = new();
    private List<dbApplications> _dbApplications = new();
    private List<string> _totalStatistics;
    private List<string> _todayStatistics;
    private readonly List<string> _defaultStatistics = new() { "0", "0", "0" };
    private string _applicationsTableSortOrder = "Title";

    public OverviewViewModel() {
        CustomizeStatisticsCommand = new RelayCommand(CustomizeStatistics);
        SortApplicationsTableCommand = new RelayCommand(SortApplicationsTable);

        BitmapImages = new ObservableCollection<BitmapImage>() {
            new BitmapImage(new Uri("pack://application:,,,/InputTracker;component/Resources/Images/console_white.png")),
            new BitmapImage(new Uri("pack://application:,,,/InputTracker;component/Resources/Images/keyboard_white.png")),
            new BitmapImage(new Uri("pack://application:,,,/InputTracker;component/Resources/Images/mouse_white.png")),
        };

        CustomStatistics = new ObservableCollection<string>(_defaultStatistics);
        YearStatistics = new ObservableCollection<string>(_defaultStatistics);
        MonthStatistics = new ObservableCollection<string>(_defaultStatistics);
        WeekStatistics = new ObservableCollection<string>(_defaultStatistics);

        _applicationListener = new ApplicationListener();
        _applicationListener.OnApplicationChanged += _OnApplicationChangedEvent;
        _applicationListener.HookApplication();

        _dbStatistics = new List<DBStatistics>();

        _dbApplications = DatabaseController.GetApplications().
            OrderBy(x => x.Title).ThenBy(x => x.KeyStrokes).ToList();

        _UpdateApplicationsTable(_dbApplications);

        _totalStatistics = new List<string>(_defaultStatistics);
        _todayStatistics = new List<string>(_defaultStatistics);
    }

    public void CustomizeStatistics(object obj) {
        if (SelectedStatisticsTimeSpan == "Today") {
            CustomStatistics = new ObservableCollection<string>(_totalStatistics);
            SelectedStatisticsTimeSpan = "Total";
        } else if (SelectedStatisticsTimeSpan == "Total") {
            CustomStatistics = new ObservableCollection<string>(_todayStatistics);
            SelectedStatisticsTimeSpan = "Today";
        }
    }

    public void SortApplicationsTable(object obj) {
        string orderBy = (string)obj;

        if (orderBy == "Title") {
            if (!_IsTitleOrderAscending) {
                _dbApplications = _dbApplications.OrderBy(x => x.Title).ThenBy(x => x.KeyStrokes).ToList();
                _IsTitleOrderAscending = true;
            } else {
                _dbApplications = _dbApplications.OrderByDescending(x => x.Title).ThenBy(x => x.KeyStrokes).ToList();
                _IsTitleOrderAscending = false;
            }

            _IsKeyStrokesOrderAscending = false;
            _IsMouseClicksOrderAscending = false;
        } else if (orderBy == "KeyStrokes") {
            if (!_IsKeyStrokesOrderAscending) {
                _dbApplications = _dbApplications.OrderBy(x => x.KeyStrokes).ThenBy(x => x.Title).ToList();
                _IsKeyStrokesOrderAscending = true;
            } else {
                _dbApplications = _dbApplications.OrderByDescending(x => x.KeyStrokes).ThenBy(x => x.Title).ToList();
                _IsKeyStrokesOrderAscending = false;
            }

            _IsTitleOrderAscending = false;
            _IsMouseClicksOrderAscending = false;
        } else if (orderBy == "MouseClicks") {
            if (!_IsMouseClicksOrderAscending) {
                _dbApplications = _dbApplications.OrderBy(x => x.MouseClicks).ThenBy(x => x.Title).ToList();
                _IsMouseClicksOrderAscending = true;
            } else {
                _dbApplications = _dbApplications.OrderByDescending(x => x.MouseClicks).ThenBy(x => x.Title).ToList();
                _IsMouseClicksOrderAscending = false;
            }

            _IsTitleOrderAscending = false;
            _IsKeyStrokesOrderAscending = false;
        }

        _applicationsTableSortOrder = orderBy;
        _UpdateApplicationsTable(_dbApplications);
    }

    private void _OnApplicationChangedEvent(object sender, ApplicationChangedArgs e) {
        _UpdateStatistics();

        _dbApplications = DatabaseController.GetApplications().
            OrderBy(x => x.Title).ThenBy(x => x.KeyStrokes).ToList();

        if (_applicationsTableSortOrder == "Title") {
            _IsTitleOrderAscending = !_IsTitleOrderAscending;
        } else if (_applicationsTableSortOrder == "KeyStrokes") {
            _IsKeyStrokesOrderAscending = !_IsKeyStrokesOrderAscending;
        } else if (_applicationsTableSortOrder == "MouseClicks") {
            _IsMouseClicksOrderAscending = !_IsMouseClicksOrderAscending;
        }

        SortApplicationsTable(_applicationsTableSortOrder);
        _UpdateApplicationsTable(_dbApplications);
    }

    private void _UpdateStatistics() {
        _dbStatistics = DatabaseController.GetStatistics();
        DBStatistics stats;

        DateTime curr = DateTime.Today;
        DateTime start, end;

        // Statistics: Total
        stats = DBStatistics.Get(_dbStatistics);
        _totalStatistics[0] = StringUtils.FormatCount(stats.Applications);
        _totalStatistics[1] = StringUtils.FormatCount(stats.KeyStrokes);
        _totalStatistics[2] = StringUtils.FormatCount(stats.MouseClicks);

        CustomStatistics = new ObservableCollection<string>(_totalStatistics);

        // Statistics: Today
        stats = DBStatistics.Get(_dbStatistics, curr, curr);
        _todayStatistics[0] = StringUtils.FormatCount(stats.Applications);
        _todayStatistics[1] = StringUtils.FormatCount(stats.KeyStrokes);
        _todayStatistics[2] = StringUtils.FormatCount(stats.MouseClicks);

        // Statistics: Year
        start = new DateTime(curr.Year, 1, 1);
        end = new DateTime(curr.Year, 12, 31);
        stats = DBStatistics.Get(_dbStatistics, start, end);
        YearStatistics[0] = StringUtils.FormatCount(stats.Applications);
        YearStatistics[1] = StringUtils.FormatCount(stats.KeyStrokes);
        YearStatistics[2] = StringUtils.FormatCount(stats.MouseClicks);

        // Statistics: Month
        int daysInMonth = DateTime.DaysInMonth(curr.Year, curr.Month);
        start = new DateTime(curr.Year, curr.Month, 1);
        end = new DateTime(curr.Year, curr.Month, daysInMonth);
        stats = DBStatistics.Get(_dbStatistics, start, end);
        MonthStatistics[0] = StringUtils.FormatCount(stats.Applications);
        MonthStatistics[1] = StringUtils.FormatCount(stats.KeyStrokes);
        MonthStatistics[2] = StringUtils.FormatCount(stats.MouseClicks);

        // Statistics: Week
        int dayOfWeek = (int)curr.DayOfWeek == 0 ? 7 : (int)curr.DayOfWeek;
        start = curr.AddDays(-dayOfWeek + 1);
        end = start.AddDays(7);
        stats = DBStatistics.Get(_dbStatistics, start, end);
        WeekStatistics[0] = StringUtils.FormatCount(stats.Applications);
        WeekStatistics[1] = StringUtils.FormatCount(stats.KeyStrokes);
        WeekStatistics[2] = StringUtils.FormatCount(stats.MouseClicks);
    }

    private void _UpdateApplicationsTable(List<dbApplications> dbApplications) {
        ApplicationsTableTitles = new ObservableCollection<string>(
             dbApplications.Select(x => x.Title).ToList()
         );

        ApplicationsTableKeyStrokes = new ObservableCollection<string>(
            dbApplications.Select(x => StringUtils.FormatCount(x.KeyStrokes)).ToList()
        );

        ApplicationsTableMouseClicks = new ObservableCollection<string>(
            dbApplications.Select(x => StringUtils.FormatCount(x.MouseClicks)).ToList()
        );
    }
}