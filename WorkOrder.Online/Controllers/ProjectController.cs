using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IOrganizationService _organizationService;
        public ProjectController(
            IHttpContextAccessor httpContextAccessor,
            IProjectService projectService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _projectService = projectService;
            _organizationService = organizationService;
        }

        [HttpGet("Projects")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new ProjectListViewModel()
                {
                    Projects = await _projectService.GetProjects(CurrentUser.OrganizationId),
                    OrganizationSelector = new OrganizationSelectorViewModel
                    {
                        Organizations = await _organizationService.GetOrganizationsSelectList(),
                        SelectedOrganizationId = HttpContext.User.IsInRole("Administrator") ? CurrentUser.OrganizationId : 0,
                        disabled = HttpContext.User.IsInRole("Administrator")
                    },
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

       
        [HttpPost("Projects/[action]")]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            try
            {
                var result = await _projectService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Projects/[action]")]
        public async Task<IActionResult> Update(ProjectViewModel model)
        {
            try
            {
                var result = await _projectService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Projects/list")]
        public async Task<IActionResult> GetProjectList(int organizationId)
        {
            try
            {
                var model = new ProjectListViewModel() { Projects = await _projectService.GetProjects(organizationId) };
                return PartialView("_projects", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("Projects/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _projectService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
