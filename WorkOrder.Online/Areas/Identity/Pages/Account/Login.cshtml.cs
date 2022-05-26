// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;

namespace WorkOrder.Online.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IStringLocalizer<LoginModel> _localizer;

        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger,
            IStringLocalizer<LoginModel> localizer)
        {
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
            public TranslationModel Translation { get; set; }
        }

        public class TranslationModel
        {
            public string Message { get; set; }
            public string RememberMe { get; set; }
            public string ConnectButton { get; set; }
            public string ForgotPassword { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Message1 { get; set; }
            public string Message2 { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (HttpContext.Request.Query.ContainsKey("lang"))
                CultureInfo.CurrentUICulture = new CultureInfo(HttpContext.Request.Query["lang"], false);
            else
                CultureInfo.CurrentUICulture = new CultureInfo("fr", false);

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Input = new InputModel()
            {
                Translation = new TranslationModel()
                {
                    Message = _localizer["Message"],
                    RememberMe = _localizer["RememberMe"],
                    ConnectButton = _localizer["ConnectButton"],
                    ForgotPassword = _localizer["ForgotPassword"],
                    Email = _localizer["Email"],
                    Password = _localizer["Password"],
                    Message1 = _localizer["Message1"],
                    Message2 = _localizer["Message2"],
                }
            };

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (HttpContext.Request.Query.ContainsKey("lang"))
                CultureInfo.CurrentUICulture = new CultureInfo(HttpContext.Request.Query["lang"], false);
            else
                CultureInfo.CurrentUICulture = new CultureInfo("fr", false);

            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    //Get claims
                    var identityUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    IList<Claim> claims = await _signInManager.UserManager.GetClaimsAsync(identityUser);

                    //Get language in garage
                    var defaultLanguage = "fr";

                    _logger.LogInformation("User logged in.");

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    Input = new InputModel()
                    {
                        Translation = new TranslationModel()
                        {
                            Message = _localizer["Message"],
                            RememberMe = _localizer["RememberMe"],
                            ConnectButton = _localizer["ConnectButton"],
                            ForgotPassword = _localizer["ForgotPassword"],
                            Email = _localizer["Email"],
                            Password = _localizer["Password"],
                        }
                    };

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
