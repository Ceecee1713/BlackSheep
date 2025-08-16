using System.Collections;
using UnityEngine;

//This script is to be attached to buttons that'll show a new canvas after clicking (nextCanvasToSetActive)
//This will be controlled by the "isNextCanvasADialogueCanvas" bool

//If "isNextCanvasADialogueCanvas" is true:
//it'll mean the next canvas to be set active will be a dialogue UI and dialogue will be said once that UI is active

//If "isNextCanvasADialogueCanvas" is false, it'll mean the next canvas won't be a dialogue canvas

public class SwitchToDialogueUIButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    [SerializeField]
    private GameObject nextCanvasToSetActive; 
    [SerializeField]
    private GameObject pauseMenu;
    
    [SerializeField]
    private bool isNextCanvasADialogueCanvas = false;

    private bool _dontRepeat = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY = 0.5f;

    void Update()
    {
        if(_calledCoroutine == true)
            return;

        if(currentCanvasGroup.alpha == 1.0f && _calledCoroutine == false)
        {
            StartCoroutine(AllowClicking());
            _calledCoroutine = true;
        }
    }

    public void OnSwitchUIClick()
    {
        if(_dontRepeat == true || pauseMenu.activeSelf == true)
            return;

        if(_allowClicking == true)
        {
            _dontRepeat = true;
            AudioManager.Instance.PlayButtonSound();
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : isNextCanvasADialogueCanvas));
        }
    }

    IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
