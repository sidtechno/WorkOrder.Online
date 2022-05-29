using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkOrder.Online.Data.Interfaces;
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

        public async Task<IEnumerable<OrganizationViewModel>> GetOrganizations()
        {
            var organizations = await _organizationFactory.GetOrganizations();
            return organizations.Adapt<IEnumerable<OrganizationViewModel>>();
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
