using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class OrganizationController : BaseController
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(
            IHttpContextAccessor httpContextAccessor,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("Organizations")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new OrganizationListViewModel()
                {
                    Organizations = await _organizationService.GetOrganizations(),
                    RootUrl = BaseRootUrl
                };

                return View(model);
            }
            catch (Exception)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

       
        //[HttpPost("Roles/[action]")]
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
        //        // ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost("Roles/[action]")]
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
        //        //ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet("Roles/list")]
        //public async Task<IActionResult> GetRoleList()
        //{
        //    try
        //    {
        //        var model = new RoleListViewModel() { Roles = await GetRoles() };
        //        return PartialView("_roles", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("Roles/[action]")]
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
        //        //ex.ToExceptionless().Submit();
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}
    }
}
