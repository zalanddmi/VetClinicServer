using Microsoft.AspNetCore.Mvc;
using VetClinicServer.Requests;
using VetClinicServer.Services;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugController(DrugService service) : Controller
    {
        private readonly DrugService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetPageDrugs([FromQuery]GetPagedDrugsRequest request)
        {
            PaginatedList<DrugViewModel> drugs = await _service.GetPaged(request);
            return Ok(drugs);
        }

        [HttpGet("{id}")]
        public IActionResult GetDrug(int id)
        {
            try
            {
                DrugViewModel drug = _service.GetById(id);
                return Ok(drug);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateDrug(DrugViewModel model)
        {
            _service.Create(model);
            return Ok();
        }
    }
}
