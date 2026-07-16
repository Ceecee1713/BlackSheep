using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shooting person canvas (Dealer, you, or sheep shooting canvases) 
/// </summary>

/// <remarks>
/// 
/// This script is to be attached to UI canvases that are for the "animation" of x person being shot when a card round is finished
/// 
/// This script works together with scripts: "GameConfiguration", "CardGameRoundNumber"
/// See <see cref="GameConfiguration"/> for general game information is structured.
/// See <see cref="CardGameRoundNumber"/> for how each card game round number is structured.
/// 
///</remarks>

public class ShootPersonCanvass : MonoBehaviour
{
    [Header ("Scriptable Objects")]
    [SerializeField]
    private CardGameRoundNumber cardGameRoundNumber;
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [Header ("Game Objects")]
    [SerializeField]
    private GameObject dealerDialogueCanvas;
    [SerializeField]
    private GameObject dealerDialogueCanvasRoundFive;
    [SerializeField]
    private GameObject badEndDialogueCanvas; 
    [SerializeField]
    private GameObject gun; //Game object of gun image
    [SerializeField]
    private GameObject flashImage; //Game object of flash image. The flash image is just an opaque image covering the screen completely (in this case, black)

    [Header ("Values")]
    [SerializeField]
    private float delayBeforeFadingFromFlashImage = 0.5f;
    [SerializeField]
    private float fadingSpeed = 1.25f;

    private Image _gunImage;
    private CanvasGroup _thisCanvasGroup;
    private Color transparentColour, opaqueColour;

    private bool _hasBeenCalledOnce = false;

    private const float DELAY = 0.5f;
    private const bool IS_NEXT_CANVAS_A_DIALOGUE_CANVAS = true;

    void Awake()
    {
        _thisCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        _gunImage = gun.GetComponent<Image>();

        transparentColour = new Color (_gunImage.color.r, _gunImage.color.g, _gunImage.color.b, 0f); 
        opaqueColour = new Color (_gunImage.color.r, _gunImage.color.g, _gunImage.color.b, 1f); 
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        flashImage.SetActive(false);
        _gunImage.color = transparentColour;
        _hasBeenCalledOnce = false;
    }

    void Update()
    {
        if(_hasBeenCalledOnce == false && _thisCanvasGroup.alpha == 1.0f)
        {
            StartCoroutine(Shoot());
            _hasBeenCalledOnce = true;
        }
    }

    //IEnumerator controlling the "animation" of shooting the gun image and determining/prompting the new dialogue canvas to show, as well as increase music volume
    private IEnumerator Shoot()
    {
        float elapsedTime = 0f;
        float elapsedPercentage = 0f;

        gun.SetActive(true);
        AudioManager.Instance.PlayRevolverSound();

        while(elapsedPercentage < 1.0f)
        {
            elapsedPercentage = elapsedTime / fadingSpeed;
            _gunImage.color = Color.Lerp(transparentColour, opaqueColour, elapsedPercentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(DELAY);

        flashImage.SetActive(true);
        AudioManager.Instance.PlayGunshotSound();

        yield return new WaitForSeconds(delayBeforeFadingFromFlashImage);

        _thisCanvasGroup.alpha = 0.0f;
        gun.SetActive(false);

        //Here, I'm minusing the "cardGameRoundNumber.CurrentRoundNumber" by 1 because there's an order of operations difference 
        //as this coroutine is called when a round ends AFTER the card round number ("cardGameRoundNumber.CurrentRoundNumber)" value increases.
        //For example, if the card round number 5, in this coroutine, the value will actually be 6, because of this order of operations difference
        //Thus, there'll be an offset, so I need to minus by 1 

        if(cardGameRoundNumber.CurrentRoundNumber-1 == gameConfiguration.RoundNumberToRemoveDealerAndNormalCards)
        {
            Debug.Log("Show Dealer Dialogue Canvas for Round Five");
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : dealerDialogueCanvasRoundFive, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS)); //Publish to "CanvasManager"
            EventBus.Instance.Publish(new IncreaseMusicVolume()); //Publish to "BGM"
            StopAllCoroutines();
        }

        else if(cardGameRoundNumber.CurrentRoundNumber-1 != gameConfiguration.MaxAmountOfRounds)
        {
            Debug.Log("Show Normal Dealer Dialogue Canvas");
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : dealerDialogueCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS)); //Publish to "CanvasManager"
            EventBus.Instance.Publish(new IncreaseMusicVolume()); //Publish to "BGM"
            StopAllCoroutines();
        }
            
        else //Reached the last round of the game
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : badEndDialogueCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS)); //Publish to "CanvasManager"

        StopAllCoroutines();
    }
}
