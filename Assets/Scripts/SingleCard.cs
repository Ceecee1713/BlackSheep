using UnityEngine;
using UnityEngine.UI;

public class SingleCard : MonoBehaviour, ShuffleListener
{
    [SerializeField]
    private CardType typeOfCard;

    [SerializeField]
    private Image cardIcon;

    public ShuffleEvent shuffleEvent;

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

    public void OnShuffleNotified(CardType assignedCardType)
    {
        typeOfCard = assignedCardType;
        cardIcon.sprite = typeOfCard.CardSprite;

        //Slide cards up to a max height from bottom of the screen
    }
}
