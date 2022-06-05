// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace WorkOrder.Online.Areas.Identity.Pages.Account
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        private readonly IStringLocalizer<LockoutModel> _localizer;

        public LockoutModel(IStringLocalizer<LockoutModel> localizer)
        {
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public TranslationModel Translation { get; set; }
        }

        public class TranslationModel
        {
            public string AccessDenied { get; set; }
            public string AccessDeniedText { get; set; }
            public string Slogan { get; set; }

        }

        public void OnGet()
        {
            Input = new InputModel()
            {
                Translation = new TranslationModel()
                {
                    AccessDenied = _localizer["AccessDenied"],
                    AccessDeniedText = _localizer["AccessDeniedText"],
                    Slogan = _localizer["Slogan"]
                }
            };
        }
    }
}
