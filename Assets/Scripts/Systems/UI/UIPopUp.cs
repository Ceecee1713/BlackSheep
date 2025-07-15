using UnityEngine;

public class UIPopUp : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.Instance.OnFadeCurrentCanvasEvent.Invoke(true); //Fade current canvas in (50% transparency)
        EventManager.Instance.OnNoInputEvent.Invoke(false); //Don't allow player input
    }

    void OnDisable()
    {
        EventManager.Instance.OnFadeCurrentCanvasEvent.Invoke(false); //Restore current canvas alpha to 100%
        EventManager.Instance.OnNoInputEvent.Invoke(true); //Allow player input
    }
}
