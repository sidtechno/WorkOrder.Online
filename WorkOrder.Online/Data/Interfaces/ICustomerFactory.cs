using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface ICustomerFactory
    {
        Task<IEnumerable<CustomerModel>> GetCustomers(int organizationId);
        Task<int> Create(CustomerModel model);
        Task<int> Update(CustomerModel model);
        Task<int> Delete(int customerId);

    }
}
