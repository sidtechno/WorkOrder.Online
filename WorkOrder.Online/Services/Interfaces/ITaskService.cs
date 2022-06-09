using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskViewModel>> GetTasks(int organizationId);
        Task<int> Create(TaskViewModel model);
        Task<int> Update(TaskViewModel model);
        Task<int> Delete(int taskId);
    }
}
