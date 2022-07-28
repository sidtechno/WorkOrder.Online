using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderFactory _workOrderFactory;

        public WorkOrderService(IWorkOrderFactory workOrderFactory)
        {
            _workOrderFactory = workOrderFactory;
        }

        //public async Task<IEnumerable<ProjectViewModel>> GetProjects(int organizationId, bool activeOnly = true)
        //{
        //    var projects = await _projectFactory.GetProjects(organizationId);

        //    if (activeOnly)
        //    {
        //        projects = projects.Where(p => !p.IsDeleted);
        //    }

        //    foreach(var project in projects)
        //    {
        //        project.ProjectsCategories = await _projectFactory.GetProjectCategories(project.Id);
        //    }

        //    return projects.Adapt<IEnumerable<ProjectViewModel>>();
        //}

        //public async Task<IEnumerable<ProjectCategoryViewModel>> GetProjectCategories(int projectId)
        //{
        //    var categories = await _projectFactory.GetProjectCategories(projectId);
        //    return categories.Adapt<IEnumerable<ProjectCategoryViewModel>>();
        //}

        //public async Task<int> Create(ProjectViewModel model)
        //{
        //    try
        //    {
        //        var factoryModel = model.Adapt<ProjectModel>();
        //        var projectId = await _projectFactory.Create(factoryModel);

        //        return projectId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> Update(ProjectViewModel model)
        //{
        //    try
        //    {
        //        var factoryModel = model.Adapt<ProjectModel>();
        //        var projectId = await _projectFactory.Update(factoryModel);

        //        return projectId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<int> Delete(int projectId)
        //{
        //    return await _projectFactory.Delete(projectId);
        //}
    }
}
