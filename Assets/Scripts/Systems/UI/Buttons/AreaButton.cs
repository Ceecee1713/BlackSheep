using System.Collections;
using UnityEngine;

//This script is to be attached to casino area buttons for the casino area button canvas

public class AreaButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    [SerializeField]
    private ButtonOptions buttonOptions; 

    private bool _dontRepeat = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY = 0.5f;

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

    IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
