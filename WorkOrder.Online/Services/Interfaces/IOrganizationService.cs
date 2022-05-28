using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<IEnumerable<SelectListItem>> GetOrganizationsSelectList();
    }
}
