using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private string _currentLanguage;

        public BaseController(IHttpContextAccessor httpContextAccessor,
            IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public string BaseRootUrl => BuildRootUrl();

        private string BuildRootUrl()
        {
            return $"{_httpContextAccessor.HttpContext?.Request?.Scheme}://{_httpContextAccessor.HttpContext?.Request?.Host}";
        }

        public UserCredentials CurrentUser
        {
            get { return _userService.GetCurrentUserCredentials(); }
        }
    }
}
