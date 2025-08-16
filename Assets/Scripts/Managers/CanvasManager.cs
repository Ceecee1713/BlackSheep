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
    private GameObject [] canvases; //Must contain ALL UI canvases (excluding pause menu, card gameplay tutorial pop up, plain black screen)

    [Header ("For Card Gameplay Canvas")]
    [SerializeField]
    private GameObject cardGameplayCanvas;
    [SerializeField]
    private GameObject interactableLayout;

    private GameObject _currentCanvas;
    private CanvasGroup _interactableLayoutCanvasGroup;
    private CanvasGroup _currentActiveCanvasGroup, _newCanvasGroup;

    private float _durationOfFade;

    private const float DELAY = 0.3f;

    void Start()
    {
        _durationOfFade = gameConfiguration.DurationOfScreenFade;
        _interactableLayoutCanvasGroup = interactableLayout.GetComponent<CanvasGroup>();
        
        AddCardsToListenToDealer();

        EventBus.Instance.Subscribe<FadeCurrentCanvas>(UIPopUpIsActive);
        EventBus.Instance.Subscribe<ChangeToNewCanvas>(SwitchCanvas);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(ShowCardLayoutAfterShuffling);
    }

    private void AddCardsToListenToDealer() //Add cards to listen to "Dealer" script for when it'll be time to shuffle
    {
        interactableLayout.SetActive(true);
        _interactableLayoutCanvasGroup.alpha = 1.0f;
        cardGameplayCanvas.SetActive(true);
        Invoke("TurnOffCardCanvas", DELAY);
    }
    
    private void TurnOffCardCanvas()
    {
        interactableLayout.SetActive(false);
        _interactableLayoutCanvasGroup.alpha = 0.0f;
        cardGameplayCanvas.SetActive(false);
    }

    private void ShowCardLayoutAfterShuffling(CompletedShufflingCards completedShufflingCards) 
    {
        _interactableLayoutCanvasGroup.alpha = 1.0f;
        interactableLayout.SetActive(true);
    }

    private void UIPopUpIsActive(FadeCurrentCanvas fadeCurrentCanvas) //When pause or tutorial menus are up, fade the current canvas' alpha
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i].activeSelf == true)  //Checking if a canvas is active (only one canvas)
            {
                CanvasGroup canvasGroup = canvases[i].GetComponent<CanvasGroup>();

                if(fadeCurrentCanvas.FadeCanvas == true) //Fade current canvas' alpha
                {
                    canvasGroup.alpha = 0.3f;
                    break;
                }

                else
                {
                    canvasGroup.alpha = 1.0f; //Restore current canvas' alpha
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
                EventBus.Instance.Publish(new StartNewRound()); //Reset player's cards' status, shuffle the player's cards and play shuffling animation
            }

            StopAllCoroutines();
        }  
    }
}

