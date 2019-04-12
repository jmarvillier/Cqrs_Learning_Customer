using CustomerApi.Commands;
using CustomerApi.Models;
using CustomerApi.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CustomerApi.Controllers
{
    [Route("customer")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICommandHandler<Command> _commandHandler;
        private readonly ICustomerRepository _sqLiteRepository;
        private readonly IDBCustomerRepository<CustomerObject> _mongoRepository;

        public CustomersController(
            ICustomerRepository sqLiteRepository,
            IDBCustomerRepository<CustomerObject> mongoRepository,
            ICommandHandler<Command> commandHandler
            )
        {
            _commandHandler = commandHandler;
            _sqLiteRepository = sqLiteRepository;
            _mongoRepository = mongoRepository;

            if (_mongoRepository.GetCustomers().Count == 0)
            {
                var customerCmd = new CreateCustomerCommand
                {
                    Name = "Prénom NOM",
                    Email = "prenomnom.com",
                    Age = 23,
                    Phones = new List<CreatePhoneCommand>
                    {
                        new CreatePhoneCommand { Type = PhoneType.CELLPHONE, AreaCode = 123, Number = 7543010 }
                    }
                };
                _commandHandler.Execute(customerCmd);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public List<CustomerObject> Get()
        {
            return _mongoRepository.GetCustomers();
        }

        [HttpGet]
        [Route("getcustomer/{id}", Name = "getcustomer")]
        public IActionResult GetById(long id)
        {
            var product = _mongoRepository.GetCustomer(id);
            if (product == null)
                return NotFound();

            return new ObjectResult(product);
        }

        [HttpGet]
        [Route("getcustomerbymail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            var product = _mongoRepository.GetCustomerByEmail(email);

            if (product == null)
                return NotFound();

            return new ObjectResult(product);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Post([FromBody] CreateCustomerCommand customer)
        {
            _commandHandler.Execute(customer);
            return CreatedAtRoute("getcustomer", new { id = customer.Id }, customer);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult Put(long id, [FromBody] UpdateCustomerCommand customer)
        {
            var record = _sqLiteRepository.GetById(id);

            if (record == null)
                return NotFound();

            customer.Id = id;
            _commandHandler.Execute(customer);

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult Delete(long id)
        {
            var record = _sqLiteRepository.GetById(id);

            if (record == null)
                return NotFound();

            _commandHandler.Execute(new DeleteCustomerCommand { Id = id });

            return NoContent();
        }

        /*
        [HttpGet]
        [Route("getcustomers")]
        public IActionResult GetAll()
        {
            var customers = _sqLiteRepository.GetAll();

            if (customers == null)
                return NotFound();

            return new ObjectResult(customers);
        }

        [HttpGet]
        [Route("getcustomer/{id}", Name = "getcustomer")]
        public IActionResult GetByID(long id)
        {
            var customer = _sqLiteRepository.GetById(id);

            if (customer == null)
                return NotFound();

            return new ObjectResult(customer);

        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Post([FromBody] CustomerRecord customer)
        {
            var created = _sqLiteRepository.Create(customer);
            return CreatedAtRoute("getcustomer", new { id = created.Id }, created);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public IActionResult Put(long id, [FromBody] CustomerRecord customer)
        {
            var record = _sqLiteRepository.GetById(id);

            if (record == null)
                return NotFound();

            customer.Id = id;
            _sqLiteRepository.Update(customer);

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult Delete(long id)
        {
            var record = _sqLiteRepository.GetById(id);

            if (record == null)
                return NotFound();

            _sqLiteRepository.Remove(id);

            return NoContent();
        }*/
    }
}
