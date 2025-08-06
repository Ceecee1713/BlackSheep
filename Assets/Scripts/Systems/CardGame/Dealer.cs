using System.Collections.Generic;
using UnityEngine;

public class Dealer : Singleton<Dealer>
{
    private List <ShuffleListener> _allCards = new(); //These will be the cards the player will interact with
    private List <int> _unavailableCardNumbers  = new(); //Prevent indexes of "_allCards" from being picked again IF number exists in this list

    private CardType [] _availableCardTypes = new CardType[5]; //All POSSIBLE cards to be given out 
    private CardType _singleCardType; //Hold a reference from "_availableCardTypes" array at a specific index

    private CardGameRoundNumber _cardGameRoundNumber;
    
    private int _randomCardIndex; //Representing a random index from "_allCards"
    private int _randomCardTypeNumber; //Representing a random index from "_availableCardTypes"
    private int _roundNumberToRemoveSheepCard, _roundNumberToRemoveDealerAndNormalCards; 

    private bool _excludeSheepCard, _onlyHavePlayerAndGunCards; //Exclude card bools for later rounds (limit cards to be given out)

    private const float DELAY = 1.5f; 
    
    private void Start()
    {
        var config = Resources.Load<GameConfiguration>("GameConfiguration");
        _roundNumberToRemoveSheepCard = config.RoundNumberToRemoveSheepCard;
        _roundNumberToRemoveDealerAndNormalCards = config.RoundNumberToRemoveDealerAndNormalCards;
        
        var cardTypes = Resources.LoadAll<CardType>("CardType");
        _cardGameRoundNumber = Resources.Load<CardGameRoundNumber>("RoundNumber");

        for(int i = 0; i < _availableCardTypes.Length; i++)
            _availableCardTypes[i] = cardTypes[i];

        EventBus.Instance.Subscribe<FinishedRound>(CheckToRemoveCardTypes);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<ShuffleCards>(ShuffleEvent);
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {
        _unavailableCardNumbers.Clear();
        _allCards.Clear();
    }

    public void AddCard(ShuffleListener card)
    {
        _allCards.Add(card);
    }

    public void RemoveCard(ShuffleListener card)
    {
        _allCards.Remove(card);
    }

    private void CheckToRemoveCardTypes(FinishedRound finishedRound)
    {
        RemoveCardTypes();
    }

    private void ShuffleEvent(ShuffleCards shuffleCards)
    {
        Invoke("StartShufflingCards", DELAY);
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        Invoke("StartShufflingCards", DELAY);
    }

    private void RemoveCardTypes() //Limit card types to be given out to the player in later rounds
    {
        //Here, I'm adding the "cardGameRoundNumber.CurrentRoundNumber" by 1 
        //Because there's an order of operations difference as this method is called when a round ends 
        //BEFORE the "cardGameRoundNumber.CurrentRoundNumber" value increases to indicate the card game has advanced by 1 to a new round:
        //The "_cardGameRoundNumber.CurrentRoundNumber" will be one value lower than what it actually is because of this order of operations
        //Thus, there needs to be an offset (adding by one)

        if(_cardGameRoundNumber.CurrentRoundNumber+1 >= _roundNumberToRemoveSheepCard)
            _excludeSheepCard = true;

        if(_cardGameRoundNumber.CurrentRoundNumber+1 >= _roundNumberToRemoveDealerAndNormalCards)
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

        ShuffleCards();
    }

    private void ShuffleCards() //Selecting a random card type from "_availableCardTypes"
    {
        for(int amountOfRemainingCards = 0; amountOfRemainingCards < 2; amountOfRemainingCards++)
        {
            _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
            _singleCardType = _availableCardTypes[_randomCardTypeNumber];

            if(_excludeSheepCard == true) //Keep finding cards if they equal the "Sheep" card type
            {
                while(_singleCardType.typeOfCard == AllCardTypes.Sheep)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _singleCardType = _availableCardTypes[_randomCardTypeNumber];
                }  
            }

            if(_onlyHavePlayerAndGunCards == true) //Keep finding cards if they equal the "Dealer" or "NoValue" card type
            {
                while(_singleCardType.typeOfCard == AllCardTypes.Dealer)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _singleCardType = _availableCardTypes[_randomCardTypeNumber];
                }  

                while(_singleCardType.typeOfCard == AllCardTypes.NoValue)
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
            Invoke("StartShufflingCards", DELAY);
    }
    */
