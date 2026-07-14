using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// The opening UI screen that shows up once the game starts
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to the UI that'll be the first active UI upon startng up the game on a new scene
/// This UI will be a dialogue UI of some sort
/// 
///</remarks>

public class LoadUI : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox;

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
        Tween firstTween = _canvasGroup.DOFade(1f, _durationOfFade);
        yield return firstTween.WaitForCompletion();
        dialogueBox.DisplayDialogueBox();
        StopAllCoroutines();
    }
}
