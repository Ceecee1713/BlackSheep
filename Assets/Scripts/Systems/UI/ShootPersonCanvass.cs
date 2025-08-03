using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//This script is to be attached to UI canvases that also have a canvas group component on them
//These will be the canvases for the "animation" of x person being shot when a card round is finished

public class ShootPersonCanvass : MonoBehaviour
{
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [SerializeField]
    private GameObject dealerDialogueCanvas, dealerDialogueCanvasRoundFive, badEndDialogueCanvas; 
    [SerializeField]
    private GameObject gun; 
    [SerializeField]
    private GameObject flashImage; 

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

        AudioManager.Instance.PlayGunshotSound();
        flashImage.SetActive(true);

        yield return new WaitForSeconds(delayBeforeFadingFromFlashImage);

        _thisCanvasGroup.alpha = 0.0f;
        gun.SetActive(false);

        if(GamblingTable.Instance.RoundNumber == gameConfiguration.RoundNumberToRemoveDealerAndNormalCards)
        {
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : dealerDialogueCanvasRoundFive, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            EventBus.Instance.Publish(new IncreaseMusicVolume());
            StopAllCoroutines();
        }

        if(GamblingTable.Instance.RoundNumber != gameConfiguration.MaxAmountOfRounds)
        {
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : dealerDialogueCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
            EventBus.Instance.Publish(new IncreaseMusicVolume());
        }
            
        else //Reached the last round of the game
            EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : badEndDialogueCanvas, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));

        StopAllCoroutines();
    }
}
