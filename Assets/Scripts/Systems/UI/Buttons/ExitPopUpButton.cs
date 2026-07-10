using UnityEngine;

/// <summary>
/// Managing an exit UI button
/// </summary>
/// 
/// <remarks>
/// This script is to be attached any UI button that'll close a UI Pop Up (set inactive) 
/// 
/// This script works together with scripts: "ShouldGameBeStopped"
/// See <see cref="ShouldGameBeStopped"/> for how this script is structured.
/// 
///</remarks>

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
