using UnityEngine;

public class SkipDialogueButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox;

    private bool _stopRepeat = false;
    
    public void OnSkipDialogueClick()
    {
        if(_stopRepeat == true)
            return;

        AudioManager.Instance.PlayButtonSound();
        dialogueBox.SkipDialogue();
        _stopRepeat = true;
    }
}
