using UnityEngine;

public enum ButtonDelegate 
{
    OptionOne,
    OptionTwo
}

public class DialogueButton : MonoBehaviour
{
    public DialogueBox DialogueBox;
    public ButtonDelegate ButtonDelegate;

    public void OnDialogueButtonClick()
    {
        DialogueBox.GrabButtonData(ButtonDelegate);
    }
}
