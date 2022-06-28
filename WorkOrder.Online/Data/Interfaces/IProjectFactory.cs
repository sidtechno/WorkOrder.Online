using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IProjectFactory
    {
        Task<IEnumerable<ProjectModel>> GetProjects(int organizationId);
        Task<int> Create(ProjectModel model);
        Task<int> Update(ProjectModel model);
        Task<int> Delete(int projectId);

    }
}
