using UnityEngine;

//This script is to be attached to the button that'll trigger
//Few dialogue messages for the "dialogueBox"

//This script is to give separate, few dialogues
//To act independently of the dialogue system's branching dialogue

public class FewMessageButton : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox;

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
