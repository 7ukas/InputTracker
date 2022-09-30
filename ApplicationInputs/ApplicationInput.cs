namespace InputTracker;

internal class ApplicationInput {
    public string Date { get { return _date.ToString("yyyy-MM-dd HH:mm:ss.fff"); } }
    public string DayDate { get { return _date.ToString("yyyy-MM-dd"); } }
    public string Title { get; init; } = _defaultTitle;
    public string WindowTitle { get; init; } = _defaultTitle;
    public string RegularText { get; init; }
    public string RegularLog { get; init; }
    public string RawText { get; init; }
    public string RawLog { get; init; }
    public int KeyStrokes { get; init; }
    public int MouseClicks { get; init; }

    private const string _defaultTitle = "NOT DEFINED";
    private readonly DateTime _date;

    public ApplicationInput(DateTime date, string applicationTitle, string applicationWindowTitle, 
        KeyInputStructure keyInputStructure, ButtonInputStructure buttonInputStructure) {
        _date = date;

        if (applicationTitle.Length > 0) Title = applicationTitle;
        if (applicationWindowTitle.Length > 0) WindowTitle = applicationWindowTitle;

        RegularText = keyInputStructure?.RegularText;
        RegularLog = new StringBuilder().Append($"[{Date} | {WindowTitle} ({Title})]:\n")
            .Append(RegularText.Length > 0 ? $"{RegularText}\n\n" : "\n").ToString();

        RawText = keyInputStructure?.RawText;
        RawLog = new StringBuilder().Append($"[{Date} | {WindowTitle} ({Title})]:\n")
            .Append(RawText.Length > 0 ? $"{RawText}\n\n" : "\n").ToString();

        KeyStrokes = (int)(keyInputStructure?.Count);
        MouseClicks = (int)(buttonInputStructure?.Count);
    }
}
