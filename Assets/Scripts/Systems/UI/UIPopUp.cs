using UnityEngine;

//This script is for the pause menu and tutorial card game menu
//This script is to be attached to game objects that are either the pause menu or the tutorial card game menu

public class UIPopUp : MonoBehaviour
{
    private ShouldGameBeStopped _shouldGameBeStopped;

    void Start()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
    }

    void OnEnable() //Fade current canvas slightly and don't allow player input
    {
        EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: true));
        EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: false));
    }

    void OnDisable() //Restore current canvas alpha to 100% and allow player input
    {
        if(_shouldGameBeStopped.PreventPlaying == true)
            return;

        EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: false));
        EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: true));
    }
}
