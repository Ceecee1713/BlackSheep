using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShuffleButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    private int _numberOfShufflesPerRound = 0;
    private int _maxNumberOfShufflesPerRound = 3;

    void Start()
    {
        _numberOfShufflesPerRound = _maxNumberOfShufflesPerRound;
    }

    public void OnShuffle()
    {
        if(_numberOfShufflesPerRound < _maxNumberOfShufflesPerRound)
        {
            _numberOfShufflesPerRound--;
            buttonText.text = "Shuffle x " + _numberOfShufflesPerRound;

            //Shuffle cards below
        }
    }
}
