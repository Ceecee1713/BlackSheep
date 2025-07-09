using System;
using System.Collections;
using UnityEngine;
using TMPro;

//Remember to remove self from event manager as an event listener

public class DialogueBox : MonoBehaviour, EventListener
{
    [SerializeField]
    private DialogueData dialogueData;

    [SerializeField]
    private GameObject nextCanvasToSetActive; //UI Canvas to set active when all dialogue is said 

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
    private float typingSpeed = 0.05f;

    public bool aaa = false; //Temporary

    private int index = -1; //Index to go through the dialogue message arrays from "dialogueData"

    private bool _writeButtonOneDialogue, _writeButtonTwoDialogue; //Influenced inside "ChangeDialogueFromButtonEvent" method (based on "DialogueButton" button clicks)
    private bool _hasActivatedButtonOptions = false; //Stop looping from turning button options again after one instance
    private bool _finishedTypingMessage = false; //Prevent going through messages when they're not fully typed out
    private bool _callDialogueCanvasSwitch = false; //Prevent repetivie calls on Update based on an event call

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);

        if(aaa == true) //Refactor
            GetNormalDialogue();
    }

    void OnEnable() 
    {
        //EventManager.Instance.AddEventListener(this);
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        /*
        if(eventName == AllEventNames.)
        {
            
        }
        */
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
        if(_writeButtonOneDialogue == false && _writeButtonTwoDialogue == false && index+1 == dialogueData.NormalDialogue.Length && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            EventManager.Instance.OnNewDialogueCanvasEvent?.Invoke(nextCanvasToSetActive);
            return;
        }

        if(_writeButtonOneDialogue == true && index+1 == dialogueData.ButtonOneDialogue.Length && _finishedTypingMessage == true)
        {
            _callDialogueCanvasSwitch = true;
            EventManager.Instance.OnNewDialogueCanvasEvent?.Invoke(nextCanvasToSetActive);
            return;
        }

        if(_writeButtonTwoDialogue == true && index == dialogueData.ButtonTwoDialogue.Length && _finishedTypingMessage == true) 
        {
            _callDialogueCanvasSwitch = true;
            EventManager.Instance.OnNewDialogueCanvasEvent?.Invoke(nextCanvasToSetActive);
            return;
        }
    }

    private void CheckWhichDialogueToBeSaidNext()
    {
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

    public void TypeFirstMessage() //Called by "CanvasManager"
    {
        GetNormalDialogue();
    }

    private void GetNormalDialogue()
    {
        if(index+1 == dialogueData.NormalDialogue.Length || dialogueData.NormalDialogue == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.NormalDialogue[index].Message));
        characterNameText.text = dialogueData.NormalDialogue[index].CharacterTitle.ToString();
    }

    private void GetDialogueFromButtonOne()
    {
        if(index+1 == dialogueData.ButtonOneDialogue.Length || dialogueData.ButtonOneDialogue == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.ButtonOneDialogue[index].Message));
        characterNameText.text = dialogueData.ButtonOneDialogue[index].CharacterTitle.ToString();
    }

    private void GetDialogueFromButtonTwo()
    {
        if(index+1 == dialogueData.ButtonTwoDialogue.Length || dialogueData.ButtonTwoDialogue == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(dialogueData.ButtonTwoDialogue[index].Message));
        characterNameText.text = dialogueData.ButtonTwoDialogue[index].CharacterTitle.ToString();
    }

    public void ChangeDialogueFromButtonEvent(ButtonType buttonType) //Called by "DialogueButton" scripts
    {
        if(buttonType == ButtonType.OptionOne && oneButtonDisplay.activeSelf == true) //If button one was chosen on a one button display
        {
            if(dialogueData.PromptOneButtonDisplay != true && dialogueData.PromptTwoButtonDisplay != false) 
                return;

            index = -1;
            _writeButtonOneDialogue = true;
            oneButtonDisplay.SetActive(false);
            GetDialogueFromButtonOne();
        }

        if(buttonType == ButtonType.OptionOne && twoButtonsDisplay.activeSelf == true) //If button one was chosen on a two button display
        {
            if(dialogueData.PromptTwoButtonDisplay != true && dialogueData.PromptOneButtonDisplay != false)
                return;

            index = -1;
            _writeButtonOneDialogue = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonOne();
        }

        if(buttonType == ButtonType.OptionTwo) //If button two was chosen (No need for a two button display check)
        {
            if(dialogueData.PromptTwoButtonDisplay != true && dialogueData.PromptOneButtonDisplay != false)
                return;

            index = -1;
            _writeButtonTwoDialogue = true;
            twoButtonsDisplay.SetActive(false);
            GetDialogueFromButtonTwo();
        }
    }

    IEnumerator TypeMessage(string message)
    {
        _finishedTypingMessage = false;

        dialogueText.text = ""; //Clearing the "dialogueText".text for the new monster dialouge to be said
        
        foreach (char letter in message.ToCharArray()) //Conversion of string to a char array to mimick a "typing" effect of dialouge
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Time in between of each character being typed out
        } 

        yield return null;

        if(index+1 == dialogueData.NormalDialogue.Length && _hasActivatedButtonOptions == false) //Show the button displays based on "dialogueData" 
        {
            if(dialogueData.PromptOneButtonDisplay == true && dialogueData.PromptTwoButtonDisplay == false) //Show one button display
            {
                oneButtonDisplay.SetActive(true);
                _finishedTypingMessage = true;
                _hasActivatedButtonOptions = true;
                StopAllCoroutines();
            }

            if(dialogueData.PromptTwoButtonDisplay == true && dialogueData.PromptOneButtonDisplay == false) //Show two button display
            {
                twoButtonsDisplay.SetActive(true);
                _finishedTypingMessage = true;
                _hasActivatedButtonOptions = true;
                StopAllCoroutines();
            }
        }

        _finishedTypingMessage = true;
        StopAllCoroutines();
    }
}
