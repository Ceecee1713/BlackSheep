using UnityEngine;

//This script is to be attached to buttons that'll show "nextCanvasToSetActive" after clicking
//This will be controlled by the "isNextCanvasADialogueCanvas" 

//If "isNextCanvasADialogueCanvas" is true:
//it'll mean the next canvas to be set active will be a dialogue UI and dialogue will be said once that UI is active

//If "isNextCanvasADialogueCanvas" is false, it'll mean the next canvas won't be a dialogue canvas

public class SwitchToDialogueUIButton : MonoBehaviour
{
    [SerializeField]
    private GameObject nextCanvasToSetActive; 
    
    [SerializeField]
    private bool isNextCanvasADialogueCanvas = false;

    private bool _dontRepeat = false;

    public void OnSwitchUIClick()
    {
        if(_dontRepeat == true)
            return;

        _dontRepeat = true;
        AudioManager.Instance.PlayButtonSound();
        EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : isNextCanvasADialogueCanvas));
    }
}
