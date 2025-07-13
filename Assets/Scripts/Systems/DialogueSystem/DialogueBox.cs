using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

//Remember to remove self from event manager as an event listener

public class DialogueBox : MonoBehaviour, EventListener
{
    [SerializeField]
    private DialogueData dialogueData;
    
    [SerializeField]
    private GameObject nextCanvasToSetActive; //UI Canvas to set active when all dialogue is said, unprompted by button clicks 

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

    public bool TypeFirstMessageOnAwake = false; //Temporary

    private int _index = -1; //Index to go through the dialogue message arrays from "dialogueData"
    
    private const float _typingSpeed = 0.015f;
    
    private bool _writeButtonOneDialogue, _writeButtonTwoDialogue; //Based on "DialogueButton" button clicks in "ChangeDialogueFromButtonEvent()"
    //Tell which dialogue from which button has been chosen 
    
    private bool _hasActivatedButtonOptions = false; 
    private bool _finishedTypingMessage = false; //Prevent going through messages when they're not fully typed out
    private bool _allowGoingThroughMessages = false; //Prevent going through messages entirely
    private bool _callDialogueCanvasSwitch = false; //Prevent repetivie calls on Update based on "CheckToSwitchDialogueCanvases()"
    private bool _isThisADialogueCanvas = false;
        
    void Awake()
    {
        EventManager.Instance.AddEventListener(this);

        if (TypeFirstMessageOnAwake == true) //Temporary statement
        {
            CanvasGroup canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1.0f;
            DisplayDialogueBox();
        }
    }

    void OnEnable() 
    {
        
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        
    }

    void Update()
    {
        if(_callDialogueCanvasSwitch == true)
            return;

        if (Input.GetMouseButtonDown(0)) //Refactor this input
        {
            CheckToSwitchDialogueCanvases();
            CheckWhichDialogueToBeSaidNext();
        }
    }
    
    private void CheckToSwitchDialogueCanvases()
    {
        //If all normal dialogue messages have been said when there's no button prompting from "ChangeDialogueFromButtonEvent()"
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && _index == dialogueData.NormalDialogue.Length-1 && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            _isThisADialogueCanvas = true;
            EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, _isThisADialogueCanvas);
            return;
        }

        //If all button one dialogue messages have been said 
        if(_writeButtonOneDialogue == true && _index == dialogueData.ButtonOneDialogue.Length-1 && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            _isThisADialogueCanvas = true;
            EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, _isThisADialogueCanvas);
            return;
        }

        //If all button two dialogue messages have been said 
        if(_writeButtonTwoDialogue == true && _index == dialogueData.ButtonTwoDialogue.Length-1 && _finishedTypingMessage == true) 
        {
            _callDialogueCanvasSwitch = true;
            _isThisADialogueCanvas = true;
            EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, _isThisADialogueCanvas);
        }
    }

    private void CheckWhichDialogueToBeSaidNext()
    {
        if (_allowGoingThroughMessages != true)
            return;
        
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && _finishedTypingMessage == true) //Go through normal dialogue
        {
            GetNormalDialogue();
            return;
        }
                        
        if(_writeButtonOneDialogue == true && _writeButtonTwoDialogue == false && _finishedTypingMessage == true) //Go through button one dialogue
        {
            GetDialogueFromButtonOne();
            return;
        }

        if(_writeButtonTwoDialogue == true && _writeButtonOneDialogue == false && _finishedTypingMessage == true) //Go through button two dialogue
            GetDialogueFromButtonTwo();  
    }
    
    public void DisplayDialogueBox() //Called by "CanvasManager"
    {
        StartCoroutine(PrepareFirstMessage());
    }

    /*
    public void ChangeCanvas(GameObject canvasToSwicthTo)
    {
        _isThisADialogueCanvas = false;
        EventManager.Instance.OnNewCanvasEvent?.Invoke(canvasToSwicthTo, _isThisADialogueCanvas);
    }
    */

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

    IEnumerator PrepareFirstMessage()
    {
        //Resetting values
        _index = -1; 
        _callDialogueCanvasSwitch = false; 
        _hasActivatedButtonOptions = false;
        _writeButtonOneDialogue = false;
        _writeButtonTwoDialogue = false;
        _allowGoingThroughMessages = true;
        
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
            yield return new WaitForSeconds(_typingSpeed); //Time in between of each character being typed out
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
