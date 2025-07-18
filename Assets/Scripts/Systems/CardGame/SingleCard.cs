using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//Remember to unsubscribe from the events in Awake when this game object is destroyed or when a new scene is being loaded
//Remember to remove self from "Dealer.Instance" when this game object is destroyed or when a new scene is being loaded

public class SingleCard : MonoBehaviour, ShuffleListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private Ease ease;
    [SerializeField] 
    private Image cardIcon;

    private Transform _parentTransform, _cardParent;
    private RectTransform _leftCardSlotRect, _rightCardSlotRect; 
    private CardType _cardType;

    private Tween tween;

    private bool _cardHasBeenPlayed = false;
    private bool _allowInput = false;

    private const float TWEEN_DURATION = 0.6f;

    void Awake()
    {
        Dealer.Instance.AddCard(this);

        _cardParent = transform.parent;
        _leftCardSlotRect = GamblingTable.Instance.LeftCardSlot.SlotRect;
        _rightCardSlotRect = GamblingTable.Instance.RightCardSlot.SlotRect;

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(AllowInput);
        EventBus.Instance.Subscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<ShuffleCards>(ShuffleEvent);
    }

    private void AllowInput(CompletedShufflingCards completedShufflingCards)
    {
        _allowInput = true;
    }

    private void ShuffleEvent(ShuffleCards shuffleCards)
    {
        DoNotAllowInput();
    }

    private void FinishedRoundEvent(FinishedRound finishedRound)
    {
        DoNotAllowInput();
    }

    private void DoNotAllowInput()
    {
        _allowInput = false;
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        transform.SetParent(_cardParent);

        tween?.Kill(); 
        tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease); //Move the card to the position of its parent at 0,0,0 

        _allowInput = true;
        _cardHasBeenPlayed = false;
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    public void OnShuffleNotified(CardType assignedCardType) //Called by "Dealer" script
    {
        if(_cardHasBeenPlayed == true)
            return;

        _cardType = assignedCardType;
        cardIcon.sprite = _cardType.CardSprite;
    }

    //OnBeginDrag, OnDrag and OnEndDrag are for interactions with the mouse for dragging the GameObject this script is attached to

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_cardHasBeenPlayed == true || _allowInput == false)
            return;

        _parentTransform = transform.parent; 
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_cardHasBeenPlayed == true || _allowInput == false)
            return;

        transform.position = Input.mousePosition;
    }

    private void CheckRightCardSlot() //Check the right card slot to change the values of the left card slot
    {
        if(GamblingTable.Instance.RightCardSlot.SlotType == null) //If the right card slot is empty, let this card move into the left card slot
        {
            transform.SetParent(_leftCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign left card slot
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.LeftCardSlot.SlotType = _cardType;
            GamblingTable.Instance.LeftCardSlot.IsSlotOccupied = true;
            return; 
        }

        //Check if the right slot has a sheep, player, or dealer card
        if(GamblingTable.Instance.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || GamblingTable.Instance.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Player 
        || GamblingTable.Instance.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
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
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.LeftCardSlot.SlotType = _cardType;
            GamblingTable.Instance.LeftCardSlot.IsSlotOccupied = true;
            return; 
        }

        if(GamblingTable.Instance.RightCardSlot.SlotType.typeOfCard == AllCardTypes.Gun) //Check if the right slot has a gun card
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
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.LeftCardSlot.SlotType = _cardType;
            GamblingTable.Instance.LeftCardSlot.IsSlotOccupied = true;
        }
    }

    private void CheckLeftCardSlot() //Check left card slot to change the values of the right card slot
    {
        if(GamblingTable.Instance.LeftCardSlot.SlotType == null) //If the left card slot is empty, let this card move into the right card slot
        {
            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign right card slot
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.RightCardSlot.SlotType = _cardType;
            GamblingTable.Instance.RightCardSlot.IsSlotOccupied = true;
            return;
        }

        //Check if the left slot is either empty, has a sheep, player, or dealer card
        if(GamblingTable.Instance.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Sheep || GamblingTable.Instance.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Player 
        || GamblingTable.Instance.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Dealer)
        {
            //If this card is a sheep, player, or dealer card, return back to original position of its parent at 0,0,0 
            if(_cardType.typeOfCard == AllCardTypes.Sheep || _cardType.typeOfCard == AllCardTypes.Player || _cardType.typeOfCard == AllCardTypes.Dealer)
            {
                transform.SetParent(_parentTransform);
                tween?.Kill();
                tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
                return;
            }

            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            //Assign right card slot
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.RightCardSlot.SlotType = _cardType;
            GamblingTable.Instance.RightCardSlot.IsSlotOccupied = true;
            return; 
        }

        if(GamblingTable.Instance.LeftCardSlot.SlotType.typeOfCard == AllCardTypes.Gun) //Check if the left slot has a gun card
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
            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.RightCardSlot.SlotType = _cardType;
            GamblingTable.Instance.RightCardSlot.IsSlotOccupied = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
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

        //Move card to "_leftCardSlotRect" if there's an overlap and the left slot is empty
        if (CardsOverlap(cardTransform, _leftCardSlotRect) && GamblingTable.Instance.LeftCardSlot.IsSlotOccupied == false) 
            CheckRightCardSlot();


        //Move card to "_leftCardSlotRect" if there's an overlap and the right slot is empty    
        else if(CardsOverlap(cardTransform,_rightCardSlotRect) && GamblingTable.Instance.RightCardSlot.IsSlotOccupied == false)
            CheckLeftCardSlot();
            

        else //Move card back to its parent at 0,0,0
        {
            transform.SetParent(_parentTransform); 
            tween?.Kill();
            tween = transform.DOLocalMove(Vector3.zero, TWEEN_DURATION, false).SetEase(ease);
        }
    }

    private bool CardsOverlap(RectTransform card, RectTransform cardSlot) //Checking if "cardRect" overlaps with "cardSlotRect"
    {
        var cardRect = new Rect(card.localPosition.x, card.localPosition.y, card.rect.width, card.rect.height);
        var cardSlotRect = new Rect(cardSlot.localPosition.x, cardSlot.localPosition.y, cardSlot.rect.width,
            cardSlot.rect.height);

        return cardRect.Overlaps(cardSlotRect);
    }
}

    /* //Temporary
    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.NewRoundEvent) 
        {
            transform.SetParent(cardLayoutGroup.transform);
            _allowInput = true;
            _cardHasBeenPlayed = false;
        }
        
        if(eventName == AllEventNames.ShuffleEvent || eventName == AllEventNames.FinishedRoundEvent)
            _allowInput = false;
    }
    */