using UnityEngine;

/// <summary>
/// Overseeing whether the player has finished the game completely once.
/// </summary>

/// <remarks>
/// This script is to determine if the game has been completed once by the player.
/// If TRUE, a skip dialogue button will show on dialogue canvases to skip dialogue
/// 
/// Singleton — access globally via <see cref="FinishGame.Instance"/>.
/// 
/// This script works together with scripts: "DialogueBox", "EndScreenUI"
/// See <see cref="DialogueBox"/>
/// See <see cref="EndScreenUI"/> 
/// 
/// </remarks>

public class FinishGame : Singleton<FinishGame>
{
    public bool HasPlayerFinishedGameOnce = false;
}
