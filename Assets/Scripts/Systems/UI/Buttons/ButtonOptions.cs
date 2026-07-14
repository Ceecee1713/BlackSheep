using UnityEngine;

/// <summary>
/// This script is to be attached to the canvas that shows the casino area options to explore the casino
/// </summary>

/// <remarks>
/// 
/// This script works together with scripts: "AreaButton"
/// See <see cref="AreaButton"/> for changing variable values.
/// 
///</remarks>

public class ButtonOptions : MonoBehaviour
{
    /// <summary>
    /// Public because "AreaButton" script needs to access/assign this value as this script uses this value to determine when to show "thirdButtonOption"
    /// </summary>
    [HideInInspector]
    public int counter; 

    [SerializeField]
    private GameObject thirdButtonOption; //Button that'll, when clicked, show a new UI canvas and a new area of the casino

    private const int _numberToReachToDisplayButton = 2;

    private bool _showButton = false;

    void OnEnable()
    {
    }

    void OnDisable()
    {
        if(_showButton == true)
            thirdButtonOption.SetActive(true);
    }

    void Update()
    {
        if(counter >= _numberToReachToDisplayButton)
            _showButton = true;
    }
}
