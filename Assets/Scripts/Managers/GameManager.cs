using UnityEngine;

/// <remarks>
/// This script works together with scripts: "ShouldGameBeStopped", "SingleCard"
/// See <see cref="ShouldGameBeStopped"/> for its structure and how it works. 
/// This script is a scriptable object script
///</remarks>

public class GameManager : MonoBehaviour
{
    private ShouldGameBeStopped _shouldGameBeStopped;
    
    void Awake()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
        _shouldGameBeStopped.PreventPlaying = false;
    }
}


