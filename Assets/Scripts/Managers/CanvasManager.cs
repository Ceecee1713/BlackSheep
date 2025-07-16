using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CanvasManager : MonoBehaviour 
{
    [SerializeField]
    private GameConfiguration gameConfiguration;
    
    [Header ("Main Canvases")]
    [SerializeField]
    private GameObject [] canvases; //Must contain ALL UI canvases (excluding pause menu, tutorial UI, plain black screen)

    [Header ("For Card Gameplay Canvas")]
    [SerializeField]
    private GameObject cardGameplayCanvas;
    [SerializeField]
    private GameObject interactableLayout;
    
    //public bool StartNewRound = false; //Temporary

    private float _durationOfFade;

    private GameObject _currentCanvas;
    private CanvasGroup _interactableLayoutCanvasGroup;
    private CanvasGroup _currentActiveCanvasGroup, _newCanvasGroup;

    void Awake()
    {
        _durationOfFade = gameConfiguration.DurationOfScreenFade;
        _interactableLayoutCanvasGroup = interactableLayout.GetComponent<CanvasGroup>();
        
        interactableLayout.SetActive(true);
        _interactableLayoutCanvasGroup.alpha = 1.0f;
        cardGameplayCanvas.SetActive(true);

        Invoke("TurnOffCardCanvas", 0.5f);


        EventBus.Instance.Subscribe<FadeCurrentCanvas>(ChangeCurrentCanvasAlpha);
        EventBus.Instance.Subscribe<ChangeToNewCanvas>(SwitchCanvas);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(ShowCardLayout);
    }

    /* //Temporary
    void Update() 
    {
        if(StartNewRound == true) 
        {
            cardGameplayCanvas.SetActive(true);
            interactableLayout.SetActive(true);
            StartNewRound = false;
            EventManager.Instance.OnNewRoundEvent.Invoke();
        }
    }
    */
    
    private void TurnOffCardCanvas()
    {
        interactableLayout.SetActive(false);
        _interactableLayoutCanvasGroup.alpha = 0.0f;
        cardGameplayCanvas.SetActive(false);
    }

    private void ShowCardLayout(CompletedShufflingCards completedShufflingCards)
    {
        _interactableLayoutCanvasGroup.alpha = 1.0f;
        interactableLayout.SetActive(true);
    }

    /* //Temporary
    public void OnEventCalled(AllEventNames eventName)
    {
        /* 
        if(eventName == AllEventNames.FinishedRoundEvent) 
        {
            TurnOffCardCanvas();
        }

        if(eventName == AllEventNames.NewRoundEvent) 
        {
            cardGameplayCanvas.SetActive(true);
        }
        
    }
    */

    private void ChangeCurrentCanvasAlpha(FadeCurrentCanvas fadeCurrentCanvas)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].activeSelf == true) 
            {
                CanvasGroup canvasGroup = canvases[i].GetComponent<CanvasGroup>();

                if(fadeCurrentCanvas.FadeCanvas == true)
                {
                    canvasGroup.alpha = 0.3f;
                    break;
                }

                else
                {
                    canvasGroup.alpha = 1.0f;
                    break;
                }    
            }
        }
    }

    private void SwitchCanvas(ChangeToNewCanvas changeToNewCanvas) 
    {
        if (changeToNewCanvas.NewCanvas == null)
            return;

        for(int i = 0; i < canvases.Length; i++)
        {
            if(canvases[i] == changeToNewCanvas.NewCanvas) //Checking if "changeToNewCanvas.NewCanvas" exists in the "canvases" array
            {
                SwitchCanvases(changeToNewCanvas.NewCanvas, changeToNewCanvas.IsNewCanvasADialogueCanvas);
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
                _interactableLayoutCanvasGroup.alpha = 0.0f;
                interactableLayout.SetActive(false);
            }

            if(canvasToSetActive == cardGameplayCanvas)
            {
                interactableLayout.SetActive(true);
                EventManager.Instance.OnNewRoundEvent.Invoke(); //Reset player cards' positions and status, shuffle cards and play shuffling animation
            }

            //yield return null;
            StopAllCoroutines();
        }  
    }
}
