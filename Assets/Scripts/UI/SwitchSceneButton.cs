using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup mainCanvasGroup;

    [SerializeField]
    private string sceneNameToLoadOnClick;

    [SerializeField]
    private DurationOfFadingScreens durationOfFadingScreens;

    private bool _hasBeenClicked = false;

    public void OnSwitchSceneClick()
    {
        if(_hasBeenClicked == true)
            return;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        _hasBeenClicked = true;
        Tween tween = mainCanvasGroup.DOFade(0f, durationOfFadingScreens.DurationOfScreenFade);
        yield return tween.WaitForCompletion();
        SceneManager.LoadSceneAsync(sceneNameToLoadOnClick);
        yield return null;
        //StopAllCoroutines();
    }
}
