using System.Collections.Generic;
using UnityEngine;

//Remember to clear "_unavailableCardNumbers" and "_allCards" when dealer isn't active. 
//Remember to unsubscribe from events in Start when a new scene is loaded and such

public class Dealer : Singleton<Dealer>
{
    private List <ShuffleListener> _allCards = new(); //These will be the cards the player will interact with
    private List <int> _unavailableCardNumbers  = new(); //Prevent indexes of "_allCards" from being picked again IF number exists in this list

    private CardType [] _availableCardTypes = new CardType[5]; //All POSSIBLE cards to be given out 
    private CardType _singleCardType; //Hold a reference from "_availableCardTypes" array at a specific index
    
    private int _randomCardIndex; //Representing a random index from "_allCards"
    private int _randomCardTypeNumber; //Representing a random index from "_availableCardTypes"
    private int _roundNumberToRemoveSheepCard, _roundNumberToRemoveDealerAndNormalCards; 

    private float _delay; 

    private bool _excludeSheepCard, _onlyHavePlayerAndGunCards; //Exclude card bools for later rounds (limit cards to be given out)
    
    private void Start()
    {
        var config = Resources.Load<GameConfiguration>("GameConfiguration");// store in a local variable to avoid multiple calls

        _roundNumberToRemoveSheepCard = config.RoundNumberToRemoveSheepCard;
        _roundNumberToRemoveDealerAndNormalCards = config.RoundNumberToRemoveDealerAndNormalCards;
        _delay = config.DurationToMoveCardsUpDown;

        var cardTypes = Resources.LoadAll<CardType>("CardType");

        for(int i = 0; i < _availableCardTypes.Length; i++)
            _availableCardTypes[i] = cardTypes[i];

        EventBus.Instance.Subscribe<FinishedRound>(CheckToRemoveCardTypes);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<ShuffleCards>(ShuffleEvent);
    }


    public void AddCard(ShuffleListener card)
    {
        _allCards.Add(card);
    }

    public void RemoveCard(ShuffleListener card)
    {
        _allCards.Remove(card);
    }

    /*
    public override void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<FinishedRound>(CheckToRemoveCardTypes);
        EventBus.Instance.Unsubscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Unsubscribe<ShuffleCards>(ShuffleEvent);

        _unavailableCardNumbers.Clear();
        _allCards.Clear();
    }
    */

    private void CheckToRemoveCardTypes(FinishedRound finishedRound)
    {
        RemoveCardTypes();
    }

    private void ShuffleEvent(ShuffleCards shuffleCards)
    {
        Invoke("StartShufflingCards", _delay);
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        Invoke("StartShufflingCards", _delay);
    }

    private void RemoveCardTypes() //Limit card types to be given out to the player in later rounds
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

    /* //Temporary
    public void OnEventCalled(AllEventNames eventName) 
    {
        if(eventName == AllEventNames.ShuffleEvent || eventName == AllEventNames.NewRoundEvent)
            Invoke("StartShufflingCards", _delay);
    }
    */
