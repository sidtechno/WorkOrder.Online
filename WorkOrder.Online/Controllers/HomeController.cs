using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("home/[action]")]
        public async Task<IActionResult> CreateRole(string name)
        {
            try
            {
                 return NoContent();
            }
            catch (Exception ex)
            {
                // ex.ToExceptionless().Submit();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}