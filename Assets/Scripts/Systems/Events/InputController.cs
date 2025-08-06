using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenuCanvas;

    private CardGameInputs _cardGameInputs;

    private bool _stopInput = false;
    private bool _stopCoroutineRepeats = false;

    private const float DELAY = 0.5f;

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

    void Update()
    {
        if(PauseMenuCanvas.activeSelf == true)
        {
            _stopCoroutineRepeats = false;
            _stopInput = true;
        }

        if(PauseMenuCanvas.activeSelf == false  && _stopCoroutineRepeats == false)
        {
            StartCoroutine(AllowInput());
            _stopCoroutineRepeats = true;
        }
    }

    private void OnNextMessage(InputAction.CallbackContext val)
    {
        if(_stopInput == false)
            EventBus.Instance.Publish(new NextMessage());
    }

    IEnumerator AllowInput()
    {
        yield return new WaitForSeconds(DELAY);
        _stopInput = false;
    }
}
