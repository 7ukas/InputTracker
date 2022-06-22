namespace InputTracker; 

internal class ButtonClickedArgs : EventArgs {
    public MouseButton ButtonClicked { get; private set; }

    public ButtonClickedArgs(MouseButton button) {
        ButtonClicked = button;
    }
}
