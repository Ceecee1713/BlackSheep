using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Data of a player card type
/// </summary>

[CreateAssetMenu(fileName = "CardType", menuName = "Cards Scriptable Objects/Assign a New Action Card Type")]
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
