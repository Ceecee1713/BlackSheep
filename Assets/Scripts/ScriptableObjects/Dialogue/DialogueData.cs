using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Managing all dialogues in the game
/// </summary>
/// 
/// <remarks>
/// 
/// Variables are allowed to be empty, null or false. Not every variable needs to have a value.
/// It depends on the context where the dialogue is placed, whether to branch off from other dialogue based on a button click,
/// (indicated by this scriptable object), whether or not to have variables have values to them
/// 
///</remarks>

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue Scriptable Objects/Create a New Dialogue Branch")]
public class DialogueData : ScriptableObject
{
    public bool PromptOneButtonDisplay, PromptTwoButtonDisplay;
    public string ButtonOneText, ButtonTwoText;
    public Character [] NormalDialogue;
    public Character [] ButtonOneDialogue;
    public Character [] ButtonTwoDialogue;
}

[Serializable]
public struct Character
{
    public CharacterName CharacterTitle;
    [TextArea(2,5)] public string Message; 
}

public enum CharacterName 
{
    Narrator,
    DEALER,
    Dealer,
    Nameless,
    SheepOne,
    SheepTwo, 
    Bartender,
    Personnel
}

