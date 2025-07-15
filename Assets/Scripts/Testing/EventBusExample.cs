using UnityEngine;

public class EventBusExample : MonoBehaviour
{
    private void Start()
    {
        EventBus.Instance.Subscribe<CardShuffledEvent>(OnCardShuffled);
        EventBus.Instance.Subscribe<RoundFinishedEvent>(OnRoundFinished);
    }

    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.Unsubscribe<CardShuffledEvent>(OnCardShuffled);
            EventBus.Instance.Unsubscribe<RoundFinishedEvent>(OnRoundFinished);
        }
    }

    private void OnCardShuffled(CardShuffledEvent cardEvent)
    {
        Debug.Log($"Cards shuffled! Total: {cardEvent.TotalCards}, Round: {cardEvent.RoundNumber}, Time: {cardEvent.Timestamp}");
    }

    private void OnRoundFinished(RoundFinishedEvent roundEvent)
    {
        Debug.Log($"Round {roundEvent.RoundNumber} finished. Success: {roundEvent.WasSuccessful}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventBus.Instance.Publish(new CardShuffledEvent(totalCards: 52, roundNumber: 1));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            EventBus.Instance.Publish(new RoundFinishedEvent(roundNumber: 1, wasSuccessful: true));
        }
    }
}