using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Security.Claims;
using WorkOrder.Online.Data.Interfaces;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserFactory _userFactory;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(SignInManager<IdentityUser> userIdentity,
            IUserFactory userFactory,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _userIdentity = userIdentity;
            _contextAccessor = contextAccessor;
            _userFactory = userFactory;
            _userManager = userManager;
        }
        public UserCredentials GetCurrentUserCredentials()
        {
            var user = new UserCredentials()
            {
                UserId = GetUserId(),
                Claims = GetCurrentUserClaims(),
                FullName = GetUserFullName(),
                Language = GetCurrentLanguage()
            };

            return user;
        }

        public async Task<IEnumerable<UserViewModel>> GetUserFacade(int garageId = 0)
        {
            var users = await _userFactory.GetUserFacade(garageId);

            return users.Adapt<IEnumerable<UserViewModel>>();
        }

        public async Task<IList<Claim>> GetUserClaims(string userId)
        {
            var user = await GetUserById(userId);
            return await _userIdentity.UserManager.GetClaimsAsync(user);
        }

        public async Task<IdentityUser> GetUserById(string userId)
        {
            return await _userIdentity.UserManager.FindByIdAsync(userId);
        }

        public async Task<int> GetRemainingUsers(int garageId)
        {
            //var garage = await _garageFactory.GetGarage(garageId);
            //var currentUsers = await GetUsersForClaim("GarageId", garageId.ToString());

            //if (garage == null)
            //    throw new ApplicationException($"Garage id {garageId} not found in database");

            //var maxUserCount = garage.NbrUser;
            //var currentUserCount = currentUsers.Count();

            //return (maxUserCount - currentUserCount) < 0 ? 0 : (maxUserCount - currentUserCount);
            return 0;
        }

        private int GetGarageId()
        {
            var garageId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "GarageId")?.Value;
            return Convert.ToInt32(garageId);
        }

        private string GetUserId()
        {
            var userId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            return userId;
        }

        private string GetUserFullName()
        {
            var userId = $"{_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "FirstName")?.Value} {_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "LastName")?.Value}";
            return userId;
        }

        private IEnumerable<Claim> GetCurrentUserClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }


        private async Task<IEnumerable<IdentityUser>> GetUsersForClaim(string userClaimType, string claimValue)
        {
            var claim = new Claim(userClaimType, claimValue);
            var users = await _userManager.GetUsersForClaimAsync(claim);
            return users;
        }

        private string GetCurrentLanguage()
        {
            var feature = _contextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            return feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
        }
    }
}
