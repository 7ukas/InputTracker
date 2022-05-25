using System;
using System.Collections.Generic;
using System.Linq;

namespace InputTracker {
    public class DBStatistics {
        public DateTime Date { get; set; }
        public string ApplicationIDs { private get; set; }
        public int Applications { get; set; }
        public int KeyStrokes { get; set; }
        public int MouseClicks { get; set; }

        public DBStatistics() { }

        public int[] GetAppIDs() {
            return ApplicationIDs.Substring(0, ApplicationIDs.Length - 1)
                .Split(";").Select(x => int.Parse(x)).ToArray();
        }

        public static DBStatistics Get(List<DBStatistics> rows) {
            if (rows.Count < 1) return new DBStatistics();

            DBStatistics statistics = new DBStatistics();
            List<int> appIDs = new List<int>();

            for (int i = 0; i < rows.Count; i++) {
                appIDs.AddRange(rows[i].GetAppIDs());
                statistics.KeyStrokes += rows[i].KeyStrokes;
                statistics.MouseClicks += rows[i].MouseClicks;
            } statistics.Applications = appIDs.Distinct().Count();

            return statistics;
        }

        public static DBStatistics Get(List<DBStatistics> rows, DateTime start, DateTime end) {
            if (rows.Count < 1) return new DBStatistics();

            DBStatistics statistics = new DBStatistics();
            List<int> appIDs = new List<int>();

            for (int i = 0; i < rows.Count; i++) {
                if (start <= rows[i].Date && rows[i].Date <= end) {
                    appIDs.AddRange(rows[i].GetAppIDs());
                    statistics.KeyStrokes += rows[i].KeyStrokes;
                    statistics.MouseClicks += rows[i].MouseClicks;
                }
            }
            statistics.Applications = appIDs.Distinct().Count();

            return statistics;
        }
    }
}
