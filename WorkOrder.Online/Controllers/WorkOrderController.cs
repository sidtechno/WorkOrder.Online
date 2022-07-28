using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class WorkOrderController : BaseController
    {
        private readonly IWorkOrderService _workOrderService;
        private readonly IOrganizationService _organizationService;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;
        private readonly IProjectService _projectService;

        public WorkOrderController(
            IHttpContextAccessor httpContextAccessor,
            IWorkOrderService workOrderService,
            ICustomerService customerService,
            IOrganizationService organizationService,
            ICategoryService categoryService,
            IProjectService projectService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _workOrderService = workOrderService;
            _organizationService = organizationService;
            _customerService = customerService;
            _categoryService = categoryService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new WorkOrderListViewModel()
                {
                    WorkOrders = new List<WorkOrderViewModel>(), //await _workOrderService.GetProjects(CurrentUser.OrganizationId, true),
                    ProjectListViewModel = new ProjectListViewModel()
                    {
                        Projects = await _projectService.GetProjects(CurrentUser.OrganizationId, true),
                        CustomerSelector = new CustomerSelectorViewModel
                        {
                            Customers = await _customerService.GetCustomersSelectList(CurrentUser.OrganizationId),
                            SelectedCustomerId = 0,
                            disabled = false
                        },
                        OrganizationSelector = new OrganizationSelectorViewModel
                        {
                            Organizations = await _organizationService.GetOrganizationsSelectList(),
                            SelectedOrganizationId = 0,
                            disabled = HttpContext.User.IsInRole("Administrator")
                        },
                        CategorySelector = new CategorySelectorViewModel
                        {
                            Categories = await _categoryService.GetCategorySelectList(CurrentUser.OrganizationId, CurrentUser.Language),
                            SelectedCategoryId = 0,
                            disabled = false
                        },
                        RootUrl = BaseRootUrl
                    },
                    OrganizationSelector = new OrganizationSelectorViewModel
                    {
                        Organizations = await _organizationService.GetOrganizationsSelectList(),
                        SelectedOrganizationId = 0,
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


        //[HttpPost("Projects/[action]")]
        //public async Task<IActionResult> Create(ProjectViewModel model)
        //{
        //    try
        //    {
        //        var result = await _projectService.Create(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpPost("Projects/[action]")]
        //public async Task<IActionResult> Update(ProjectViewModel model)
        //{
        //    try
        //    {
        //        var result = await _projectService.Update(model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Projects/List")]
        //public async Task<IActionResult> GetProjectList(int organizationId, bool activeOnly)
        //{
        //    try
        //    {
        //        var model = new ProjectListViewModel()
        //        {
        //            Projects = await _projectService.GetProjects(organizationId, activeOnly),
        //        };
        //        return PartialView("_projects", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Projects/ProjectCategories/{projectId}")]
        //public async Task<IActionResult> GetProjectProjectCategories(int projectId)
        //{
        //    try
        //    {
        //        return Ok(await _projectService.GetProjectCategories(projectId));
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Projects/Customers")]
        //public async Task<IActionResult> GetProjectCustomers(int organizationId)
        //{
        //    try
        //    {
        //        return Ok(await _customerService.GetCustomersSelectList(organizationId));
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Projects/Categories")]
        //public async Task<IActionResult> GetProjectCategories(int organizationId)
        //{
        //    try
        //    {
        //        return Ok(await _categoryService.GetCategorySelectList(organizationId, CurrentUser.Language));
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpGet("Projects/NextSequence/{organizationId}")]
        //public async Task<IActionResult> GetNextProjectSequence(int organizationId)
        //{
        //    try
        //    {
        //        var organization = await _organizationService.GetOrganization(organizationId);
        //        var nextSequence = organization.ProjectStartSequence + 1;
        //        return Ok(nextSequence);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.ToExceptionless().Submit();
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete("Projects/[action]")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var result = await _projectService.Delete(id);
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
