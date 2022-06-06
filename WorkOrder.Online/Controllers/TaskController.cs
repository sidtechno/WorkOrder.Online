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
        public TaskController(
            IHttpContextAccessor httpContextAccessor,
            ITaskService taskService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _taskService = taskService;
        }

        [HttpGet("Tasks")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new TaskListViewModel()
                {
                    Tasks = await _taskService.GetTasks(CurrentUser.OrganizationId),
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

        //[HttpGet("Organizations/{id}")]
        //public async Task<IActionResult> GetOrganization(int id)
        //{
        //    try
        //    {
        //        return Ok(await _organizationService.GetOrganization(id));
        //    }
        //    catch (Exception)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("Organizations/[action]")]
        //public async Task<IActionResult> Create(OrganizationViewModel model)
        //{
        //    try
        //    {
        //        var result = await _organizationService.Create(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("Organizations/[action]")]
        //public async Task<IActionResult> Update(OrganizationViewModel model)
        //{
        //    try
        //    {
        //        var result = await _organizationService.Update(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Organizations/list")]
        //public async Task<IActionResult> GetOrganizationList()
        //{
        //    try
        //    {
        //        var model = new OrganizationListViewModel() { Organizations = await _organizationService.GetOrganizations() };
        //        return PartialView("_organizations", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("Organizations/[action]")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var result = await _organizationService.Delete(id);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}
    }
}
