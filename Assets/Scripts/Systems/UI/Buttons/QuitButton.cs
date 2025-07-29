using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private const float DELAY = 2.0f;
    private bool _dontRepeat = false;

    public void OnQuitClick()
    {
        if(_dontRepeat)
            return;

        _dontRepeat = true;
        AudioManager.Instance.PlayButtonSound();
        Invoke("Quit", DELAY);
    }

    private void Quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false; 
	    Application.Quit();
    }
}
