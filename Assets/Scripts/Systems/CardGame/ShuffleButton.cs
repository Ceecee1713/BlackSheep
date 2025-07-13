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

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
    }

    void OnEnable()
    {
        _maxNumberOfShufflesPerRound = gameConfiguration.MaxNumberOfShufflesPerRound;
        ResetButton();
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.NewRoundEvent)
        {
            ResetButton();
        }
    }

    private void ResetButton()
    {
        button.SetActive(true);
        _numberOfShufflesPerRound = _maxNumberOfShufflesPerRound;
        buttonText.text = "Shuffle x " + _numberOfShufflesPerRound;
    }

    public void OnShuffle()
    {
        if(GamblingTable.Instance.CardHasBeenPlayed == false)
        {
           if(_numberOfShufflesPerRound > 0)
            {
                _numberOfShufflesPerRound--;
                buttonText.text = "Shuffle x " + _numberOfShufflesPerRound;

                EventManager.Instance.OnShuffleEvent.Invoke();
            }

            if(_numberOfShufflesPerRound == 0)
            {
                EventManager.Instance.RemoveEventListener(this);
                button.SetActive(false);
            } 
        }
    }
}
