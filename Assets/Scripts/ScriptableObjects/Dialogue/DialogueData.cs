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
/// This script works together with scripts: "DialogueButton", "DialogueBox"
/// See <see cref="DialogueButton"/> for button types.
/// See <see cref="DialogueBox"/> for dialogue is assigned and iterated through.
/// 
///</remarks>

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue Scriptable Objects/Create a New Dialogue Branch")]
public class DialogueData : ScriptableObject
{
    /// <summary>
    /// Show either a game object that'll display two UI buttons, or a display that'll show one UI button
    /// </summary>
    public bool PromptOneButtonDisplay, PromptTwoButtonDisplay; 

    /// <summary>
    /// Text for the UI buttons
    /// </summary>
    public string ButtonOneText, ButtonTwoText; 

    /// <summary>
    /// First dialogue to iterate through when a UI becomes active
    /// </summary>
    public Character [] NormalDialogue;

    /// <summary>
    /// Dialogue to be said if a dialogue button labelled "OptionOne" for Button Type is clicked
    /// </summary>
    public Character [] ButtonOneDialogue; 

    /// <summary>
    /// Dialogue to be said if a dialogue button labelled "OptionTwo" for Button Type is clicked
    /// </summary>
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

