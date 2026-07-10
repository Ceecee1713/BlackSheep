using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Manages swapping of and fading UI canvases alphas in the game, including publishing events after UI canvas swapping
/// </summary>
/// 
/// <remarks>
/// No other script should be swapping out or fading UI canvases
/// 
/// This script works together with scripts: "GameConfiguration", "SingleCard"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// 
/// See <see cref="SingleCard"/> for each player card is structured.
/// This script, its start method, adds itself as a listener to another script, "Dealer"  for referencing purposes
/// Thus, the cards need to be active upon start for the "Dealer" before any card game starts at the beginning of the game scene loading
/// 
///</remarks>

public class CanvasManager : MonoBehaviour 
{
    [SerializeField]
    private GameConfiguration gameConfiguration; //General game information: screen fading values, moving cards values, card game values
    
    [Header ("Main Canvases")]
    [SerializeField]
    private GameObject [] canvases; //Must contain ALL UI canvases (excluding pause menu, card gameplay tutorial pop up, plain black screen)

    [Header ("For Card Gameplay Canvas")]
    [SerializeField]
    private GameObject cardGameplayCanvas; //UI Canvas for the card game. This SHOULD BE the prefab: Card Gameplay Canvas
    [SerializeField]
    private GameObject interactableLayout; //Child object of "Card Gameplay Canvas" prefab handling all interactble child objects under it like player's cards

    private GameObject _currentCanvas; //Current active UI Canvas
    private GameObject _newCanvas;

    private CanvasGroup _interactableLayoutCanvasGroup; //Layout group of "interactableLayout"
    private CanvasGroup _currentActiveCanvasGroup, _newCanvasGroup;

    private bool _isNewUICanvasADialogueCanvas;

    private float _durationOfFade;

    private const float DELAY = 0.3f;

    void Start()
    {
        //Assign Values
        _durationOfFade = gameConfiguration.DurationOfScreenFade;
        _interactableLayoutCanvasGroup = interactableLayout.GetComponent<CanvasGroup>();
        
        AddCardsToListenToDealer();

        EventBus.Instance.Subscribe<FadeCurrentCanvas>(UIPopUpIsActive);
        EventBus.Instance.Subscribe<ChangeToNewCanvas>(SwitchCanvas);
        EventBus.Instance.Subscribe<CompletedShufflingCards>(ShowCardLayoutAfterShuffling);
    }

    //Making the card gameplay canvas active and alpha to 1.0
    //So the player's cards can add themselvers as listeners to "Dealer" script for the card game
    private void AddCardsToListenToDealer() 
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

    private void UIPopUpIsActive(FadeCurrentCanvas fadeCurrentCanvas) //When paused or when tutorial menus are up, dim the current canvas' alpha
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

    /*
    private void SwitchCanvas(ChangeToNewCanvas changeToNewCanvas) //Event method
    {
        if (changeToNewCanvas.NewCanvas == null)
            return;

        for(int i = 0; i < canvases.Length; i++)
        {
            //Checking if the canvas to change to (changeToNewCanvas.NewCanvas) exists in the "canvases" list 
            if(canvases[i] == changeToNewCanvas.NewCanvas) 
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
    */

    private void SwitchCanvas(ChangeToNewCanvas changeToNewCanvas) 
    {
        _newCanvas = changeToNewCanvas.NewCanvas;
        _isNewUICanvasADialogueCanvas = changeToNewCanvas.IsNewCanvasADialogueCanvas; 

        if (changeToNewCanvas.NewCanvas == null)
            return;

        for(int i = 0; i < canvases.Length; i++)
        {

            if (canvases[i].activeSelf == true) //Checking if a canvas is active (only one canvas)
            {
                _currentCanvas = canvases[i];
                _currentActiveCanvasGroup = canvases[i].GetComponent<CanvasGroup>();
                break;
            }

            _newCanvasGroup = _newCanvas.GetComponent<CanvasGroup>();
            StartCoroutine(FadeCanvases(_newCanvasGroup, _currentActiveCanvasGroup, _newCanvas, _currentCanvas, _isNewUICanvasADialogueCanvas));
        }
    }

    private IEnumerator FadeCanvases(CanvasGroup canvasGroupToSwitchTo, CanvasGroup currentCanvasGroup, GameObject canvasToSetActive, GameObject currentCanvas, bool isNextCanvasADialogueCanvas)
    {
        //Fade out of current active canvas and set inactive
        Tween firstTween = currentCanvasGroup.DOFade(0f, _durationOfFade);
        yield return firstTween.WaitForCompletion();
        _currentCanvas.SetActive(false);

        //Fade into new canvas and set active
        canvasToSetActive.SetActive(true);
        Tween secondTween = canvasGroupToSwitchTo.DOFade(1.0f, _durationOfFade);
        yield return secondTween.WaitForCompletion();

        if(isNextCanvasADialogueCanvas == true)
        {
            //Prompt the first message of dialogue to be said for dialogue canvas
            DialogueBox dialogueBox = canvasToSetActive.GetComponent<DialogueBox>();
            dialogueBox.DisplayDialogueBox();
            StopAllCoroutines();
        }

        else //Non-dialogue canvas
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

