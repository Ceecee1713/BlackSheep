using UnityEngine;

public class CardShuffledEvent : BaseEvent
{
    public int TotalCards { get; }
    public int RoundNumber { get; }

    public CardShuffledEvent(int totalCards, int roundNumber)
    {
        TotalCards = totalCards;
        RoundNumber = roundNumber;
    }
}

public class RoundFinishedEvent : BaseEvent
{
    public int RoundNumber { get; }
    public bool WasSuccessful { get; }

    public RoundFinishedEvent(int roundNumber, bool wasSuccessful)
    {
        RoundNumber = roundNumber;
        WasSuccessful = wasSuccessful;
    }
}

public class NewRoundStartedEvent : BaseEvent
{
    public int RoundNumber { get; }

    public NewRoundStartedEvent(int roundNumber)
    {
        RoundNumber = roundNumber;
    }
}

public class CanvasSwitchEvent : BaseEvent
{
    public GameObject TargetCanvas { get; }
    public bool IsDialogueCanvas { get; }

    public CanvasSwitchEvent(GameObject targetCanvas, bool isDialogueCanvas)
    {
        TargetCanvas = targetCanvas;
        IsDialogueCanvas = isDialogueCanvas;
    }
}