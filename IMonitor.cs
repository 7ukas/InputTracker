namespace InputTracker {
    public interface IMonitor<T> {
        public void Add(T input);
        public void Remove(T input);
        public void Clear();
        public int Count();
    }
}
