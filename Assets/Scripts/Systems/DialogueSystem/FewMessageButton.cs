using UnityEngine;

/// <summary>
/// A dialogue button children under a dialogue button display
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to a choice button that'll trigger a few dialogue messages for the "dialogueBox".
/// A choice button that's under a dialogue button display
/// This script is to give separate, few dialogues to act independently of the dialogue system's branching dialogue structure
/// 
/// This script works together with scripts: "DialogueBox"
/// See <see cref="DialogueBox"/> for dialogue is assigned and iterated through.
/// 
///</remarks>

public class FewMessageButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox; //Can directly reference from as these two scripts will be on the same UI canvas game object

    [SerializeField]
    private GameObject button;

    [TextArea(2,5)] public string [] Messages;

    private int _counter = -1;

    public void OnMessageButtonClick()
    {
        _counter++;

        if(_counter >= Messages.Length)
        {
            button.SetActive(false);
            return;
        }

        if(_counter < Messages.Length)
            dialogueBox.ReceiveMessage(Messages[_counter]);
    }
}
