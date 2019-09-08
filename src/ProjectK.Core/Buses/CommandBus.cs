using ProjectK.Core.Commands;

namespace ProjectK.Core.Buses
{
    public class CommandBus : Bus<ICommand>, ICommandBus
    {
    }
}
