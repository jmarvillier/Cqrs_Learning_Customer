using System.Collections.Generic;

namespace CustomerApi.Models.Repositories
{
    public interface IDBCustomerRepository<T>
    {
        List<T> GetCustomers();

        T GetCustomer(long id);

        T GetCustomerByEmail(string mail);

        void Create(T customer);

        void Update(T customer);

        void Remove(long id);
    }
}
