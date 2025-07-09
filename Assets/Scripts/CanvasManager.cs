using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

//Remember to remove self from event manager as an event listener and a canvas listener

public class CanvasManager : MonoBehaviour, EventListener, CanvasListener
{
    [SerializeField]
    private GameObject [] canvases;

    [SerializeField]
    private GameObject cardGameplayCanvas;

    [SerializeField] 
    private float durationOfFade = 1.0f;
    
    public bool StartNewRound = false;

    private GameObject _currentCanvas;
    private CanvasGroup _currentActiveCanvasGroup, _newCanvasGroup;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
        EventManager.Instance.AddCanvasListener(this);
    }

    void Update() 
    {
        if(StartNewRound == true) 
        {
            //Refactor later
            cardGameplayCanvas.SetActive(true);
            StartNewRound = false;
            EventManager.Instance.OnNewRoundEvent.Invoke();
        }
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.FinishedRoundEvent)
        {
            //Refactor later 
            cardGameplayCanvas.SetActive(false);
        }
    }

    public void OnCanvasEventCalled(GameObject canvasToSetActive, bool isThisADialogueCanvas)
    {
        if (canvasToSetActive == null)
            return;
        
        if(isThisADialogueCanvas == true) //Fade dialogue canvases
        {
            for(int i = 0; i < canvases.Length; i++)
            {
                if(canvases[i] == canvasToSetActive) //Checking if "canvasToSetActive" exists in the "canvases" array
                {
                    SwitchCanvases(canvasToSetActive, true);
                    break;
                }
            }
        }
        
        else //Fade normal canvases
        {
            for(int i = 0; i < canvases.Length; i++)
            {
                if(canvases[i] == canvasToSetActive) //Checking if "canvasToSetActive" exists in the "canvases" array
                {
                    SwitchCanvases(canvasToSetActive, false);
                    break;
                }
            }
        }
    }

    private void SwitchCanvases(GameObject canvasToSetActive, bool isThisADialogueCanvas)
    {
        if (isThisADialogueCanvas == true) //Fade dialogue canvases
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                if (canvases[i].activeSelf == true) //Checking the active canvas (only one canvas)
                {
                    _currentCanvas = canvases[i];
                    _currentActiveCanvasGroup = canvases[i].GetComponent<CanvasGroup>();
                    break;
                }
            }

            _newCanvasGroup = canvasToSetActive.GetComponent<CanvasGroup>();
            StartCoroutine(FadeDialogueCanvases(_newCanvasGroup, _currentActiveCanvasGroup, canvasToSetActive, _currentCanvas));
        }

        else //Fade normal canvases
        {
            for (int i = 0; i < canvases.Length; i++)
            {
                if (canvases[i].activeSelf == true) //Checking the active canvas (only one canvas)
                {
                    _currentCanvas = canvases[i];
                    _currentActiveCanvasGroup = canvases[i].GetComponent<CanvasGroup>();
                    break;
                }
            }

            _newCanvasGroup = canvasToSetActive.GetComponent<CanvasGroup>();
            StartCoroutine(FadeCanvases(_newCanvasGroup, _currentActiveCanvasGroup, canvasToSetActive, _currentCanvas));
        }
    }
    
    IEnumerator FadeDialogueCanvases(CanvasGroup canvasGroupToSwitchTo, CanvasGroup currentCanvasGroup, GameObject canvasToSetActive, GameObject currentCanvas)
    {
        //Fade canvases
        Tween firstTween = currentCanvasGroup.DOFade(0f, durationOfFade);
        yield return firstTween.WaitForCompletion();
        _currentCanvas.SetActive(false);
        
        canvasToSetActive.SetActive(true);
        Tween secondTween = canvasGroupToSwitchTo.DOFade(1.0f, durationOfFade);
        yield return secondTween.WaitForCompletion();
        
        //Prompt the first message of dialogue to be said
        DialogueBox dialogueBox = canvasToSetActive.GetComponent<DialogueBox>();
        dialogueBox.DisplayDialogueBox();
        StopAllCoroutines();
    }
    
    IEnumerator FadeCanvases(CanvasGroup canvasGroupToSwitchTo, CanvasGroup currentCanvasGroup, GameObject canvasToSetActive, GameObject currentCanvas)
    {
        Tween firstTween = currentCanvasGroup.DOFade(0f, durationOfFade);
        yield return firstTween.WaitForCompletion();
        _currentCanvas.SetActive(false);
        
        canvasToSetActive.SetActive(true);
        Tween secondTween = canvasGroupToSwitchTo.DOFade(1.0f, durationOfFade);
        yield return secondTween.WaitForCompletion();
        
        StopAllCoroutines();
    }
}
