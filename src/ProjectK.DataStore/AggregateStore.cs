using System;
using System.Linq;
using ProjectK.Core.Aggregates;

namespace ProjectK.DataStore
{
    public class AggregateStore<T> : IAggregateStore<T> where T: IAggregate
    {
        private readonly IDataRepository<T> _store;

        public AggregateStore(IDataRepository<T> store)
        {
            _store = store;
        }

        public void Persist(T aggregate)
        {
            _store.Update(aggregate);
        }

        public T Get(Guid id)
        {
            return _store.FirstOrDefault(i => i.Id == id);
        }
    }
}
