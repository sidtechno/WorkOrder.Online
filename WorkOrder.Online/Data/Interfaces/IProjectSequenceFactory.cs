using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IProjectSequenceFactory
    {
        Task<ProjectSequenceModel> GetProjectSequence(int organizationId);
        //Task<int> Create(ProjectModel model);
        //Task<int> Update(ProjectModel model);
        //Task<int> Delete(int projectId);

    }
}
