using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
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
            return $"{_httpContextAccessor.HttpContext?.Request?.Scheme}{_httpContextAccessor.HttpContext?.Request?.Host}";

            //var url = _httpContextAccessor.HttpContext?.Request?.GetEncodedUrl();

            //if (url.IndexOf('/') == -1) return string.Empty;
            //var newurl = url.Substring(0, url.LastIndexOf($"/{CurrentLanguage}"));
            //return $"{newurl}/{CurrentLanguage}";
        }

        //public UserCredentials CurrentUser
        //{
        //    get { return _userService.GetCurrentUserCredentials(); }
        //}

        private string CurrentLanguage
        {
            get
            {
                return "fr";

                //if (string.IsNullOrEmpty(_currentLanguage))
                //{
                //    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
                //    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                //}

                //return _currentLanguage;
            }
        }
    }
}
