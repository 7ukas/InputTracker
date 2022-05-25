using System;

namespace InputTracker {
    public class ApplicationInput {
        private DateTime _date;

        private string _appTitle;
        private string _appWindowTitle;

        private KeyMonitor _keyMonitor;
        private MouseMonitor _mouseMonitor;

        private string _rawText;
        private string _regularText;

        public ApplicationInput(DateTime date, string appTitle, string appWindowTitle, KeyMonitor keyMonitor, MouseMonitor mouseMonitor) {
            _date = date;

            _appTitle = appTitle;
            _appWindowTitle = appWindowTitle;

            _keyMonitor = keyMonitor;
            _mouseMonitor = mouseMonitor;

            _rawText = keyMonitor.RawText;
            _regularText = keyMonitor.RegularText;
        }

        public string Date {
            get { return _date.ToString("yyyy-MM-dd HH:mm:ss.fff"); }
        }

        public string DayDate {
            get { return _date.ToString("yyyy-MM-dd"); }
        }

        public string Title {
            get { return _appTitle; }
        }

        public string WindowTitle {
            get { return _appWindowTitle; }
        }

        public string RawText {
            get { return _rawText; }
        }

        public string RegularText {
            get { return _regularText; }
        }

        public int KeyStrokes {
            get { return _keyMonitor.Count(); }
        }

        public int MouseClicks {
            get { return _mouseMonitor.Count(); }
        }

        public static bool Equals(ApplicationInput a, ApplicationInput b) {
            return a.Date == b.Date;
        }
    }
}
