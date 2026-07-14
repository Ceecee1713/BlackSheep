using System;
using UnityEngine;

/// <summary>
/// Interface for each player card ("SingleCard") to inherit from
/// </summary>

/// <remarks>
/// 
/// This script allows for the "Dealer" script and "SingleCard' scripts to interact while being decoupled. 
/// This script allows for the player cards to be assigned a card type determined by the "Dealer" script
/// 
/// See <see cref="Dealer"/> for determining the possible card types is structured.
/// See <see cref="SingleCard"/> for how each player card is structured.
/// See <see cref="CardType"/> for how each player card's data is structured.
/// 
///</remarks>

public interface ShuffleListener
{
    public void AssignNewCardType(CardType assignedCardType);
}
