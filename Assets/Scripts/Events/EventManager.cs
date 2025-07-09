using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityAction OnShuffleEvent;
    public UnityAction OnFinishedRoundEvent;
    public UnityAction OnNewRoundEvent;
    public UnityAction <GameObject> OnNewDialogueCanvasEvent;

    private List <EventListener> _eventListeners = new(); 
    private CanvasListener _canvasListener;

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

    public void AddCanvasListener(CanvasListener canvasListener)
    {
        _canvasListener = canvasListener;
    }

    public void ClearCanvasListener()
    {
        _canvasListener = null;
    }

    private void Start()
    {
        OnShuffleEvent += OnShuffleEventCalled;
        OnFinishedRoundEvent += OnFinishedRoundEventCalled;
        OnNewRoundEvent += OnNewRoundEventCalled;
        OnNewDialogueCanvasEvent += OnNewDialogueCanvasEventCalled;
    }

    private void OnNewDialogueCanvasEventCalled(GameObject canvas)
    {
        _canvasListener.OnCanvasEventCalled(canvas);
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
