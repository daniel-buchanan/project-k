using ProjectK.Core.Events;
using ProjectK.Core.Extrapolations;

namespace ProjectK.Core.Extrapolations
{
    public interface IExtrapolator<T> where T: IExtrapolation
    {
        void Extrapolate(IEvent @event);
    }
}
