using UnityEngine;

public class CanvasManager : MonoBehaviour, EventListener
{
    [SerializeField]
    private GameObject cardGameplayCanvas;

    public bool StartNewRound = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
    }

    void OnEnable()
    {
        //EventManager.Instance.AddEventListener(this);
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    void Update()
    {
        if(StartNewRound == true)
        {
            cardGameplayCanvas.SetActive(true);
            StartNewRound = false;
            EventManager.Instance.OnNewRoundEvent.Invoke();
        }
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.FinishedRoundEvent)
        {
            cardGameplayCanvas.SetActive(false);
        }
    }
}
