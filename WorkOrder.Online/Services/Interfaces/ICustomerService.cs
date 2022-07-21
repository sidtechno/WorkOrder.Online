using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetCustomers(int organizationId);
        Task<IEnumerable<SelectListItem>> GetCustomersSelectList(int organizationId);
        Task<int> Create(CustomerViewModel model);
        Task<int> Update(CustomerViewModel model);
        Task<int> Delete(int customerId);
        Task<IEnumerable<ResponsibleViewModel>> GetResponsibles(int customerId);
        Task<int> AddResponsible(ResponsibleViewModel model);
        Task<int> UpdateResponsible(ResponsibleViewModel model);
        Task<int> DeleteResponsible(int responsibleId);
        Task<int> ImportCustomers(IEnumerable<CustomerViewModel> customers);
    }
}
