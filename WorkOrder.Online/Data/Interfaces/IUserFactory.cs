using WorkOrder.Online.Data.Models;

namespace WorkOrder.Online.Data.Interfaces
{
    public interface IUserFactory
    {
        Task<IEnumerable<UserFacadeModel>> GetUserFacade(int organizationId = 0);

    }
}
