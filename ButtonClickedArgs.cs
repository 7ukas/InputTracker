using System;
using System.Windows.Input;

namespace InputTracker {
    public class ButtonClickedArgs : EventArgs {
        public MouseButton ButtonClicked { get; private set; }

        public ButtonClickedArgs(MouseButton button) {
            ButtonClicked = button;
        }
    }
}
