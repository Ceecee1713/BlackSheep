using UnityEngine;
using DG.Tweening;

/// <summary>
/// Manages the background music that's meant to be looped
/// </summary>

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource startingMusic;
    [SerializeField]
    private AudioSource musicLoop;
    [SerializeField]
    private float _musicLoopStartingVolume = 0.2f;

    private Tween _tween;

    private bool _activatedMusicLoop = false;

    private const float DURATION = 1.0f;

    void Start()
    {
        EventBus.Instance.Subscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<IncreaseMusicVolume>(IncreaseVolumeAfterShooting);
    }

    void Update()
    {
        if(_activatedMusicLoop == true)
            return;

        //if "startingMusic" has finished playing and if "_activatedMusicLoop" is false
        if(startingMusic.time >= startingMusic.clip.length && !_activatedMusicLoop)
        {
            musicLoop.Play();
            _activatedMusicLoop = true;
        }
    }

    //"IncreaseMusicVolume" is the name of an event. Empty event
    private void IncreaseVolumeAfterShooting(IncreaseMusicVolume increaseMusicVolume) //Published by "ShootPersonCanvass"
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION); //Restore to starting volume
        }
    }

    //"StartNewRound" is the name of an event. Empty event
    private void OnNewCardRound(StartNewRound startNewRound) //Published by "CanvasManager"
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION); //Restore to starting volume
        }
    }

    //"FinishedRound" is the name of an event. Empty event
    private void FinishedRoundEvent(FinishedRound finishedRound) //Published by "GamblingTable"
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(0.0f, DURATION);
        } 
    }
}
