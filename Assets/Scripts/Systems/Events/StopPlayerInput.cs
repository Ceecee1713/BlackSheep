using UnityEngine;

//This contains all the events (data types) that the game uses

public class StopPlayerInput : IEvent //Prevent player input to scripts that are subscribed to this event
{
    public bool AllowPlayerInput;

    public StopPlayerInput(bool allowPlayerInput)
    {
        AllowPlayerInput = allowPlayerInput;
    }
}

public class FadeCurrentCanvas : IEvent //Fade the current UI canvas when either the pause menu or card gameplay tutorial menu is active
{
    public bool FadeCanvas;

    public FadeCurrentCanvas(bool fadeCanvas)
    {
        FadeCanvas = fadeCanvas;
    }
}

public class ChangeToNewCanvas : IEvent //Change current UI canvas to a new one (newCanvas) and if it's a dialogue canvas, determined by (isNewCanvasADialogueCanvas)
{
    public bool IsNewCanvasADialogueCanvas;
    public GameObject NewCanvas;

    public ChangeToNewCanvas(GameObject newCanvas, bool isNewCanvasADialogueCanvas)
    {
        NewCanvas = newCanvas;
        IsNewCanvasADialogueCanvas = isNewCanvasADialogueCanvas;
    }
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

public class CardHasBeenPlayed : IEvent   //Allows or prevents for the shuffle button to be used within a card round
{
    public bool CardPlayed;

    public CardHasBeenPlayed(bool cardPlayed)
    {
        CardPlayed = cardPlayed;
    }
}

public class NextMessage : IEvent //Dialogue box script to go through messages
{
}

public class IncreaseMusicVolume : IEvent //BGM script to increase music volume after shooting a person (finish a card round)
{
}
