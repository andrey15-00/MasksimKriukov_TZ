using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGame
{
    public class EventDispatcher : MonoBehaviour
    {
        private Dictionary<Type, List<Action<IGameMessage>>> _subscribers = new Dictionary<Type, List<Action<IGameMessage>>>();

        public void Publish<T>(T message) where T : IGameMessage
        {
            Type type = typeof(T);

            List<Action<IGameMessage>> subscribers;
            if (_subscribers.TryGetValue(type, out subscribers))
            {
                foreach(var subscriber in subscribers)
                {
                    subscriber?.Invoke(message);
                }
            }
        }

        public void Subscribe<T>(Action<T> subscriber) where T : IGameMessage
        {
            Type type = typeof(T);

            List<Action<IGameMessage>> subscribers;

            Action<IGameMessage> action = (data) =>
            {
                subscriber?.Invoke((T)data);
            };

            if (_subscribers.TryGetValue(type, out subscribers))
            {
                subscribers.Add(action);
            }
            else
            {
                _subscribers[type] = new List<Action<IGameMessage>>()
                {
                    action
                };
            }
        }
    }
}