using System.Collections;
using UnityEngine;

public class EyeAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private CanvasGroup currentCanvasGroup;

    [SerializeField] 
    private string animationTriggerName;

    [SerializeField]
    private float delayBetweenBlinkAnimation;

    private bool _playAnimation = false;

    void Update()
    {
        if (currentCanvasGroup.alpha == 1.0f)
        {
            if (_playAnimation == true)
            {
                StopAllCoroutines();
                StartCoroutine(PlayBlinkAnimation());
                _playAnimation = false;
            }
        }

        else
        {
            StopAllCoroutines();
            _playAnimation = true;
        }
    }

    private IEnumerator PlayBlinkAnimation()
    {
        yield return new WaitForSeconds(delayBetweenBlinkAnimation);
        animator.SetTrigger(animationTriggerName);
        yield return new WaitForSeconds(delayBetweenBlinkAnimation);
        _playAnimation = true;
    }
}
