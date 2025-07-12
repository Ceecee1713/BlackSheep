using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CardsAnim : MonoBehaviour
{
    [SerializeField]
    private RectTransform [] playingCardTransforms = new RectTransform [4];

    [SerializeField]
    private float moveDistance = 100.0f;

    //public bool PlayCardDownAnimation = false; //Temporary
    //public bool PlayCardUpAnimation = false; //Temporary

    private float _duration = 1.0f;

    /*
    void Update()
    {
        if(PlayCardDownAnimation == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCardsDown());
            PlayCardDownAnimation = false;
        }

        if(PlayCardUpAnimation == true)
        {
            StopAllCoroutines();
            StartCoroutine(MoveCardsUp());
            PlayCardUpAnimation = false;
        }
    }
    */

    IEnumerator MoveCardsDown()
    {
        Sequence sequence = DOTween.Sequence();
        
        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y - moveDistance, _duration, false);
            sequence.Join(tween);
        }
        
        yield return sequence.WaitForCompletion();
    }

    IEnumerator MoveCardsUp()
    {
        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y + moveDistance, _duration, false);
            sequence.Join(tween);
        }
        
        yield return sequence.WaitForCompletion();
    }
}
