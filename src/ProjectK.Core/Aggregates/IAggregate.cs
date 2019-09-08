using System;

namespace ProjectK.Core.Aggregates
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }
    }
}
