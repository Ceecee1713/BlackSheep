using UnityEngine;

public interface EventListener 
{
    public void OnEventCalled(AllEventNames eventName);
}

public enum AllEventNames
{
    ShuffleEvent,
    FinishedRoundEvent,
    NewRoundEvent
}
