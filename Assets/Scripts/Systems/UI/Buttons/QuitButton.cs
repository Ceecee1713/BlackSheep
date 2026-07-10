using System.Collections;
using UnityEngine;

/// <summary>
/// Managing a quit UI button
/// </summary>

/// <remarks>
/// This script works together with scripts: "ShouldGameBeStopped"
/// See <see cref="ShouldGameBeStopped"/> for how this script is structured.
/// </remarks>

public class QuitButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    private ShouldGameBeStopped _shouldGameBeStopped;

    private bool _hasQuitButtonBeenClicked = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float QUIT_DELAY = 2.0f;
    private const float DELAY_BEFORE_ALLOWING_CLICKING = 0.2f;

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
        if(_hasQuitButtonBeenClicked || _shouldGameBeStopped.PreventPlaying == true)
            return;

        if(_allowClicking == true)
        {
            _hasQuitButtonBeenClicked = true;
            _shouldGameBeStopped.PreventPlaying = true;
            AudioManager.Instance.PlayButtonSound();
            Invoke("Quit", QUIT_DELAY);
        }
    }

    private void Quit()
    {
	    Application.Quit();
    }

    private IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ALLOWING_CLICKING);
        _allowClicking = true;
        yield return null;
    }
}
