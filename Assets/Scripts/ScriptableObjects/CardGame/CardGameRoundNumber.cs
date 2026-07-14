using UnityEngine;

[CreateAssetMenu(fileName = "RoundNumber", menuName = "Scriptable Objects/Create a Value To Represent A Card Round")]
public class CardGameRoundNumber : ScriptableObject
{
    public int CurrentRoundNumber;
}

//Which scripts use this scriptable object