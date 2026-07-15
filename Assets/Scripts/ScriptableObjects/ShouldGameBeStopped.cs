using UnityEngine;

/// <remarks>
/// 
/// This scriptable object is for prevent/allow whether certain UI buttons can be clicked if either a switch scene UI button or quit UI button has been clicked
/// This scriptable object can allow stop/allow to advance through dialogue 
/// 
/// This script works together with scripts: "GameManager", "UIPopUp", "ExitPopUpButton", "QuitButton", "SwitchSceneButton"
/// See <see cref="GameManager"/>
/// See <see cref="UIPopUp"/> 
/// See <see cref="ExitPopUpButton"/> 
/// See <see cref="QuitButton"/> 
/// See <see cref="SwitchSceneButton"/> 
/// 
/// </remarks>

[CreateAssetMenu(fileName = "ShouldGameBeStopped", menuName = "Scriptable Objects/Create a Bool to Prevent Playing")]
public class ShouldGameBeStopped : ScriptableObject
{
    public bool PreventPlaying;
}
