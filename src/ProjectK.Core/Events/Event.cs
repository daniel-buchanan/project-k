using System;

namespace ProjectK.Core.Events
{
    public class Event : IEvent
    {
        public Event()
        {
            
        }

        public Event(int sequence, Guid subject, string aggregate, string kind, string data, DateTimeOffset timestamp)
        {
            Sequence = sequence;
            Kind = kind;
            PrimaryAggregate = aggregate;
            Subject = subject;
            Data = data;
            Timestamp = timestamp;
        }

        public int Sequence { get; }
        public DateTimeOffset Timestamp { get; }
        public string Kind { get; }
        public string PrimaryAggregate { get; }
        public Guid Subject { get; }
        public string Data { get; }
    }
}
