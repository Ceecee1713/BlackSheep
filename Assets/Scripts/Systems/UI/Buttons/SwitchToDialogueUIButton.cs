using UnityEngine;

//This script is to be attached to buttons that'll show a dialogue UI or not after clicking
//This will be controlled by the "isNextCanvasADialogueCanvas" 

//If true, it'll mean the next canvas to be set active will be a dialogue UI and dialogue will be said once that UI is active
//If false, it'll mean the next canvas won't be a dialogue canvas

public class SwitchToDialogueUIButton : MonoBehaviour
{
    [SerializeField]
    private GameObject nextCanvasToSetActive; 
    
    [SerializeField]
    private bool isNextCanvasADialogueCanvas = false;

    public void OnSwitchUIClick()
    {
        EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : isNextCanvasADialogueCanvas));
    }
}
