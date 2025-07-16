using UnityEngine;

//This script is to be attached to buttons that will be on the card gameplay UI
//This script are for buttons that'll show a small pop up on top of the card gameplay UI
//Such as a small pause menu and a tutorial pop up

//Remember to unsubscribe from the events in Awake when this game object is destroyed or when a new scene is being loaded

public class CardUIPopUpButton : MonoBehaviour
{
    [SerializeField] 
    private GameObject uiPopUpToSetActive;

    private bool _allowInput = false;

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

        uiPopUpToSetActive.SetActive(true);
    }
}
