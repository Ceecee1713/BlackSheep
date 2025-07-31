using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using DG.Tweening;

public class SwitchSceneButton : MonoBehaviour
{
    [SerializeField]
    private string sceneNameToLoadOnClick;

    private bool _hasBeenClicked = false;
    private const float DELAY = 2.0f;

    public void OnSwitchSceneClick()
    {
        if(_hasBeenClicked == true)
            return;

        _hasBeenClicked = true;
        AudioManager.Instance.PlayButtonSound();
        Invoke("ChangeScene", DELAY);
    }

    private void ChangeScene()
    {
        SceneManager.LoadSceneAsync(sceneNameToLoadOnClick);
    }
}
