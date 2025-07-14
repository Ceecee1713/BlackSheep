using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] 
    private GameObject tutorialObject;

    private bool _hasBeenClickedOnce = false;

    public void OnTutorialClick()
    {
        if (_hasBeenClickedOnce == false)
        {
            tutorialObject.SetActive(true);
            _hasBeenClickedOnce = true;
        }
            
        else
        {
            tutorialObject.SetActive(false);
            _hasBeenClickedOnce = false;
        }
    }

    public void OnExitClick()
    {
        tutorialObject.SetActive(false);
    }
}
