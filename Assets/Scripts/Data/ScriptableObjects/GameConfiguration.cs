using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Scriptable Objects/Create Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    public float DurationOfScreenFade; 

    public int RoundNumberToRemoveSheepCard;
    public int RoundNumberToRemoveDealerAndNormalCards; 

    public int MaxNumberOfPlayedCards; //Number representing max number of card plays need to be met before moving to next round
    public int MaxAmountOfRounds;
    public int MaxNumberOfShufflesPerRound;
}
