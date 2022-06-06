using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface ITaskFactory
    {
        Task<IEnumerable<TaskModel>> GetTasks(int organizationId);
    }
}
