using UnityEngine;

/// <summary>
/// Keeping count of the current card game round number
/// </summary>

/// <remarks>
/// 
/// This script works together with scripts: "ShuffleButton", "Dealer", "GamblingTable", "ShootPersonCanvass"
/// See <see cref="ShuffleButton"/>
/// See <see cref="Dealer"/> 
/// See <see cref="GamblingTable"/> 
/// See <see cref="ShootPersonCanvass"/> 
/// 
/// </remarks>

[CreateAssetMenu(fileName = "RoundNumber", menuName = "Scriptable Objects/Create a Value To Represent A Card Round")]
public class CardGameRoundNumber : ScriptableObject
{
    public int CurrentRoundNumber;
}
