using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Commands
{
    public interface ICommandHandler<T> where T : Command
    {
        void Execute(T command);
    }
}
