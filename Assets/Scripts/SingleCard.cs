using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Remember to remove self from "EventManager.Instance" when game isn't active and such

public class SingleCard : MonoBehaviour, ShuffleListener, EventListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] 
    private Image cardIcon;
    [SerializeField] 
    private HorizontalLayoutGroup cardLayoutGroup;

    private Transform _parentTransformAfterDrag;
    private RectTransform _leftCardSlotRect, _rightCardSlotRect; 
    private CardType typeOfCard;

    private bool _cardHasBeenPlayed = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);

        _leftCardSlotRect = GamblingTable.Instance.LeftCardSlot.SlotRect;
        _rightCardSlotRect = GamblingTable.Instance.RightCardSlot.SlotRect;
    }

    void OnEnable()
    {
        Dealer.Instance.AddCard(this);
    }

    void OnDisable()
    {
        Dealer.Instance.RemoveCard(this); 
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.NewRoundEvent) //|| eventName == AllEventNames.FinishedRoundEvent) 
        {
            transform.SetParent(cardLayoutGroup.transform);
            _cardHasBeenPlayed = false;
        }
    }

    public void OnShuffleNotified(CardType assignedCardType) //Called by "Dealer" script
    {
        if(_cardHasBeenPlayed == true)
            return;

        typeOfCard = assignedCardType;
        cardIcon.sprite = typeOfCard.CardSprite;

        //Slide cards up to a max height from bottom of the screen (make cards below the screen)
    }

    //Methods below are for interactions with the mouse for dragging the GameObject this script is attached to

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(_cardHasBeenPlayed == true)
            return;

        _parentTransformAfterDrag = transform.parent; 
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_cardHasBeenPlayed == true)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_cardHasBeenPlayed == true)
            return;

        var cardTransform = transform.GetComponent<RectTransform>();

        //Move card to "_leftCardSlotRect" if there's an overlap and the left slot is empty
        if (CardsOverlap(cardTransform, _leftCardSlotRect) && GamblingTable.Instance.LeftCardSlot.IsSlotOccupied == false) 
        {
            transform.SetParent(_leftCardSlotRect);
            _cardHasBeenPlayed = true;

            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.LeftCardSlot.IsSlotOccupied = true;
        }
            
        //Move card to "_leftCardSlotRect" if there's an overlap and the right slot is empty    
        else if(CardsOverlap(cardTransform,_rightCardSlotRect) && GamblingTable.Instance.RightCardSlot.IsSlotOccupied == false)
        {
            transform.SetParent(_rightCardSlotRect);
            _cardHasBeenPlayed = true;

            GamblingTable.Instance.NumberOfPlayedCards++;
            GamblingTable.Instance.RightCardSlot.IsSlotOccupied = true;
        }
            
        else
            transform.SetParent(_parentTransformAfterDrag); //Return back to original position (its position in the "cardLayoutGroup" horizontal group before Play)
    }

    private bool CardsOverlap(RectTransform card, RectTransform cardSlot) //Checking if "cardRect" overlaps with "cardSlotRect"
    {
        var cardRect = new Rect(card.localPosition.x, card.localPosition.y, card.rect.width, card.rect.height);
        var cardSlotRect = new Rect(cardSlot.localPosition.x, cardSlot.localPosition.y, cardSlot.rect.width,
            cardSlot.rect.height);

        return cardRect.Overlaps(cardSlotRect);
    }
}