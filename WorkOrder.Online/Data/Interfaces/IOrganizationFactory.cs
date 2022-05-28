using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IOrganizationFactory
    {
        Task<IEnumerable<OrganizationModel>> GetOrganizations();

    }
}
