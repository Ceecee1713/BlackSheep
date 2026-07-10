using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handling all the inputs. This is to be placed on an empty game object acting as an 'Input Manager'
/// </summary>
/// 
/// <remarks>
/// This script works together with scripts: "ShouldGameBeStopped"
/// See <see cref="ShouldGameBeStopped"/> for how this script is structured.
/// </remarks>

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

    private void OnNextMessage(InputAction.CallbackContext val) //Advance through dialogue
    {
        if(_stopInput == false && _shouldGameBeStopped.PreventPlaying != true)
            EventBus.Instance.Publish(new NextMessage());
    }

    private void OnPauseGame(InputAction.CallbackContext val) //Open/Close Pause Menu
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

    private IEnumerator AllowInput()
    {
        yield return new WaitForSeconds(DELAY);
        _stopInput = false;
    }
}
