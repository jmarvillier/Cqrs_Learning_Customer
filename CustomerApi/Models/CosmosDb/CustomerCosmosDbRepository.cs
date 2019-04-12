using CustomerApi.Models.Repositories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models.CosmosDb
{
    public class CustomerCosmosDbRepository : IDBCustomerRepository<CustomerObject>
    {
        private readonly DocumentClient _client;
        private readonly string _customerDB = "CustomerDB";
        private readonly string _customerCollection = "Customers";
        private readonly FeedOptions _feedOptions = new FeedOptions { PartitionKey = new PartitionKey("customer") };
        private readonly RequestOptions _requestOptions = new RequestOptions { PartitionKey = new PartitionKey("customer") };

        public CustomerCosmosDbRepository()
        {
            _client = new DocumentClient(new Uri("https://localhost:8081"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }

        public async void Create(CustomerObject customer)
        {
            if (!await IsCustomerExists(customer))
                await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_customerDB, _customerCollection), customer);
        }

        public CustomerObject GetCustomer(long id)
        {
            var customer = _client.CreateDocumentQuery<CustomerEntity>(
                          UriFactory.CreateDocumentCollectionUri(_customerDB, _customerCollection), _feedOptions)
                          .Where(_ => _.Id == id);

            return customer.FirstOrDefault();
        }

        public CustomerObject GetCustomerByEmail(string mail)
        {
            var customer = _client.CreateDocumentQuery<CustomerEntity>(
              UriFactory.CreateDocumentCollectionUri(_customerDB, _customerCollection), _feedOptions)
              .Where(_ => _.Email == mail);

            return customer.FirstOrDefault();
        }

        public List<CustomerObject> GetCustomers()
        {
            var customer = _client.CreateDocumentQuery<CustomerEntity>(
                UriFactory.CreateDocumentCollectionUri(_customerDB, _customerCollection), _feedOptions)
                .Where(_ => true)
                .ToList();

            customer.ToList().ForEach(_ => _.Age = 21);

            var collection = _client.CreateDocumentCollectionQuery(
               UriFactory.CreateDatabaseUri(_customerDB))
               .Where(col => col.Id == _customerCollection)
               .AsEnumerable()
               .FirstOrDefault();

            return null;
        }

        public async void Remove(long id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_customerDB, _customerCollection, id.ToString()));
        }

        public async void Update(CustomerObject customer)
        {
            var customerData = (CustomerEntity)customer;

            if (await IsCustomerExists(customer))
            {
                await _client.UpsertDocumentAsync(UriFactory.CreateDocumentUri(_customerDB, _customerCollection, customerData.Id.ToString()), customer);
            }
        }

        private async Task<bool> IsCustomerExists(CustomerObject customer)
        {
            try
            {
                var customerData = (CustomerEntity)customer;
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_customerDB, _customerCollection, customerData.Id.ToString()), _requestOptions);

            }
            catch (DocumentClientException)
            {
                return false;
            }

            return true;
        }
    }
}
