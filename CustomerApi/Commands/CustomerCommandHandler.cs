using CustomerApi.Events;
using CustomerApi.Models.Repositories;
using CustomerApi.Models.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Commands
{
    public class CustomerCommandHandler: ICommandHandler<Command>
    {
        private ICustomerRepository _repository;
        private AMQPEventPublisher _eventPublisher;
        public CustomerCommandHandler(AMQPEventPublisher eventPublisher, ICustomerRepository repository)
        {
            _eventPublisher = eventPublisher;
            _repository = repository;
        }
        public void Execute(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command is null");
            }
            if (command is CreateCustomerCommand createCommand)
            {
                CustomerRecord created = _repository.Create(createCommand.ToCustomerRecord());
                _eventPublisher.PublishEvent(createCommand.ToCustomerEvent(created.Id));
            }
            else if (command is UpdateCustomerCommand updateCommand)
            {
                CustomerRecord record = _repository.GetById(updateCommand.Id);
                _repository.Update(updateCommand.ToCustomerRecord(record));
                _eventPublisher.PublishEvent(updateCommand.ToCustomerEvent());
            }
            else if (command is DeleteCustomerCommand deleteCommand)
            {
                _repository.Remove(deleteCommand.Id);
                _eventPublisher.PublishEvent(deleteCommand.ToCustomerEvent());
            }
        }
    }
}
