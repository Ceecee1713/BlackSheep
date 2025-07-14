using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

//Remember to remove self from event manager as an event listener and a canvas listener

public class CanvasManager : MonoBehaviour, EventListener, CanvasListener
{
    [SerializeField]
    private GameConfiguration gameConfiguration;
    
    [SerializeField]
    private GameObject [] canvases; //Must contain ALL UI canvases

    [SerializeField]
    private GameObject cardGameplayCanvas, cardsLayout;
    
    //public bool StartNewRound = false; //Temporary

    private float _durationOfFade;

    private GameObject _currentCanvas;
    private CanvasGroup _cardsLayoutCanvasGroup;
    private CanvasGroup _currentActiveCanvasGroup, _newCanvasGroup;

    void Awake()
    {
        _durationOfFade = gameConfiguration.DurationOfScreenFade;
        
        EventManager.Instance.AddEventListener(this);
        EventManager.Instance.AddCanvasListener(this);

        _cardsLayoutCanvasGroup = cardsLayout.GetComponent<CanvasGroup>();
        cardsLayout.SetActive(true);
        cardGameplayCanvas.SetActive(true);
        Invoke("TurnOffCardCanvas", 0.5f);
    }

    /*
    void Update() 
    {
        if(StartNewRound == true) //Temporary
        {
            cardGameplayCanvas.SetActive(true);
            cardsLayout.SetActive(true);
            StartNewRound = false;
            EventManager.Instance.OnNewRoundEvent.Invoke();
        }
    }
    */
    
    private void TurnOffCardCanvas()
    {
        cardsLayout.SetActive(false);
        _cardsLayoutCanvasGroup.alpha = 0.0f;
        cardGameplayCanvas.SetActive(false);
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.ShuffleEventComplete)
        {
            _cardsLayoutCanvasGroup.alpha = 1.0f;
        }

        /*
        if(eventName == AllEventNames.FinishedRoundEvent) //Temporary
        {
            TurnOffCardCanvas();
        }

        if(eventName == AllEventNames.NewRoundEvent) //Temporary
        {
            cardGameplayCanvas.SetActive(true);
        }
        */
    }

    public void OnCanvasEventCalled(GameObject canvasToSetActive, bool isNextCanvasADialogueCanvas)
    {
        if (canvasToSetActive == null)
            return;

        for(int i = 0; i < canvases.Length; i++)
        {
            if(canvases[i] == canvasToSetActive) //Checking if "canvasToSetActive" exists in the "canvases" array
            {
                SwitchCanvases(canvasToSetActive, isNextCanvasADialogueCanvas);
                break;
            }
        }
    }

    private void SwitchCanvases(GameObject canvasToSetActive, bool isNextCanvasADialogueCanvas)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].activeSelf == true) //Checking if a canvas is active (only one canvas)
            {
                _currentCanvas = canvases[i];
                _currentActiveCanvasGroup = canvases[i].GetComponent<CanvasGroup>();
                break;
            }
        }

        _newCanvasGroup = canvasToSetActive.GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvases(_newCanvasGroup, _currentActiveCanvasGroup, canvasToSetActive, _currentCanvas, isNextCanvasADialogueCanvas));
    }

    IEnumerator FadeCanvases(CanvasGroup canvasGroupToSwitchTo, CanvasGroup currentCanvasGroup, GameObject canvasToSetActive, GameObject currentCanvas, bool isNextCanvasADialogueCanvas)
    {
        //Fade out of current active canvas
        Tween firstTween = currentCanvasGroup.DOFade(0f, _durationOfFade);
        yield return firstTween.WaitForCompletion();
        _currentCanvas.SetActive(false);

        //Fade into new canvas
        canvasToSetActive.SetActive(true);
        Tween secondTween = canvasGroupToSwitchTo.DOFade(1.0f, _durationOfFade);
        yield return secondTween.WaitForCompletion();

        if(isNextCanvasADialogueCanvas == true)
        {
            //Prompt the first message of dialogue to be said
            DialogueBox dialogueBox = canvasToSetActive.GetComponent<DialogueBox>();
            dialogueBox.DisplayDialogueBox();
            //yield return null;
            StopAllCoroutines();
        }

        else
        {
            if(currentCanvas == cardGameplayCanvas)
            {
                _cardsLayoutCanvasGroup.alpha = 0.0f;
                cardsLayout.SetActive(false);
            }

            if(canvasToSetActive == cardGameplayCanvas)
            {
                cardsLayout.SetActive(true);
                EventManager.Instance.OnNewRoundEvent.Invoke(); //Reset player cards' positions and status, shuffle cards and play shuffling animation
            }

            //yield return null;
            StopAllCoroutines();
        }  
    }
}
