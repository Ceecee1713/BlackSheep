using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Remember to remove self from "EventManager.Instance" when game isn't active and such
//Remember to remove self from "Dealer.Instance" when game isn't active and such

public class SingleCard : MonoBehaviour, ShuffleListener, EventListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private Image cardIcon;
    [SerializeField] 
    private HorizontalLayoutGroup cardLayoutGroup;

    private Transform _parentTransformAfterDrag;
    private RectTransform _leftCardSlotRect, _rightCardSlotRect; 
    private CardType _cardType;

    private bool _cardHasBeenPlayed = false;
    private bool _allowInput = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
        Dealer.Instance.AddCard(this);

        _leftCardSlotRect = GamblingTable.Instance.LeftCardSlot.SlotRect;
        _rightCardSlotRect = GamblingTable.Instance.RightCardSlot.SlotRect;
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.NewRoundEvent) //eventName == AllEventNames.FinishedRoundEvent
        {
            transform.SetParent(cardLayoutGroup.transform);
            _allowInput = true;
            _cardHasBeenPlayed = false;
        }

        if(eventName == AllEventNames.ShuffleEvent)
            _allowInput = false;

        if(eventName == AllEventNames.ShuffleEventComplete)
            _allowInput = true;
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

        _parentTransformAfterDrag = transform.parent; 
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
            //If this card is a sheep, player, or dealer card, return back to original position (its position in the "cardLayoutGroup" horizontal group)
            if(_cardType.typeOfCard == AllCardTypes.Sheep || _cardType.typeOfCard == AllCardTypes.Player || _cardType.typeOfCard == AllCardTypes.Dealer)
            {
                transform.SetParent(_parentTransformAfterDrag); 
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
            //If this card is a gun card, return back to original position (its position in the "cardLayoutGroup" horizontal group)
            if(_cardType.typeOfCard == AllCardTypes.Gun)
            {
                transform.SetParent(_parentTransformAfterDrag);
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
            //If this card is a sheep, player, or dealer card, return back to original position (its position in the "cardLayoutGroup" horizontal group)
            if(_cardType.typeOfCard == AllCardTypes.Sheep || _cardType.typeOfCard == AllCardTypes.Player || _cardType.typeOfCard == AllCardTypes.Dealer)
            {
                transform.SetParent(_parentTransformAfterDrag);
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
            //If this card is a gun card, return back to original position (its position in the "cardLayoutGroup" horizontal group)
            if(_cardType.typeOfCard == AllCardTypes.Gun)
            {
                transform.SetParent(_parentTransformAfterDrag); 
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

        if(_cardType.typeOfCard == AllCardTypes.NoValue)
        {
            transform.SetParent(_parentTransformAfterDrag); //Return back to original position (its position in the "cardLayoutGroup" horizontal group)
            return;
        }

        var cardTransform = transform.GetComponent<RectTransform>();

        //Move card to "_leftCardSlotRect" if there's an overlap and the left slot is empty
        if (CardsOverlap(cardTransform, _leftCardSlotRect) && GamblingTable.Instance.LeftCardSlot.IsSlotOccupied == false) 
            CheckRightCardSlot();


        //Move card to "_leftCardSlotRect" if there's an overlap and the right slot is empty    
        else if(CardsOverlap(cardTransform,_rightCardSlotRect) && GamblingTable.Instance.RightCardSlot.IsSlotOccupied == false)
            CheckLeftCardSlot();
            

        else
            transform.SetParent(_parentTransformAfterDrag); //Return back to original position (its position in the "cardLayoutGroup" horizontal group)
    }

    private bool CardsOverlap(RectTransform card, RectTransform cardSlot) //Checking if "cardRect" overlaps with "cardSlotRect"
    {
        var cardRect = new Rect(card.localPosition.x, card.localPosition.y, card.rect.width, card.rect.height);
        var cardSlotRect = new Rect(cardSlot.localPosition.x, cardSlot.localPosition.y, cardSlot.rect.width,
            cardSlot.rect.height);

        return cardRect.Overlaps(cardSlotRect);
    }
}