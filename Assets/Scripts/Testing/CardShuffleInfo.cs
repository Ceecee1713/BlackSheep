using UnityEngine;

public class CardShuffleInfo : IEvent
{
    public bool IsShuffled { get; set; }
}

public class PlayerFootStep : IEvent
{
    public PlayerFootStep(Vector3 pos, float speed)
    {
        PlayerLocation = pos;
        Speed = speed;
    }

    public Vector3 PlayerLocation { get; set; }
    public float Speed { get; set; }
}
