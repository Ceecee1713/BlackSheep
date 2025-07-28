using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource startingMusic;
    [SerializeField]
    private AudioSource musicLoop;

    private bool _activatedMusicLoop = false;

    void Update()
    {
        if(_activatedMusicLoop)
            return;

        if(!startingMusic.isPlaying)
        {
            musicLoop.Play();
            _activatedMusicLoop = true;
        }
    }
}
