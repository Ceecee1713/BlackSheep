using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Remember to remove self from  "EventManager.Instance" when game isn't active and such

public class ShuffleButton : MonoBehaviour, EventListener
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
        EventManager.Instance.AddEventListener(this);
        _maxNumberOfShufflesPerRound = gameConfiguration.MaxNumberOfShufflesPerRound;
        ResetButton();

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void Update()
    {
        if(_numberOfShufflesPerRound == 0)
            button.SetActive(false);
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.FinishedRoundEvent) //AllEventNames.NewRoundEvent
        {
            ResetButton();
            _allowInput = false;
            button.SetActive(false);
        }

        if(eventName == AllEventNames.ShuffleEventComplete)
        {
            _allowInput = true;
            button.SetActive(true);
        }   
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

                EventManager.Instance.OnShuffleEvent.Invoke(); //Disable player input for moving cards, shuffle cards and play shuffling animation
                button.SetActive(false);
            }
        }
    }
}
