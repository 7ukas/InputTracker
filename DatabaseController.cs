using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace InputTracker {
    public class DatabaseController {
        // Connection
        private static string _connectionName = "Default";

        // Tables
        public static string InputsTable = "Inputs";
        public static string ApplicationsTable = "Applications";
        public static string SequenceTable = "sqlite_sequence";
        public static string KeyboardKeysTable = "KeyboardKeys";
        public static string StatisticsTable = "Statistics";

        public static SQLiteConnection GetConnection() {
            ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings[_connectionName];
            string connectionString = connectionSettings.ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
            
            return new SQLiteConnection(connectionString, true);
        }

        public static Task Update(ApplicationInput input) {
            if (input == null) return null;

            using (IDbConnection connection = GetConnection()) {
                StringBuilder query = new StringBuilder();

                // Applications Table
                query.AppendLine($"INSERT OR IGNORE INTO {ApplicationsTable} (Application, KeyStrokes, MouseClicks) SELECT '{input.Title}', 0, 0;")
                    .Append($"UPDATE {ApplicationsTable} SET KeyStrokes = KeyStrokes + {input.KeyStrokes}, ")
                    .AppendLine($"MouseClicks = MouseClicks + {input.MouseClicks} WHERE Application = '{input.Title}';")
                    .AppendLine($"UPDATE {SequenceTable} SET seq = seq - 1;")

                // Inputs Table
                    .Append($"INSERT OR IGNORE INTO {InputsTable} VALUES (")
                    .Append($"'{input.Date}', ")
                    .Append($"(SELECT ID FROM {ApplicationsTable} WHERE Application = '{input.Title}'), ")
                    .Append($"'{StringUtils.EscapeTitle(input.WindowTitle)}', ")
                    .Append($"'{StringUtils.EscapeText(input.RawText)}', ")
                    .Append($"'{StringUtils.EscapeText(input.RegularText)}', ")
                    .Append($"{input.KeyStrokes}, ")
                    .AppendLine($"{input.MouseClicks});")

                // Statistics Table
                    .AppendLine($"INSERT OR IGNORE INTO {StatisticsTable} VALUES ('{input.DayDate}', '', 0, 0, 0);")
                    .Append($"UPDATE {StatisticsTable} SET ApplicationIDs = ApplicationIDs || IIF (")
                    .Append($"(SELECT ApplicationIDs NOT LIKE '%' || A.ID || ';%' FROM {ApplicationsTable} A ")
                    .Append($"WHERE A.Application = '{input.Title}'), ")
                    .Append($"(SELECT A.ID || ';' FROM {ApplicationsTable} A WHERE A.Application = '{input.Title}'), ''), ")
                    .Append($"Applications = (SELECT LENGTH(ApplicationIDs) - LENGTH(REPLACE(ApplicationIDs, ';', ''))), ")
                    .Append($"KeyStrokes = KeyStrokes + {input.KeyStrokes}, ")
                    .AppendLine($"MouseClicks = MouseClicks + {input.MouseClicks} WHERE Date = '{input.DayDate}';");

                connection.Execute(query.ToString());
            }

            return Task.CompletedTask;
        }

        public static List<DBInput> GetInputs(DateTime start, DateTime end, int maxRows) {
            using (IDbConnection connection = GetConnection()) {
                string startStr = start.ToString("yyyy-MM-dd HH:mm:ss");
                string endStr = end.ToString("yyyy-MM-dd HH:mm:ss");

                return connection.Query<DBInput>(
                    $"SELECT I.Date AS Date, A.Application AS Application, I.Window AS Window, I.RawText AS RawText, " +
                    $"I.RegularText AS RegularText, I.KeyStrokes AS KeyStrokes, I.MouseClicks AS MouseClicks " +
                    $"FROM {InputsTable} I JOIN {ApplicationsTable} A ON I.ApplicationID = A.ID " +
                    $"WHERE Date BETWEEN '{startStr}' AND '{endStr}' LIMIT {maxRows}").ToList();
            }
        }

        public static List<DBApplication> GetApplications() {
            using (IDbConnection connection = GetConnection()) {
                return connection.Query<DBApplication>(
                    $"SELECT Application AS Title, KeyStrokes, MouseClicks FROM {ApplicationsTable}").ToList();
            }
        }

        public static List<DBKeyboardKey> GetKeyboardKeys() {
            using (IDbConnection connection = GetConnection()) {
                return connection.Query<DBKeyboardKey>(
                    $"SELECT ID AS KeyboardKey, RawText AS RawText, RegularText AS RegularText" +
                    $", ShiftText AS ShiftText FROM {KeyboardKeysTable}").ToList();
            }
        }

        public static List<DBStatistics> GetStatistics() {
            using (IDbConnection connection = GetConnection()) {
                return connection.Query<DBStatistics>(
                    $"SELECT Date, ApplicationIDs, Applications, KeyStrokes, MouseClicks FROM {StatisticsTable}").ToList();
            }
        }

        public static void ClearDatabase() {
            using (IDbConnection connection = GetConnection()) {
                connection.Execute(
                    $"DELETE FROM {InputsTable}; " +
                    $"DELETE FROM {ApplicationsTable}; " +
                    $"DELETE FROM {StatisticsTable}; " +
                    $"UPDATE {SequenceTable} SET seq = 1;"
                );
            }
        }
    }
}
