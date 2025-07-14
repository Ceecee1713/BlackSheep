using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CardSlot
{
    public RectTransform SlotRect; //Needs reference from Inspector
    public Image SlotImage; //Needs reference from Inspector
    public CardType SlotType;
    public bool IsSlotOccupied;
}

public class GamblingTable : Singleton<GamblingTable>
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
    public int NumberOfPlayedCards;
    [HideInInspector]
    public int RoundNumber;
    [HideInInspector]
    public bool CardHasBeenPlayed = false;

    private int _maxAmountOfRounds;
    private int _maxNumberOfPlayedCards; //Number representing max number of plays need to be met before moving to next round

    private const bool IS_NEXT_CANVAS_A_DIALOGUE_CANVAS = false;

    private void Start()
    {
        _maxAmountOfRounds = Resources.Load<GameConfiguration>("GameConfiguration").MaxAmountOfRounds;
        _maxNumberOfPlayedCards = Resources.Load<GameConfiguration>("GameConfiguration").MaxNumberOfPlayedCards;
        RoundNumber = 1;
    }

    void Update()
    {
        RoundNumber = Mathf.Clamp(RoundNumber, 1, _maxAmountOfRounds);

        if(NumberOfPlayedCards >= _maxNumberOfPlayedCards)
        {
            //Resetting all values
            NumberOfPlayedCards = 0;
            CardHasBeenPlayed = false;
            RightCardSlot.IsSlotOccupied = false;
            LeftCardSlot.IsSlotOccupied = false;
            RightCardSlot.SlotType = null;
            LeftCardSlot.SlotType = null;

            RoundNumber++;
            EventManager.Instance.OnFinishedRoundEvent.Invoke(); //Reset Shuffle Button and remove card types that can be given out on shuffling
            CheckForWhichCanvasToSwitchedTo();
        }

        if(NumberOfPlayedCards > 0)
            CardHasBeenPlayed = true;
    }

    private void CheckForWhichCanvasToSwitchedTo()
    {
        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep)
        {
            dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootSheepDialogue; //Setting dialogue for dealer to say
            EventManager.Instance.OnNewCanvasEvent?.Invoke(shootingSheepCanvas, IS_NEXT_CANVAS_A_DIALOGUE_CANVAS);
            return;
        }

        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Player || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Player)
        {
            dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootPlayerDialogue; //Setting dialogue for dealer to say
            EventManager.Instance.OnNewCanvasEvent?.Invoke(shootingPlayerCanvas, IS_NEXT_CANVAS_A_DIALOGUE_CANVAS);
            return;
        }

        if(LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer || RightCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            dealerCanvasDialogueBox.dialogueData = cardGameDialogue.RoundDialogue[RoundNumber-1].ShootDealerDialogue; //Setting dialogue for dealer to say
            EventManager.Instance.OnNewCanvasEvent?.Invoke(shootingDealerCanvas, IS_NEXT_CANVAS_A_DIALOGUE_CANVAS);
        }
    }
}

