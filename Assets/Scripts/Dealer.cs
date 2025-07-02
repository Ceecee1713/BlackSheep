using System.Collections.Generic;
using UnityEngine;

public class Dealer : ShuffleEvent
{
    [SerializeField]
    private CardType [] AvailableCardTypes; //All POSSIBLE cards to be given out (not guarenteed)

    [SerializeField]
    private List <CardType> UnavailableCardTypes = new List <CardType>(); //Limit cards to be given out. Based on the "excludeCard" bools

    private CardType singleCardType; //Hold a reference from "AvailableCardTypes" array at a specific index

    private int randomCardNumber;

    private bool excludeSheepCard, excludeNoValueCard, excludeDealerCard; //Exclude card bools for later rounds (limit cards to be given out)

    public void StartShuffle()
    {
        ClearCardSelectionHistory();
        GuarenteeCards();
    }

    private void GuarenteeCards() //Guarentee to pass a "Player" and "Gun" card
    {
        for(int i = 0; i < AvailableCardTypes.Length; i++)
        {
            if(AvailableCardTypes[i].typeOfCard == AllCardTypes.Player) //Check for existing "Player" card 
            {
                singleCardType = AvailableCardTypes[i];
                NotifyShuffleListener(singleCardType); //Pass "Player" card
                break;
            }
        }

        for(int i = 0; i < AvailableCardTypes.Length; i++) 
        {
           if(AvailableCardTypes[i].typeOfCard == AllCardTypes.Gun) //Check for existing "Gun" card 
            {
                singleCardType = AvailableCardTypes[i];
                NotifyShuffleListener(singleCardType); //Pass "Gun" card
                break;
            }
        }

        ShuffleCards();
    }

    private void ShuffleCards()
    {
        randomCardNumber = Random.Range(0, AvailableCardTypes.Length);
        singleCardType = AvailableCardTypes[randomCardNumber];

        //while(UnavailableCardTypes.Contains(singleCardType))  //To be used for exclusion of cards in later rounds
            //randomCardNumber = Random.Range(0, AvailableCardTypes.Length);  //To be used for exclusion of cards in later rounds

        //singleCardType = AvailableCardTypes[randomCardNumber];  //To be used for exclusion of cards in later rounds
        //unavailableCardNumbers.Add(randomCardNumber);  //To be used for exclusion of cards in later rounds


        NotifyShuffleListener(singleCardType);
    }
}
