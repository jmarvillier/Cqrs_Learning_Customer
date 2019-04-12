using CustomerApi.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Commands
{
    public class DeleteCustomerCommand: Command
    {
        internal CustomerDeletedEvent ToCustomerEvent()
        {
            return new CustomerDeletedEvent
            {
                Id = this.Id
            };
        }
    }
}
