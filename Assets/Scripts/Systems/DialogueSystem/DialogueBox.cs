using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public DialogueData dialogueData;
    
    [Header ("For Next Canvas To Be Displayed")]
    [SerializeField]
    private GameObject nextCanvasToSetActive; //UI Canvas to set active when all dialogue is said, unprompted by button clicks 
    [SerializeField] 
    private bool _isNextCanvasADialogueCanvas = false;
    [SerializeField] 
    private bool _ignoreCanvasChanging = false; //For the dialogue box on the UI that'll determine the player's ending (with free button choices)

    [Header ("Button Displays")]
    [SerializeField]
    private GameObject oneButtonDisplay;
    [SerializeField]
    private GameObject twoButtonsDisplay;

    [Header ("Texts To Influence")]
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private TextMeshProUGUI characterNameText;
    [SerializeField] 
    private TextMeshProUGUI dialogueButtonOneText; 
    [SerializeField] 
    private TextMeshProUGUI dialogueButtonTwoText;

    private int _index = -1; //Index to go through the dialogue message arrays from "dialogueData"
    
    private bool _writeButtonOneDialogue, _writeButtonTwoDialogue; //Based on "DialogueButton" button clicks in "ChangeDialogueFromButtonEvent()"
    //Tell which dialogue from which button has been chosen 
    
    private bool _allowInput = false;
    private bool _moveToNextMessage = false;
    private bool _hasActivatedButtonOptions = false; 
    private bool _finishedTypingMessage = false; //Prevent going through messages when they're not fully typed out
    private bool _allowGoingThroughMessages = false; //Prevent going through messages entirely
    private bool _callDialogueCanvasSwitch = false; //Prevent repetivie calls on Update based on "CheckToSwitchDialogueCanvases()"

    private const float TYPING_SPEED = 0.015f;
        
    void Awake()
    {
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
    }

    private void OnNextMessageEvent(NextMessage nextMessage)
    {
        _moveToNextMessage = true;
    }

    private void ResetValues()
    {
        dialogueText.text = "";
        characterNameText.text = "";
        dialogueButtonOneText.text = "";
        dialogueButtonTwoText.text = "";

        oneButtonDisplay.SetActive(false);
        twoButtonsDisplay.SetActive(false);
    }

    private void IsInputAllowed(StopPlayerInput stopPlayerInput)
    {
        _allowInput = stopPlayerInput.AllowPlayerInput;
    }
    
    private void CheckToSwitchDialogueCanvases()
    {
        if(_ignoreCanvasChanging == true)
            return;

        //If all normal dialogue messages have been said when there's no button prompting from "ChangeDialogueFromButtonEvent()"
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
        
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && _finishedTypingMessage == true) 
        {
            GetNormalDialogue();
            return;
        }
                        
        if(_writeButtonOneDialogue == true && _writeButtonTwoDialogue == false && _finishedTypingMessage == true)
        {
            GetDialogueFromButtonOne();
            return;
        }

        if(_writeButtonTwoDialogue == true && _writeButtonOneDialogue == false && _finishedTypingMessage == true)
            GetDialogueFromButtonTwo();  
    }
    
    public void DisplayDialogueBox() //Called by "CanvasManager"
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

    public void ChangeDialogueFromButtonEvent(ButtonType buttonType) //Called by "DialogueButton" scripts
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
            
            //Type first message of dialogue prompted by button one
            _writeButtonOneDialogue = true;
            _allowGoingThroughMessages = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonOne();
        }

        if(buttonType == ButtonType.OptionTwo) //If button two was chosen (on a two button display)
        {
            if(dialogueData.PromptTwoButtonDisplay != true && dialogueData.PromptOneButtonDisplay != false)
                return;
            
            //Type first message of dialogue prompted by button two
            _writeButtonTwoDialogue = true;
            _allowGoingThroughMessages = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonTwo();
        }
    }

    public void ReceiveMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator PrepareFirstMessage()
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

    IEnumerator TypeMessage(string message)
    {
        _finishedTypingMessage = false;

        dialogueText.text = ""; //Clearing the "dialogueText".text for the new monster dialouge to be said
        
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
        StopAllCoroutines();
    }
}
