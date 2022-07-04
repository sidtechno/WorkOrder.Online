using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;
        private readonly IOrganizationService _organizationService;
        public TaskController(
            IHttpContextAccessor httpContextAccessor,
            ITaskService taskService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _taskService = taskService;
            _organizationService = organizationService;
        }

        [HttpGet("Tasks")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new TaskListViewModel()
                {
                    Tasks = await _taskService.GetTasks(CurrentUser.OrganizationId),
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

        [HttpPost("Tasks/[action]")]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            try
            {
                var result = await _taskService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Tasks/[action]")]
        public async Task<IActionResult> Update(TaskViewModel model)
        {
            try
            {
                var result = await _taskService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Tasks/list")]
        public async Task<IActionResult> GetTaskList(int organizationId)
        {
            try
            {
                var model = new TaskListViewModel() { Tasks = await _taskService.GetTasks(organizationId) };
                return PartialView("_tasks", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("Tasks/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _taskService.Delete(id);
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
