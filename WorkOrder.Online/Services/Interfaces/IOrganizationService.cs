using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<SelectListItem>> GetOrganizationsSelectList();
        Task<IEnumerable<OrganizationViewModel>> GetOrganizations();
    }
}
