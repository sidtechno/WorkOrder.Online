using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IOrganizationService _organizationService;
        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IProductService productService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _productService = productService;
            _organizationService = organizationService;
        }

        [HttpGet("Products")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new ProductListViewModel()
                {
                    Products = await _productService.GetProducts(CurrentUser.OrganizationId),
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

        [HttpPost("Products/[action]")]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            try
            {
                var result = await _productService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Products/[action]")]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            try
            {
                var result = await _productService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Products/list")]
        public async Task<IActionResult> GetProductsList(int organizationId)
        {
            try
            {
                var model = new ProductListViewModel() { Products = await _productService.GetProducts(organizationId) };
                return PartialView("_products", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("Products/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _productService.Delete(id);
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
