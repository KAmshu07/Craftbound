using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class EventDispatcher
{
    private static readonly object lockObject = new object();
    private static readonly Dictionary<Type, List<Subscriber>> GlobalSubscribers = new Dictionary<Type, List<Subscriber>>();
    private static readonly Dictionary<Type, Dictionary<string, List<Subscriber>>> ChannelSubscribers = new Dictionary<Type, Dictionary<string, List<Subscriber>>>();

    public static bool DebugMode { get; set; } = false;

    public static void Subscribe<T>(Action<T> handler, string channel = null, Func<T, bool> filter = null, int priority = 0) where T : class
    {
        lock (lockObject)
        {
            var subscriber = new Subscriber
            {
                Handler = handler,
                Filter = filter,
                Priority = priority
            };

            if (channel == null)
            {
                if (!GlobalSubscribers.ContainsKey(typeof(T)))
                {
                    GlobalSubscribers[typeof(T)] = new List<Subscriber>();
                }
                GlobalSubscribers[typeof(T)].Add(subscriber);
                GlobalSubscribers[typeof(T)].Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }
            else
            {
                if (!ChannelSubscribers.ContainsKey(typeof(T)))
                {
                    ChannelSubscribers[typeof(T)] = new Dictionary<string, List<Subscriber>>();
                }

                if (!ChannelSubscribers[typeof(T)].ContainsKey(channel))
                {
                    ChannelSubscribers[typeof(T)][channel] = new List<Subscriber>();
                }

                ChannelSubscribers[typeof(T)][channel].Add(subscriber);
                ChannelSubscribers[typeof(T)][channel].Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }

            if (DebugMode)
            {
                Debug.Log($"Subscribed: {typeof(T).Name}, Channel: {channel}, Priority: {priority}");
            }
        }
    }

    public static void Unsubscribe<T>(Action<T> handler, string channel = null) where T : class
    {
        lock (lockObject)
        {
            bool unsubscribed = false;
            if (channel == null)
            {
                if (GlobalSubscribers.ContainsKey(typeof(T)))
                {
                    unsubscribed = GlobalSubscribers[typeof(T)].RemoveAll(s => s.Handler.Equals(handler)) > 0;
                }
            }
            else
            {
                if (ChannelSubscribers.ContainsKey(typeof(T)) && ChannelSubscribers[typeof(T)].ContainsKey(channel))
                {
                    unsubscribed = ChannelSubscribers[typeof(T)][channel].RemoveAll(s => s.Handler.Equals(handler)) > 0;
                }
            }

            if (DebugMode && unsubscribed)
            {
                Debug.Log($"Unsubscribed: {typeof(T).Name}, Channel: {channel}");
            }
        }
    }

    public static void Publish<T>(T eventData, string channel = null) where T : class
    {
        lock (lockObject)
        {
            List<Subscriber> subscribers = null;

            if (channel == null)
            {
                if (GlobalSubscribers.ContainsKey(typeof(T)))
                {
                    subscribers = GlobalSubscribers[typeof(T)];
                }
            }
            else
            {
                if (ChannelSubscribers.ContainsKey(typeof(T)) && ChannelSubscribers[typeof(T)].ContainsKey(channel))
                {
                    subscribers = ChannelSubscribers[typeof(T)][channel];
                }
            }

            if (subscribers != null)
            {
                foreach (var subscriber in subscribers)
                {
                    if (subscriber.Filter == null || ((Func<T, bool>)subscriber.Filter)(eventData))
                    {
                        try
                        {
                            ((Action<T>)subscriber.Handler)(eventData);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error while handling event: {typeof(T).Name}, Handler: {subscriber.Handler.GetType().Name}, Error: {ex}");
                        }
                    }
                }
            }

            if (DebugMode)
            {
                Debug.Log($"Published: {typeof(T).Name}, Channel: {channel}");
            }
        }
    }

    public static async Task PublishAsync<T>(T eventData, string channel = null) where T : class
    {
        await Task.Run(() => Publish(eventData, channel));
    }

    private class Subscriber
    {
        public object Handler { get; set; }
        public object Filter { get; set; }
        public int Priority { get; set; }
    }
}
