using UnityEngine;

//This script is for the pause menu and tutorial card game menu

public class UIPopUp : MonoBehaviour
{
    void OnEnable() //Fade current canvas slightly and don't allow player input
    {
        EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: true));
        EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: false));
    }

    void OnDisable() //Restore current canvas alpha to 100% and allow player input
    {
        if(EventBus.Instance != null)
        {
            EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: false));
            EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: true));
        }
    }
}
