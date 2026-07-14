using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// The opening UI screen 
/// </summary>

/// <remarks>
/// 
/// This script is to be attached only to the start menu UI screen
/// 
///</remarks>

public class LoadStartUI : MonoBehaviour
{
    [SerializeField]
    private AudioSource bgm;

    private CanvasGroup _canvasGroup;
    private float _durationOfFade = 1.5f;
    private const float DELAY = 0.25f;

    void Start()
    {
        _canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        StartCoroutine(ShowCanvas());
    }

    private IEnumerator ShowCanvas()
    {
        yield return new WaitForSeconds(DELAY);
        bgm.Play();
        Tween firstTween = _canvasGroup.DOFade(1f, _durationOfFade);
        yield return firstTween.WaitForCompletion();
        StopAllCoroutines();
    }
}
