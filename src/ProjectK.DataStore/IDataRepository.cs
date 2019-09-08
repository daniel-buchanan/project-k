using ProjectK.Core.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectK.DataStore
{
    public interface IDataRepository<T> : IEnumerable<T> where T: IAggregate
    {
        void Add(T @object);
        void Update(T @object);
        void Clear();
    }
}
