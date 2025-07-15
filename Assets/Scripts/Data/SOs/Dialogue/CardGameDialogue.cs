using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
