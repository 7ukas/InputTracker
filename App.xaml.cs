using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using InputTracker;

namespace InputTracker {
    public partial class App : Application {
        public static LiveSettings LiveSettings = new LiveSettings();
        public static DatabaseSettings DatabaseSettings = new DatabaseSettings();

        // Keys cache for frequent use
        public static Dictionary<Key, DBKeyboardKey> KeyboardKeys =
            DatabaseController.GetKeyboardKeys()
            .ToDictionary(x => x.KeyboardKey, x => x);
    }

    public class LiveSettings {
        public bool IsTracking { get; set; }
        public string Style { get; set; }

        public LiveSettings() {
            IsTracking = false;
            Style = "Regular";
        }
    }

    public class DatabaseSettings {
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
