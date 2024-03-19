using Microsoft.AspNetCore.Mvc;
using VetClinicServer.Requests;
using VetClinicServer.Services;

namespace VetClinicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugController(DrugService service) : Controller
    {
        private readonly DrugService _service = service;

        [HttpPost]
        public async Task<IActionResult> GetPageDrugs(GetPagedDrugsRequest request)
        {
            var drugs = await _service.GetPaged(request);
            return Ok(drugs);
        }
    }
}
