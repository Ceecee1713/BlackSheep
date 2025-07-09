using System;
using System.Collections.Generic;
using UnityEngine;

//Remember to remove self from event manager as an event listener and a canvas listener

public class CanvasManager : MonoBehaviour, EventListener, CanvasListener
{
    [SerializeField]
    private List <GameObject> canvases = new();

    [SerializeField]
    private GameObject cardGameplayCanvas;

    public bool StartNewRound = false;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
        EventManager.Instance.AddCanvasListener(this);
    }

    void Update() 
    {
        if(StartNewRound == true) 
        {
            //Refactor 
            cardGameplayCanvas.SetActive(true);
            StartNewRound = false;
            EventManager.Instance.OnNewRoundEvent.Invoke();
        }
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.FinishedRoundEvent)
        {
            //Refactor 
            cardGameplayCanvas.SetActive(false);
        }
    }

    public void OnCanvasEventCalled(GameObject canvasToSetActive)
    {
        if(canvasToSetActive != null)
        {
            if(!canvases.Contains(canvasToSetActive))
                return;

            FadeSwitchDialogueCanvases(canvasToSetActive);
        }
    }

    private void FadeSwitchDialogueCanvases(GameObject canvasToSetActive)
    {
        for(int i = 0; i < canvases.Count; i++)
        {
            if(canvases[i].activeSelf == true) //Checking for only ONE ACTIVE CANVAS 
            {
                //Add fading animation for the canvas, THEN set it to inactive (when alpha reaches 0)
                //For this animation, have a separate canvas (ONE CANVAS) that's just completely black that'll be switched on/off
                //For the current canvas that's fading out and for the NEW canvas that'll be faded in
                canvases[i].SetActive(false);
                break;
            }
        }
                    
        canvasToSetActive.SetActive(true);
        //Add fade animation to fade the screen in, THEN call the first message method
        //For this animation, make sure the alpha is 0 upon activation, then fade in (increase its alpha)
        //And add short delay when the alpha is 1 

        DialogueBox dialogueBox = canvasToSetActive.GetComponent<DialogueBox>();
        dialogueBox.TypeFirstMessage();
    }
}
