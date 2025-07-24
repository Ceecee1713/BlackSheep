using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private CardGameInputs _cardGameInputs;

    void OnEnable()
    {
        if (_cardGameInputs == null)
            _cardGameInputs = new CardGameInputs();

        _cardGameInputs.CardGame.NextMessage.performed += OnNextMessage;
        _cardGameInputs.Enable();
    }

    void OnDisable()
    {
        _cardGameInputs.CardGame.NextMessage.performed -= OnNextMessage;
        _cardGameInputs.Disable();
    }


    void OnNextMessage(InputAction.CallbackContext val)
    {
        EventBus.Instance.Publish(new NextMessage());
    }
}
