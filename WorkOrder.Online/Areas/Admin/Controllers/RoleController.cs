using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Areas.Admin.Models;
using WorkOrder.Online.Controllers;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseController
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _userIdentity = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new RoleListViewModel()
                {
                    Roles = new List<RoleViewModel>(), // await GetRoles(),
                    RootUrl = BaseRootUrl
                };

                return View(model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        //private async Task<List<RoleViewModel>> GetRoles()
        //{
        //    try
        //    {
        //        var result = new List<RoleViewModel>();

        //        var RolesList = _roleManager.Roles.OrderBy(x => x.Name).ToList();

        //        var roles = RolesList.Adapt<IEnumerable<RoleViewModel>>();

        //        foreach (var role in roles)
        //        {
        //            var RolesUserlist = await _userIdentity.UserManager.GetUsersInRoleAsync(role.Name);
        //            result.Add(new RoleViewModel()
        //            {
        //                Id = role.Id,
        //                Name = role.Name,
        //                UserCount = RolesUserlist.Count
        //            });
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //        return null;
        //    }
        //}

        //[HttpPost("/{lang:lang}/roles/[action]")]
        //public async Task<IActionResult> CreateRole(string name)
        //{
        //    try
        //    {
        //        var role = new IdentityRole(name);
        //        var result = await _roleManager.CreateAsync(role);

        //        if (result.Succeeded)
        //        {
        //            return NoContent();
        //        }
        //        else
        //            return BadRequest(result.Errors.First().Description);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost("/{lang:lang}/roles/[action]")]
        //public async Task<IActionResult> UpdateRole(string id, string name)
        //{
        //    try
        //    {
        //        var role = await _roleManager.FindByIdAsync(id);
        //        if (role == null)
        //            return NotFound("Role not found.");

        //        role.Name = name;

        //        var result = await _roleManager.UpdateAsync(role);
        //        if (result.Succeeded)
        //        {
        //            return NoContent();
        //        }
        //        else
        //            return BadRequest(result.Errors.First().Description);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet("/{lang:lang}/roles/list")]
        //public async Task<IActionResult> GetRoleList()
        //{
        //    try
        //    {
        //        var model = new RoleListViewModel() { Roles = await GetRoles() };
        //        return PartialView("_roles", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("/{lang:lang}/roles/[action]")]
        //public async Task<ActionResult> DeleteRole(string id)
        //{
        //    try
        //    {
        //        var role = await _roleManager.FindByIdAsync(id);
        //        if (role == null)
        //            return NotFound("Role not found.");

        //        var result = await _roleManager.DeleteAsync(role);
        //        if (result.Succeeded)
        //        {
        //            return NoContent();
        //        }
        //        else
        //            return BadRequest(result.Errors.First().Description);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
