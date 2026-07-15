using UnityEngine;

/// <summary>
/// Controls the PROMPTING of playing sounds during animating. 
/// </summary>

/// <remarks>
/// This script is to be attached to a parent game object ("For Animating") on the card gameplay canvas 
/// that has an animator component that controls the dealer shuffling animations.
/// This parent game object, as its children, should be the game objects that'll be animated.
/// 
/// The animations are the dealer shuffling and dealing cards on the card gameplay canvas.
/// This script is to prompt "AudioManager" to play sounds while the animations are happening.
/// See <see cref="AudioManager"/> 
/// 
/// </remarks>

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
