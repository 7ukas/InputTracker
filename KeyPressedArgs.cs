using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace InputTracker {
    public class KeyPressedArgs : EventArgs {
        public Key KeyPressed { get; private set; }
        public Dictionary<string, bool> Modifiers { get; private set; }

        public KeyPressedArgs(Key key, Dictionary<string, bool> modifiers) {
            KeyPressed = key;
            Modifiers = new Dictionary<string, bool>(modifiers);
        }
    }
}
