using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DealerSprites", menuName = "Sprite SOs/Assign Sprites for Dealer")]
public class DealerSprites : ScriptableObject
{
    [Header ("For Right Hand")]
    public Sprite RightHandReachingOver;
    public Sprite RightHandGrabbingCards;
    public Sprite RightHandShuffling;
    public Sprite RightHandHoldingCards, RightHandPassingCards;

    [Header ("For Left Hand")]
    public Sprite LeftHandShuffling;
    public Sprite LeftHandHoldingCards;
}
