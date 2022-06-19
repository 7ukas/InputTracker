using System;
using System.Linq;
using System.Text;

namespace InputTracker {
    public sealed class StringUtils {
        /* Replaces certain special characters for database */
        public static string EscapeTitle(string str) {
            return str.Replace("\\", "[backslash]")
                    .Replace("\'", "[qoute]")
                    .Replace("\"", "[double_qoute]");
        }

        public static string EscapeText(string str) {
            return str.Replace("\'", "\"");
        }

        public static string FormatCount(int count) {
            // Adds comma to given count; <1,000,000,000
            string countStr = count.ToString();
            int len = countStr.Length;

            if (countStr.All(char.IsDigit) && len > 3) {
                if (len < 7) {
                    return $"{countStr.Substring(0, len - 3)}," +
                           $"{countStr.Substring(len - 3)}";
                } else {
                    string start = countStr.Substring(0, len % 3 == 0 ? 3 : len % 3);
                    return $"{start}," +
                           $"{countStr.Substring(start.Length, 3)}," +
                           $"{countStr.Substring(len - 3)}";
                }
            } else return countStr;
        }

        public static string FormatLastUpdated(TimeSpan timeSpan) {
            int s = (int)timeSpan.TotalSeconds;
            if (s == 0) return "Now";

            StringBuilder time = new StringBuilder();

            if (s >= 86400) { // Days
                time.Append($"{s / 86400} day");
                if (s > 172799) time.Append("s");
                if (s % 86400 != 0) time.Append(" ");
                s = s % 86400;
            }
            if (s >= 3600 && s < 86400) { // Hours
                time.Append($"{s / 3600} hour");
                if (s > 7199) time.Append("s");
                if (s % 3600 != 0) time.Append(" ");
                s = s % 3600;
            }
            if (s >= 60 && s < 3600) { // Minutes
                time.Append($"{s / 60} minute");
                if (s > 119) time.Append("s");
                if (s % 60 != 0) time.Append(" ");
                s = s % 60;
            }
            if (s > 0 && s < 60) { // Seconds
                time.Append($"{s} second");
                if (s > 1) time.Append("s");
            }
            time.Append(" ago");

            return time.ToString();
        }

        public static string ShrinkLog(string log, int charLimit) {
            if (log.Length > charLimit) {
                string[] logLines = log.ToString().Split("\n");
                int len = logLines.Length;
                int limit = len % 2 == 1 ? len - 1 : len;

                len = log.Length;
                for (int i = 0; i < limit; i += 2) {
                    len = len - logLines[i].Length - logLines[i + 1].Length;
                    if (len < charLimit)
                        break;
                }

                int count = log.Length - len +
                    log.Substring(log.Length - len).IndexOf("\n\n") + 2;

                log = log.Remove(0, count);
            }

            return log;
        }

        public static string ShrinkTitle(string title, int charLimit) {
            return title.Length > charLimit ?
                $"{title.Substring(0, charLimit - 4).Trim()} ..." : title;
        }

        public static string RemovePath(string filename) {
            int slashIdx = filename.LastIndexOf('\\') + 1;
            int dotIdx = filename.LastIndexOf('.');

            if (slashIdx >= 0 && dotIdx > slashIdx && dotIdx < filename.Length) {
                return filename.Substring(slashIdx, dotIdx - slashIdx);
            } else return filename;
        }
    }
}
