using System.Collections;
using UnityEngine;

/// <summary>
/// Managing a UI button that, when clicked, will prompt to show a new UI canvas 
/// </summary>

/// <remarks>
/// This script is to be attached to buttons that'll prompt a new canvas after clicking (nextCanvasToSetActive)
/// This will be controlled by the "isNextCanvasADialogueCanvas" bool
///
/// If "isNextCanvasADialogueCanvas" is true:
/// it'll mean the next canvas to be set active will be a dialogue UI and dialogue will be said once that UI is active
///
/// If "isNextCanvasADialogueCanvas" is false, it'll mean the next canvas won't be a dialogue canvas
/// 
/// </remarks>

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

    private bool _hasBeenClicked = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY_BEFORE_ALLOWING_CLICKING = 0.2f;

    void OnDisable()
    {
        _calledCoroutine = false;
        _allowClicking = false;
    }

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
        if(_hasBeenClicked == true || pauseMenu.activeSelf == true)
            return;

        if(_allowClicking == true)
        {
            _hasBeenClicked = true;
            AudioManager.Instance.PlayButtonSound();
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : isNextCanvasADialogueCanvas)); //Publish to "CanvasManager"
        }
    }

    private IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ALLOWING_CLICKING);
        _allowClicking = true;
        yield return null;
    }
}
