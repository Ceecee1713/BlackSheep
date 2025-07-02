using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleCard : MonoBehaviour, ShuffleListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CardType typeOfCard;

    [SerializeField] private Image cardIcon;

    public RectTransform cardSlotRect;

    private Transform _parentTransformAfterDrag;

    void OnEnable()
    {
        //ShuffleEvent.Instance.AddShuffleListener(this);
    }

    void OnDisable()
    {
        //ShuffleEvent.Instance.RemoveShuffleListener(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parentTransformAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var cardTransform = transform.GetComponent<RectTransform>();
        
        if (CardsOverlap(cardTransform, cardSlotRect)) //Move card to card slot rect if there's an overlap
            transform.SetParent(cardSlotRect);
        else
            transform.SetParent(_parentTransformAfterDrag); //Return back to original position 
    }

    public void OnShuffleNotified(CardType assignedCardType)
    {
        typeOfCard = assignedCardType;
        cardIcon.sprite = typeOfCard.CardSprite;

        //Slide cards up to a max height from bottom of the screen
    }

    private bool CardsOverlap(RectTransform card, RectTransform cardSlot) //Checking if "cardRect" overlaps with "cardSlotRect"
    {
        var cardRect = new Rect(card.localPosition.x, card.localPosition.y, card.rect.width, card.rect.height);
        var cardSlotRect = new Rect(cardSlot.localPosition.x, cardSlot.localPosition.y, cardSlot.rect.width,
            cardSlot.rect.height);

        return cardRect.Overlaps(cardSlotRect);
    }
}