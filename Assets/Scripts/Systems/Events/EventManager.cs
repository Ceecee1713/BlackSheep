using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public UnityAction OnShuffleEvent; //Used for when the player clicks the Shuffle Button
    public UnityAction OnShuffleEventComplete; //Used for when the player clicks the Shuffle Button AND after a shuffle finishes from starting a new round ("OnNewRoundEvent")
    public UnityAction OnFinishedRoundEvent;
    public UnityAction OnNewRoundEvent;

    public UnityAction <bool> OnNoInputEvent;

    public UnityAction <GameObject, bool> OnNewCanvasEvent;
    public UnityAction <bool> OnFadeCurrentCanvasEvent;
    

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
        OnShuffleEventComplete += OnShuffleEventCompleteCalled;
        OnFinishedRoundEvent += OnFinishedRoundEventCalled;
        OnNewRoundEvent += OnNewRoundEventCalled;

        OnNoInputEvent += OnNoInputEventCalled;

        OnNewCanvasEvent += OnNewCanvasEventCalled;
        OnFadeCurrentCanvasEvent += OnFadeCurrentCanvasEventCalled;
    }

    private void OnNewCanvasEventCalled(GameObject canvas, bool isThisADialogueCanvas)
    {
        _canvasListener.OnSwitchCanvasEventCalled(canvas, isThisADialogueCanvas);
    }

    private void OnFadeCurrentCanvasEventCalled(bool fadeCanvasIn)
    {
        _canvasListener.OnFadeCurrentCanvasAlpha(fadeCanvasIn);
    }

    //Methods below: //For each "eventListener" inside the "_eventListeners" List, call the "OnEventCalled(AllEventNames)" method 

    private void OnNoInputEventCalled(bool allowInput)
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnNoInputEventCalled(allowInput);
        });
    }

    

    private void OnShuffleEventCalled()
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnEventCalled(AllEventNames.ShuffleEvent);
        });
    }

    private void OnShuffleEventCompleteCalled()
    {
        _eventListeners.ForEach((eventListener) => {
            eventListener.OnEventCalled(AllEventNames.ShuffleEventComplete);
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
