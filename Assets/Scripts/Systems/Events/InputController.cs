using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuCanvas;

    private CardGameInputs _cardGameInputs;
    private ShouldGameBeStopped _shouldGameBeStopped;

    private bool _stopInput = false;
    private bool _pauseMenuIsOpen = false;
    private bool _stopCoroutineRepeats = false;

    private const float DELAY = 0.5f;

    void OnEnable()
    {
        if (_cardGameInputs == null)
            _cardGameInputs = new CardGameInputs();

        if(_shouldGameBeStopped == null)
            _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");

        _cardGameInputs.CardGame.NextMessage.performed += OnNextMessage;
        _cardGameInputs.CardGame.PauseGame.performed += OnPauseGame;
        _cardGameInputs.Enable();
    }

    void OnDisable()
    {
        _cardGameInputs.CardGame.NextMessage.performed -= OnNextMessage;
        _cardGameInputs.CardGame.PauseGame.performed += OnPauseGame;
        _cardGameInputs.Disable();
    }

    void Update()
    {
        if(pauseMenuCanvas.activeSelf == true)
        {
            _stopCoroutineRepeats = false;
            _stopInput = true;
        }

        if(pauseMenuCanvas.activeSelf == false  && _stopCoroutineRepeats == false)
        {
            StartCoroutine(AllowInput());
            _stopCoroutineRepeats = true;
        }
    }

    private void OnNextMessage(InputAction.CallbackContext val)
    {
        if(_stopInput == false && _shouldGameBeStopped.PreventPlaying != true)
            EventBus.Instance.Publish(new NextMessage());
    }

    private void OnPauseGame(InputAction.CallbackContext val)
    {
        if(_pauseMenuIsOpen == false)
        {
            pauseMenuCanvas.SetActive(true);
            _pauseMenuIsOpen = true;
        }
            
        else
        {   pauseMenuCanvas.SetActive(false);
           _pauseMenuIsOpen = false; 
        }
    }

    IEnumerator AllowInput()
    {
        yield return new WaitForSeconds(DELAY);
        _stopInput = false;
    }
}
