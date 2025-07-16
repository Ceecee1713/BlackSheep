using UnityEngine;

public interface EventListener 
{
    public void OnEventCalled(AllEventNames eventName);
}

public enum AllEventNames
{
    ShuffleEvent, //Disable input for moving the player's cards, shuffle cards and play shuffling animation
    ShuffleEventComplete, //Make the shuffle button visible again and player input for moving their cards
    FinishedRoundEvent, //Reset the Shuffle Button's status and remove card types that can be given out to the player
    NewRoundEvent, //Reset player's cards' positions and status, shuffle the player's cards and play shuffling animation
}
