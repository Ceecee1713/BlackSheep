using UnityEngine;

public interface EventListener 
{
    public void OnEventCalled(AllEventNames eventName);
}

public interface CanvasListener 
{
    public void OnCanvasEventCalled(GameObject canvasToSetActive, bool isThisADialogueCanvas);
}

public enum AllEventNames
{
    ShuffleEvent,
    FinishedRoundEvent,
    NewRoundEvent,
    SwitchCanvas,
}
