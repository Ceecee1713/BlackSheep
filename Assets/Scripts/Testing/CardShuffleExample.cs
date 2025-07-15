using UnityEngine;

public class CardShuffleExampleReceiver : MonoBehaviour
{
    void Start()
    {
        EventBus.Instance.Subscribe<CardShuffleInfo>(HandleCardShuffle);
        EventBus.Instance.Subscribe<PlayerFootStep>(HandlePlayerFootstep);
    }

    void HandleCardShuffle(CardShuffleInfo info)
    {
        Debug.Log(info.IsShuffled);
    }

    void HandlePlayerFootstep(PlayerFootStep footStep)
    {
        Debug.Log($"Player location is {footStep.PlayerLocation}");
    }
}

public class CardShuffleExampleSender : MonoBehaviour
{
    void Start()
    {
        Invoke("SendMessage", 1f);
    }

    void SendMessage()
    {
        var playerTransform = transform.position;
        PlayerFootStep footstep = new PlayerFootStep(playerTransform, 1f);
        EventBus.Instance.Publish(new CardShuffleInfo(){IsShuffled = true}); //This is an event invoke.
        //If this event doesn't exist, make a new event
        //If it does exist, just invoke the event
             
        //imagine this is a player
        EventBus.Instance.Publish(footstep);
    }
}
