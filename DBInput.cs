namespace InputTracker {
    public class DBInput {
        public string Date { get; set; }
        public string Application { get; set; }
        public string Window { get; set; }
        public string RawText { get; set; }
        public string RegularText { get; set; }
        public int KeyStrokes { get; set; }
        public int MouseClicks { get; set; }
    }
}
