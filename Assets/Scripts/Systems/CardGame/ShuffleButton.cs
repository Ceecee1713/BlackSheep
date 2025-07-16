using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Remember to unsubscribe from the events in Awake when this game object is destroyed or when a new scene is being loaded

public class ShuffleButton : MonoBehaviour
{
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private GameObject button;

    private int _numberOfShufflesPerRound = 0;
    private int _maxNumberOfShufflesPerRound;

    private bool _allowInput = false;

    void Awake()
    {
        _maxNumberOfShufflesPerRound = gameConfiguration.MaxNumberOfShufflesPerRound;
        ResetButton();

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(SetButtonActive);
        EventBus.Instance.Subscribe<FinishedRound>(DoNotAllowInput);
    }

    void Update()
    {
        if(_numberOfShufflesPerRound == 0)
            button.SetActive(false);
    }

    private void DoNotAllowInput(FinishedRound finishedRound)
    {
        ResetButton();
        _allowInput = false;
        button.SetActive(false);
    }

    private void SetButtonActive(CompletedShufflingCards completedShufflingCards)
    {
        if(GamblingTable.Instance.RoundNumber == gameConfiguration.MaxAmountOfRounds)
            return;

        _allowInput = true;
        button.SetActive(true);
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    private void ResetButton()
    {
        _numberOfShufflesPerRound = _maxNumberOfShufflesPerRound;
        buttonText.text = "Shuffle x " + _numberOfShufflesPerRound;
    }

    public void OnShuffle()
    {
        if(_allowInput != true)
            return;

        if(GamblingTable.Instance.CardHasBeenPlayed == false)
        {
           if(_numberOfShufflesPerRound > 0)
            {
                _numberOfShufflesPerRound--;
                buttonText.text = "Shuffle x " + _numberOfShufflesPerRound;
                EventBus.Instance.Publish(new ShuffleCards()); //Disable input for moving the player's cards, shuffle cards and play shuffling animation
                button.SetActive(false);
            }
        }
    }
}
