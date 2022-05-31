using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<SelectListItem>> GetOrganizationsSelectList();
        Task<IEnumerable<OrganizationViewModel>> GetOrganizations();
        Task<OrganizationViewModel> GetOrganization(int organizationId);
        Task<int> Create(OrganizationViewModel model);
        Task<int> Update(OrganizationViewModel model);
        Task<int> Delete(int organizationId);
    }
}
