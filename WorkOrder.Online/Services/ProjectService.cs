using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectFactory _projectFactory;

        public ProjectService(IProjectFactory projectFactory)
        {
            _projectFactory = projectFactory;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjects(int organizationId)
        {
            var products = await _projectFactory.GetProjects(organizationId);
            return products.Adapt<IEnumerable<ProjectViewModel>>();
        }

        public async Task<int> Create(ProjectViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<ProjectModel>();
                var projectId = await _projectFactory.Create(factoryModel);

                return projectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(ProjectViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<ProjectModel>();
                var projectId = await _projectFactory.Update(factoryModel);

                return projectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int projectId)
        {
            return await _projectFactory.Delete(projectId);
        }
    }
}
