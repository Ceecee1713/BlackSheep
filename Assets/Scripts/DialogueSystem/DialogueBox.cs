using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public GameObject OneButton;
    public GameObject TwoButtons;

    public DialogueData data;
    public TextMeshProUGUI dialogueText;

    public int index = 0;

    public float typingSpeed = 0.05f;

    private bool writeButtonOneDialogue, writeButtonTwoDialogue; //Influenced by the button clicks (should be passed by an event)

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(writeButtonOneDialogue == false && writeButtonTwoDialogue == false)
                GetNormalMessage();

            if(writeButtonOneDialogue == true && writeButtonTwoDialogue == false)
                GetButtonOneMessage();

            if(writeButtonTwoDialogue == true && writeButtonOneDialogue == false)
                GetButtonTwoMessage();
        }
    }

    private void GetNormalMessage()
    {
        if(index+1 == data.NormalCharacterDialogues.Length || data.NormalCharacterDialogues == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeSingleSentence(data.NormalCharacterDialogues[index].Message));
    }

    private void GetButtonOneMessage()
    {
        if(index+1 == data.OneButtonCharacterDialogues.Length || data.OneButtonCharacterDialogues == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeSingleSentence(data.OneButtonCharacterDialogues[index].Message));
    }

    private void GetButtonTwoMessage()
    {
        if(index+1 == data.TwoButtonCharacterDialogues.Length || data.TwoButtonCharacterDialogues == null) //If index is out of bounds or the dialogue array is null
            return;

        index++;
        StopAllCoroutines();
        StartCoroutine(TypeSingleSentence(data.TwoButtonCharacterDialogues[index].Message));
    }

    public void GrabButtonData(ButtonDelegate buttonDelegate) //Called by button script, should be passed from an event
    {
        if(buttonDelegate == ButtonDelegate.OptionOne && OneButton.activeSelf == true)
        {
            if(data.PromptOneButtonOption != true && data.PromptTwoButtonOption != false)
                return;

            index = -1;
            writeButtonOneDialogue = true;
            OneButton.SetActive(false);
            GetButtonOneMessage();
        }

        if(buttonDelegate == ButtonDelegate.OptionOne && TwoButtons.activeSelf == true)
        {
            if(data.PromptTwoButtonOption != true && data.PromptOneButtonOption != false)
                return;

            index = -1;
            writeButtonOneDialogue = true;
            TwoButtons.SetActive(false);
            GetButtonOneMessage();
        }

        if(buttonDelegate == ButtonDelegate.OptionTwo)
        {
            if(data.PromptTwoButtonOption != true && data.PromptOneButtonOption != false)
                return;

            index = -1;
            writeButtonTwoDialogue = true;
            TwoButtons.SetActive(false);
            GetButtonTwoMessage();
        }
    }

    IEnumerator TypeSingleSentence(string message)
    {
        dialogueText.text = ""; //Clearing the "dialogueText".text for the new monster dialouge to be said
        
        foreach (char letter in message.ToCharArray()) //Conversion of string to a char array to mimick a "typing" effect of dialouge
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Time in between of each character being typed out
        } 

        yield return null;

        if(index == data.NormalCharacterDialogues.Length-1) //Show the button UI based on dialogue SO
        {
            if(data.PromptOneButtonOption == true && data.PromptTwoButtonOption == false)
            {
                OneButton.SetActive(true);
                StopAllCoroutines();
            }

            if(data.PromptTwoButtonOption == true && data.PromptOneButtonOption == false)
            {
                TwoButtons.SetActive(true);
                StopAllCoroutines();
            }
        }
    }

}
