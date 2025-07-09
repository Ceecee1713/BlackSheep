using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/Create a New Dialogue Branch")]
public class DialogueData : ScriptableObject
{
    public bool PromptOneButtonDisplay, PromptTwoButtonDisplay;
    public Character [] NormalDialogue;
    public Character [] ButtonOneDialogue;
    public Character [] ButtonTwoDialogue;
}

[Serializable]
public struct Character
{
    public CharacterName CharacterTitle;
    public string Message;
}

public enum CharacterName 
{
    Narrator,
    Dealer,
    DEALER,
    They,
    Patron, 
    Bartender,
    Personnel
}

