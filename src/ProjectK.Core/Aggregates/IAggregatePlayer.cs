using System;

namespace ProjectK.Core.Aggregates
{
    public interface IAggregatePlayer<T> where T: IAggregate
    {
        T PlayTo(Guid subjectId, DateTime timestamp);
        T PlayTo(Guid subjectId, int sequence);
        T PlayToNow(Guid subjectId);
    }
}
