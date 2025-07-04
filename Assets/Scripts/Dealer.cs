using System.Collections.Generic;
using UnityEngine;

//Remember to clear "_unavailableCardNumbers" when dealer isn't active
//"_allCards" is 'cleared' by "SingleCard" scripts as they remove themselves in their scripts

public class Dealer : Singleton<Dealer>, EventListener
{
    private List <ShuffleListener> _allCards = new(); //These will be the cards the player will interact with
    private List <int> _unavailableCardNumbers  = new(); //Prevent indexes of "_allCards" from being picked again IF number exists in this list

    private CardType [] _availableCardTypes = new CardType[5]; //All POSSIBLE cards to be given out 
    private CardType _singleCardType; //Hold a reference from "_availableCardTypes" array at a specific index
    
    private int _randomCardIndex; //Representing a random index from "_allCards"
    private int _randomCardTypeNumber; //Representing a random index from "_availableCardTypes"
    private int _indexOfAvailableCardTypes = 0; //Used just to add elements to the "_availableCardTypes" arrray
    private int _roundNumberToRemoveSheepCard = 4; 
    private int _roundNumberToRemoveDealerAndNormalCards = 5;

    private bool _excludeSheepCard, _onlyHavePlayerAndGunCards; //Exclude card bools for later rounds (limit cards to be given out)
    
    private void Start()
    {
        foreach(CardType cardType in Resources.FindObjectsOfTypeAll(typeof(CardType)) as CardType[])
        {
            _availableCardTypes[_indexOfAvailableCardTypes] = cardType;
            _indexOfAvailableCardTypes++;
        }

        EventManager.Instance.AddEventListener(this);
    }

    void OnEnable()
    {
        //EventManager.Instance.AddEventListener(this);
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void AddCard(ShuffleListener card)
    {
        _allCards.Add(card);
    }

    public void RemoveCard(ShuffleListener card)
    {
        _allCards.Remove(card);
    }
    
    public void OnEventCalled(AllEventNames eventName) 
    {
        if(eventName == AllEventNames.FinishedRoundEvent)
            RemoveCardTypes();

        if(eventName == AllEventNames.ShuffleEvent || eventName == AllEventNames.NewRoundEvent)
            StartShufflingCards();
    }

    private void RemoveCardTypes()
    {
        if(GamblingTable.Instance.RoundNumber == _roundNumberToRemoveSheepCard)
            _excludeSheepCard = true;

        if(GamblingTable.Instance.RoundNumber == _roundNumberToRemoveDealerAndNormalCards)
            _onlyHavePlayerAndGunCards = true;
    }
    
    public void StartShufflingCards()
    {
        ClearCardSelectionHistory();
        GuarenteeCards();
    }
    
    private void ClearCardSelectionHistory()
    {
        _unavailableCardNumbers.Clear();
    }

    private void GuarenteeCards() //Guarentee to pass a "Player" and "Gun" card
    {
        for(int i = 0; i < _availableCardTypes.Length; i++)
        {
            if(_availableCardTypes[i].typeOfCard == AllCardTypes.Player) //Check for existing "Player" card 
            {
                _singleCardType = _availableCardTypes[i];
                PassRandomCardType(_singleCardType); //Pass "Player" card
                break;
            }
        }

        for(int i = 0; i < _availableCardTypes.Length; i++) 
        {
            if(_availableCardTypes[i].typeOfCard == AllCardTypes.Gun) //Check for existing "Gun" card 
           {
                _singleCardType = _availableCardTypes[i];
                PassRandomCardType(_singleCardType); //Pass "Gun" card
                break;
           }
        }

        if(_onlyHavePlayerAndGunCards == false)
            ShuffleCards();
    }

    private void ShuffleCards() //Selecting a random card type from "_availableCardTypes"
    {
        for(int amountOfRemainingCards = 0; amountOfRemainingCards < 2; amountOfRemainingCards++)
        {
            _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
            _singleCardType = _availableCardTypes[_randomCardTypeNumber];

            if(_excludeSheepCard == true)
            {
                while(_singleCardType.typeOfCard == AllCardTypes.Sheep)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _singleCardType = _availableCardTypes[_randomCardTypeNumber];
                }  
            }

            PassRandomCardType(_singleCardType);
        }
    }

    private void PassRandomCardType(CardType cardType) //Selecting a random card to pass the card type from "ShuffleCards"
    {
        _randomCardIndex = Random.Range(0, _allCards.Count); //Grab a random index of "_allCards"

        while(_unavailableCardNumbers.Contains(_randomCardIndex)) //Prevent already selected "_allCards" indexes from being chosen again
            _randomCardIndex = Random.Range(0, _allCards.Count);

        _unavailableCardNumbers.Add(_randomCardIndex); //Index of "_allCards" cannot be picked again
        
        _allCards[_randomCardIndex].OnShuffleNotified(cardType);
    }
}
