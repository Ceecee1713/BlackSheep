using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardType", menuName = "Cards/Assign a New Action Card Type")]
public class CardType : ScriptableObject
{
    public AllCardTypes typeOfCard;
    public Sprite CardSprite;
}

public enum AllCardTypes 
{
    Sheep,
    Player,
    Dealer,
    NoValue, 
    Gun
}
