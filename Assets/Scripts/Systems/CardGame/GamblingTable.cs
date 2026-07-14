using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acts as a manager overlooking the card table ("Card Gameplay Canvas")
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to an empty game object.
/// This script holds data for the card slots that the player must place their cards down on to continue to the next round
/// and prompting to change canvases and set up new dialogue to be said
/// 
/// This script works together with scripts: "GameConfiguration", "CardGameDialogue", "CardGameRoundNumber", "DialogueBox", "SingleCard"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// See <see cref="CardGameDialogue"/> for how all dialogue for the card game is structured. 
/// See <see cref="CardGameRoundNumber"/> for how each card game round number is structured.
/// See <see cref="DialogueBox"/> for how the dialogue box is structured. 
/// See <see cref="SingleCard"/> for how this script assigns "CardSlot" values. 
/// 
///</remarks>

[Serializable]
public struct CardSlot
{
    public RectTransform SlotRect; //Needs reference from Inspector. Needs to hold a rect transform representing the card slot where the player should place down their cards on
    public CardType SlotType; //Don't need references from Inspector 
    public bool IsSlotOccupied; //Don't need references from Inspector
}

public class GamblingTable : MonoBehaviour
{
    [SerializeField]
    private CardGameDialogue cardGameDialogue; 
    [SerializeField]
    private DialogueBox dealerCanvasDialogueBox; //Dialogue box script for the dealer canvas used for round 1-4 of the card game

    [Header ("Canvases Based on Who Gets Shot")]
    [SerializeField]
    private GameObject shootingPlayerCanvas;
    [SerializeField]
    private GameObject shootingDealerCanvas;
    [SerializeField]
    private GameObject shootingSheepCanvas;

    /// <summary>
    /// Public CardSlots because "SingleCard" script needs to access values/assign new values to "CardSlot" when a player card has been played
    /// </summary>
    public CardSlot LeftCardSlot; 
    public CardSlot RightCardSlot;

    [HideInInspector]
    public int NumberOfPlayedCards = 0;
    [HideInInspector]
    public int RoundNumber;

    private CardGameRoundNumber _cardGameRoundNumber;

    private int _maxAmountOfRounds;
    private int _maxNumberOfPlayedCards; //Max number of card plays need to be met before moving to next round 

    private const bool IS_NEXT_CANVAS_A_DIALOGUE_CANVAS = false;

    private void Awake() //Assigning Values
    {
        var config = Resources.Load<GameConfiguration>("GameConfiguration");
        _maxAmountOfRounds = config.MaxAmountOfRounds;
        _maxNumberOfPlayedCards = config.MaxNumberOfPlayedCards;

        _cardGameRoundNumber = Resources.Load<CardGameRoundNumber>("RoundNumber");

        RoundNumber = 1;
        _cardGameRoundNumber.CurrentRoundNumber = RoundNumber;
    }

    void Update()
    {
        if(NumberOfPlayedCards > 0) //Alow player to interact with the card shuffle button if the player has played a card
            EventBus.Instance.Publish(new CardHasBeenPlayed(cardPlayed: true)); 

        if(NumberOfPlayedCards >= _maxNumberOfPlayedCards)
        {
            EventBus.Instance.Publish(new FinishedRound()); //Reset "ShuffleButton" script and remove card types that can be given out to the player
            CheckForWhichCanvasToSwitchedTo();

            RoundNumber++;
            _cardGameRoundNumber.CurrentRoundNumber = RoundNumber;
        }
    }

    private void CheckForWhichCanvasToSwitchedTo() //Setting up dialogue data and prompting to change to a new canvas
    {
        //Here, I'm minusing the "RoundNumber" by 1 to access the index of the "cardGameDialogue.RoundDialogue" array 
        //Because without it, the index that'll be accessed would be out of bounds and/or the wrong index that I want, being one index ahead

        //If the card type in the left card slot OR the right card slot is a sheep type
        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep)
        {
            if(RoundNumber <= cardGameDialogue.RoundDialogue.Length) 
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber -1].ShootSheepDialogue; //Setting sheep dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingSheepCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
            return;
        }

        //If the card type in the left card slot OR the right card slot is a player type
        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Player || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Player)
        {
            if(RoundNumber <= cardGameDialogue.RoundDialogue.Length) 
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber -1].ShootPlayerDialogue; //Setting player dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingPlayerCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
            return;
        }

        //If the card type in the left card slot OR the right card slot is a dealer type
        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            if(RoundNumber <= cardGameDialogue.RoundDialogue.Length) 
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber -1].ShootDealerDialogue; //Setting dealer dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingDealerCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
        }
    }

    private void ResetValues()
    {
        NumberOfPlayedCards = 0;

        EventBus.Instance.Publish(new CardHasBeenPlayed(cardPlayed: false)); //Don't allow player input to interact with the card shuffle button

        RightCardSlot.IsSlotOccupied = false;
        LeftCardSlot.IsSlotOccupied = false;

        RightCardSlot.SlotType = null;
        LeftCardSlot.SlotType = null;
    }
}

