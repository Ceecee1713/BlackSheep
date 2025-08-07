using UnityEngine;

//This script is only to be used on menus that have the options 
//Of moving to a different scene or quitting (like the pause menu)

//This script is to be attached to the button that will close this menu

public class ExitPopUpButton : MonoBehaviour
{
    [SerializeField]
    private GameObject currentCanvas;

    private ShouldGameBeStopped _shouldGameBeStopped;

    void Start()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
    }

    public void OnExitUIClick()
    {
        if(_shouldGameBeStopped.PreventPlaying == true)
            return;

        currentCanvas.SetActive(false);
    }
}
