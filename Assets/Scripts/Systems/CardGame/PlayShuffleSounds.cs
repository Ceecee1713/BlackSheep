using UnityEngine;

public class PlayShuffleSounds : MonoBehaviour
{
    public void PlayShuffleSound()
    {
        AudioManager.Instance.PlayCardShuffleSound();
    }

    public void PlayCardSlideSound()
    {
        AudioManager.Instance.PlayCardSlideSound();
    }
}
