using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuoteScreen : MonoBehaviour, EventListener
{
    [SerializeField]
    private DurationOfFadingScreens durationOfFadingScreens;

    [SerializeField]
    private CanvasGroup [] quoteCanvasGroups;

    [SerializeField]
    private GameObject nextCanvasToSetActive; 

    [SerializeField]
    private float initialDelay = 1.0f;

    //[SerializeField]
    private CanvasGroup mainCanvasGroup;

    private int _index = -1;

    private bool _allowGoingThroughCanvases = false;
    private bool _isNextCanvasADialogueCanvas = true;
    private bool _hasChangedCanvas = false;

    void Awake()
    {
        mainCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        Invoke("IterateThroughQuotes", initialDelay);
    }

    void Update()
    {
        if(_hasChangedCanvas == true)
            return;

        if (Input.GetMouseButtonDown(0) && _allowGoingThroughCanvases == true) //Refactor this input
        {
            if(_index == quoteCanvasGroups.Length-1)
            {
                EventManager.Instance.OnNewCanvasEvent?.Invoke(nextCanvasToSetActive, _isNextCanvasADialogueCanvas);
                _hasChangedCanvas = true;
                return;
            }

            IterateThroughQuotes();
        }
    }

    public void OnEventCalled(AllEventNames eventName)
    {

    }

    private void IterateThroughQuotes()
    {
        if(_index+1 == quoteCanvasGroups.Length) //If _index is out of bounds 
            return;

        _index++;
        StartCoroutine(ShowSingleQuote());
    }
    
    IEnumerator ShowSingleQuote()
    {
        Tween tween = quoteCanvasGroups[_index].DOFade(1f, durationOfFadingScreens.DurationOfScreenFade);
        yield return tween.WaitForCompletion();
        _allowGoingThroughCanvases = true;
        yield return null;
        StopAllCoroutines();
    }
}
