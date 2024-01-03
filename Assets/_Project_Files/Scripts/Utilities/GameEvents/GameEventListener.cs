using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventResponse
{
    public GameEvent Event;
    public int EventID;
    public UnityEvent Response;
}

public class GameEventListener : MonoBehaviour
{
    public List<EventResponse> EventResponses;

    private void OnEnable()
    {
        foreach (var eventResponse in EventResponses)
        {
            eventResponse.Event.RegisterListener(this, eventResponse.EventID);
        }
    }

    private void OnDisable()
    {
        foreach (var eventResponse in EventResponses)
        {
            eventResponse.Event.UnregisterListener(this, eventResponse.EventID);
        }
    }

    public void OnEventRaised(GameEvent raisedEvent, int eventID)
    {
        foreach (var eventResponse in EventResponses)
        {
            if (eventResponse.Event == raisedEvent && eventResponse.EventID == eventID)
            {
                eventResponse.Response.Invoke();
            }
        }
    }
}
