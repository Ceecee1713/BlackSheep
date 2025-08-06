using UnityEngine;

//This script is to be attached to the canvas that shows the casino area options to explore 
//This script controls when to show "thirdButtonOption"

public class ButtonOptions : MonoBehaviour
{
    [HideInInspector]
    public int counter; 

    [SerializeField]
    private GameObject thirdButtonOption;

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
