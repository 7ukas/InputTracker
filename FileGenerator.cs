namespace InputTracker;

internal sealed class FileGenerator {
    private List<DBInput> _inputs;

    public FileGenerator(List<DBInput> inputs) {
        _inputs = new List<DBInput>(inputs);
    }

    public void GenerateCsv(bool application, bool window, bool regularText, bool rawText, bool keyStrokes, bool mouseClicks) {
        StringBuilder csv = new StringBuilder();

        if (application) csv.Append("Application,");
        if (window) csv.Append("Window,");
        if (regularText) csv.Append("Regular Text,");
        if (rawText) csv.Append("Raw Text,");
        if (keyStrokes) csv.Append("Key Strokes,");
        if (mouseClicks) csv.Append("Mouse Clicks,");

        csv.Remove(csv.Length - 1, 1);
        csv.AppendLine();

        foreach (DBInput input in _inputs) {
            if (application) csv.Append($"{input.Application},");
            if (window) csv.Append($"{input.Window},");
            if (regularText) csv.Append($"{input.RegularText},");
            if (rawText) csv.Append($"{input.RawText},");
            if (keyStrokes) csv.Append($"{input.KeyStrokes},");
            if (mouseClicks) csv.Append($"{input.MouseClicks},");

            csv.Remove(csv.Length - 1, 1);
            csv.AppendLine();
        }

        _SaveFile(csv.ToString(), "csv");
    }

    public void GenerateTxt(bool application, bool window, bool regularText, bool rawText, bool keyStrokes, bool mouseClicks) {
        StringBuilder txt = new StringBuilder();

        foreach (DBInput input in _inputs) {
            if (application) txt.Append($"Application: {input.Application},    ");
            if (window) txt.Append($"Window: {input.Window},    ");
            if (regularText) txt.Append($"Regular Text: {input.RegularText},    ");
            if (rawText) txt.Append($"Raw Text: {input.RawText},    ");
            if (keyStrokes) txt.Append($"Key Strokes: {input.KeyStrokes},    ");
            if (mouseClicks) txt.Append($"Mouse Clicks: {input.MouseClicks},    ");

            txt.Remove(txt.Length - 1, 1);
            txt.AppendLine();
        }

        _SaveFile(txt.ToString(), "txt");
    }

    private void _SaveFile(string str, string file) {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.FileName = $"Inputs.{file.ToLower()}";
        saveFileDialog.Filter = $"{file.ToUpper()} File (*.{file.ToLower()})|*.{file.ToLower()}|All Files (*.*)|*.*";
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        if (saveFileDialog.ShowDialog() == true)
            File.WriteAllText(saveFileDialog.FileName, str);
    }
}
