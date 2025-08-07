using UnityEngine;

[CreateAssetMenu(fileName = "ShouldGameBeStopped", menuName = "Scriptable Objects/Create bool to Prevent Playing")]
public class ShouldGameBeStopped : ScriptableObject
{
    public bool PreventPlaying;
}

//This script is for controlling whether other buttons (like exit pop up, quit or switch scene buttons)
//From being able to clicked if either a switch scene button or quit button has been clicked
//It also stops dialogue from being said based on a click (refer to InputController and DialogueBox scripts)