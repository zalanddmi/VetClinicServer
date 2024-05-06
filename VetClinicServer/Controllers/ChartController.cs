using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinicServer.Services;

namespace VetClinicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChartController(ChartService chartService) : Controller
    {
        private readonly ChartService _chartService = chartService;

        [HttpGet]
        public IActionResult GetChartData()
        {
            try
            {
                return Ok(_chartService.GetChartData());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
