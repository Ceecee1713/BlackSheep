using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CardSlot
{
    public RectTransform SlotRect; //Needs reference from Inspector
    public CardType SlotType;
    public bool IsSlotOccupied;
}

public class GamblingTable : MonoBehaviour
{
    [SerializeField]
    private CardGameDialogue cardGameDialogue; 
    [SerializeField]
    private DialogueBox dealerCanvasDialogueBox; 

    [Header ("Canvases based on who gets shot")]
    [SerializeField]
    private GameObject shootingPlayerCanvas;
    [SerializeField]
    private GameObject shootingDealerCanvas;
    [SerializeField]
    private GameObject shootingSheepCanvas;

    public CardSlot LeftCardSlot;
    public CardSlot RightCardSlot;

    [HideInInspector]
    public int NumberOfPlayedCards = 0;
    [HideInInspector]
    public int RoundNumber;
    //[HideInInspector]
    //public bool CardHasBeenPlayed = false;

    private CardGameRoundNumber _cardGameRoundNumber;

    private int _maxAmountOfRounds;
    private int roundNumberToRemoveDealerAndNormalCards;
    private int _maxNumberOfPlayedCards; //Number representing max number of plays need to be met before moving to next round

    private const bool IS_NEXT_CANVAS_A_DIALOGUE_CANVAS = false;

    private void Start()
    {
        var config = Resources.Load<GameConfiguration>("GameConfiguration");

        _maxAmountOfRounds = config.MaxAmountOfRounds;
        _maxNumberOfPlayedCards = config.MaxNumberOfPlayedCards;
        roundNumberToRemoveDealerAndNormalCards = config.RoundNumberToRemoveDealerAndNormalCards;

        _cardGameRoundNumber = Resources.Load<CardGameRoundNumber>("RoundNumber");

        RoundNumber = 1;
        _cardGameRoundNumber.CurrentRoundNumber = RoundNumber;
    }

    void Update()
    {
        RoundNumber = Mathf.Clamp(RoundNumber, 1, _maxAmountOfRounds);

        if(NumberOfPlayedCards > 0)
            //CardHasBeenPlayed = true;
            EventBus.Instance.Publish(new CardHasBeenPlayed(cardPlayed: true));

        if(NumberOfPlayedCards >= _maxNumberOfPlayedCards)
        {
            EventBus.Instance.Publish(new FinishedRound()); //Reset the Shuffle Button's status and remove card types that can be given out to the player
            CheckForWhichCanvasToSwitchedTo();

            if(RoundNumber < roundNumberToRemoveDealerAndNormalCards)
                RoundNumber++;

            _cardGameRoundNumber.CurrentRoundNumber = RoundNumber;
        }
    }

    private void CheckForWhichCanvasToSwitchedTo()
    {
        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep)
        {
            if(RoundNumber < roundNumberToRemoveDealerAndNormalCards)
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootSheepDialogue; //Setting sheep dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingSheepCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
            return;
        }

        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Player || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Player)
        {
            if(RoundNumber < roundNumberToRemoveDealerAndNormalCards)
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootPlayerDialogue; //Setting player dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingPlayerCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
            return;
        }

        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            if(RoundNumber < roundNumberToRemoveDealerAndNormalCards)
                dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootDealerDialogue; //Setting dealer dialogue for dealer to say

            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : shootingDealerCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            ResetValues();
        }
    }

    private void ResetValues()
    {
        NumberOfPlayedCards = 0;
        //CardHasBeenPlayed = false;
        EventBus.Instance.Publish(new CardHasBeenPlayed(cardPlayed: false));
        RightCardSlot.IsSlotOccupied = false;
        LeftCardSlot.IsSlotOccupied = false;
        RightCardSlot.SlotType = null;
        LeftCardSlot.SlotType = null;
    }
}

