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


//Below is what I'm using for my game

public class StopPlayerInput : IEvent
{
    public bool AllowPlayerInput;

    public StopPlayerInput(bool allowPlayerInput)
    {
        AllowPlayerInput = allowPlayerInput;
    }
}

public class FadeCurrentCanvas : IEvent
{
    public bool FadeCanvas;

    public FadeCurrentCanvas(bool fadeCanvas)
    {
        FadeCanvas = fadeCanvas;
    }
}

public class ChangeToNewCanvas : IEvent
{
    public bool IsNewCanvasADialogueCanvas;
    public GameObject NewCanvas;

    public ChangeToNewCanvas(GameObject newCanvas, bool isNewCanvasADialogueCanvas)
    {
        NewCanvas = newCanvas;
        IsNewCanvasADialogueCanvas = isNewCanvasADialogueCanvas;
    }
}
