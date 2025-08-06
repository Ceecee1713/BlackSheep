using UnityEngine;

//This script is to be attached to casino area buttons for the casino area button canvas

public class AreaButton : MonoBehaviour
{
    [SerializeField]
    private ButtonOptions buttonOptions; 

    private bool _dontRepeat = false;

    public void OnButtonClick()
    {
        if(_dontRepeat == true)
            return;

        _dontRepeat = true;
        buttonOptions.counter++;
    }
}
