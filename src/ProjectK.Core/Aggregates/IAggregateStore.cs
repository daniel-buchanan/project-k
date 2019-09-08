using System;

namespace ProjectK.Core.Aggregates
{
    public interface IAggregateStore<T> where T: IAggregate
    {
        void Persist(T aggregate);
        T Get(Guid id);
    }
}
