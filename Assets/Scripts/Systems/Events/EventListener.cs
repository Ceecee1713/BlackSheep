using UnityEngine;

public interface EventListener 
{
    public void OnEventCalled(AllEventNames eventName);
}

public enum AllEventNames
{
    ShuffleEvent, //Disable player input for moving cards, shuffle cards and play shuffling animation
    ShuffleEventComplete, //Make shuffle button visible again and allow card player input
    FinishedRoundEvent, //Reset Shuffle Button and remove card types that can be given out on shuffling
    NewRoundEvent, //Reset player cards' positions and status, shuffle cards and play shuffling animation
}
