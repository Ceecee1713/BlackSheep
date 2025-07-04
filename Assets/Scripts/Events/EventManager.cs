using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityAction OnShuffleEvent;
    public UnityAction OnFinishedRoundEvent;
    public UnityAction OnNewRoundEvent;

    private List <EventListener> _eventListeners = new(); 

    public void AddEventListener(EventListener eventListener)
    {
        _eventListeners.Add(eventListener);
    }

    public void RemoveEventListener(EventListener eventListener)
    {
        _eventListeners.Remove(eventListener);
    }

    private void ClearEventListeners()
    {
        _eventListeners.Clear();
    }

    private void Start()
    {
        OnShuffleEvent += OnShuffleEventCalled;
        OnFinishedRoundEvent += OnFinishedRoundEventCalled;
        OnNewRoundEvent += OnNewRoundEventCalled;
    }

    //Methods below: //For each "eventListener" inside the "_eventListeners" List, call the "OnEventCalled(AllEventNames)" method 

    private void OnShuffleEventCalled()
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnEventCalled(AllEventNames.ShuffleEvent);
        });
    }

    private void OnFinishedRoundEventCalled()
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnEventCalled(AllEventNames.FinishedRoundEvent);
        });
    }

    private void OnNewRoundEventCalled()
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnEventCalled(AllEventNames.NewRoundEvent);
        });
    }
}
