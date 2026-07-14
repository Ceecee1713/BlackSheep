using UnityEngine;
using TMPro;

/// <summary>
/// A dialogue button children under a dialogue button display
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to a choice button that prompts new dialogue that's under a dialogue button display
/// 
/// This script works together with scripts: "DialogueBox"
/// See <see cref="DialogueBox"/> for dialogue is assigned and iterated through.
/// 
///</remarks>

public enum ButtonType 
{
    OptionOne,
    OptionTwo
}

public class DialogueButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox; //Can directly reference from as these two scripts will be on the same UI canvas game object

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
