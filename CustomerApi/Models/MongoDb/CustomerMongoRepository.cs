using CustomerApi.Models.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models.MongoDb
{
    public class CustomerMongoRepository : IDBCustomerRepository<CustomerObject>
    {
        private readonly IMongoDatabase _db;
        private readonly string _customerDB = "CustomerDB";
        private readonly string _customerCollection = "Customers";

        public CustomerMongoRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _db = client.GetDatabase(_customerDB);
        }

        public void Create(CustomerObject customer)
        {
            _db.GetCollection<CustomerEntity>(_customerCollection).InsertOne((CustomerEntity)customer);
        }

        public CustomerObject GetCustomer(long id)
        {
            return _db.GetCollection<CustomerEntity>(_customerCollection).Find(_ => _.Id == id).SingleOrDefault();
        }

        public CustomerObject GetCustomerByEmail(string mail)
        {
            return _db.GetCollection<CustomerEntity>(_customerCollection).Find(_ => _.Email == mail).SingleOrDefault();
        }

        public List<CustomerObject> GetCustomers()
        {
            return _db.GetCollection<CustomerEntity>(_customerCollection).Find(_ => true).ToList().ToList<CustomerObject>();
        }

        public void Remove(long id)
        {
            var filter = Builders<CustomerEntity>.Filter.Where(_ => _.Id == id);
            _db.GetCollection<CustomerEntity>(_customerCollection).DeleteOne(filter);
        }

        public void Update(CustomerObject customer)
        {
            var customerData = (CustomerEntity)customer;
            var filter = Builders<CustomerEntity>.Filter.Where(_ => _.Id == customerData.Id);
            _db.GetCollection<CustomerEntity>(_customerCollection).ReplaceOne(filter, customerData);
        }
    }
}
