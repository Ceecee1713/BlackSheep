using UnityEngine;

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
