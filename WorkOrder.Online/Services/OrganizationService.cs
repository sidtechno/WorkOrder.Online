using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationFactory _organizationFactory;

        public OrganizationService(IOrganizationFactory organizationFactory)
        {
            _organizationFactory = organizationFactory;
        }

        public async Task<int> Create(OrganizationViewModel model)
        {
            try
            {
                model.IsActive = true;
                model.CreationDate = DateTime.Now;

                var factoryModel = model.Adapt<OrganizationModel>();
                var organizationId = await _organizationFactory.Create(factoryModel);

                return organizationId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Update(OrganizationViewModel model)
        {
            try
            {
                var factoryModel = model.Adapt<OrganizationModel>();
                var organizationId = await _organizationFactory.Update(factoryModel);

                return organizationId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int organizationId)
        {
            return await _organizationFactory.Delete(organizationId);
        }

        public async Task<IEnumerable<OrganizationViewModel>> GetOrganizations()
        {
            var organizations = await _organizationFactory.GetOrganizations();
            return organizations.Adapt<IEnumerable<OrganizationViewModel>>();
        }
        public async Task<OrganizationViewModel> GetOrganization(int organizationId)
        {
            var organization = await _organizationFactory.GetOrganization(organizationId);
            return organization.Adapt<OrganizationViewModel>();
        }

        public async Task<IEnumerable<SelectListItem>> GetOrganizationsSelectList()
        {
            var organizations = await _organizationFactory.GetOrganizations();

            return organizations.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).OrderBy(o => o.Text);
        }
    }
}
