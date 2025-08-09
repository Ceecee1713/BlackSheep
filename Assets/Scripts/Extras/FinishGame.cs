using UnityEngine;

public class FinishGame : Singleton<FinishGame>
{
    public bool HasPlayerFinishedGameOnce = false;
}

//This is to determine if the game has been completed once by the player
//To show skip dialogue buttons on DialogueBox scripts
