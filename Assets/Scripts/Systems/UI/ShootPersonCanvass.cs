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
    private GameObject dealerDialogueCanvas, badEndDialogueCanvas; 
    [SerializeField]
    private GameObject gun; 
    [SerializeField]
    private GameObject flashImage; 

    [SerializeField]
    private float delayBeforeFadingFromWhite = 0.5f;
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

        while(elapsedPercentage < 1.0f)
        {
            elapsedPercentage = elapsedTime / fadingSpeed;
            _gunImage.color = Color.Lerp(transparentColour, opaqueColour, elapsedPercentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(DELAY);

        flashImage.SetActive(true);

        yield return new WaitForSeconds(delayBeforeFadingFromWhite);

        _thisCanvasGroup.alpha = 0.99f;

        if(GamblingTable.Instance.RoundNumber != gameConfiguration.MaxAmountOfRounds)
            EventManager.Instance.OnNewCanvasEvent?.Invoke(dealerDialogueCanvas, IS_NEXT_CANVAS_A_DIALOGUE_CANVAS);

        else
            EventManager.Instance.OnNewCanvasEvent?.Invoke(badEndDialogueCanvas, IS_NEXT_CANVAS_A_DIALOGUE_CANVAS);

        StopAllCoroutines();
    }
}
