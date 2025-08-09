using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EndScreenUI : MonoBehaviour
{
    [Header ("For Title Text")]
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private string titleMessage;
    [SerializeField]
    private float typingSpeed = 0.25f;

    [Header ("Canvas Groups")]
    [SerializeField]
    private CanvasGroup currentCanvasGroup;
    [SerializeField]
    private CanvasGroup currentCanvasOptions;


    private bool _typedOutTitleText = false;

    private const float DURATION_TO_FADE = 0.7f;

    void Update()
    {
        if(currentCanvasGroup.alpha == 1.0f && _typedOutTitleText != true)
        {
            StartCoroutine(ShowEndScreen());
            FinishGame.Instance.HasPlayerFinishedGameOnce = true;
            _typedOutTitleText = true;
        }
    }

    private IEnumerator ShowEndScreen()
    {
        titleText.text = ""; //Clearing the "titleMessage".text for the dialouge to be said
        
        foreach (char letter in titleMessage.ToCharArray()) //Conversion of string to a char array to mimick a "typing" effect of dialouge
        {
            titleText.text += letter;
            yield return new WaitForSeconds(typingSpeed); //Time in between of each character being typed out
        } 

        yield return new WaitForSeconds(typingSpeed); //Time in between of each character being typed out

        Tween tween = currentCanvasOptions.DOFade(1.0f, DURATION_TO_FADE);
        yield return tween.WaitForCompletion();

        StopAllCoroutines();
    }
}
