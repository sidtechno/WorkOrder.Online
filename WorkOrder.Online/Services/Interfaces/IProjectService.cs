using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetProjects(int organizationId);
        Task<int> Create(ProjectViewModel model);
        Task<int> Update(ProjectViewModel model);
        Task<int> Delete(int projectId);
    }
}
