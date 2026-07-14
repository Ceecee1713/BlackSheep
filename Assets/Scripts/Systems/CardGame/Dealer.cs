using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acts as a manager determining the possible card types to be given to the player when it's a new card round
/// </summary>

/// <remarks>
/// 
/// Singleton — access globally via <see cref="Dealer.Instance"/>.
/// This script is to be attached to an empty game object and to NOT persist across scenes.
/// 
/// This script works together with scripts: "GameConfiguration", "CardType", "CardGameRoundNumber", "ShuffleListener"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// See <see cref="CardType"/> for how each player card's data is structured.
/// See <see cref="CardGameRoundNumber"/> for how each card game round number is structured.
/// See <see cref="ShuffleListener"/> for how each card is structured to listen for new card assignments. 
/// 
///</remarks>

public class Dealer : Singleton<Dealer>
{
    private List <ShuffleListener> _allCards = new(); //These will be the cards the player will interact with
    private List <int> _unavailableCardNumbers  = new(); 
    //Prevent indexes of "_allCards" from being picked again (preventing already chosen player cards that have gotten assigned from being picked again)

    private CardType [] _availableCardTypes = new CardType[4]; //All possible card types that can be assigned to the player cards.
    private CardType _cardTypeToBeAssignedTo; //Hold a reference from "_availableCardTypes" array at a specific index

    private CardGameRoundNumber _cardGameRoundNumber;
    
    private int _randomPlayerCardNumber; //Representing a random index from "_allCards"
    private int _randomCardTypeNumber; //Representing a random index from "_availableCardTypes"
    private int _roundNumberToRemoveSheepCard, _roundNumberToRemoveDealerAndNormalCards; 

    private bool _excludeSheepCard, _onlyHavePlayerAndGunCards; //Determine if certain cards should be excluded for later rounds

    private const float DELAY = 1.5f; 
    
    private void Start()
    {
        //Assigning Values
        var config = Resources.Load<GameConfiguration>("GameConfiguration");
        _roundNumberToRemoveSheepCard = config.RoundNumberToRemoveSheepCard;
        _roundNumberToRemoveDealerAndNormalCards = config.RoundNumberToRemoveDealerAndNormalCards;
        
        //Assigning Values
        var cardTypes = Resources.LoadAll<CardType>("CardType");
        _cardGameRoundNumber = Resources.Load<CardGameRoundNumber>("RoundNumber");

        //Adding all possible card types into a list
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

    private void RemoveCardTypes() //Limit possible card types to be assigned to the player cards
    {
        //Here, I'm adding the "cardGameRoundNumber.CurrentRoundNumber" by 1 
        //Because there's an order of operations difference as this method is called when a round ends.
        //BEFORE the current card round number ("cardGameRoundNumber.CurrentRoundNumber") value increases by 1 to indicate the card game has advanced to a new round,
        //the current card round number ("cardGameRoundNumber.CurrentRoundNumber") will be one value lower than what it actually is because of this order of operations
        //where this method is called when a round ends. Thus, there'll be an offset, so I need to add by 1 

        if(_cardGameRoundNumber.CurrentRoundNumber +1 >= _roundNumberToRemoveSheepCard)
            _excludeSheepCard = true;

        if(_cardGameRoundNumber.CurrentRoundNumber +1 >= _roundNumberToRemoveDealerAndNormalCards)
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
            if(_availableCardTypes[i].typeOfCard == AllCardTypes.Player) //Check for existing "Player" card in the possible cards list
            {
                _cardTypeToBeAssignedTo = _availableCardTypes[i];
                PassRandomCardType(_cardTypeToBeAssignedTo); //Pass "Player" card to be assigned to a player card
                break;
            }
        }

        for(int i = 0; i < _availableCardTypes.Length; i++) 
        {
            if(_availableCardTypes[i].typeOfCard == AllCardTypes.Gun) //Check for existing "Gun" card in the possible cards list
           {
                _cardTypeToBeAssignedTo = _availableCardTypes[i];
                PassRandomCardType(_cardTypeToBeAssignedTo); //Pass "Gun" card to be assigned to a player card
                break;
           }
        }

        ShuffleCards();
    }

    private void ShuffleCards() //Selecting a random card type from "_availableCardTypes" (possible cards list) to be passed
    {
        for(int amountOfRemainingCards = 0; amountOfRemainingCards < 2; amountOfRemainingCards++)
        {
            _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
            _cardTypeToBeAssignedTo = _availableCardTypes[_randomCardTypeNumber];

            if(_excludeSheepCard == true) //Keep finding cards if they equal the "Sheep" card type
            {
                while(_cardTypeToBeAssignedTo.typeOfCard == AllCardTypes.Sheep)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _cardTypeToBeAssignedTo = _availableCardTypes[_randomCardTypeNumber];
                }  
            }

            if(_onlyHavePlayerAndGunCards == true) //Keep finding cards if they equal the "Dealer" or "NoValue" card type
            {
                while(_cardTypeToBeAssignedTo.typeOfCard == AllCardTypes.Dealer)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _cardTypeToBeAssignedTo = _availableCardTypes[_randomCardTypeNumber];
                }  

                while(_cardTypeToBeAssignedTo.typeOfCard == AllCardTypes.NoValue)
                {
                    _randomCardTypeNumber = Random.Range(0, _availableCardTypes.Length);
                    _cardTypeToBeAssignedTo = _availableCardTypes[_randomCardTypeNumber];
                }  
            }

            PassRandomCardType(_cardTypeToBeAssignedTo);
        }
    }

    private void PassRandomCardType(CardType cardType) //Selecting a random player card type to be assigned a new "cardType"
    {
        _randomPlayerCardNumber = Random.Range(0, _allCards.Count); //Grab a random index of "_allCards" (all player cards)

        //Prevent already selected "_allCards" indexes from being chosen again
        //Prevent already chosen player cards that have gotten assigned from being picked again
        while(_unavailableCardNumbers.Contains(_randomPlayerCardNumber)) 
            _randomPlayerCardNumber = Random.Range(0, _allCards.Count);

        _unavailableCardNumbers.Add(_randomPlayerCardNumber); //Index of "_allCards" cannot be picked again. This player card can't be chosen again
        
        _allCards[_randomPlayerCardNumber].AssignNewCardType(cardType); //Assign card type to player card
    }
}

