namespace ProjectK.Core.Events
{
    public interface IEventProcessor
    {
        string Kind { get; }

        void Process(IEvent @event);
    }
}
