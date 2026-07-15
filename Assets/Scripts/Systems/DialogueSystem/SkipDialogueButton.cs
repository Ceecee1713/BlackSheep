using UnityEngine;

/// <summary>
/// A button that'll skip dialogue on the UI canvas it's attached to
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to a choice button that'll skip through all the current dialogue on the UI canvas.
/// This script works together with scripts: "DialogueBox"
/// See <see cref="DialogueBox"/> for dialogue is assigned and iterated through.
/// 
///</remarks>

public class SkipDialogueButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox; //Can directly reference from as these two scripts will be on the same UI canvas game object

    private bool _stopRepeat = false;

    void OnEnable()
    {
        _stopRepeat = false;
    }
    
    public void OnSkipDialogueClick()
    {
        if(_stopRepeat == true || dialogueBox.PauseMenuCanvas.activeSelf == true)
            return;

        AudioManager.Instance.PlayButtonSound();
        dialogueBox.SkipDialogue();
        _stopRepeat = true;
    }
}
