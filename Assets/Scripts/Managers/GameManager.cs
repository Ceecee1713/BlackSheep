using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ShouldGameBeStopped _shouldGameBeStopped;
    
    void Awake()
    {
        _shouldGameBeStopped = Resources.Load<ShouldGameBeStopped>("ShouldGameBeStopped");
        _shouldGameBeStopped.PreventPlaying = false;
    }
}
