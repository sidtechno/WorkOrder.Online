using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Services.Interfaces
{
    public interface IUserService
    {
        UserCredentials GetCurrentUserCredentials();
        Task<IList<Claim>> GetUserClaims(string userId);
        Task<IdentityUser> GetUserById(string userId);
        Task<int> GetRemainingUsers(int organizationId);
        Task<IEnumerable<UserViewModel>> GetUserFacade(int organizationId = 0);
    }
}
