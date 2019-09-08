using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectK.Core.Commands;

namespace ProjectK.Core.Buses
{
    public interface ICommandBus : IBus<ICommand>
    {
    }
}
