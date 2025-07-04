using System.Collections.Generic;
using UnityEngine;

public  class ShuffleEvent : MonoBehaviour
{
    /*
    private List <ShuffleListener> _subjectListeners = new();

    private List <int> UnavailableCardNumbers { get; set; } = new(); 

    private int _randomCardIndex;
    
    public void AddShuffleListener(ShuffleListener shuffleListener)
    {
        _subjectListeners.Add(shuffleListener);
    }

    public void RemoveShuffleListener(ShuffleListener shuffleListener)
    {
        _subjectListeners.Remove(shuffleListener);
    }

    protected void ClearCardSelectionHistory()
    {
        UnavailableCardNumbers.Clear();
    }

    protected void NotifyShuffleListener(CardType cardType) //Method selects a random index of "_subjectListeners" and pass a "CardType"
    {
        _randomCardIndex = Random.Range(0, _subjectListeners.Count);

        while(UnavailableCardNumbers.Contains(_randomCardIndex)) //Prevent already selected "_subjectListeners" indexes from being chosen again
            _randomCardIndex = Random.Range(0, _subjectListeners.Count);

        UnavailableCardNumbers.Add(_randomCardIndex);
        
        _subjectListeners[_randomCardIndex].OnShuffleNotified(cardType);
    }
    */
}
