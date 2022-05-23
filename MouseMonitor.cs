using System.Collections.Generic;
using System.Windows.Input;

namespace InputTracker {
    public class MouseMonitor {
        private List<MouseButton> _buttons;

        public MouseMonitor() {
            _buttons = new List<MouseButton>();
        }

        public void Add(MouseButton button) {
            _buttons.Add(button);
        }

        public int Count() {
            return _buttons.Count;
        }
    }
}
