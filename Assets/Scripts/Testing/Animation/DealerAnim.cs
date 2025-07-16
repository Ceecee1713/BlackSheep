using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DealerAnim : MonoBehaviour
{
    [SerializeField]
    private DealerSprites dealerSprites;

    [Header ("Card Decks")]
    [SerializeField]
    private GameObject playerCardDeck;
    [SerializeField]
    private GameObject shuffledCardDeck;
    [SerializeField] 
    private Transform spawnShuffledCardDeckTransform; 
    [SerializeField]
    private float maxHeightFromEndCardSlidePosition = 600.0f;

    [Header ("Player's Hand")]
    [SerializeField]
    private GameObject playerHand;
    [SerializeField]
    private float playerHandOffset = 100.0f;

    [Header ("Dealer's Right Hand")] 
    [SerializeField]
    private GameObject rightHand; //Used to be behind "leftHand" to shuffle the cards
    [SerializeField]
    private GameObject secondRightHand; //Used to be on top of "playerCardDeck" and "shuffledCardDeck" (image sorting layering)

    [Header ("Dealer's Right Hand's Transforms")] 
    [SerializeField]
    private Transform shufflingHand;
    [SerializeField]
    private Transform reachForCardStack;
    [SerializeField]
    private Transform endShuffledCardDeckSlide;
    [SerializeField]
    private float maxHeightFromShufflingHandPosition = 200.0f;

    [Header ("Dealer's Left Hand")]
    [SerializeField]
    private GameObject leftHand; 
    [SerializeField]
    private Transform nonShufflingHandTransform; //Position left hand will take to hold the cards when the dealer is shuffling

    [Header ("Duration and Delays")]
    [SerializeField]
    private float delayBetweenShuffles;
    [SerializeField]
    private float durationToSlideCards;
    [SerializeField]
    private float delayBetweenClips; //Delay before resetting images and their positions

    private Image leftHandImage, rightHandImage, secondRightHandImage;
    private Sprite restingLeftHandSprite, restingRightHandSprite; //In the future, grab these from Start method where these game objects are in the correct position

    private Transform shuffledCardDeckTransform;
    private Transform leftHandTransform, rightHandTransform, playerHandTransform;
    private Vector3 leftHandRestPosition, rightHandRestPosition, playerHandRestPosition;

    private Tween firstTween;

    private float startingShufflingHandYPosition;
    private float newShufflingHandYPosition;
    private float endCardSlideYPosition, receivedCardYPosition;
    private float offScreenPlayerHandYPosition; //For player's hand

    private int maxNumberOfShufflingLoops = 4;

    void Start() //Have both of the dealer's hands at their starting positions before play
    {
        rightHandImage = rightHand.GetComponent<Image>();
        secondRightHandImage = secondRightHand.GetComponent<Image>();
        leftHandImage = leftHand.GetComponent<Image>();

        playerHandTransform = playerHand.GetComponent<Transform>();
        leftHandTransform = leftHand.GetComponent<Transform>();
        rightHandTransform = rightHand.GetComponent<Transform>(); 
        shuffledCardDeckTransform = shuffledCardDeck.GetComponent<Transform>();

        //Setting Positions and Sprites
        playerHandRestPosition = new Vector3 (playerHandTransform.position.x, playerHandTransform.position.y, playerHandTransform.position.z);
        rightHandRestPosition = new Vector3 (rightHandTransform.position.x, rightHandTransform.position.y, rightHandTransform.position.z);
        leftHandRestPosition = new Vector3 (leftHandTransform.position.x, leftHandTransform.position.y, leftHandTransform.position.z);
        restingRightHandSprite = rightHandImage.sprite;
        restingLeftHandSprite = leftHandImage.sprite;


        //Setting Y values
        startingShufflingHandYPosition = shufflingHand.position.y; 
        newShufflingHandYPosition = startingShufflingHandYPosition + maxHeightFromShufflingHandPosition;
        endCardSlideYPosition = endShuffledCardDeckSlide.position.y;
        offScreenPlayerHandYPosition = playerHandRestPosition.y;
        receivedCardYPosition = endCardSlideYPosition + maxHeightFromEndCardSlidePosition;
    }

    public void OnShuffleButton()
    {
        StartCoroutine(DealerShufflingCards()); //Call coroutine on events and button click
    }

    IEnumerator DealerShufflingCards()
    {
        //Above, slide up the player down / invoke event to allow player input


        //Player returns cards
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition - playerHandOffset, playerHand.transform.position.z); 
        shuffledCardDeck.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition, playerHand.transform.position.z); 
        shuffledCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will grab cards given by the player
        playerHand.transform.position = playerHandRestPosition;
        secondRightHand.transform.position = shuffledCardDeck.transform.position;
        secondRightHandImage.sprite = dealerSprites.RightHandReachingOver;
        playerCardDeck.transform.position = shuffledCardDeck.transform.position;
        rightHand.SetActive(false);
        shuffledCardDeck.SetActive(false);
        secondRightHand.SetActive(true);
        playerCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will slide the given cards up
        playerCardDeck.transform.position = new Vector3(playerHand.transform.position.x, receivedCardYPosition, playerHand.transform.position.z); 
        secondRightHand.transform.position = new Vector3(playerHand.transform.position.x, receivedCardYPosition, playerHand.transform.position.z); 
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's right hand will reach for the cards to shuffle
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        rightHandImage.sprite = dealerSprites.RightHandGrabbingCards;  
        rightHand.transform.position = new Vector3(reachForCardStack.position.x, reachForCardStack.position.y, reachForCardStack.position.z);
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's hands will be in position and have correct sprites for shuffling the cards
        playerCardDeck.SetActive(false);
        rightHand.transform.position = new Vector3(shufflingHand.position.x, shufflingHand.position.y, shufflingHand.position.z);
        rightHandImage.sprite = dealerSprites.RightHandShuffling;
        leftHand.transform.position = nonShufflingHandTransform.position;
        leftHandImage.sprite = dealerSprites.LeftHandShuffling;    
        yield return new WaitForSeconds(delayBetweenClips);



        //The dealer will choppily shuffle the cards 
        for(int numberOfShufflingLoops = 0; numberOfShufflingLoops < maxNumberOfShufflingLoops; numberOfShufflingLoops++)
        {
            rightHand.transform.position = new Vector3(shufflingHand.position.x, startingShufflingHandYPosition, shufflingHand.position.z);
            yield return new WaitForSeconds(delayBetweenShuffles);
            rightHand.transform.position = new Vector3(shufflingHand.position.x, newShufflingHandYPosition, shufflingHand.position.z);
            yield return new WaitForSeconds(delayBetweenShuffles);
        }

        rightHand.transform.position = new Vector3(shufflingHand.position.x, startingShufflingHandYPosition, shufflingHand.position.z);
        yield return new WaitForSeconds(delayBetweenShuffles);


        //The dealer will prepare to slide out the cards
        secondRightHand.transform.position = rightHand.transform.position;
        secondRightHandImage.sprite = dealerSprites.RightHandHoldingCards;
        rightHand.SetActive(false);
        secondRightHand.SetActive(true);
        leftHandImage.sprite = dealerSprites.LeftHandHoldingCards;
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer will slide out cards 
        secondRightHandImage.sprite = dealerSprites.RightHandPassingCards;
        shuffledCardDeck.transform.position = spawnShuffledCardDeckTransform.position;
        shuffledCardDeck.SetActive(true);
        firstTween = shuffledCardDeckTransform.DOMoveY(endCardSlideYPosition, durationToSlideCards, false); //Smoothly slide out the cards
        yield return firstTween.WaitForCompletion();


        //Player's hand will collect the cards and dealer's hands will reset to normal sprites and resting positions
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition - playerHandOffset, playerHand.transform.position.z); 
        rightHandImage.sprite = restingRightHandSprite;
        rightHand.transform.position = rightHandRestPosition;
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        secondRightHand.transform.position = rightHandRestPosition;
        leftHandImage.sprite = restingLeftHandSprite;
        leftHand.transform.position = leftHandRestPosition;
        yield return new WaitForSeconds(delayBetweenClips);


        //Reset player's hand and card deck image
        playerHand.transform.position = playerHandRestPosition;  
        shuffledCardDeck.SetActive(false);
        shuffledCardDeck.transform.position = spawnShuffledCardDeckTransform.position;

        //Below, slide up the player cards / invoke event to allow player input
    }
}
