using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

/// <summary>
/// Acts as the dialogue box 
/// </summary>

/// <remarks>
/// 
/// This script is to be a UI canvas game object, not the text that types out the dialogue
/// 
/// This script holds data for the card slots that the player must place their cards down on to continue to the next round
/// and prompting to change canvases and set up new dialogue to be said
/// 
/// This script works together with scripts: "DialogueData", "GamblingTable", "DialogueButton", "CanvasManager", "SkipDialogueButton", "FewMessagesButton"
/// See <see cref="DialogueData"/> for dialogue data is structured. 
/// See <see cref="GamblingTable"/> for how this script assigns "dialogueData"
/// See <see cref="DialogueButton"/> for how this script's public method is accessed
/// See <see cref="CanvasManager"/> for how this script's public method is accessed. 
/// See <see cref="SkipDialogueButton"/> for how this script's public method is accessed. 
/// See <see cref="FewMessagesButton"/> for how this script's public method is accessed. 
/// 
///</remarks>

public class DialogueBox : MonoBehaviour
{
    /// <summary>
    /// Public because "GamblingTable" script needs to access values/assign this value for the dealer dialogue on the dealer canvas for card game round 1-4
    /// </summary>
    public DialogueData dialogueData;

    public GameObject PauseMenuCanvas;
    
    [SerializeField]
    private GameObject mouseImage;
    
    [Header ("For Next Canvas To Be Displayed")]
    [SerializeField]
    private GameObject nextCanvasToSetActive; //UI Canvas to set active when all dialogue is said
    [SerializeField] 
    private bool _isNextCanvasADialogueCanvas = false;
    [SerializeField] 
    private bool _ignoreCanvasChanging = false; 
    //For the dialogue box on the player canvas for card game round 5 determining ending UI to prevent canvas changing when all dialogue is said

    [Header ("Button Displays")] 
    [SerializeField]
    private GameObject oneButtonDisplay; //Display that has one chocie button
    [SerializeField]
    private GameObject twoButtonsDisplay;  //Display that has two choice buttons
    [SerializeField]
    private GameObject skipButton;

    [Header ("Texts To Influence")]
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private TextMeshProUGUI characterNameText;
    [SerializeField] 
    private TextMeshProUGUI dialogueButtonOneText; //Button text from one button that's a child of "twoButtonsDisplay"
    [SerializeField] 
    private TextMeshProUGUI dialogueButtonTwoText; //Button text from one button that's a child of "twoButtonsDisplay"

    private CanvasGroup _canvasGroup;

    private int _index = -1; //Index to go through the dialogue message arrays from "dialogueData"
    
    private bool _writeButtonOneDialogue, _writeButtonTwoDialogue; //Determine which dialogue to write from whichever choice button has been clicked on
    
    private bool _allowInput = false;
    private bool _moveToNextMessage = false;
    private bool _hasActivatedButtonOptions = false; 
    private bool _finishedTypingMessage = false; //Prevent or allow going through messages when they're not fully typed out
    private bool _allowGoingThroughMessages = false; //Prevent or allow going through messages entirely
    private bool _callDialogueCanvasSwitch = false; //Prevent or allow repetivie calls on Update 

    private const float TYPING_SPEED = 0.015f;
        
    void Start()
    {
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();

        EventBus.Instance.Subscribe<StopPlayerInput>(IsInputAllowed);
        EventBus.Instance.Subscribe<NextMessage>(OnNextMessageEvent);
    }

    void OnEnable() 
    {
    }

    void OnDisable()
    {
        ResetValues();
    }

    void Update()
    {
        if(_callDialogueCanvasSwitch == true)
            return;

        if (_moveToNextMessage == true && _allowInput == true)
        {
            _moveToNextMessage = false;
            CheckToSwitchDialogueCanvases();
            CheckWhichDialogueToBeSaidNext();
        }

        if(_canvasGroup.alpha == 1.0f && FinishGame.Instance.HasPlayerFinishedGameOnce == true)
            skipButton.SetActive(true);
    }

    private void OnNextMessageEvent(NextMessage nextMessage) //Called by "InputController"
    {
        _moveToNextMessage = true;
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }

    private void ResetValues()
    {
        dialogueText.text = "";
        characterNameText.text = "";
        dialogueButtonOneText.text = "";
        dialogueButtonTwoText.text = "";

        StopAllCoroutines();
        
        skipButton.SetActive(false);
        mouseImage.SetActive(false);
        oneButtonDisplay.SetActive(false);
        twoButtonsDisplay.SetActive(false);
    }
    
    private void CheckToSwitchDialogueCanvases()
    {
        if(_ignoreCanvasChanging == true)
            return;

        //If all normal dialogue messages have been said when there's no button prompting different dialogue branches
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && _index == dialogueData.NormalDialogue.Length-1 && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : _isNextCanvasADialogueCanvas));
            return;
        }

        //If all button one dialogue messages have been said 
        if(_writeButtonOneDialogue == true && _index == dialogueData.ButtonOneDialogue.Length-1 && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : _isNextCanvasADialogueCanvas));
            return;
        }

        //If all button two dialogue messages have been said 
        if(_writeButtonTwoDialogue == true && _index == dialogueData.ButtonTwoDialogue.Length-1 && _finishedTypingMessage == true) 
        {
            _callDialogueCanvasSwitch = true;
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : _isNextCanvasADialogueCanvas));
        }
    }

    private void CheckWhichDialogueToBeSaidNext()
    {
        if (_allowGoingThroughMessages != true)
            return;
        
        //If there's no button prompting different dialogue branches and dialogue is done typing
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && _finishedTypingMessage == true) 
        {
            GetNormalDialogue();
            return;
        }

        //If there's button one prompting dialogue and dialogue is done typing      
        if(_writeButtonOneDialogue == true && _writeButtonTwoDialogue == false && _finishedTypingMessage == true)
        {
            GetDialogueFromButtonOne();
            return;
        }

        //If there's button two prompting dialogue and dialogue is done typing
        if(_writeButtonTwoDialogue == true && _writeButtonOneDialogue == false && _finishedTypingMessage == true)
            GetDialogueFromButtonTwo();  
    }

    /// <summary>
    /// Public as this method is called from "SkipDialogueButton"
    /// </summary>
    public void SkipDialogue() 
    {
        if(_ignoreCanvasChanging == true)
            return;

        _callDialogueCanvasSwitch = true;
        EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : _isNextCanvasADialogueCanvas));
    }
    
    /// <summary>
    /// Public as this method is called from "CanvasManager". 
    /// "CanvasManager" grabs this script as a component from a game object 
    /// </summary>
    public void DisplayDialogueBox() 
    {
        StartCoroutine(PrepareFirstMessage());
    }

    private void GetNormalDialogue()
    {
        if(_index+1 == dialogueData.NormalDialogue.Length || dialogueData.NormalDialogue == null) //If _index is out of bounds or dialogue array is null
            return;

        _index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.NormalDialogue[_index].Message));
        characterNameText.text = dialogueData.NormalDialogue[_index].CharacterTitle.ToString();
    }

    private void GetDialogueFromButtonOne()
    {
        if(_index+1 == dialogueData.ButtonOneDialogue.Length || dialogueData.ButtonOneDialogue == null) //If _index is out of bounds or dialogue array is null
            return;

        _index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.ButtonOneDialogue[_index].Message));
        characterNameText.text = dialogueData.ButtonOneDialogue[_index].CharacterTitle.ToString();
    }

    private void GetDialogueFromButtonTwo()
    {
        if(_index+1 == dialogueData.ButtonTwoDialogue.Length || dialogueData.ButtonTwoDialogue == null) //If _index is out of bounds or dialogue array is null
            return;

        _index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.ButtonTwoDialogue[_index].Message));
        characterNameText.text = dialogueData.ButtonTwoDialogue[_index].CharacterTitle.ToString();
    }

    /// <summary>
    /// Public as this method is called from "DialogueButton". 
    /// </summary>
    public void ChangeDialogueFromButtonEvent(ButtonType buttonType) 
    {
        if(buttonType == ButtonType.OptionOne && oneButtonDisplay.activeSelf == true) //If button one was chosen on a one button display
        {
            if(dialogueData.PromptOneButtonDisplay != true && dialogueData.PromptTwoButtonDisplay != false) 
                return;
            
            //Type first message of dialogue prompted by button one
            _writeButtonOneDialogue = true;
            _allowGoingThroughMessages = true;
            oneButtonDisplay.SetActive(false);
            GetDialogueFromButtonOne();
        }

        if(buttonType == ButtonType.OptionOne && twoButtonsDisplay.activeSelf == true) //If button one was chosen on a two button display
        {
            if(dialogueData.PromptTwoButtonDisplay != true && dialogueData.PromptOneButtonDisplay != false)
                return;
            
            //Type first message of dialogue prompted by button one on a two button display
            _writeButtonOneDialogue = true;
            _allowGoingThroughMessages = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonOne();
        }

        if(buttonType == ButtonType.OptionTwo) //If button two was chosen on a two button display
        {
            if(dialogueData.PromptTwoButtonDisplay != true && dialogueData.PromptOneButtonDisplay != false)
                return;
            
            //Type first message of dialogue prompted by button two on a two button display
            _writeButtonTwoDialogue = true;
            _allowGoingThroughMessages = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonTwo();
        }
    }

    /// <summary>
    /// Public as this method is called from "FewMessagesButton". 
    /// </summary>
    public void ReceiveMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    private IEnumerator PrepareFirstMessage()
    {
        //Resetting values
        _index = -1; 
        _callDialogueCanvasSwitch = false; 
        _hasActivatedButtonOptions = false;
        _writeButtonOneDialogue = false;
        _writeButtonTwoDialogue = false;
        _allowGoingThroughMessages = true;
        _allowInput = true;
        
        
        GetNormalDialogue();
        yield return null;
    }

    private IEnumerator TypeMessage(string message)
    {
        mouseImage.SetActive(false);
        _finishedTypingMessage = false;

        dialogueText.text = ""; //Clearing the "dialogueText".text for the new dialouge to be said
        
        foreach (char letter in message.ToCharArray()) //Conversion of string to a char array to mimick a "typing" effect of dialouge
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(TYPING_SPEED); //Time in between of each character being typed out
        } 

        yield return null;

        if(_index+1 == dialogueData.NormalDialogue.Length && _hasActivatedButtonOptions == false) //Show button displays if normal dialogue has all been said 
        {
            if(dialogueData.PromptOneButtonDisplay == true && dialogueData.PromptTwoButtonDisplay == false) //Show one button display
            {
                _index = -1; //Reset
                dialogueButtonOneText.text = dialogueData.ButtonOneText;
                oneButtonDisplay.SetActive(true);
                
                _finishedTypingMessage = true;
                _hasActivatedButtonOptions = true; //Prevent looping of "if" statement being called
                _allowGoingThroughMessages = false; //Prevent going through dialogue entirely
                StopAllCoroutines();
            }

            if(dialogueData.PromptTwoButtonDisplay == true && dialogueData.PromptOneButtonDisplay == false) //Show two button display
            {
                _index = -1; //Reset
                dialogueButtonOneText.text = dialogueData.ButtonOneText;
                dialogueButtonTwoText.text = dialogueData.ButtonTwoText;
                twoButtonsDisplay.SetActive(true);
                
                _finishedTypingMessage = true;
                _hasActivatedButtonOptions = true; //Prevent looping of "if" statement being called
                _allowGoingThroughMessages = false; //Prevent going through dialogue entirely
                StopAllCoroutines();
            }
        }

        _finishedTypingMessage = true;
        mouseImage.SetActive(true);
        StopAllCoroutines();
    }
}
