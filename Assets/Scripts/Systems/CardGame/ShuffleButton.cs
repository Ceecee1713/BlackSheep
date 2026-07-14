using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The card shuffle button script for the shuffle button on the "Card Gameplay Canvas"
/// </summary>

/// <remarks>
/// 
/// This script is to NOT be attached to the shuffle button on the "Card Gameplay Canvas"
/// This script is to be attached to a separate game object on the "Card Gameplay Canvas" that'll always remain active when the canvas is active
/// This is because the shuffle button game object will be change from inactive/active at times
/// 
/// This script works together with scripts: "GameConfiguration", "CardGameRoundNumber"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// See <see cref="CardGameRoundNumber"/> for how each card game round number is structured.
/// 
///</remarks>

public class ShuffleButton : MonoBehaviour
{
    [Header ("Scriptable Objects")]
    [SerializeField]
    private CardGameRoundNumber cardGameRoundNumber;
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [Header ("For Shuffle Button")]
    [SerializeField]
    private TextMeshProUGUI buttonText; //Button text of the shuffle button 
    [SerializeField]
    private GameObject button; //Shuffle button game object

    private int _numberOfShufflesPerRound = 0;
    private int _maxNumberOfShufflesPerRound;

    private bool _allowInput = false; 
    private bool _hasACardBeenPlayed = false; //Preventing input from the player

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

    private void DoNotAllowInput(FinishedRound finishedRound)
    {
        ResetButton();
        _allowInput = false;
        button.SetActive(false);
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
                EventBus.Instance.Publish(new ShuffleCards()); //Disable player input for moving the player's cards, shuffle the cards and play shuffling animation
                button.SetActive(false);
            }
        }
    }
}
