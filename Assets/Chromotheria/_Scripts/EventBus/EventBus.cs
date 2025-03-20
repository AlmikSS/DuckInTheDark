using System;
using System.Collections.Generic;
using UnityEngine;

namespace VH.Tools
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _events = new();

        public void Register<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_events.ContainsKey(type))
                _events[type] = new List<Delegate>();
            _events[type].Add(handler);
        }

        public void Unregister<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_events.ContainsKey(type))
            {
                _events[type].Remove(handler);
                if (_events[type].Count == 0)
                    _events.Remove(type);
            }
        }

        public void Invoke<T>(T e) where T : Event
        {
            var type = typeof(T);
            if (_events.ContainsKey(type))
            {
                foreach (var handler in _events[type])
                {
                    try
                    {
                        var action = handler as Action<T>;
                        action?.Invoke(e);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Ошибка в обработчике события: {ex.Message}");
                    }
                }
            }
        } 
    }

    public abstract class Event
    {
        public readonly object Sender;

        public Event(object sender)
        {
            Sender = sender;
        }
    }
}