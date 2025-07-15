using UnityEngine;

public class SwitchToDialogueUIButton : MonoBehaviour
{
    [SerializeField]
    private GameObject nextCanvasToSetActive; 
    
    [SerializeField]
    private bool isNextCanvasADialogueCanvas = false;

    public void OnSwitchUIClick()
    {
        EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, isNextCanvasADialogueCanvas);
    }
}
