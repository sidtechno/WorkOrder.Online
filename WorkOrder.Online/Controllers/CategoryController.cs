using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkOrder.Online.Models;
using WorkOrder.Online.Services.Interfaces;

namespace WorkOrder.Online.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IOrganizationService _organizationService;
        public CategoryController(
            IHttpContextAccessor httpContextAccessor,
            ICategoryService categoryService,
            IOrganizationService organizationService,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _categoryService = categoryService;
            _organizationService = organizationService;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new CategoryListViewModel()
                {
                    Categories = await _categoryService.GetCategories(CurrentUser.OrganizationId),
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

        [HttpPost("Categories/[action]")]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            try
            {
                var result = await _categoryService.Create(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("Categories/[action]")]
        public async Task<IActionResult> Update(CategoryViewModel model)
        {
            try
            {
                var result = await _categoryService.Update(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpGet("Categories/list")]
        public async Task<IActionResult> GetCategoryList(int organizationId)
        {
            try
            {
                var model = new CategoryListViewModel() { Categories = await _categoryService.GetCategories(organizationId) };
                return PartialView("_categories", model);
            }
            catch (Exception ex)
            {
                //ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("Categories/[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryService.Delete(id);
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
