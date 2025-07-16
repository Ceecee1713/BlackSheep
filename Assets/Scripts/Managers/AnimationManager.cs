using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationManager : MonoBehaviour, EventListener
{
    [Header ("Scriptable Objects")] 
    [SerializeField]
    private DealerSprites dealerSprites;
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [Header ("Card Deck")] //This is not the playable cards the player interacts with. This is an imgae game object for animation solely
    [SerializeField]
    private GameObject playerCardDeck; //Same as "shuffledCardDeck" but sorted differently in the hiearchy (image layering)
    [SerializeField]
    private GameObject shuffledCardDeck; //Same as "playerCardDeck" but sorted differently in the hiearchy (image layering)
    [SerializeField] 
    private Transform spawn_ShuffledCardDeckTransform; 
    [SerializeField]
    private float maxHeightFromEndCardSlidePosition = 600.0f; //To increase "_endCardSlideYPosition" by this value 
    //(Distance for how far the dealer's right hand will slide cards up after grabbing the player's cards)

    [Header ("Player's Hand")]
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private float playerHandOffset = 100.0f;

    [Header ("Dealer's Right Hand")] //These objects will the same but sorted differently in the hiearchy
    [SerializeField]
    private GameObject rightHand; //Used to be behind "leftHand" to shuffle the cards
    [SerializeField]
    private GameObject secondRightHand; //Used to be on top of "playerCardDeck" and "shuffledCardDeck" (image layering)

    [Header ("Dealer's Right Hand's Transforms")] 
    [SerializeField]
    private Transform shufflingHand;
    [SerializeField]
    private Transform reachForCardStack;
    [SerializeField]
    private Transform endShuffledCardDeckSlide;
    [SerializeField]
    private float maxHeightFromShufflingHandPosition = 200.0f; //To increase "shufflingHand" y value by this value

    [Header ("Dealer's Left Hand")] 
    [SerializeField]
    private GameObject leftHand; 
    [SerializeField]
    private Transform nonShufflingHandTransform; //Position "leftHand" will take to hold the cards when the dealer is shuffling

    [Header ("Duration and Delays")]
    [SerializeField]
    private float delayBetweenEachShuffle;
    [SerializeField]
    private float durationToSlideCardsAcrossTable;
    [SerializeField]
    private float delayBetweenClips;
    
    [Header ("Interactable Player Cards")]
    [SerializeField]
    private RectTransform [] playingCardTransforms = new RectTransform [4]; 
    [SerializeField]
    private float moveDistance = 100.0f; //Distance minusing/adding to move "playingCardTransforms" up or down from their current positions

    private Image _leftHandImage, _rightHandImage, _secondRightHandImage;
    private Sprite _restingLeftHandSprite, _restingRightHandSprite; 

    private Transform _shuffledCardDeckTransform;
    private Transform _leftHandTransform, _rightHandTransform, _playerHandTransform;
    private Vector3 _leftHandRestPosition, _rightHandRestPosition, _playerHandRestPosition;

    private float _startingShufflingHandYPosition;
    private float _newShufflingHandYPosition; //Value for how much Y offset from "shufflingHand" for when the right hand is shuffling
    private float _endCardSlideYPosition, _receivedCardYPosition;
    private float _offScreenPlayerHandYPosition; //For player's hand
    private float _durationToMoveCards; //For the animation of player's interactable cards to move up or down

    private const int maxNumberOfShufflingLoops = 4;

    void Awake()
    {
        EventManager.Instance.AddEventListener(this);
    }

    void Start()
    {
        _durationToMoveCards = gameConfiguration.DurationToMoveCardsUpDown;

        _rightHandImage = rightHand.GetComponent<Image>();
        _secondRightHandImage = secondRightHand.GetComponent<Image>();
        _leftHandImage = leftHand.GetComponent<Image>();

        _playerHandTransform = playerHand.GetComponent<Transform>();
        _leftHandTransform = leftHand.GetComponent<Transform>();
        _rightHandTransform = rightHand.GetComponent<Transform>(); 
        _shuffledCardDeckTransform = shuffledCardDeck.GetComponent<Transform>();

        //Setting Positions and Sprites
        _playerHandRestPosition = new Vector3 (_playerHandTransform.position.x, _playerHandTransform.position.y, _playerHandTransform.position.z);
        _rightHandRestPosition = new Vector3 (_rightHandTransform.position.x, _rightHandTransform.position.y, _rightHandTransform.position.z);
        _leftHandRestPosition = new Vector3 (_leftHandTransform.position.x, _leftHandTransform.position.y, _leftHandTransform.position.z);
        _restingRightHandSprite = _rightHandImage.sprite;
        _restingLeftHandSprite = _leftHandImage.sprite;


        //Setting Y values
        _startingShufflingHandYPosition = shufflingHand.position.y; 
        _newShufflingHandYPosition = _startingShufflingHandYPosition + maxHeightFromShufflingHandPosition;
        _endCardSlideYPosition = endShuffledCardDeckSlide.position.y;
        _offScreenPlayerHandYPosition = _playerHandRestPosition.y;
        _receivedCardYPosition = _endCardSlideYPosition + maxHeightFromEndCardSlidePosition;
    }

    void OnEnable() 
    {
        
    }

    void OnDisable()
    {
        //EventManager.Instance.RemoveEventListener(this); //Change to be used when a new scene is being loaded / outside of playmode
    }

    public void OnEventCalled(AllEventNames eventName)
    {
        if(eventName == AllEventNames.ShuffleEvent)
            StartCoroutine(MoveCardsDown());

        if(eventName == AllEventNames.NewRoundEvent)
            StartCoroutine(DealerShufflingCards(true));
    }

    IEnumerator MoveCardsDown() 
    {
        Sequence sequence = DOTween.Sequence();
        
        for(int i = 0; i < playingCardTransforms.Length; i++)
        {
            Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y - moveDistance, _durationToMoveCards, false);
            sequence.Join(tween);
        }
        
        yield return sequence.WaitForCompletion();
        yield return DealerShufflingCards(false);
    }


    IEnumerator DealerShufflingCards(bool isThisANewRound)
    {
        //Player returns cards
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, _endCardSlideYPosition - playerHandOffset, playerHand.transform.position.z); 
        shuffledCardDeck.transform.position = new Vector3(playerHand.transform.position.x, _endCardSlideYPosition, playerHand.transform.position.z); 
        shuffledCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will grab cards given by the player
        playerHand.transform.position = _playerHandRestPosition;
        secondRightHand.transform.position = shuffledCardDeck.transform.position;
        _secondRightHandImage.sprite = dealerSprites.RightHandReachingOver;
        playerCardDeck.transform.position = shuffledCardDeck.transform.position;
        rightHand.SetActive(false);
        shuffledCardDeck.SetActive(false);
        secondRightHand.SetActive(true);
        playerCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will slide the given player cards up
        playerCardDeck.transform.position = new Vector3(playerHand.transform.position.x, _receivedCardYPosition, playerHand.transform.position.z); 
        secondRightHand.transform.position = new Vector3(playerHand.transform.position.x, _receivedCardYPosition + (maxHeightFromEndCardSlidePosition/2), playerHand.transform.position.z); 
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's right hand will reach for the cards to shuffle
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        _rightHandImage.sprite = dealerSprites.RightHandGrabbingCards;  
        rightHand.transform.position = new Vector3(reachForCardStack.position.x, reachForCardStack.position.y, reachForCardStack.position.z);
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's hands will be in position and have correct sprites for shuffling the cards
        playerCardDeck.SetActive(false);
        rightHand.transform.position = new Vector3(shufflingHand.position.x, shufflingHand.position.y, shufflingHand.position.z);
        _rightHandImage.sprite = dealerSprites.RightHandShuffling;
        leftHand.transform.position = nonShufflingHandTransform.position;
        _leftHandImage.sprite = dealerSprites.LeftHandShuffling;    
        yield return new WaitForSeconds(delayBetweenClips);



        //The dealer will choppily shuffle the cards 
        for(int numberOfShufflingLoops = 0; numberOfShufflingLoops < maxNumberOfShufflingLoops; numberOfShufflingLoops++)
        {
            rightHand.transform.position = new Vector3(shufflingHand.position.x, _startingShufflingHandYPosition, shufflingHand.position.z);
            yield return new WaitForSeconds(delayBetweenEachShuffle);
            rightHand.transform.position = new Vector3(shufflingHand.position.x, _newShufflingHandYPosition, shufflingHand.position.z);
            yield return new WaitForSeconds(delayBetweenEachShuffle);
        }

        rightHand.transform.position = new Vector3(shufflingHand.position.x, _startingShufflingHandYPosition, shufflingHand.position.z);
        yield return new WaitForSeconds(delayBetweenEachShuffle);


        //The dealer will prepare to slide out the cards
        secondRightHand.transform.position = rightHand.transform.position;
        _secondRightHandImage.sprite = dealerSprites.RightHandHoldingCards;
        rightHand.SetActive(false);
        secondRightHand.SetActive(true);
        _leftHandImage.sprite = dealerSprites.LeftHandHoldingCards;
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer will slide out cards 
        _secondRightHandImage.sprite = dealerSprites.RightHandPassingCards;
        shuffledCardDeck.transform.position = spawn_ShuffledCardDeckTransform.position;
        shuffledCardDeck.SetActive(true);
        Tween firstTween = _shuffledCardDeckTransform.DOMoveY(_endCardSlideYPosition, durationToSlideCardsAcrossTable, false); //Smoothly slide out the cards
        yield return firstTween.WaitForCompletion();


        //Player's hand will collect the cards and dealer's hands will reset to normal sprites and resting positions
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, _endCardSlideYPosition - playerHandOffset, playerHand.transform.position.z); 
        _rightHandImage.sprite = _restingRightHandSprite;
        rightHand.transform.position = _rightHandRestPosition;
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        secondRightHand.transform.position = _rightHandRestPosition;
        _leftHandImage.sprite = _restingLeftHandSprite;
        leftHand.transform.position = _leftHandRestPosition;
        yield return new WaitForSeconds(delayBetweenClips);


        //Reset player's hand and card deck image
        playerHand.transform.position = _playerHandRestPosition;  
        shuffledCardDeck.SetActive(false);
        shuffledCardDeck.transform.position = spawn_ShuffledCardDeckTransform.position;
        yield return new WaitForSeconds(delayBetweenClips);


        if(isThisANewRound == true)
        {
            //EventManager.Instance.OnShuffleEventComplete.Invoke(); //Show player cards' visibility, allow card player input and make shuffle button visible again
            EventBus.Instance.Publish(new CompletedShufflingCards());
            StopAllCoroutines();
        }

        else
        {
            //Move cards up
            Sequence sequence = DOTween.Sequence();

            for(int i = 0; i < playingCardTransforms.Length; i++)
            {
                Tween tween = playingCardTransforms[i].DOAnchorPosY(playingCardTransforms[i].anchoredPosition.y + moveDistance, _durationToMoveCards, false);
                sequence.Join(tween);
            }
            
            yield return sequence.WaitForCompletion();
            //EventManager.Instance.OnShuffleEventComplete.Invoke(); //Make shuffle button visible again and allow card player input
            EventBus.Instance.Publish(new CompletedShufflingCards());
            StopAllCoroutines();
        }
    }
}
