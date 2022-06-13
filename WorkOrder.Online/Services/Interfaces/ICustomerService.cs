using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetCustomers(int organizationId);
        Task<int> Create(CustomerViewModel model);
        Task<int> Update(CustomerViewModel model);
        Task<int> Delete(int customerId);
    }
}
