using UnityEngine;

public class UIPopUp : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: true));
        EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: false));
    }

    void OnDisable()
    {
        EventBus.Instance.Publish(new FadeCurrentCanvas(fadeCanvas: false));
        EventBus.Instance.Publish(new StopPlayerInput(allowPlayerInput: true));
    }
}
