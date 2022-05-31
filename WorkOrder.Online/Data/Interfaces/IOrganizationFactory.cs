using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IOrganizationFactory
    {
        Task<IEnumerable<OrganizationModel>> GetOrganizations();
        Task<int> Create(OrganizationModel model);
        Task<int> Update(OrganizationModel model);
        Task<int> Delete(int organizationId);
        Task<OrganizationModel> GetOrganization(int organizationId);
    }
}
