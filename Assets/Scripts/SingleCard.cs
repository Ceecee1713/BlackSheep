using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SingleCard : MonoBehaviour, ShuffleListener, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private CardType typeOfCard;

    [SerializeField]
    private Image cardIcon;

    public ShuffleEvent shuffleEvent;

    private Transform parentTransformAfterDrag;

    void OnEnable()
    {
        //ShuffleEvent.Instance.AddShuffleListener(this); //For singleton
        shuffleEvent.AddShuffleListener(this);
    }

    void OnDisable()
    {
        //ShuffleEvent.Instance.RemoveShuffleListener(this); //For singleton
        shuffleEvent.RemoveShuffleListener(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransformAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransformAfterDrag);
    }

    public void OnShuffleNotified(CardType assignedCardType)
    {
        typeOfCard = assignedCardType;
        cardIcon.sprite = typeOfCard.CardSprite;

        //Slide cards up to a max height from bottom of the screen
    }
}
