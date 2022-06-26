namespace InputTracker;

internal class HistoryViewModel : BaseViewModel {
    public ICommand AdjustMaxRowsCommand { get; init; }
    public ICommand AdjustStartTimeCommand { get; init; }
    public ICommand AdjustEndTimeCommand { get; init; }
    public ICommand VerifySearchCommand { get; init; }
    public ICommand SearchCommand { get; init; }
    public ICommand RefreshCommand { get; init; }
    public ICommand GenerateCsvFileCommand { get; init; }
    public ICommand GenerateTxtFileCommand { get; init; }
    public ICommand ClearDatabaseCommand { get; init; }

    public bool ApplicationColumn { get; set; } = true;
    public bool WindowColumn { get; set; } = false;
    public bool RegularTextColumn { get; set; } = true;
    public bool RawTextColumn { get; set; } = false;
    public bool KeyStrokesColumn { get; set; } = false;
    public bool MouseClicksColumn { get; set; } = false;
    public bool EmptyEntries { get; set; } = true;
    public string MaxRows { get; private set; } = "10000";
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today;
    public string StartTime { get; private set; } = "00:00:01";
    public string EndTime { get; private set; } = "23:59:59";
    public bool SearchEnabled { get; set; } = true;
    public Brush MaxRowsBackground { get; set; } = Brushes.White;
    public Brush StartTimeBackground { get; set; } = Brushes.White;
    public Brush EndTimeBackground { get; set; } = Brushes.White;
    public string Rows { get; private set; } = "0";
    public string Applications { get; private set; } = "0";
    public string KeyStrokes { get; private set; } = "0";
    public string MouseClicks { get; private set; } = "0";

    private const int _maxRows = 100000;
    private const int _maxRowsTextLength = 6;
    private const int _requiredTimeTextLength = 8;
    private readonly Brush _validColor = Brushes.White;
    private readonly Brush _invalidColor = (Brush)new BrushConverter().ConvertFrom("#E74C3C"); // Red
    private readonly DataGrid _dataGrid;
    private FileGenerator _fileGenerator;

    private static class _ColumnIndex {
        public const int
        ApplicationIndex = 1,
        WindowIndex = 2,
        RegularTextIndex = 4,
        RawTextIndex = 3,
        KeyStrokesIndex = 5,
        MouseClicksIndex = 6;
    }

    public HistoryViewModel(DataGrid dataGrid) : this() {
        _dataGrid = dataGrid;
    }

    public HistoryViewModel() {
        AdjustMaxRowsCommand = new RelayCommand(AdjustMaxRows);
        AdjustStartTimeCommand = new RelayCommand(AdjustStartTime);
        AdjustEndTimeCommand = new RelayCommand(AdjustEndTime);
        VerifySearchCommand = new RelayCommand(VerifySearch);
        SearchCommand = new RelayCommand(Search);
        RefreshCommand = new RelayCommand(Refresh);
        GenerateCsvFileCommand = new RelayCommand(GenerateCsvFile);
        GenerateTxtFileCommand = new RelayCommand(GenerateTxtFile);
        ClearDatabaseCommand = new RelayCommand(ClearDatabase);
    }

    public void AdjustMaxRows(object obj) {
        string maxRows = (string)obj;

        if (maxRows.Length > _maxRowsTextLength ||
            maxRows.Any(c => !char.IsDigit(c))) {
            MaxRowsBackground = _invalidColor;
        }

        int rows;
        int.TryParse(maxRows, out rows);

        if (rows <= _maxRows && rows > 0) {
            MaxRows = maxRows;
            MaxRowsBackground = _validColor;
        } else { MaxRowsBackground = _invalidColor; }
    }

    public void AdjustStartTime(object obj) {
        string startTime = (string)obj;

        if (_IsTimeFormatCorrect(startTime, true)) {
            StartTime = startTime;
            StartTimeBackground = _validColor;
        } else StartTimeBackground = _invalidColor;
    }

    public void AdjustEndTime(object obj) {
        string endTime = (string)obj;

        if (_IsTimeFormatCorrect(endTime, false)) {
            EndTime = endTime;
            EndTimeBackground = _validColor;
        } else EndTimeBackground = _invalidColor;
    }

    public void VerifySearch(object obj) {
        Debug.WriteLine("verifySearch");
        SearchEnabled = 
            StartTimeBackground == _validColor && 
            EndTimeBackground == _validColor &&
            MaxRowsBackground == _validColor;
    }

    public void Search(object obj) {
        DateTime start = Convert.ToDateTime($"{StartDate.ToString().Substring(0,10)} {StartTime}");
        DateTime end = Convert.ToDateTime($"{EndDate.ToString().Substring(0, 10)} {EndTime}");
        int maxRows = int.Parse(MaxRows);

         List<DBInput> dbInputs = EmptyEntries ? 
            DatabaseController.GetInputs(start, end, maxRows).ToList() :
            DatabaseController.GetInputs(start, end, maxRows)
                .Where(i => i.RegularText.Length > 0).ToList();

        _dataGrid.ItemsSource = dbInputs;
        _fileGenerator = new FileGenerator(dbInputs);

        _dataGrid.Columns[_ColumnIndex.ApplicationIndex].Visibility = _BooleanToVisibility(ApplicationColumn);
        _dataGrid.Columns[_ColumnIndex.WindowIndex].Visibility = _BooleanToVisibility(WindowColumn);
        _dataGrid.Columns[_ColumnIndex.RegularTextIndex].Visibility = _BooleanToVisibility(RegularTextColumn);
        _dataGrid.Columns[_ColumnIndex.RawTextIndex].Visibility = _BooleanToVisibility(RawTextColumn);
        _dataGrid.Columns[_ColumnIndex.KeyStrokesIndex].Visibility = _BooleanToVisibility(KeyStrokesColumn);
        _dataGrid.Columns[_ColumnIndex.MouseClicksIndex].Visibility = _BooleanToVisibility(MouseClicksColumn);
        
        Rows = StringUtils.FormatCount(dbInputs.Count);
        Applications = StringUtils.FormatCount(dbInputs.GroupBy(x => x.Application).Select(x => x.Key).Count());

        int keyStrokes = 0; dbInputs.ForEach(x => keyStrokes += x.KeyStrokes);
        KeyStrokes = StringUtils.FormatCount(keyStrokes);

        int mouseClicks = 0; dbInputs.ForEach(x => mouseClicks += x.MouseClicks);
        MouseClicks = StringUtils.FormatCount(mouseClicks);
    }

    public void Refresh(object obj) {
        ApplicationColumn = RegularTextColumn = EmptyEntries = true;
        WindowColumn = RawTextColumn = KeyStrokesColumn = MouseClicksColumn = false;

        StartDate = EndDate = DateTime.Today;
        StartTimeBackground = EndTimeBackground = _validColor;

        StartTime = "00:00:01";
        EndTime = "23:59:59";
        MaxRows = "10000";

        _dataGrid.ItemsSource = null;
        _fileGenerator = null;

        Rows = Applications = KeyStrokes = MouseClicks = "0";
    }

    public void GenerateCsvFile(object obj) {
        if (_fileGenerator != null) {
            _fileGenerator.GenerateCsv(
                ApplicationColumn, WindowColumn, RegularTextColumn, 
                RawTextColumn, KeyStrokesColumn, MouseClicksColumn
            );
        }
    }

    public void GenerateTxtFile(object obj) {
        if (_fileGenerator != null) {
            _fileGenerator.GenerateTxt(
                ApplicationColumn, WindowColumn, RegularTextColumn,
                RawTextColumn, KeyStrokesColumn, MouseClicksColumn
            );
        }
    }

    public void ClearDatabase(object obj) {
        // Warn before proceeding data removal from database
        if (MessageBox.Show(
            $"Are you sure you want to REMOVE ALL EXISTING DATA from the database?",
            WindowMain.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
            DatabaseController.ClearDatabase();
            Refresh(null);
        }
    }

    private bool _IsTimeFormatCorrect(string time, bool startTime) {
        if (time.Length != _requiredTimeTextLength) {
            return false;
        } else {
            for (int i = 0; i < time.Length; i++) {
                if (time[i] != time[i] && (!char.IsDigit(time[i]) || time[i] != ':')) {
                    return false;
                }
            }
        }

        DateTime dateTime;
        DateTime.TryParseExact(time, "HH:mm:ss",
            CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);

        return (startTime && dateTime < DateTime.Parse(EndTime)) || 
            (!startTime && dateTime > DateTime.Parse(StartTime)) &&
            dateTime.ToString().Substring(11) != "00:00:00";
    }

    private Visibility _BooleanToVisibility(bool value) {
        return value ? Visibility.Visible : Visibility.Collapsed;
    }
}
