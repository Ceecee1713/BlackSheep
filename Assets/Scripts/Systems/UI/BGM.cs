using UnityEngine;
using DG.Tweening;

public class BGM : MonoBehaviour
{
    [SerializeField]
    private AudioSource startingMusic;
    [SerializeField]
    private AudioSource musicLoop;
    [SerializeField]
    private float _musicLoopStartingVolume = 0.2f;

    private Tween _tween;

    private const float DURATION = 1.0f;

    private bool _activatedMusicLoop = false;

    void Start()
    {
        EventBus.Instance.Subscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<IncreaseMusicVolume>(IncreaseMusicVolume);
    }

    /*
    private void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<FinishedRound>(FinishedRoundEvent);
        EventBus.Instance.Unsubscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Unsubscribe<IncreaseMusicVolume>(IncreaseMusicVolume);
    }
    */

    void Update()
    {
        if(!startingMusic.isPlaying && _activatedMusicLoop == false)
        {
            musicLoop.Play();
            _activatedMusicLoop = true;
        }
    }

    private void IncreaseMusicVolume(IncreaseMusicVolume increaseMusicVolume)
    {
        _tween?.Kill();
        _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION);
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        _tween?.Kill();
        _tween = musicLoop.DOFade(_musicLoopStartingVolume, DURATION);
    }

    private void FinishedRoundEvent(FinishedRound finishedRound)
    {
        _tween?.Kill();
        _tween = musicLoop.DOFade(0.0f, DURATION);
    }
}
