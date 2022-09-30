namespace InputTracker;

internal class HistoryViewModel : BaseViewModel {
    // Commands
    public ICommand AdjustMaxRowsCommand { get; init; }
    public ICommand AdjustStartTimeCommand { get; init; }
    public ICommand AdjustEndTimeCommand { get; init; }
    public ICommand SearchCommand { get; init; }
    public ICommand RefreshCommand { get; init; }
    public ICommand GenerateCsvFileCommand { get; init; }
    public ICommand GenerateTxtFileCommand { get; init; }
    public ICommand ClearDatabaseCommand { get; init; }

    // Properties
    public bool ApplicationColumn { 
        get { return _columnsStatus[HistoryColumnIndex.Application]; }
        set {
            _columnsStatus[HistoryColumnIndex.Application] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.Application].Visibility = 
                    _BooleanToVisibility(value);
            }
        } 
    }
    public bool WindowColumn {
        get { return _columnsStatus[HistoryColumnIndex.Window]; }
        set {
            _columnsStatus[HistoryColumnIndex.Window] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.Window].Visibility =
                    _BooleanToVisibility(value);
            }
        }
    }
    public bool RegularTextColumn {
        get { return _columnsStatus[HistoryColumnIndex.RegularText]; }
        set {
            _columnsStatus[HistoryColumnIndex.RegularText] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.RegularText].Visibility =
                    _BooleanToVisibility(value);
            }
        }
    }
    public bool RawTextColumn {
        get { return _columnsStatus[HistoryColumnIndex.RawText]; }
        set {
            _columnsStatus[HistoryColumnIndex.RawText] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.RawText].Visibility =
                    _BooleanToVisibility(value);
            }
        }
    }
    public bool KeyStrokesColumn {
        get { return _columnsStatus[HistoryColumnIndex.KeyStrokes]; }
        set {
            _columnsStatus[HistoryColumnIndex.KeyStrokes] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.KeyStrokes].Visibility =
                    _BooleanToVisibility(value);
            }
        }
    }
    public bool MouseClicksColumn {
        get { return _columnsStatus[HistoryColumnIndex.MouseClicks]; }
        set {
            _columnsStatus[HistoryColumnIndex.MouseClicks] = value;

            if (_dataGrid.Columns.Count > 0) {
                _dataGrid.Columns[HistoryColumnIndex.MouseClicks].Visibility =
                    _BooleanToVisibility(value);
            }
        }
    }
    public bool EmptyEntries { get; set; } = true;
    public string MaxRows { get; set; } = _defaultMaxRows.ToString();
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime EndDate { get; set; } = DateTime.Today;
    public string StartTime { get; set; } = _defaultStartTime;
    public string EndTime { get; set; } = _defaultEndTime;
    public bool SearchEnabled { get; set; } = true;
    public Brush MaxRowsBackground { get; set; }
    public Brush StartTimeBackground { get; set; }
    public Brush EndTimeBackground { get; set; }
    public string Rows { get; private set; } = "0";
    public string Applications { get; private set; } = "0";
    public string KeyStrokes { get; private set; } = "0";
    public string MouseClicks { get; private set; } = "0";

    // Current status of data grid columns
    private readonly Dictionary<int, bool> _columnsStatus = new Dictionary<int, bool>() {
        { HistoryColumnIndex.Application, true },
        { HistoryColumnIndex.Window, false },
        { HistoryColumnIndex.RegularText, true },
        { HistoryColumnIndex.RawText, false },
        { HistoryColumnIndex.KeyStrokes, false },
        { HistoryColumnIndex.MouseClicks, false }
    };

    // Time and MaxRows
    private const string _defaultStartTime = "00:00:01";
    private const string _defaultEndTime = "23:59:59";
    private const int _defaultMaxRows = 100000;
    private const int _maxRowsTextLength = 6;
    private const int _requiredTimeTextLength = 8;

    // Background Brushes
    private readonly Brush _validColor = Brushes.White; // Very light blue
    private readonly Brush _invalidColor = (Brush)new BrushConverter().ConvertFrom("#E74C3C"); // Red

    // Data Grid
    private readonly DataGrid _dataGrid;

    // File Generator
    private FileGenerator _fileGenerator;

    public HistoryViewModel() {
        AdjustMaxRowsCommand = new RelayCommand(AdjustMaxRows);
        AdjustStartTimeCommand = new RelayCommand(AdjustStartTime);
        AdjustEndTimeCommand = new RelayCommand(AdjustEndTime);
        SearchCommand = new RelayCommand(Search);
        RefreshCommand = new RelayCommand(Refresh);
        GenerateCsvFileCommand = new RelayCommand(GenerateCsvFile);
        GenerateTxtFileCommand = new RelayCommand(GenerateTxtFile);
        ClearDatabaseCommand = new RelayCommand(ClearDatabase);
    }

    public HistoryViewModel(DataGrid dataGrid) : this() {
        _dataGrid = dataGrid;
    }

    public void AdjustMaxRows(object obj) {
        string maxRows = (string)obj;

        if (maxRows.Length > _maxRowsTextLength ||
            maxRows.Any(c => !char.IsDigit(c))) {
            MaxRowsBackground = _invalidColor;
        }

        int rows;
        int.TryParse(maxRows, out rows);

        if (rows <= _defaultMaxRows && rows > 0) {
            MaxRows = maxRows;
            MaxRowsBackground = _validColor;
        } else { MaxRowsBackground = _invalidColor; }

        _VerifySearchButton();
    }

    public void AdjustStartTime(object obj) {
        string startTime = (string)obj;

        if (_IsTimeFormatCorrect(startTime, true)) {
            StartTime = startTime;
            StartTimeBackground = _validColor;
        } else StartTimeBackground = _invalidColor;

        _VerifySearchButton();
    }

    public void AdjustEndTime(object obj) {
        string endTime = (string)obj;

        if (_IsTimeFormatCorrect(endTime, false)) {
            EndTime = endTime;
            EndTimeBackground = _validColor;
        } else EndTimeBackground = _invalidColor;

        _VerifySearchButton();
    }

    private void _VerifySearchButton() {
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

        _dataGrid.Columns[HistoryColumnIndex.Application].Visibility = _BooleanToVisibility(ApplicationColumn);
        _dataGrid.Columns[HistoryColumnIndex.Window].Visibility = _BooleanToVisibility(WindowColumn);
        _dataGrid.Columns[HistoryColumnIndex.RegularText].Visibility = _BooleanToVisibility(RegularTextColumn);
        _dataGrid.Columns[HistoryColumnIndex.RawText].Visibility = _BooleanToVisibility(RawTextColumn);
        _dataGrid.Columns[HistoryColumnIndex.KeyStrokes].Visibility = _BooleanToVisibility(KeyStrokesColumn);
        _dataGrid.Columns[HistoryColumnIndex.MouseClicks].Visibility = _BooleanToVisibility(MouseClicksColumn);
        
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

        StartTime = _defaultStartTime;
        EndTime = _defaultEndTime;
        MaxRows = _defaultMaxRows.ToString();

        StartTimeBackground = EndTimeBackground = MaxRowsBackground = _validColor;

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
            LiveViewModel.IsTracking = false;
            Refresh(null);
        }
    }

    private bool _IsTimeFormatCorrect(string time, bool isStartingTime) {
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

        return (isStartingTime && dateTime < DateTime.Parse(EndTime)) || 
            (!isStartingTime && dateTime > DateTime.Parse(StartTime)) &&
            dateTime.ToString().Substring(11) != "00:00:00";
    }

    private Visibility _BooleanToVisibility(bool value) {
        return value ? Visibility.Visible : Visibility.Collapsed;
    }
}
