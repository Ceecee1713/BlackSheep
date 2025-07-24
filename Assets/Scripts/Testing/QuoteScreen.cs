using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuoteScreen : MonoBehaviour
{
    [SerializeField]
    private GameConfiguration gameConfiguration;

    [SerializeField]
    private CanvasGroup [] quoteCanvasGroups; //These canvas groups will be canvas groups attached to texts

    [SerializeField]
    private GameObject nextCanvasToSetActive; 

    //[SerializeField]
    private CanvasGroup mainCanvasGroup;

    private int _index = -1;

    private bool _allowGoingThroughCanvases = false;
    private bool _hasChangedCanvas = false;

    private const bool IS_NEXT_CANVAS_A_DIALOGUE_CANVAS = true;

    private const float INITIAL_DELAY = 1.0f;

    void Awake()
    {
        mainCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        Invoke("IterateThroughQuotes", INITIAL_DELAY);
    }

    void Update()
    {
        if(_hasChangedCanvas == true)
            return;

        if (Input.GetMouseButtonDown(0) && _allowGoingThroughCanvases == true) 
        {
            if(_index == quoteCanvasGroups.Length-1)
            {
                EventBus.Instance.Publish(new ChangeToNewCanvas(newCanvas : nextCanvasToSetActive, isNewCanvasADialogueCanvas : IS_NEXT_CANVAS_A_DIALOGUE_CANVAS));
                _hasChangedCanvas = true;
                return;
            }

            IterateThroughQuotes();
        }
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
        Tween tween = quoteCanvasGroups[_index].DOFade(1f, gameConfiguration.DurationOfScreenFade);
        yield return tween.WaitForCompletion();
        _allowGoingThroughCanvases = true;
        yield return null;
        StopAllCoroutines();
    }
}
