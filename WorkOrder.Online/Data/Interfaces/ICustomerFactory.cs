using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface ICustomerFactory
    {
        Task<IEnumerable<CustomerModel>> GetCustomers(int organizationId);
        Task<int> Create(CustomerModel model);
        Task<int> Update(CustomerModel model);
        Task<int> Delete(int customerId);
        Task<IEnumerable<ResponsibleModel>> GetResponsibles(int customerId);
        Task<int> CreateResponsible(ResponsibleModel model);
        Task<int> UpdateResponsible(CustomerModel model);
        Task<int> DeleteResponsible(int responsibleId);
        Task<int> ImportCustomers(IEnumerable<CustomerViewModel> customers);
    }
}
