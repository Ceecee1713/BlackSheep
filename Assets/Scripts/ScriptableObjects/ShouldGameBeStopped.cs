using UnityEngine;

[CreateAssetMenu(fileName = "ShouldGameBeStopped", menuName = "Scriptable Objects/Create a Bool to Prevent Playing")]
public class ShouldGameBeStopped : ScriptableObject
{
    public bool PreventPlaying;
}

//This scriptable object is for prevent/allow whether buttons like exit pop up, quit or switch scene buttons
//can be clicked if either a switch scene button or quit button has been clicked
//It also stops advancing dialogue (refer to InputController and DialogueBox scripts)