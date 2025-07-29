using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource buttonSFX;

    [Header ("For Card Game")]
    [SerializeField]
    private AudioSource cardPopSFX;
    [SerializeField]
    private AudioSource cardSlideSFX;
    [SerializeField]
    private AudioSource cardShuffleSFX;
    [SerializeField]
    private AudioSource revolverSFX;
    [SerializeField]
    private AudioSource gunshotSFX;

    public void PlayButtonSound()
    {
        if(buttonSFX.isPlaying)
            buttonSFX.Stop();

        buttonSFX.Play();
    }

    public void PlayCardSound()
    {
        if(cardPopSFX.isPlaying)
            cardPopSFX.Stop();

        cardPopSFX.Play();
    }

    public void PlayCardShuffleSound()
    {
        cardShuffleSFX.Play();
    }

    public void PlayCardSlideSound()
    {
        cardSlideSFX.Play();
    }

    public void PlayGunshotSound()
    {
        gunshotSFX.Play();
    }

    public void PlayRevolverSound()
    {
        revolverSFX.Play();
    }
}
