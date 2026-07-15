using UnityEngine;

//This contains all the events (data types) that the game uses
//Scroll down to find the event you want

//Subscribers: DialogueBox, CardUIPopUpButton, ShuffleButton, SingleCard
//Publishers: UIPopUp
//Purpose: Prevent/Allow player input to scripts that are subscribed to this event
public class StopPlayerInput : IEvent 
{
    public bool AllowPlayerInput;

    public StopPlayerInput(bool allowPlayerInput)
    {
        AllowPlayerInput = allowPlayerInput;
    }
}

//Subscribers: CanvasManager
//Publishers: UIPopUp
//Purpose: Fade the current UI canvas when either the pause menu or card gameplay tutorial menu is active
public class FadeCurrentCanvas : IEvent 
{
    public bool FadeCanvas;

    public FadeCurrentCanvas(bool fadeCanvas)
    {
        FadeCanvas = fadeCanvas;
    }
}

//Subscribers: CanvasManager
//Publishers: GamblingTable, DialogueBox, ShootPersonCanvass, SwitchToDialogueUIButton
//Purpose: Change current UI canvas to a new one (newCanvas) and mark if it's a dialogue canvas, determined by (isNewCanvasADialogueCanvas)
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

//Subscribers: AnimationManager, Dealer, SingleCard, CardUIPopUpButton
//Publishers: ShuffleButton
//Purpose: Disable input for moving the player's cards and clicking on any UI buttons on the card gameplay canvas, 
// shuffle player's cards and play shuffling animation
public class ShuffleCards : IEvent  
{
}

//Subscribers: ShuffleButton, SingleCard, CanvasManager, CardUIPopUpButton
//Publishers: AnimationManager
//Purpose: Make the shuffle button visible again and allow player input for the shuffle button,
// allow player input for moving their cards, show and allow input for the interactable UI bar on card gameplay canvas
public class CompletedShufflingCards : IEvent 
{
}

//Subscribers: Dealer, BGM, SingleCard, ShuffleButton, CardUIPopUpButton
//Publishers: GamblingTable
//Purpose: Fade bgm to mute, check to remove any card types, prevent player input to move their cards, reset the shuffle button's status 
// and don't allow input on any UI buttons on the card gameplay canvas
public class FinishedRound : IEvent 
{
}

//Subscribers: Dealer, AnimationManager, BGM, SingleCard, CardUIPopUpButton
//Publishers: CanvassManager
//Purpose: Reset player's cards' positions and status, move player's cards back in their original position, restore bgm to its starting volume,
// shuffle the player's cards and play shuffling animation
public class StartNewRound : IEvent  
{
}

//Subscribers: ShuffleButton
//Publishers: GamblingTable
//Purpose: Allows or prevents for the shuffle button to be used within a card round (if a card has been played or not)
public class CardHasBeenPlayed : IEvent  
{
    public bool CardPlayed;

    public CardHasBeenPlayed(bool cardPlayed)
    {
        CardPlayed = cardPlayed;
    }
}

//Subscribers: DialogueBox
//Publishers: InputController
//Purpose: Iterate through dialogue messages
public class NextMessage : IEvent 
{
}

//Subscribers: BGM
//Publishers: ShootPersonCanvass
//Purpose: Increase music volume after shooting a person (finished a card round)
public class IncreaseMusicVolume : IEvent
{
}
