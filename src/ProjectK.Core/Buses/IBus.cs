using System;

namespace ProjectK.Core.Buses
{
    public interface IBus
    {
        void Publish(object toPublish);
        void RegisterListener(Action<object> listener);
        void DeregisterListener(Action<object> listener);
    }

    public interface IBus<T>
    {
        void Publish(T toPublish);
        void RegisterListener(Action<T> listener);
        void DeregisterListener(Action<T> listener);
    }
}
