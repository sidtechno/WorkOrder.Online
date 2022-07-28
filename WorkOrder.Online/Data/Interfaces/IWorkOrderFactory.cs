using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IWorkOrderFactory
    {
        Task<IEnumerable<WorkOrderModel>> GetWorkOrders(int organizationId);
        //Task<IEnumerable<ProjectModel>> GetProjects(int organizationId);
        //Task<IEnumerable<ProjectCategoryModel>> GetProjectCategories(int projectId);
        //Task<int> Create(ProjectModel model);
        //Task<int> Update(ProjectModel model);
        //Task<int> Delete(int projectId);

    }
}
