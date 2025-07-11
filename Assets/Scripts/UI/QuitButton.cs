using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnQuitClick()
    {
        //UnityEditor.EditorApplication.isPlaying = false; 
	    Application.Quit();
    }
}
