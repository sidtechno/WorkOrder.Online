using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class UserController : BaseController
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IOrganizationService _organizationService;
        private readonly ICategoryService _categoryService;
        private readonly Dictionary<string, string> _roles;
        private readonly UserManager<IdentityUser> _userManager;

        private const string DefaultNewUserPassword = "Soleil123!";

        public UserController(SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ICategoryService categoryService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _userIdentity = signInManager;
            _roleManager = roleManager;
            _userService = userService;
            _organizationService = organizationService;
            _categoryService = categoryService;
            _roles = ((UserClaimsPrincipalFactory<IdentityUser, IdentityRole>)_userIdentity.ClaimsFactory).RoleManager.Roles.ToDictionary(r => r.Id, r => r.Name);
            _userManager = userManager;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            try
            {
                //Get current user
                var user = await GetCurrentUserAsync();
                var claims = await _userService.GetUserClaims(user.Id);

                var language = claims.Any(c => c.Type == "Language") ? claims.FirstOrDefault(c => c.Type == "Language")?.Value : "FR";

                var users = HttpContext.User.IsInRole("Administrator") ? await GetUsers(CurrentUser.OrganizationId) : await GetUsers();

                var model = new UserListViewModel()
                {
                    RootUrl = BaseRootUrl,
                    Users = users,
                    RemainingUsers = !HttpContext.User.IsInRole("SuperAdmin") ? await _userService.GetRemainingUsers(CurrentUser.OrganizationId) : 9999,
                    OrganizationSelector = new OrganizationSelectorViewModel
                    {
                        Organizations = await _organizationService.GetOrganizationsSelectList(),
                        SelectedOrganizationId = HttpContext.User.IsInRole("Administrator") ? CurrentUser.OrganizationId : 0,
                        disabled = HttpContext.User.IsInRole("Administrator")
                    },
                    CategorySelector = new CategorySelectorViewModel
                    {
                        Categories = await _categoryService.GetCategorySelectList(CurrentUser.OrganizationId, CurrentUser.Language),
                        SelectedCategoryId = 0,
                        disabled = false
                    }
                };

                ViewBag.Roles = _roles;

                return View(model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Users/[action]")]
        public async Task<IActionResult> UserExist(string username)
        {
            var result = await _userIdentity.UserManager.FindByNameAsync(username);
            return Ok(result == null ? false : true);
        }

        [HttpPost("Users/[action]")]
        public async Task<IActionResult> CreateUser(string userName, string firstName, string lastName, string selectedOrganizationId, string email, string cellphone, string costHour, string[] roles, string categories)
        {
            try
            {
                var fullName = $"{firstName} {lastName}";
                var user = new IdentityUser { Email = email.ToLower(), UserName = userName.ToLower(), EmailConfirmed = true };

                //security to avoid string in database
                var organizationIdSelected = 0;
                int.TryParse(selectedOrganizationId, out organizationIdSelected);
                if (organizationIdSelected == 0)
                    throw new ApplicationException($"Cannot convert {selectedOrganizationId} to int in method CreateUser ");

                var result = await _userIdentity.UserManager.CreateAsync(user, DefaultNewUserPassword);
                if (result.Succeeded)
                {
                    if (firstName != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));

                    if (lastName != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));

                    if (selectedOrganizationId != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("OrganizationId", organizationIdSelected.ToString()));

                    if (cellphone != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Cellphone", cellphone));

                    if (categories != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Categories", categories));

                    if (!string.IsNullOrWhiteSpace(costHour))
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("HourlyRate", costHour));
                    else
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("HourlyRate", "0"));

                    foreach (string role in roles)
                        await _userIdentity.UserManager.AddToRoleAsync(user, role);

                    //Send email to a new user, for the password initialization
                    //await NewUserCompleteRegistration(user, fullName);

                    return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Users/[action]")]
        public async Task<IActionResult> UpdateUser(string id, string userName, string email, string firstName, string lastName, string selectedOrganizationId, string locked, string cellphone, string costHour, string[] roles, string categories)
        {
            try
            {
                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                user.UserName = userName;
                user.Email = email;
                user.LockoutEnd = locked == null ? default(DateTimeOffset?) : DateTimeOffset.MaxValue;

                var result = await _userIdentity.UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var userRoles = await _userIdentity.UserManager.GetRolesAsync(user);

                    foreach (string role in roles.Except(userRoles))
                    {
                        await _userIdentity.UserManager.AddToRoleAsync(user, role);
                    }

                    foreach (string role in userRoles.Except(roles))
                    {
                        await _userIdentity.UserManager.RemoveFromRoleAsync(user, role);
                    }

                    var userClaims = await _userIdentity.UserManager.GetClaimsAsync(user);

                    if (firstName != null)
                    {
                        if (!userClaims.Any(c => c.Type == "FirstName")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));
                        }
                        else if (userClaims.Any(c => c.Type == "FirstName") && userClaims.First(c => c.Type == "FirstName").Value != firstName)
                        {
                            var originalFirstname = userClaims.First(c => c.Type == "FirstName").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("FirstName", originalFirstname));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));
                        }

                    }

                    if (lastName != null)
                    {
                        if (!userClaims.Any(c => c.Type == "LastName")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));
                        }
                        else if (userClaims.Any(c => c.Type == "LastName") && userClaims.First(c => c.Type == "LastName").Value != lastName)
                        {
                            var originalLastname = userClaims.First(c => c.Type == "LastName").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("LastName", originalLastname));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));
                        }

                    }

                    if (selectedOrganizationId != null)
                    {
                        if (!userClaims.Any(c => c.Type == "OrganizationId"))
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("OrganizationId", selectedOrganizationId));
                        }
                        else if (userClaims.Any(c => c.Type == "OrganizationId") && userClaims.First(c => c.Type == "OrganizationId").Value != selectedOrganizationId)
                        {
                            var originalOrganizationId = userClaims.First(c => c.Type == "OrganizationId").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("OrganizationId", originalOrganizationId));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("OrganizationId", selectedOrganizationId));
                        }
                    }

                    if (cellphone != null)
                    {
                        if (!userClaims.Any(c => c.Type == "Cellphone")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Cellphone", cellphone));
                        }
                        else if (userClaims.Any(c => c.Type == "Cellphone") && userClaims.First(c => c.Type == "Cellphone").Value != cellphone)
                        {
                            var originalCellphone = userClaims.First(c => c.Type == "Cellphone").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("Cellphone", originalCellphone));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Cellphone", cellphone));
                        }

                    }

                    if (categories != null)
                    {
                        if (!userClaims.Any(c => c.Type == "Categories")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Categories", categories));
                        }
                        else if (userClaims.Any(c => c.Type == "Categories") && userClaims.First(c => c.Type == "Categories").Value != categories)
                        {
                            var originalCategories = userClaims.First(c => c.Type == "Categories").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("Categories", originalCategories));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("Categories", categories));
                        }

                    }
                    if (costHour != null)
                    {
                        if (!userClaims.Any(c => c.Type == "HourlyRate")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("HourlyRate", costHour));
                        }
                        else if (userClaims.Any(c => c.Type == "HourlyRate") && userClaims.First(c => c.Type == "HourlyRate").Value != costHour)
                        {
                            var originalCostHour = userClaims.First(c => c.Type == "HourlyRate").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("HourlyRate", originalCostHour));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("HourlyRate", costHour));
                        }

                    }
                    return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("Users/[action]")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                var result = await _userIdentity.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return NoContent();
                }
                else
                    return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Users/[action]")]
        public async Task<IActionResult> ResetPassword(string id, string password, string verify)
        {
            try
            {
                if (password != verify)
                    return BadRequest("Passwords entered do not match.");

                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                if (await _userIdentity.UserManager.HasPasswordAsync(user))
                    await _userIdentity.UserManager.RemovePasswordAsync(user);

                var result = await _userIdentity.UserManager.AddPasswordAsync(user, password);
                if (result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("Users/list")]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var users = HttpContext.User.IsInRole("Administrator") ? await GetUsers(CurrentUser.OrganizationId) : await GetUsers();

                var model = new UserListViewModel() { Users = users };
                return PartialView("_users", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Users/UserCategories")]
        public async Task<IActionResult> GetUserCategories(int organizationId, string categories)
        {
            try
            {
                var result = new List<CategoryViewModel>();
                if (categories == null) return Ok(result);

                var categorieList = await _categoryService.GetCategories(organizationId);

                categories.Split(",").ToList().ForEach(p =>
                {
                    var category = categorieList.FirstOrDefault(e => e.Id == Convert.ToInt32(p));

                    result.Add(new CategoryViewModel()
                    {
                        Id = category.Id,
                        Description_Fr = category.Description_Fr,
                        Description_En = category.Description_En,
                    });
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userIdentity.UserManager.GetUserAsync(HttpContext.User);

        private async Task<IEnumerable<UserViewModel>> GetUsers(int organizationId = 0)
        {
            try
            {
                var result = await _userService.GetUserFacade(organizationId);

                return result;
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return null;
            }
        }

        private async Task<string> BuildClientToDisplay(IList<string> clients)
        {
            try
            {
                var builder = new StringBuilder();

                if (!clients.Any()) return string.Empty;

                clients.ToList().ForEach(async c =>
                {
                    builder.Append(c);
                    builder.Append("<br />");
                });

                return builder.Remove(builder.Length - 6, 6).ToString();
            }
            catch (Exception ex)
            {
                // ex.ToExceptionless().Submit();
                return null;
            }
        }

        private async Task NewUserCompleteRegistration(IdentityUser user, string userFullName)
        {
            try
            {
                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = System.Web.HttpUtility.UrlEncode(token);
                string urlDomain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
                string callbackUrl = $"{urlDomain}/Identity/Account/CompleteRegistration?token={token}";

                //TODO
                //await _emailService.SendEmailCompleteRegistration(user.Email, userFullName, urlDomain, token, callbackUrl);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
            }
        }
    }
}
