using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace InputTracker {
    public class FileGenerator {
        private List<DBInput> _inputs;

        public FileGenerator(List<DBInput> inputs) {
            _inputs = new List<DBInput>(inputs);
        }

        public void GenerateCSV(bool applicationCol, bool windowCol, bool rawTextCol, bool regularTextCol, bool keyStrokesCol, bool mouseClicksCol) {
            StringBuilder csv = new StringBuilder();

            string date = "Date,";
            string application = applicationCol ? "Application," : "";
            string window = windowCol ? "Window," : "";
            string rawText = rawTextCol ? "Raw text," : "";
            string regularText = regularTextCol ? "Regular text," : "";
            string keyStrokes = keyStrokesCol ? "Key strokes," : "";
            string mouseClicks = mouseClicksCol ? "Mouse clicks," : "";

            string line = $"{date}{application}{window}{rawText}{regularText}{keyStrokes}{mouseClicks}";
            csv.AppendLine(line.Substring(0, line.Length - 1));

            foreach (DBInput input in _inputs) {
                date = $"{input.Date},";
                application = applicationCol ? $"{input.Application}," : "";
                window = windowCol ? $"{input.Window}," : "";
                rawText = rawTextCol ? $"{input.RawText}," : "";
                regularText = regularTextCol ? $"{input.RegularText}," : "";
                keyStrokes = keyStrokesCol ? $"{input.KeyStrokes}," : "";
                mouseClicks = mouseClicksCol ? $"{input.MouseClicks}," : "";

                line = $"{date}{application}{window}{rawText}{regularText}{keyStrokes}{mouseClicks}";
                csv.AppendLine(line.Substring(0, line.Length - 1));
            }

            _SaveFile(csv.ToString(), "csv");
        }

        public void GenerateTXT(bool applicationCol, bool windowCol, bool rawTextCol, bool regularTextCol, bool keyStrokesCol, bool mouseClicksCol) {
            StringBuilder txt = new StringBuilder();

            foreach (DBInput input in _inputs) {
                string date = $"Date: {input.Date}    ";
                string application = applicationCol ? $"Application: {input.Application}    " : "";
                string window = windowCol ? $"Window: {input.Window}    " : "";
                string rawText = rawTextCol ? $"Raw Text: {input.RawText}    " : "";
                string regularText = regularTextCol ? $"Regular text: {input.RegularText}    " : "";
                string keyStrokes = keyStrokesCol ? $"Key strokes: {input.KeyStrokes}    " : "";
                string mouseClicks = mouseClicksCol ? $"MouseClicks: {input.MouseClicks}    " : "";

                string line = $"{date}{application}{window}{rawText}{regularText}{keyStrokes}{mouseClicks}";
                txt.AppendLine(line.Substring(0, line.Length - 1));
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
}
