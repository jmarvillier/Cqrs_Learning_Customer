using CustomerApi.Models.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models.Repositories
{
    public interface ICustomerRepository
    {
        CustomerRecord Create(CustomerRecord customer);

        void Update(CustomerRecord customer);

        void Remove(long id);

        IQueryable<CustomerRecord> GetAll();

        CustomerRecord GetById(long id);
    }
}
