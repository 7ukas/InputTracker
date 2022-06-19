using System.Collections.Generic;
using System.Windows.Input;

namespace InputTracker {
    public class MouseMonitor : IMonitor<MouseButton> {
        private List<MouseButton> _inputs;

        public MouseMonitor() {
            _inputs = new List<MouseButton>();
        }

        public void Add(MouseButton input) {
            _inputs.Add(input);
        }

        public void Remove(MouseButton input) {
            _inputs.Remove(input);
        }

        public void Clear() {
            _inputs.Clear();
        }

        public int Count() {
            return _inputs.Count;
        }
    }
}
