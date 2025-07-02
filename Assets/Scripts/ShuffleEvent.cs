using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class ShuffleEvent : Singleton<ShuffleEvent>
{
    public List <ShuffleListener> SubjectListeners = new List<ShuffleListener>(); 

    public List <int> unavailableCardNumbers = new List<int>(); 

    private int randomCardIndex;

    public void Start()
    {
        Debug.Log(SubjectListeners.Count);
    }

    public void AddShuffleListener(ShuffleListener ShuffleListener)
    {
        SubjectListeners.Add(ShuffleListener);
    }

    public void RemoveShuffleListener(ShuffleListener ShuffleListener)
    {
        SubjectListeners.Remove(ShuffleListener);
    }

    public void ClearCardSelectionHistory()
    {
        unavailableCardNumbers.Clear();
    }

    public void NotifyShuffleListener(CardType cardType) //Method selects a random index of "SubjectListeners" and pass a "CardType"
    {
        randomCardIndex = Random.Range(0, SubjectListeners.Count);

        while(unavailableCardNumbers.Contains(randomCardIndex)) //Prevent already selected "SubjectListeners" indexes from being chosen again
            randomCardIndex = Random.Range(0, SubjectListeners.Count);

        unavailableCardNumbers.Add(randomCardIndex);
        
        SubjectListeners[randomCardIndex].OnShuffleNotified(cardType);

        /*
        //For each "ShuffleListener" inside the "SubjectListeners" List, call "OnShuffleNotified()" method in "ShuffleListener" interface
        SubjectListeners.ForEach((ShuffleListener) => {
            ShuffleListener.OnShuffleNotified();
        });
        */
    }
}
