using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//Move all dealer sprites into an SO
//Modify so there'll be another image representing the cards to be slid across the table
//So when the player collects the card, the left hand can go back to resting poisiton and its resting sprite

public class DealerShuffleAnim : MonoBehaviour
{
    [Header ("Player's Hand")]
    [SerializeField]
    private GameObject playerHand;

    [Header ("Dealer's Right Hand")] 
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private Sprite restingRightHandSprite;
    [SerializeField] //Remove later
    private Transform rightHandRestTransform; //In the future, grab these transforms from Start method where these game objects are in the correct position
    [SerializeField]
    private Transform shufflingHandTransform;
    [SerializeField]
    private Transform grabCardsTransform;
    [SerializeField]
    private Transform spawnCardsTransform, endCardSlideTransform;
    [SerializeField]
    private float maxHeightFromstartingShufflingHandTransformYPosition = 200.0f;

    [Header ("Dealer's Left Hand")]
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private Sprite restingLeftHandSprite;
    [SerializeField] //Remove later
    private Transform leftHandRestTransform; //In the future, grab these transforms from Start method where these game objects are in the correct position

    [Header ("Dealer's Hand Sprites")]
    [SerializeField]
    private Sprite reachingHand;
    [SerializeField]
    private Sprite shufflingHand, nonShufflingHand;
    [SerializeField]
    private Sprite holdingCards, passingCards;
    [SerializeField]
    private Sprite cardDeck;

    [Header ("Duration and Delay Times")]
    [SerializeField]
    private float durationBetweenShuffling;
    [SerializeField]
    private float durationToSlideCards;
    [SerializeField]
    private float delayForShuffleAndReset; //Delay before shuffling animation starts and when to reset images and their positions

    private Image rightHandImage;
    private Image leftHandImage;

    private Transform leftHandTransform, rightHandTransform;
    private Transform playerRestingTransform;

    private Tween firstTween, secondTween;

    private float startingShufflingHandTransformYPosition;
    private float newShufflingHandTransformYPosition;
    private float endCardSlideYPosition;
    private float offScreenYPosition; //For player's hand

    private int maxNumberOfShufflingLoops = 2;

    void Start()
    {
        rightHandImage = rightHand.GetComponent<Image>();
        leftHandImage = leftHand.GetComponent<Image>();

        playerRestingTransform = playerHand.GetComponent<Transform>();
        leftHandTransform = leftHand.GetComponent<Transform>();
        rightHandTransform = rightHand.GetComponent<Transform>(); 

        //rightHandRestTransform.position = rightHandTransform.position; //Add back in later
        //leftHandRestTransform.position = leftHandTransform.position; //Add back in later

        //Setting Y values for the right hand, where cards will stop sliding at, and the player's hand at a resting position
        startingShufflingHandTransformYPosition = shufflingHandTransform.position.y; 
        newShufflingHandTransformYPosition = startingShufflingHandTransformYPosition + maxHeightFromstartingShufflingHandTransformYPosition;
        endCardSlideYPosition = endCardSlideTransform.position.y;
        offScreenYPosition = playerRestingTransform.position.y;

        StartCoroutine(DealerShufflingCards()); //Call coroutine on events and button click
    }

    IEnumerator DealerShufflingCards()
    {
        //The dealer's right hand will reach for the cards
        rightHandImage.sprite = reachingHand;
        rightHand.transform.position = new Vector3(grabCardsTransform.position.x, grabCardsTransform.position.y, grabCardsTransform.position.z);
        yield return new WaitForSeconds(delayForShuffleAndReset);

        //The dealer's hands will be in position and have correct sprites for shuffling the cards
        rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, shufflingHandTransform.position.y, shufflingHandTransform.position.z);
        rightHandImage.sprite = shufflingHand;
        leftHandImage.sprite = nonShufflingHand;

        yield return new WaitForSeconds(delayForShuffleAndReset);

        //The dealer will choppily shuffle the cards 
        for(int numberOfShufflingLoops = 0; numberOfShufflingLoops < maxNumberOfShufflingLoops; numberOfShufflingLoops++)
        {
            rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, startingShufflingHandTransformYPosition, shufflingHandTransform.position.z);
            yield return new WaitForSeconds(durationBetweenShuffling);
            rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, newShufflingHandTransformYPosition, shufflingHandTransform.position.z);
            yield return new WaitForSeconds(durationBetweenShuffling);
        }

        rightHand.transform.position = new Vector3(shufflingHandTransform.position.x, startingShufflingHandTransformYPosition, shufflingHandTransform.position.z);
        yield return new WaitForSeconds(durationBetweenShuffling);


        //Below, modify so there'll be another image representing the cards to be slid across the table
        //So when the player collects the card, the left hand can go back to resting poisiton and its resting sprite


        leftHand.SetActive(false);
        rightHandImage.sprite = holdingCards;
        yield return new WaitForSeconds(durationBetweenShuffling);

        //The dealer will slide out cards 
        rightHandImage.sprite = passingCards;
        leftHand.transform.position = spawnCardsTransform.position;
        leftHandImage.sprite = cardDeck;
        leftHand.SetActive(true);
        firstTween = leftHandTransform.DOMoveY(endCardSlideYPosition, durationToSlideCards, false);
        yield return firstTween.WaitForCompletion();


        //Player's hand will collect the cards
        firstTween = playerHand.transform.DOMoveY(endCardSlideYPosition, durationToSlideCards, false);
        rightHandImage.sprite = restingRightHandSprite;
        rightHand.transform.position = rightHandRestTransform.position;
        //Set left hand to go back to its resting position and resting sprite, in this line


        yield return firstTween.WaitForCompletion();
        firstTween = playerHand.transform.DOMoveY(offScreenYPosition, durationToSlideCards, false);
        secondTween = leftHandTransform.transform.DOMoveY(offScreenYPosition, durationToSlideCards, false);

        yield return new WaitForSeconds(delayForShuffleAndReset);


        //Remove later
        leftHandImage.sprite = restingLeftHandSprite;
        leftHand.transform.position = leftHandRestTransform.position;


        //Below, slide up the player cards / invoke event to allow player input
    }
}
