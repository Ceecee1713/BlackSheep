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

    //[SerializeField]
    //private Sprite defaultCardSlotSprite; 

    [HideInInspector]
    public int NumberOfPlayedCards;
    private int maxNumberOfPlayedCards = 2; //Number representing max number of plays need to be met before moving to next round

    [HideInInspector]
    public int RoundNumber;
    private int _maxAmountOfRounds = 6;

    [HideInInspector]
    public bool CardHasBeenPlayed = false;

    void Update()
    {
        RoundNumber = Mathf.Clamp(RoundNumber, 1, _maxAmountOfRounds);

        if(NumberOfPlayedCards >= maxNumberOfPlayedCards)
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

    /*
    public void ResetCardSlotSprites()
    {
        LeftCardSlot.SlotImage.sprite = defaultCardSlotSprite;
        RightCardSlot.SlotImage.sprite = defaultCardSlotSprite;
    }

    public void SetCardSlotSprite(Image cardSlotImage, Sprite cardSprite)
    {
        cardSlotImage.sprite = cardSprite;
    }
    */
}

