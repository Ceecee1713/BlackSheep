using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//This script only plays the animation for the card game 
//(The dealer shuffling the cards and the player cards sliding up/down)

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameConfiguration gameConfiguration;
    
    [SerializeField]
    private Animator dealerAnimator;
    
    [Header ("Interactable Player Cards")]
    [SerializeField]
    private RectTransform [] playingCardTransforms = new RectTransform [4]; 
    [SerializeField]
    private float moveDistance = 100.0f; //Distance minusing/adding to move "playingCardTransforms" up or down from their current positions

    private float _durationToMoveCards; //For the animation of player's interactable cards to move up or down
    
    private const float DELAY = 1.0f;
    private const float SHORTDELAY = 3.5f;
    private const float LONGDELAY = 4.0f;

    void Start()
    {
        _durationToMoveCards = gameConfiguration.DurationToMoveCardsUpDown;
        
        EventBus.Instance.Subscribe<StartNewRound>(OnNewCardRound);
        EventBus.Instance.Subscribe<ShuffleCards>(ShuffleEvent);
    }

    private void ShuffleEvent(ShuffleCards shuffleCards)
    {
        StartCoroutine(MoveCardsDown(false));
    }

    private void OnNewCardRound(StartNewRound startNewRound)
    {
        StartCoroutine(MoveCardsDown(true));
    }

    IEnumerator MoveCardsDown(bool isThisANewRound) 
    {
        if(isThisANewRound == true)
            yield return new WaitForSeconds(DELAY);

        Sequence sequence = DOTween.Sequence();
        
        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y - moveDistance, _durationToMoveCards, false);
            sequence.Join(tween);
        }
        
        yield return sequence.WaitForCompletion();

        if (isThisANewRound == true)
        {
            dealerAnimator.SetTrigger("PlayFullDealerShuffle");
            yield return new WaitForSeconds(SHORTDELAY);
            yield return MoveCardsUp();
        }

        else
        {
            dealerAnimator.SetTrigger("PlayDealerAnim");
            yield return new WaitForSeconds(LONGDELAY);
            yield return MoveCardsUp();
        }
    }


    IEnumerator MoveCardsUp()
    {
        EventBus.Instance.Publish(new CompletedShufflingCards()); //Make the shuffle button visible again and player input for moving their cards
            
        //Move cards up
        Sequence sequence = DOTween.Sequence();

        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y + moveDistance, _durationToMoveCards, false);
            sequence.Join(tween);
        }
            
        yield return sequence.WaitForCompletion();
        StopAllCoroutines();
    }
}
