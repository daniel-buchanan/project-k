using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectK.Core.Commands
{
    public interface ICommandAuthoriser<T> where T: ICommand
    {
        bool Authorise(T command);
    }
}
