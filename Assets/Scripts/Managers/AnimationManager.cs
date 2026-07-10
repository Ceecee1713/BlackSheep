using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Manages the animations of the dealer shuffling cards and the player's cards sliding up/down
/// </summary>
/// 
/// <remarks>
/// This script works together with scripts: "GameConfiguration"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// </remarks>

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    private GameConfiguration gameConfiguration; //General game information: screen fading values, moving cards values, card game values
    
    [SerializeField]
    private Animator dealerAnimator; //Animator that'll be on the card gameplay canvas attached to the card dealer
    
    [Header ("Interactable Player Cards")]
    [SerializeField]
    private RectTransform [] playingCardTransforms = new RectTransform [4]; //RectTransforms of card objects: UI images meant to be interactable (dragged)
    [SerializeField]
    private float moveDistance = 100.0f; //Distance minusing/adding to move "playingCardTransforms" up or down from their current positions

    private float _durationToMoveCards; //Total time duration to move the player's interactable cards up or down
    
    private const float DELAY_BEFORE_MOVING_CARDS = 1.0f;
    private const float SHORT_DELAY_BEFORE_MOVING_CARDS_UP = 3.5f;
    private const float LONG_DELAY_BEFORE_MOVING_CARDS_UP = 4.0f;

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

    private IEnumerator MoveCardsDown(bool isThisANewRound) //Move all player's cards (playingCardTransforms) down by their rect transforms
    {
        if(isThisANewRound == true)
            yield return new WaitForSeconds(DELAY_BEFORE_MOVING_CARDS);

        Sequence sequence = DOTween.Sequence();
        
        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y - moveDistance, _durationToMoveCards, false);
            sequence.Join(tween);
        }
        
        yield return sequence.WaitForCompletion();

        if (isThisANewRound == true) //Trigger an animation from 'dealerAnimator' animator, then prompt "MoveCardsUp" IEnumerator to move player cards up
        {
            dealerAnimator.SetTrigger("PlayFullDealerShuffle");
            yield return new WaitForSeconds(SHORT_DELAY_BEFORE_MOVING_CARDS_UP);
            yield return MoveCardsUp();
        }

        else //Trigger an animation from 'dealerAnimator' animator, then prompt "MoveCardsUp" IEnumerator to move player cards up
        {
            dealerAnimator.SetTrigger("PlayDealerAnim");
            yield return new WaitForSeconds(LONG_DELAY_BEFORE_MOVING_CARDS_UP);
            yield return MoveCardsUp();
        }
    }


    private IEnumerator MoveCardsUp() //Move all player's cards (playingCardTransforms) up by their rect transforms
    {
        EventBus.Instance.Publish(new CompletedShufflingCards()); //Make the shuffle button visible again and player input for moving their cards
            
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
