using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskFactory _taskFactory;

        public TaskService(ITaskFactory taskFactory)
        {
            _taskFactory = taskFactory;
        }

        public async Task<IEnumerable<TaskViewModel>> GetTasks(int organizationId)
        {
            var tasks = await _taskFactory.GetTasks(organizationId);
            return tasks.Adapt<IEnumerable<TaskViewModel>>();
        }

        public async Task<int> Create(TaskViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<TaskModel>();
                var taskId = await _taskFactory.Create(factoryModel);

                return taskId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(TaskViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<TaskModel>();
                var taskId = await _taskFactory.Update(factoryModel);

                return taskId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int taskId)
        {
            return await _taskFactory.Delete(taskId);
        }


        //public async Task<OrganizationViewModel> GetOrganization(int organizationId)
        //{
        //    var organization = await _organizationFactory.GetOrganization(organizationId);
        //    return organization.Adapt<OrganizationViewModel>();
        //}

        //public async Task<IEnumerable<SelectListItem>> GetOrganizationsSelectList()
        //{
        //    var organizations = await _organizationFactory.GetOrganizations();

        //    return organizations.Select(x => new SelectListItem
        //    {
        //        Value = x.Id.ToString(),
        //        Text = x.Name
        //    }).OrderBy(o => o.Text);
        //}
    }
}
