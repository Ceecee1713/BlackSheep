using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlling which dialogue branches will be prompted by the card game (managing the order of dialogue branches)
/// </summary>

[CreateAssetMenu(fileName = "CardGameDialogue", menuName = "Dialogue Scriptable Objects/Create Dialogue For All Card Rounds")]
public class CardGameDialogue : ScriptableObject
{
    public RoundDialogue [] RoundDialogue = new RoundDialogue[6];
}

[Serializable]
public struct RoundDialogue
{
    public DialogueData ShootSheepDialogue;
    public DialogueData ShootDealerDialogue;
    public DialogueData ShootPlayerDialogue;
}
