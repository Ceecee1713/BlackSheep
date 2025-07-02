using System.Collections.Generic;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    //Make these load on references within code
    [SerializeField]
    private CardType [] availableCardTypes; //All POSSIBLE cards to be given out (not guarenteed)

    [SerializeField]
    private List <CardType> unavailableCardTypes = new List <CardType>(); //Limit cards to be given out. Based on the "excludeCard" bools
    
    private List <ShuffleListener> _subjectListeners = new(); //These will be the cards the player will interact with 
    private List <int> UnavailableCardNumbers { get; set; } = new(); //Prevent indexes of "_subjectListeners" from being picked again IF number exists in this list

    private CardType _singleCardType; //Hold a reference from "availableCardTypes" array at a specific index
    
    private int _randomCardIndex; //Representing a random index from "availableCardTypes"
    private int _randomCardNumber; //Representing a random index from "_subjectListeners"

    private bool _excludeSheepCard, _excludeNoValueCard, _excludeDealerCard; //Exclude card bools for later rounds (limit cards to be given out)
    
    public void StartShufflingCards()
    {
        ClearCardSelectionHistory();
        GuarenteeCards();
    }
    
    
    
    
    
    public void AddShuffleListener(ShuffleListener shuffleListener)
    {
        _subjectListeners.Add(shuffleListener);
    }

    public void RemoveShuffleListener(ShuffleListener shuffleListener)
    {
        _subjectListeners.Remove(shuffleListener);
    }

    private void NotifyShuffleListener(CardType cardType) //Method selects a random index of "_subjectListeners" and pass a "CardType"
    {
        _randomCardIndex = Random.Range(0, _subjectListeners.Count);

        while(UnavailableCardNumbers.Contains(_randomCardIndex)) //Prevent already selected "_subjectListeners" indexes from being chosen again
            _randomCardIndex = Random.Range(0, _subjectListeners.Count);

        UnavailableCardNumbers.Add(_randomCardIndex); //Index of "_subjectListeners" cannot be picked again
        
        _subjectListeners[_randomCardIndex].OnShuffleNotified(cardType);
    }
    
    
    
    
    private void ClearCardSelectionHistory()
    {
        UnavailableCardNumbers.Clear();
    }

    private void GuarenteeCards() //Guarentee to pass a "Player" and "Gun" card
    {
        for(int i = 0; i < availableCardTypes.Length; i++)
        {
            if(availableCardTypes[i].typeOfCard == AllCardTypes.Player) //Check for existing "Player" card 
            {
                _singleCardType = availableCardTypes[i];
                NotifyShuffleListener(_singleCardType); //Pass "Player" card
                break;
            }
        }

        for(int i = 0; i < availableCardTypes.Length; i++) 
        {
            if(availableCardTypes[i].typeOfCard == AllCardTypes.Gun) //Check for existing "Gun" card 
           {
                _singleCardType = availableCardTypes[i];
                NotifyShuffleListener(_singleCardType); //Pass "Gun" card
                break;
           }
        }

        ShuffleCards();
    }

    private void ShuffleCards()
    {
        _randomCardNumber = Random.Range(0, availableCardTypes.Length);
        _singleCardType = availableCardTypes[_randomCardNumber];

        //while(unavailableCardTypes.Contains(singleCardType))  //To be used for exclusion of cards in later rounds
            //randomCardNumber = Random.Range(0, availableCardTypes.Length);  //To be used for exclusion of cards in later rounds

        //singleCardType = availableCardTypes[randomCardNumber];  //To be used for exclusion of cards in later rounds
        //unavailableCardNumbers.Add(randomCardNumber);  //To be used for exclusion of cards in later rounds


        NotifyShuffleListener(_singleCardType);
    }
}
