using System.Collections;
using UnityEngine;

/// <summary>
/// This script is to be attached to buttons that'll open a UI pop up, such as a pause menu or a tutorial pop up
/// </summary>

/// <remarks>
/// 
/// This script will be used for buttons on dialogue canvases and the card gameplay canvas
/// 
///</remarks>

public class CardUIPopUpButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    [SerializeField] 
    private GameObject uiPopUpToSetActive;

    private bool _allowInput = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY = 0.5f;

    void Start()
    {
        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(AllowInput);
        EventBus.Instance.Subscribe<ShuffleCards>(OnShufflingCards);
        EventBus.Instance.Subscribe<FinishedRound>(DoNotAllowInput);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
    }

    void OnEnable()
    {
        _allowInput = true;
    }

    void OnDisable()
    {
        _allowInput = false;
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

    //Receives a "StopPlayerInput" event with parameters:
    //(bool) "AllowPlayerInput" - If TRUE = Allow player input for button interaction. If FALSE = DO NOT allow player input for button interaction
    private void IsInputAllowed(StopPlayerInput stopPlayerInput) //Published by "UIPopUp"
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    //"StartNewRound" is the name of an event. Empty event
    private void OnNewCardRound(StartNewRound startNewRound) //Published by "CanvasManager"
    {
        _allowInput = false;
    }

    //"ShuffleCards" is the name of an event. Empty event
    private void OnShufflingCards(ShuffleCards shuffleCards) //Published by ""ShuffleButton"
    {
        _allowInput = false;
    }

    //"CompletedShufflingCards" is the name of an event. Empty event
    private void AllowInput(CompletedShufflingCards completedShufflingCards) //Published by "AnimationManager"
    {
        _allowInput = true;
    }

    //"FinishedRound" is the name of an event. Empty event
    private void DoNotAllowInput(FinishedRound finishedRound) //Published by "GamblingTable"
    {
        _allowInput = false;
    }

    public void OnShowUIPopUpClick()
    {
        if(_allowInput == false || currentCanvasGroup.alpha != 1.0f)
            return;

        if(_allowClicking == true)
        {
           AudioManager.Instance.PlayButtonSound();
            uiPopUpToSetActive.SetActive(true); 
        }
    }

    private IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
