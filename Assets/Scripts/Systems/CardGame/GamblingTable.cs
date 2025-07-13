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

    private void Start()
    {
        _maxAmountOfRounds = Resources.Load<GameConfiguration>("GameConfiguration").MaxAmountOfRounds;
        _maxNumberOfPlayedCards = Resources.Load<GameConfiguration>("GameConfiguration").MaxNumberOfPlayedCards;
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
            EventManager.Instance.OnFinishedRoundEvent.Invoke();
        }

        if(NumberOfPlayedCards > 0)
            CardHasBeenPlayed = true;
    }
}

