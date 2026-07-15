using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening;

/// <summary>
/// Managing a switch scene UI button
/// </summary>

/// <remarks>
/// This script works together with scripts: "ShouldGameBeStopped"
/// See <see cref="ShouldGameBeStopped"/> for how this script is structured.
/// </remarks>

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;
    
    [SerializeField]
    private string sceneNameToLoadOnClick;

    private ShouldGameBeStopped _shouldGameBeStopped;

    private bool _hasBeenClicked = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY_BEFORE_ALLOWING_CLICKING = 0.2f;
    private const float CHANGE_SCENE_DELAY = 1.5f;

    void Start()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
    }

    void OnDisable()
    {
        _calledCoroutine = false;
        _allowClicking = false;
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
    
    public void OnSwitchSceneClick()
    {
        if(_hasBeenClicked == true || _shouldGameBeStopped.PreventPlaying == true)
            return;

        if(_allowClicking == true)
        {
            _hasBeenClicked = true;
            _shouldGameBeStopped.PreventPlaying = true;
            AudioManager.Instance.PlayButtonSound();
            Invoke(nameof(ChangeScene), CHANGE_SCENE_DELAY);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadSceneAsync(sceneNameToLoadOnClick);
    }

    private IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY_BEFORE_ALLOWING_CLICKING);
        _allowClicking = true;
        yield return null;
    }
}