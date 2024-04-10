using System;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour
{
    private List<Action> unsubscribeActions = new List<Action>();

    protected void Subscribe<T>(Action<T> handler, string channel = null, Func<T, bool> filter = null, int priority = 0) where T : class
    {
        EventDispatcher.Subscribe(handler, channel, filter, priority);
        unsubscribeActions.Add(() => EventDispatcher.Unsubscribe(handler, channel));
    }

    private void OnDisable()
    {
        foreach (var action in unsubscribeActions)
        {
            action();
        }

        unsubscribeActions.Clear();
    }
}
