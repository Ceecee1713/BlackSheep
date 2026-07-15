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

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    private void OnNewCardRound(StartNewRound startNewRound) //When a new card round has started
    {
        _allowInput = false;
    }

    private void OnShufflingCards(ShuffleCards shuffleCards) //When cards are being shuffled in a round
    {
        _allowInput = false;
    }

    private void AllowInput(CompletedShufflingCards completedShufflingCards) //When cards are done being shuffled
    {
        _allowInput = true;
    }

    private void DoNotAllowInput(FinishedRound finishedRound) //When the card round has finished
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
