using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField]
    private GameObject easterEgg;
    
    void Enable()
    {
    }

    void OnDisable()
    {
        easterEgg.SetActive(false);
    }

    public void OnEasterEggClick()
    {
        easterEgg.SetActive(true);
    }
}
