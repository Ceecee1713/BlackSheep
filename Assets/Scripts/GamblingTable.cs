using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct CardSlot
{
    public bool IsSlotOccupied;
    public RectTransform SlotRect;
    public Image SlotImage;
    //public CardType slotType;
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

    public int RoundNumber;
    private int _maxAmountOfRounds = 6;

    [HideInInspector]
    public bool CardHasBeenPlayed = false;

    void Update()
    {
        RoundNumber = Mathf.Clamp(RoundNumber, 0, _maxAmountOfRounds);

        if(NumberOfPlayedCards >= maxNumberOfPlayedCards)
        {
            //Resetting all values
            NumberOfPlayedCards = 0;
            CardHasBeenPlayed = false;
            RightCardSlot.IsSlotOccupied = false;
            LeftCardSlot.IsSlotOccupied = false;

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

