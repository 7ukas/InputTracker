namespace InputTracker;

internal interface IMonitor<T> {
    public void Add(T input);
    public void RemoveLast();
    public void Clear();
    public int Count();
}
