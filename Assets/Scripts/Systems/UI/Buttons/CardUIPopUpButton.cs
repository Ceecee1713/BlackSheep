using UnityEngine;

//This script is to be attached to buttons that will be on the card gameplay UI
//This script are for buttons that'll show a small pop up on top of the card gameplay UI
//Such as a small pause menu and a tutorial pop up

public class CardUIPopUpButton : MonoBehaviour, EventListener
{
    [SerializeField] 
    private GameObject uiPopUpToSetActive;

    private bool _allowInput = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.ShuffleEventComplete)
            _allowInput = true;

        if(eventName == AllEventNames.FinishedRoundEvent)
            _allowInput = false;
    }
    
    public void OnNoInputEventCalled(bool allowInput)
    {
        _allowInput = allowInput;
    }

    public void OnShowUIPopUpClick()
    {
        if(_allowInput == false)
            return;

        uiPopUpToSetActive.SetActive(true);
    }
}
