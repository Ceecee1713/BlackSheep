using UnityEngine;
using TMPro;

public enum ButtonType 
{
    OptionOne,
    OptionTwo
}

public class DialogueButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox;

    [SerializeField]
    private ButtonType buttonType;
    
    public void OnDialogueButtonClick()
    {
        if(dialogueBox.PauseMenuCanvas.activeSelf == true)
            return;

        AudioManager.Instance.PlayButtonSound();
        dialogueBox.ChangeDialogueFromButtonEvent(buttonType);
    }
}
