using UnityEngine;

/// <summary>
/// Managing all general game information meant to be sharable across scripts that reference a scriptable object of this script
/// </summary>

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Scriptable Objects/Create Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    public float DurationOfScreenFade; //Total duration time to fade a UI canvas screen when swapping UI canvases
    public float DurationToMoveCardsUpDown; //Total duration time for cards animating moving up and down when shuffling

    public int RoundNumberToRemoveSheepCard;
    public int RoundNumberToRemoveDealerAndNormalCards; 

    public int MaxNumberOfPlayedCards; //Number representing max number of card plays need to be met before moving to next round
    //Example: "MaxNumberOfPlayedCards = 2". This means 2 cards must be played (placed down on the table) before moving to the next round

    public int MaxAmountOfRounds;
    public int MaxNumberOfShufflesPerRound;
}