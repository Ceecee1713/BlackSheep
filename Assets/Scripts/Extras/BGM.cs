using UnityEngine;
using DG.Tweening;

//This script is for the background music that's to be looped after "startingMusic" is done playing

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

    void Awake()
    {
        musicLoop.Stop();
    }

    void Start()
    {
        EventBus.Instance.Subscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<IncreaseMusicVolume>(IncreaseMusicVolume);
    }

    void Update()
    {
        if(_activatedMusicLoop == true)
            return;
            
        if(!startingMusic.isPlaying && _activatedMusicLoop == false)
        {
            musicLoop.Play();
            _activatedMusicLoop = true;
        }
    }

    private void IncreaseMusicVolume(IncreaseMusicVolume increaseMusicVolume)
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION);
        }
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION);
        }
    }

    private void FinishedRoundEvent(FinishedRound finishedRound)
    {
        if(_activatedMusicLoop == true)
        {
            _tween?.Kill();
            _tween = musicLoop.DOFade(0.0f, DURATION);
        } 
    }
}
