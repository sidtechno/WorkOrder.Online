using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;
        public CustomerController(
            IHttpContextAccessor httpContextAccessor,
            ICustomerService customerService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _customerService = customerService;
            _organizationService = organizationService;
        }

        [HttpGet("Customers")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new CustomerListViewModel()
                {
                    Customers = await _customerService.GetCustomers(CurrentUser.OrganizationId),
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

        [HttpPost("Customers/[action]")]
        public async Task<IActionResult> Create(CustomerViewModel model)
        {
            try
            {
                var result = await _customerService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Customers/[action]")]
        public async Task<IActionResult> Update(CustomerViewModel model)
        {
            try
            {
                var result = await _customerService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Customers/list")]
        public async Task<IActionResult> GetCustomersList(int organizationId)
        {
            try
            {
                var model = new CustomerListViewModel() { Customers = await _customerService.GetCustomers(organizationId) };
                return PartialView("_customers", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("Customers/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _customerService.Delete(id);
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
