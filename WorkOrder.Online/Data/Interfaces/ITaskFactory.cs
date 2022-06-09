using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface ITaskFactory
    {
        Task<IEnumerable<TaskModel>> GetTasks(int organizationId);
        Task<int> Create(TaskModel model);
        Task<int> Update(TaskModel model);
        Task<int> Delete(int taskId);

    }
}
