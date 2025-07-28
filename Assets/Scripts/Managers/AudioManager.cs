using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]
    private AudioSource buttonSFX;
    [SerializeField]
    private AudioSource cardPopSFX;
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

    public void PlayGunshotSound()
    {
        revolverSFX.Play();
    }

    public void PlayRevolverSound()
    {
        gunshotSFX.Play();
    }
}
