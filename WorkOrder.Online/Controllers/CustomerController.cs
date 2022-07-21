using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

        [HttpPost("Customers/Responsible/[action]")]
        public async Task<IActionResult> Save(ResponsibleViewModel model)
        {
            try
            {
                var result = 0;
                if(model.Id == 0)
                {
                    result = await _customerService.AddResponsible(model);
                }
                else
                {
                    result = await _customerService.UpdateResponsible(model);
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Customers/Responsibles/list")]
        public async Task<IActionResult> GetResponsibleList(int customerId)
        {
            try
            {
                var model = await _customerService.GetResponsibles(customerId);
                return PartialView("_responsibles", model);
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

        [HttpDelete("Customers/Responsible/Delete")]
        public async Task<ActionResult> DeleteResponsible(int id)
        {
            try
            {
                var result = await _customerService.DeleteResponsible(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Customers/Import")]
        public async Task<ActionResult> Import(IFormFile importFile, int organizationId)
        {
            try
            {
                //parse CSV customers file
                var customers = ImportCustomers(importFile, organizationId);

                //Import customers
                var result = await _customerService.ImportCustomers(customers);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private IEnumerable<CustomerViewModel> ImportCustomers(IFormFile importFile, int organizationId)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                Delimiter = ";"
            };

            using (var reader = new StreamReader(importFile.OpenReadStream()))

            using (var csvReader = new CsvReader(reader, config))
            {
                csvReader.Context.RegisterClassMap(new CustomerViewModelMap(organizationId));
                var customers = csvReader.GetRecords<CustomerViewModel>();
                return customers.ToList();
            }
        }

        sealed class CustomerViewModelMap : ClassMap<CustomerViewModel>
        {
            public CustomerViewModelMap(int organizationId)
            {
                Map(m => m.Address);
                Map(m => m.Email);
                Map(m => m.Responsible);
                Map(m => m.Phone);
                Map(m => m.Cellphone);
                Map(m => m.City);
                Map(m => m.State);
                Map(m => m.Name);
                Map(m => m.PostalCode);
                Map(m => m.OrganizationId).Constant(organizationId);
            }
        }
    }
}
