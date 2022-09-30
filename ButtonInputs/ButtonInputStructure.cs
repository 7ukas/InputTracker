namespace InputTracker;

internal class ButtonInputStructure {
    public int Count { get; private set; }

    private Stack<MouseButton> _buttonInputs; 

    public ButtonInputStructure() {
        _buttonInputs = new Stack<MouseButton>();
    }

    public void Add(MouseButton input) {
        _buttonInputs.Push(input);

        Count++;
    }

    public void RemoveLast() {
        if (Count > 0) {
            _buttonInputs.Pop();

            Count--;
        }
    }

    public bool Empty() {
        return Count == 0;
    }

    public void Clear() {
        _buttonInputs.Clear();
    }
}
