using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Controls each player card
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to every player card. More specifically, the MOVABLE player card, not the slot the player card sits in. 
/// The player cards should be movable with the mouse cursor
/// 
/// This script works together with scripts: "GamblingTable", "Dealer"
/// See <see cref="GamblingTable"/> for how the card gameplay canvas is structured with the card slots and its variables.
/// See <see cref="Dealer"/> for determining the possible card types is structured.
/// 
///</remarks>

public class SingleCard : MonoBehaviour, ShuffleListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private GamblingTable gamblingTable;

    [SerializeField] 
    private Ease ease; //Movement value of how smoothly the cards move when let go from the mouse cursor
    [SerializeField] 
    private Image cardIcon;

    private Transform _parentTransform, _cardParent; 
    //"_cardParent" is the original position of the card when it's in the player's hands
    //"_parentTransform" can be the original player card slot (when the card is in the player's hands), 
    //or a left/right card slot on the table during a card gameplay round

    private RectTransform _leftCardSlotRect, _rightCardSlotRect; 
    private CardType _cardType;

    private Tween tween;

    private bool _cardHasBeenPlayed = false; //Preventing input from the player
    private bool _allowInput = false; 

    private const float TWEEN_DURATION = 0.5f;

    void Start()
    {
        Dealer.Instance.AddCard(this);

        _cardParent = transform.parent;
        _leftCardSlotRect = gamblingTable.LeftCardSlot.SlotRect;
        _rightCardSlotRect = gamblingTable.RightCardSlot.SlotRect;

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(AllowInput);
        EventBus.Instance.Subscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<ShuffleCards>(ShuffleEvent);
    }

    //"CompletedShufflingCards" is the name of an event. Empty event
    private void AllowInput(CompletedShufflingCards completedShufflingCards) //Published by "AnimationManager"
    {
        _allowInput = true;
    }

    //"ShuffleCards" is the name of an event. Empty event
    private void ShuffleEvent(ShuffleCards shuffleCards) //Published by ""ShuffleButton"
    {
        DoNotAllowInput();
    }

    //"FinishedRound" is the name of an event. Empty event
    private void FinishedRoundEvent(FinishedRound finishedRound) //Published by "GamblingTable"
    {
        DoNotAllowInput();
    }

    private void DoNotAllowInput()
    {
        _allowInput = false;
    }

    //"StartNewRound" is the name of an event. Empty event
    private void OnNewCardRound(StartNewRound startNewRound) //Published by "CanvasManager"
    {
        _parentTransform = _cardParent;
        transform.SetParent(_parentTransform); 
        tween?.Kill();
        tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false); //Move the card to the position of its parent at 0,0,0 

        _allowInput = true;
        _cardHasBeenPlayed = false;
    }

    //Receives a "StopPlayerInput" event with parameters:
    //(bool) "AllowPlayerInput" - If TRUE = Allow player input for card interaction. If FALSE = DO NOT allow player input for card interaction
    private void IsInputAllowed(StopPlayerInput stopPlayerInput) //Published by "UIPopUp"
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    public void AssignNewCardType(CardType assignedCardType) //Called by "Dealer" script
    {
        if(_cardHasBeenPlayed == true)
            return;

        _cardType = assignedCardType;
        cardIcon.sprite = _cardType.CardSprite;
    }

    public void OnBeginDrag(PointerEventData eventData) //When mouse cursor BEGINS dragging on the game object this script is attached to
    {
        if(_cardHasBeenPlayed == true || _allowInput == false)
            return;

        AudioManager.Instance.PlayCardSound();
        _parentTransform = transform.parent; 
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) //When mouse cursor IS dragging on the game object this script is attached to
    {
        if(_cardHasBeenPlayed == true || _allowInput == false)
            return;

        transform.position = Input.mousePosition;
    }

    private void CheckRightCardSlot() //Check the right card slot's values IF this card can move into the left card slot 
    {
        if(gamblingTable.RightCardSlot.SlotType == null) //If the right card slot is empty, let this card move into the left card slot
        {
            //Let this card move into the left card slot
            transform.SetParent(_leftCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign left card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.LeftCardSlot.SlotType = _cardType;
            gamblingTable.LeftCardSlot.IsSlotOccupied = true;
            return; 
        }

        //Check if the right slot has a sheep, player, or dealer card
        if(gamblingTable.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || gamblingTable.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Player 
        || gamblingTable.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            //If this card is a sheep, player, or dealer card, return back to original position of its parent at 0,0,0 
            if(_cardType.typeOfCard == AllCardTypes.Sheep || _cardType.typeOfCard == AllCardTypes.Player || _cardType.typeOfCard == AllCardTypes.Dealer)
            {
                transform.SetParent(_parentTransform); 
                tween?.Kill();
                tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
                return; 
            }

            //Let this card move into the left card slot
            transform.SetParent(_leftCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign left card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.LeftCardSlot.SlotType = _cardType;
            gamblingTable.LeftCardSlot.IsSlotOccupied = true;
            return; 
        }

        if(gamblingTable.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Gun) //Check if the right slot has a gun card
        {
            //If this card is a gun card, return back to original position of its parent at 0,0,0 
            if(_cardType.typeOfCard == AllCardTypes.Gun)
            {
                transform.SetParent(_parentTransform);
                tween?.Kill();
                tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
                return; 
            }

            //Let this card move into the left card slot
            transform.SetParent(_leftCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign left card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.LeftCardSlot.SlotType = _cardType;
            gamblingTable.LeftCardSlot.IsSlotOccupied = true;
        }
    }

    private void CheckLeftCardSlot() //Check the left card slot's values IF this card can move into the left card slot 
    {
        if(gamblingTable.LeftCardSlot.SlotType == null) //If the left card slot is empty, let this card move into the right card slot
        {
            //Let this card move into the right card slot
            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign right card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.RightCardSlot.SlotType = _cardType;
            gamblingTable.RightCardSlot.IsSlotOccupied = true;
            return;
        }

        //Check if the left slot is either empty, has a sheep, player, or dealer card
        if(gamblingTable.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || gamblingTable.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Player 
        || gamblingTable.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            //If this card is a sheep, player, or dealer card, return back to original position of its parent at 0,0,0 
            if(_cardType.typeOfCard == AllCardTypes.Sheep || _cardType.typeOfCard == AllCardTypes.Player || _cardType.typeOfCard == AllCardTypes.Dealer)
            {
                transform.SetParent(_parentTransform);
                tween?.Kill();
                tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
                return;
            }

            //Let this card move into the right card slot
            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign right card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.RightCardSlot.SlotType = _cardType;
            gamblingTable.RightCardSlot.IsSlotOccupied = true;
            return; 
        }

        if(gamblingTable.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Gun) //Check if the left slot has a gun card
        {
            //If this card is a gun card, return back to original position of its parent at 0,0,0 
            if(_cardType.typeOfCard == AllCardTypes.Gun) 
            {
                transform.SetParent(_parentTransform); 
                tween?.Kill();
                tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
                return;
            }

            //Let this card move into the right card slot
            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign right card slot
            gamblingTable.NumberOfPlayedCards++;
            gamblingTable.RightCardSlot.SlotType = _cardType;
            gamblingTable.RightCardSlot.IsSlotOccupied = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData) //When mouse cursor HAS LET GO dragging on the game object this script is attached to
    {
        if(_cardHasBeenPlayed == true || _allowInput == false)
            return;

        if(_cardType.typeOfCard == AllCardTypes.NoValue) //Move card back to its parent at 0,0,0
        {
            transform.SetParent(_parentTransform); 
            tween?.Kill();
            tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
            return;
        }

        var cardTransform = transform.GetComponent<RectTransform>();

        //Check if this card is overlapping the left card slot of the card gameplay canvas (_leftCardSlotRect)
        if (CardsOverlap(cardTransform, _leftCardSlotRect) && gamblingTable.LeftCardSlot.IsSlotOccupied == false) 
            CheckRightCardSlot();


        //Check if this card is overlapping the right card slot of the card gameplay canvas (_rightCardSlotRect)
        else if(CardsOverlap(cardTransform,_rightCardSlotRect) && gamblingTable.RightCardSlot.IsSlotOccupied == false)
            CheckLeftCardSlot();
            

        else //Move card back to its parent at 0,0,0
        {
            transform.SetParent(_parentTransform); 
            tween?.Kill();
            tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
        }
    }

    //Checking is the player card transform overlaps with a card slot transform (either left/right card slot of the card gameplay canvas)
    private bool CardsOverlap(RectTransform card, RectTransform cardSlot) 
    {
        var cardRect = new Rect(card.localPosition.x, card.localPosition.y, card.rect.width, card.rect.height);
        var cardSlotRect = new Rect(cardSlot.localPosition.x, cardSlot.localPosition.y, cardSlot.rect.width,
            cardSlot.rect.height);

        return cardRect.Overlaps(cardSlotRect);
    }
}
