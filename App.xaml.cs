namespace InputTracker {
    public partial class App : Application {
        internal static LiveSettings LiveSettings = new LiveSettings();
        internal static DatabaseSettings DatabaseSettings = new DatabaseSettings();

        // Keys cache for frequent use
        internal static Dictionary<Key, DBKeyboardKey> KeyboardKeys =
            DatabaseController.GetKeyboardKeys()
            .ToDictionary(x => x.KeyboardKey, x => x);
    }

    internal class LiveSettings {
        public bool IsTracking { get; set; }
        public string Style { get; set; }

        public LiveSettings() {
            IsTracking = false;
            Style = "Regular";
        }
    }

    internal class DatabaseSettings {
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
        public bool DisplayApplications { get; set; }
        public bool DisplayWindows { get; set; }
        public bool DisplayRawText { get; set; }
        public bool DisplayRegularText { get; set; }
        public bool DisplayKeyStrokes { get; set; }
        public bool DisplayMouseClicks { get; set; }
        public bool DisplayEmptyEntries { get; set; }
        public int MaxRows { get; set; }

        public DatabaseSettings() {
            ResetToDefault();
        }

        public void ResetToDefault() {
            StartDate = DateTime.Now;
            StartTime = "00:00:01";

            EndDate = DateTime.Now;
            EndTime = "23:59:59";

            DisplayApplications = true;
            DisplayWindows = false;
            DisplayRawText = false;
            DisplayRegularText = true;
            DisplayKeyStrokes = false;
            DisplayMouseClicks = false;
            DisplayEmptyEntries = true;
            MaxRows = 10000;
        }
    }
}
