using UnityEngine;

//This script is to be attached to an object on the card gameplay canvas
//With an animator that controls the dealer shuffling animations
//As during the animations that animator will play, it can call the methods below to play these sounds

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
