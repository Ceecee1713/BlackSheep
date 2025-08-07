using System.Collections;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    private ShouldGameBeStopped _shouldGameBeStopped;

    private bool _dontRepeat = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float QUIT_DELAY = 2.0f;
    private const float DELAY = 0.5f;

    void Start()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
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

    public void OnQuitClick()
    {
        if(_dontRepeat || _shouldGameBeStopped.PreventPlaying == true)
            return;

        if(_allowClicking == true)
        {
            _dontRepeat = true;
            _shouldGameBeStopped.PreventPlaying = true;
            AudioManager.Instance.PlayButtonSound();
            Invoke("Quit", QUIT_DELAY);
        }
    }

    private void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false; 
	    Application.Quit();
    }

    IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
