using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DealerShuffleAnim : MonoBehaviour
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
    private float maxHeightFromEndCardPosition = 600.0f;

    [Header ("Player's Hand")]
    [SerializeField]
    private GameObject playerHand;
    [SerializeField] //Remove later
    private Transform playerHandRestingTransform; //In the future, grab these transforms from Start method where these game objects are in the correct position

    [Header ("Dealer's Right Hand")] 
    [SerializeField]
    private GameObject rightHand; //Used to be behind "leftHand" to shuffle the cards
    [SerializeField]
    private GameObject secondRightHand; //Used to be on top of "playerCardDeck" and "shuffledCardDeck" (image sorting layering)
    [SerializeField] //Remove later
    private Sprite restingRightHandSprite; //In the future, grab these from Start method where these game objects are in the correct position

    [Header ("Dealer's Right Hand's Transforms")] 
    [SerializeField] //Remove later
    private Transform rightHandRestTransform; //In the future, grab these transforms from Start method where these game objects are in the correct position
    [SerializeField]
    private Transform shufflingHandTransform;
    [SerializeField]
    private Transform reachForCardStackTransform;
    [SerializeField]
    private Transform endShuffledCardDeckSlideTransform;
    [SerializeField]
    private float maxHeightFromShufflingHandTransformYPosition = 200.0f;

    [Header ("Dealer's Left Hand")]
    [SerializeField]
    private GameObject leftHand;
    [SerializeField] //Remove later
    private Sprite restingLeftHandSprite; //In the future, grab these from Start method where these game objects are in the correct position
    [SerializeField] //Remove later
    private Transform leftHandRestTransform; //In the future, grab these transforms from Start method where these game objects are in the correct position
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

    private Transform leftHandTransform, rightHandTransform, shuffledCardDeckTransform;
    private Transform playerRestingTransform;

    private Tween firstTween;

    private float startingShufflingHandTransformYPosition;
    private float newShufflingHandTransformYPosition;
    private float endCardSlideYPosition, receivedCardYPosition;
    private float offScreenPlayerHandYPosition; //For player's hand

    private int maxNumberOfShufflingLoops = 4;

    void Start() //Have both of the dealer's hands at their starting positions before play
    {
        rightHandImage = rightHand.GetComponent<Image>();
        secondRightHandImage = secondRightHand.GetComponent<Image>();
        leftHandImage = leftHand.GetComponent<Image>();

        playerRestingTransform = playerHand.GetComponent<Transform>();
        leftHandTransform = leftHand.GetComponent<Transform>();
        rightHandTransform = rightHand.GetComponent<Transform>(); 
        shuffledCardDeckTransform = shuffledCardDeck.GetComponent<Transform>();

        //rightHandRestTransform.position = rightHandTransform.position; //Add back in later
        //leftHandRestTransform.position = leftHandTransform.position; //Add back in later

        //Setting Y values
        startingShufflingHandTransformYPosition = shufflingHandTransform.position.y; 
        newShufflingHandTransformYPosition = startingShufflingHandTransformYPosition + maxHeightFromShufflingHandTransformYPosition;
        endCardSlideYPosition = endShuffledCardDeckSlideTransform.position.y;
        offScreenPlayerHandYPosition = playerRestingTransform.position.y;
        receivedCardYPosition = endCardSlideYPosition + maxHeightFromEndCardPosition;

        StartCoroutine(DealerShufflingCards()); //Call coroutine on events and button click
    }

    IEnumerator DealerShufflingCards()
    {
        //Above, slide up the player down / invoke event to allow player input


        //Player returns cards
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition, playerHand.transform.position.z); //Choppily grab cards
        shuffledCardDeck.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition + 200f, playerHand.transform.position.z); //Choppily move cards 
        shuffledCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will grab cards given by the player
        playerHand.transform.position = playerHandRestingTransform.position;
        secondRightHand.transform.position = shuffledCardDeck.transform.position;
        secondRightHandImage.sprite = dealerSprites.RightHandReachingOver;
        playerCardDeck.transform.position = shuffledCardDeck.transform.position;
        rightHand.SetActive(false);
        shuffledCardDeck.SetActive(false);
        secondRightHand.SetActive(true);
        playerCardDeck.SetActive(true);
        yield return new WaitForSeconds(delayBetweenClips);


        //Dealer will slide the given cards up
        playerCardDeck.transform.position = new Vector3(playerHand.transform.position.x, receivedCardYPosition, playerHand.transform.position.z); //Choppily move cards upwards
        secondRightHand.transform.position = new Vector3(playerHand.transform.position.x, receivedCardYPosition, playerHand.transform.position.z); //Choppily move hand upwards
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's right hand will reach for the cards to shuffle
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        rightHandImage.sprite = dealerSprites.RightHandGrabbingCards;  
        rightHand.transform.position = new Vector3(reachForCardStackTransform.position.x, reachForCardStackTransform.position.y, reachForCardStackTransform.position.z);
        yield return new WaitForSeconds(delayBetweenClips);


        //The dealer's hands will be in position and have correct sprites for shuffling the cards
        playerCardDeck.SetActive(false);
        rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, shufflingHandTransform.position.y, shufflingHandTransform.position.z);
        rightHandImage.sprite = dealerSprites.RightHandShuffling;
        leftHand.transform.position = nonShufflingHandTransform.position;
        leftHandImage.sprite = dealerSprites.LeftHandShuffling;    
        yield return new WaitForSeconds(delayBetweenClips);



        //The dealer will choppily shuffle the cards 
        for(int numberOfShufflingLoops = 0; numberOfShufflingLoops < maxNumberOfShufflingLoops; numberOfShufflingLoops++)
        {
            rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, startingShufflingHandTransformYPosition, shufflingHandTransform.position.z);
            yield return new WaitForSeconds(delayBetweenShuffles);
            rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, newShufflingHandTransformYPosition, shufflingHandTransform.position.z);
            yield return new WaitForSeconds(delayBetweenShuffles);
        }

        rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, startingShufflingHandTransformYPosition, shufflingHandTransform.position.z);
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
        playerHand.transform.position = new Vector3(playerHand.transform.position.x, endCardSlideYPosition, playerHand.transform.position.z); //Choppily grab cards
        rightHandImage.sprite = restingRightHandSprite;
        rightHand.transform.position = rightHandRestTransform.position;
        rightHand.SetActive(true);
        secondRightHand.SetActive(false);
        secondRightHand.transform.position = rightHandRestTransform.position;
        leftHandImage.sprite = restingLeftHandSprite;
        leftHand.transform.position = leftHandRestTransform.position;
        yield return new WaitForSeconds(delayBetweenClips);


        //Reset player's hand and card deck image
        playerHand.transform.position = playerHandRestingTransform.position; //Choppily move hand back 
        shuffledCardDeck.SetActive(false);
        shuffledCardDeck.transform.position = spawnShuffledCardDeckTransform.position;

        //Below, slide up the player cards / invoke event to allow player input
    }
}
