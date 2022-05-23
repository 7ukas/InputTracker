using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace InputTracker {
    public class ApplicationChangedArgs : EventArgs {
        public string ApplicationTitle { get; private set; }
        public string WindowTitle { get; private set; }

        public ApplicationChangedArgs(string applicationTitle, string windowTitle) {
            ApplicationTitle = applicationTitle;
            WindowTitle = windowTitle;
        }
    }
}
