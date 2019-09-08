using System;
using System.Collections.Generic;

namespace ProjectK.Core.Buses
{
    public class Bus : IBus
    {
        private readonly List<Action<object>> _listeners;

        public Bus()
        {
            _listeners = new List<Action<object>>();
        }

        public void Publish(object toPublish)
        {
            foreach (var l in _listeners)
            {
                try
                {
                    l(toPublish);
                }
                catch (Exception e)
                {
                    // TODO: DO something with Exception
                }
            }
        }

        public void RegisterListener(Action<object> listener)
        {
            _listeners.Add(listener);
        }

        public void DeregisterListener(Action<object> listener)
        {
            _listeners.Remove(listener);
        }
    }

    public class Bus<T> : IBus<T>
    {
        private readonly List<Action<T>> _listeners;

        public Bus()
        {
            _listeners = new List<Action<T>>();
        }

        public void Publish(T toPublish)
        {
            foreach (var l in _listeners)
            {
                try
                {
                    l(toPublish);
                }
                catch (Exception e)
                {
                    // TODO: DO something with Exception
                }
            }
        }

        public void RegisterListener(Action<T> listener)
        {
            _listeners.Add(listener);
        }

        public void DeregisterListener(Action<T> listener)
        {
            _listeners.Remove(listener);
        }
    }
}
