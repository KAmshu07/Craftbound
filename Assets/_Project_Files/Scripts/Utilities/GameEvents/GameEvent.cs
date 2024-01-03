using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly Dictionary<int, List<GameEventListener>> eventListeners = new Dictionary<int, List<GameEventListener>>();

    public void Raise(int eventID)
    {
        if (eventListeners.ContainsKey(eventID))
        {
            List<GameEventListener> listenersCopy = new List<GameEventListener>(eventListeners[eventID]);
            foreach (var listener in listenersCopy)
            {
                listener.OnEventRaised(this, eventID);
            }
        }
    }

    public void RegisterListener(GameEventListener listener, int eventID)
    {
        if (!eventListeners.ContainsKey(eventID))
        {
            eventListeners[eventID] = new List<GameEventListener>();
        }

        if (!eventListeners[eventID].Contains(listener))
        {
            eventListeners[eventID].Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener, int eventID)
    {
        if (eventListeners.ContainsKey(eventID) && eventListeners[eventID].Contains(listener))
        {
            eventListeners[eventID].Remove(listener);
        }
    }
}
