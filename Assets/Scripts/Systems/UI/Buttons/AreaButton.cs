using System.Collections;
using UnityEngine;

/// <summary>
/// This script is to be attached to casino area buttons 
/// These will be the buttons that allow the player to go to different areas in the casino
/// </summary>

/// <remarks>
/// 
/// This script works together with scripts: "ButtonOptions"
/// See <see cref="ButtonOptions"/> for changing variable values.
/// 
///</remarks>

public class AreaButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    [SerializeField]
    private ButtonOptions buttonOptions; //Can directly reference from as these two scripts will be on the same UI canvas game object

    [SerializeField]
    private GameObject buttonObject;

    private bool _dontRepeat = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY = 0.5f;

    void OnEnable()
    {
        _calledCoroutine = false;
        _allowClicking = false;
    }

    void OnDisable()
    {
        if(_dontRepeat == true)
            buttonObject.SetActive(false);
    }

    void Update()
    {
        if(_calledCoroutine == true)
            return;

        if(currentCanvasGroup.alpha == 1.0f && _calledCoroutine == false)
        {
            StartCoroutine(AllowClicking());
            _calledCoroutine = true;
        }
    }

    public void OnButtonClick()
    {
        if(_dontRepeat == true)
            return;

        if(_allowClicking == true)
        {
            _dontRepeat = true;
            buttonOptions.counter++;
        }
    }

    private IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}