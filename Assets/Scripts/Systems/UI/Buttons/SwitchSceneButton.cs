using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentCanvasGroup;
    
    [SerializeField]
    private string sceneNameToLoadOnClick;

    private bool _hasBeenClicked = false;
    private bool _calledCoroutine = false;
    private bool _allowClicking = false;

    private const float DELAY = 0.5f;
    private const float CHANGE_SCENE_DELAY = 2.0f;

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
        if(_hasBeenClicked == true)
            return;

        if(_allowClicking == true)
        {
            _hasBeenClicked = true;
            AudioManager.Instance.PlayButtonSound();
            Invoke("ChangeScene", CHANGE_SCENE_DELAY);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadSceneAsync(sceneNameToLoadOnClick);
    }

    IEnumerator AllowClicking()
    {
        yield return new WaitForSeconds(DELAY);
        _allowClicking = true;
        yield return null;
    }
}
