using CustomerApi.Models.Repositories;
using System.Linq;

namespace CustomerApi.Models.SQLite
{
    public class CustomerSQLiteRepository : ICustomerRepository
    {
        private readonly CustomerSQLiteDatabaseContext _context;

        public CustomerSQLiteRepository(CustomerSQLiteDatabaseContext context)
        {
            _context = context;
        }

        public CustomerRecord Create(CustomerRecord customer)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<CustomerRecord> entry = _context.Customers.Add(customer);
            _context.SaveChanges();
            return entry.Entity;
        }

        public IQueryable<CustomerRecord> GetAll()
        {
            return _context.Customers;
        }

        public CustomerRecord GetById(long id)
        {
            return _context.Customers.Find(id);
        }

        public void Remove(long id)
        {
            _context.Customers.Remove(GetById(id));
            _context.SaveChanges();
        }

        public void Update(CustomerRecord customer)
        {
            _context.SaveChanges();
        }
    }
}
