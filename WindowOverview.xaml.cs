namespace InputTracker;

public partial class WindowOverview : UserControl {
    private List<DBApplication> _applications = new List<DBApplication>();
    private List<DBStatistics> _stats = new List<DBStatistics>();

    private const int _maxTitleLength = 28; // of Application string in Top 5 table
    private TopTableSortOrder _topTableSortOrder;

    public WindowOverview() {
        InitializeComponent();
    }

    private void _Window_Loaded(object sender, RoutedEventArgs e) {
        OnDatabaseUpdated();
    }

    public void OnDatabaseUpdated() {
        _InitializeStatistics();
        textBlock_lastUpdated.Text = "Now";
        image_Database.Source = new BitmapImage(new Uri(@"/resources/images/database_updated.png", UriKind.Relative));
    }

    public void OnDatabasePassive(string lastUpdated) {
        textBlock_lastUpdated.Text = lastUpdated;
        image_Database.Source = new BitmapImage(new Uri(@"/resources/images/database_white.png", UriKind.Relative));
    }

    private void _OnDatabasePanelClicked(object sender, MouseButtonEventArgs e) {
        WindowMain.UpdateDatabaseAsync();
    }

    private void _InitializeStatistics() {
        _stats = DatabaseController.GetStatistics();
        DBStatistics stats = null;

        DateTime curr = DateTime.Today;
        DateTime start, end;

        // Statistics: Today / Total
        if (textBlock_Main.Text == "Total") {
            stats = DBStatistics.Get(_stats);

            textBlock_Main.Text = "Total";
            textBlock_Main_Applications.Text = StringUtils.FormatCount(stats.Applications);
            textBlock_Main_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
            textBlock_Main_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);
        } else {
            stats = DBStatistics.Get(_stats, curr, curr);

            textBlock_Main.Text = "Today";
            textBlock_Main_Applications.Text = StringUtils.FormatCount(stats.Applications);
            textBlock_Main_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
            textBlock_Main_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);
        }

        // Statistics: Year
        start = new DateTime(curr.Year, 1, 1);
        end = new DateTime(curr.Year, 12, 31);
        stats = DBStatistics.Get(_stats, start, end);
        textBlock_Year_Applications.Text = StringUtils.FormatCount(stats.Applications);
        textBlock_Year_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
        textBlock_Year_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);

        // Statistics: Month
        int daysInMonth = DateTime.DaysInMonth(curr.Year, curr.Month);
        start = new DateTime(curr.Year, curr.Month, 1);
        end = new DateTime(curr.Year, curr.Month, daysInMonth);
        stats = DBStatistics.Get(_stats, start, end);
        textBlock_Month_Applications.Text = StringUtils.FormatCount(stats.Applications);
        textBlock_Month_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
        textBlock_Month_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);

        // Statistics: Week
        int dayOfWeek = (int)curr.DayOfWeek == 0 ? 7 : (int)curr.DayOfWeek;
        start = curr.AddDays(-dayOfWeek+1);
        end = start.AddDays(7);
        stats = DBStatistics.Get(_stats, start, end);
        textBlock_Week_Applications.Text = StringUtils.FormatCount(stats.Applications);
        textBlock_Week_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
        textBlock_Week_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);

        // Applications: Top 5
        _applications = DatabaseController.GetApplications();
        _InitializeApplicationsTop(_topTableSortOrder);
    }

    private void _InitializeApplicationsTop(TopTableSortOrder order) {
        if (order == TopTableSortOrder.KeyStrokes) {
            _applications = _applications
                .OrderByDescending(x => x.KeyStrokes)
                .ThenByDescending(x => x.MouseClicks).ToList();
        } else if (order == TopTableSortOrder.MouseClicks) {
            _applications = _applications
                .OrderByDescending(x => x.MouseClicks)
                .ThenByDescending(x => x.KeyStrokes).ToList();
        }

        int count = _applications.Count;

        if (count > 0) {
            textBlock_1st_Application.Text = StringUtils.ShrinkApplicationTitle(_applications[0].Title, _maxTitleLength);
            textBlock_1st_KeyStrokes.Text = StringUtils.FormatCount(_applications[0].KeyStrokes);
            textBlock_1st_MouseClicks.Text = StringUtils.FormatCount(_applications[0].MouseClicks);
            if (count > 1) {
                textBlock_2nd_Application.Text = StringUtils.ShrinkApplicationTitle(_applications[1].Title, _maxTitleLength);
                textBlock_2nd_KeyStrokes.Text = StringUtils.FormatCount(_applications[1].KeyStrokes);
                textBlock_2nd_MouseClicks.Text = StringUtils.FormatCount(_applications[1].MouseClicks);
                if (count > 2) {
                    textBlock_3rd_Application.Text = StringUtils.ShrinkApplicationTitle(_applications[2].Title, _maxTitleLength);
                    textBlock_3rd_KeyStrokes.Text = StringUtils.FormatCount(_applications[2].KeyStrokes);
                    textBlock_3rd_MouseClicks.Text = StringUtils.FormatCount(_applications[2].MouseClicks);
                    if (count > 3) {
                        textBlock_4th_Application.Text = StringUtils.ShrinkApplicationTitle(_applications[3].Title, _maxTitleLength);
                        textBlock_4th_KeyStrokes.Text = StringUtils.FormatCount(_applications[3].KeyStrokes);
                        textBlock_4th_MouseClicks.Text = StringUtils.FormatCount(_applications[3].MouseClicks);
                        if (count > 4) {
                            textBlock_5th_Application.Text = StringUtils.ShrinkApplicationTitle(_applications[4].Title, _maxTitleLength);
                            textBlock_5th_KeyStrokes.Text = StringUtils.FormatCount(_applications[4].KeyStrokes);
                            textBlock_5th_MouseClicks.Text = StringUtils.FormatCount(_applications[4].MouseClicks);
                        }
                    }
                }
            }
        }
    }

    private void _OnMainPanelClicked(object sender, MouseButtonEventArgs e) {
        DBStatistics stats;

        DateTime start = DateTime.Today;
        DateTime end = start.AddSeconds(86399);

        // Statistics: Today / Total
        if (textBlock_Main.Text == "Today") {
            stats = DBStatistics.Get(_stats);

            textBlock_Main.Text = "Total";
            textBlock_Main_Applications.Text = StringUtils.FormatCount(stats.Applications);
            textBlock_Main_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
            textBlock_Main_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);
        } else {
            stats = DBStatistics.Get(_stats, start, end);

            textBlock_Main.Text = "Today";
            textBlock_Main_Applications.Text = StringUtils.FormatCount(stats.Applications);
            textBlock_Main_KeyStrokes.Text = StringUtils.FormatCount(stats.KeyStrokes);
            textBlock_Main_MouseClicks.Text = StringUtils.FormatCount(stats.MouseClicks);
        }
    }

    private void _OnKeyStrokesColumnClicked(object sender, MouseButtonEventArgs e) {
        _topTableSortOrder = TopTableSortOrder.KeyStrokes;
        _InitializeApplicationsTop(_topTableSortOrder);
    }

    private void _OnMouseClicksColumnClicked(object sender, MouseButtonEventArgs e) {
        _topTableSortOrder = TopTableSortOrder.MouseClicks;
        _InitializeApplicationsTop(_topTableSortOrder);
    }

    private enum TopTableSortOrder {
        KeyStrokes = 0,
        MouseClicks = 1
    }
}
