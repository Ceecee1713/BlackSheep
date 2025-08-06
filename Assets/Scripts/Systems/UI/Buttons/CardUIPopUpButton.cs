using System.Collections;
using UnityEngine;

//This script is to be attached to buttons that will be on the card gameplay UI
//This script are for buttons that'll show a small pop up on top of the card gameplay UI
//Such as a small pause menu and a tutorial pop up

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

    void Awake()
    {
        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(AllowInput);
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

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        _allowInput = false;
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    private void AllowInput(CompletedShufflingCards completedShufflingCards)
    {
        _allowInput = true;
    }

    private void DoNotAllowInput(FinishedRound finishedRound)
    {
        _allowInput = false;
    }

    public void OnShowUIPopUpClick()
    {
        if(_allowInput == false)
            return;

        if(_allowClicking == true)
        {
           AudioManager.Instance.PlayButtonSound();
            uiPopUpToSetActive.SetActive(true); 
        }
    }

    IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
