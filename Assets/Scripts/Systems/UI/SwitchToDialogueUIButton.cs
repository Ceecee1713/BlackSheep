using UnityEngine;

public class SwitchToDialogueUIButton : MonoBehaviour, EventListener
{
    [SerializeField]
    private GameObject nextCanvasToSetActive; 
    
    [SerializeField]
    private bool isNextCanvasADialogueCanvas = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
    }
    
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveEventListener(this);
    }
    
    public void OnEventCalled(AllEventNames eventName)
    {

    }

    public void OnSwitchUIClick()
    {
        EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, isNextCanvasADialogueCanvas);
    }
}
