using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShuffleButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI buttonText;

    private int numberOfShufflesPerRound = 0;
    private int maxNumberOfShufflesPerRound = 3;

    public void OnShuffle()
    {
        if(numberOfShufflesPerRound < maxNumberOfShufflesPerRound)
        {
            numberOfShufflesPerRound++;

            //Shuffle cards below
        }
    }
}
