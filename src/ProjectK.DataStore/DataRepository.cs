using System.Collections;
using System.Collections.Generic;
using ProjectK.Core.Aggregates;

namespace ProjectK.DataStore
{
    public class DataRepository<T> : IDataRepository<T> where T : IAggregate
    {
        private readonly List<T> _internal;

        public DataRepository()
        {
            _internal = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T @object)
        {
            _internal.Add(@object);
        }

        public void Update(T @object)
        {
            var index = _internal.FindIndex(i => i.Id == @object.Id);

            if(index < 0) _internal.Add(@object);
            else _internal[index] = @object;
        }

        public void Clear() => _internal.Clear();
    }
}
