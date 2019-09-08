using System.Collections;
using System.Collections.Generic;

namespace ProjectK.Core
{
    public class ImmutableList<T> : IImmutableList<T>
    {
        private readonly List<T> _internal;

        public ImmutableList()
        {
            _internal = new List<T>();
        }

        protected void Add(T obj)
        {
            _internal.Add(obj);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
