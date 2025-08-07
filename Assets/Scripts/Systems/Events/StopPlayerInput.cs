using UnityEngine;

//This contains all the events (data types) that the game uses

public class StopPlayerInput : IEvent
{
    public bool AllowPlayerInput;

    public StopPlayerInput(bool allowPlayerInput)
    {
        AllowPlayerInput = allowPlayerInput;
    }
}

public class FadeCurrentCanvas : IEvent
{
    public bool FadeCanvas;

    public FadeCurrentCanvas(bool fadeCanvas)
    {
        FadeCanvas = fadeCanvas;
    }
}

public class ChangeToNewCanvas : IEvent
{
    public bool IsNewCanvasADialogueCanvas;
    public GameObject NewCanvas;

    public ChangeToNewCanvas(GameObject newCanvas, bool isNewCanvasADialogueCanvas)
    {
        NewCanvas = newCanvas;
        IsNewCanvasADialogueCanvas = isNewCanvasADialogueCanvas;
    }
}

public class DisableUIPopUps : IEvent
{
}

public class ShuffleCards : IEvent     //Disable input for moving the player's cards, shuffle cards and play shuffling animation
{
}

public class CompletedShufflingCards : IEvent    //Make the shuffle button visible again and player input for moving their cards
{
}

public class FinishedRound : IEvent    //Reset the Shuffle Button's status and remove card types that can be given out to the player
{
}

public class StartNewRound : IEvent    //Reset player's cards' positions and status, shuffle the player's cards and play shuffling animation
{
}

public class CardHasBeenPlayed : IEvent   //Allows or prevents for the shuffle button to be used within a round
{
    public bool CardPlayed;

    public CardHasBeenPlayed(bool cardPlayed)
    {
        CardPlayed = cardPlayed;
    }
}


public class NextMessage : IEvent
{
}

public class IncreaseMusicVolume : IEvent
{
}
