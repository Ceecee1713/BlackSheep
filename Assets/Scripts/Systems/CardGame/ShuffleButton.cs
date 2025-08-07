using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is to be attached on a separate game object (NOT THE PARENT OF OR THE BUTTON OBJECT)
//This script controls the shuffle button to shuffle the cards for the card game on the card gameplay canvas

public class ShuffleButton : MonoBehaviour
{
    [Header ("Scriptable Objects")]
    [SerializeField]
    private CardGameRoundNumber cardGameRoundNumber;
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [Header ("For Shuffle Button")]
    [SerializeField]
    private TextMeshProUGUI buttonText;
    [SerializeField]
    private GameObject button;

    private int _numberOfShufflesPerRound = 0;
    private int _maxNumberOfShufflesPerRound;

    private bool _allowInput = false;
    private bool _hasACardBeenPlayed = false;

    void Start()
    {
        _maxNumberOfShufflesPerRound = gameConfiguration.MaxNumberOfShufflesPerRound;
        ResetButton();

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(SetButtonActive);
        EventBus.Instance.Subscribe<FinishedRound>(DoNotAllowInput);
        EventBus.Instance.Subscribe<CardHasBeenPlayed>(HasCardBeenPlayed);
    }

    void Update()
    {
        if(_numberOfShufflesPerRound == 0 || _hasACardBeenPlayed == true)
            button.SetActive(false);
    }

    private void HasCardBeenPlayed(CardHasBeenPlayed cardHasBeenPlayed) 
    {
        _hasACardBeenPlayed = cardHasBeenPlayed.CardPlayed;
    }

    private void DoNotAllowInput(FinishedRound finishedRound)
    {
        ResetButton();
        _allowInput = false;
        button.SetActive(false);
    }

    private void SetButtonActive(CompletedShufflingCards completedShufflingCards)
    {
        if(cardGameRoundNumber.CurrentRoundNumber == gameConfiguration.MaxAmountOfRounds)
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

        if(_hasACardBeenPlayed == false)
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
