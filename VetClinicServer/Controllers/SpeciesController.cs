using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Services;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeciesController(SpeciesService speciesService) : Controller
    {
        private readonly SpeciesService _speciesService = speciesService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPageSpecies([FromQuery] GetPagedSpeciesRequest request)
        {
            PaginatedList<SpeciesViewModel> species = await _speciesService.GetPaged(request);
            var list = new PaginatedListDTO<SpeciesViewModel>
            {
                Items = species,
                PageNumber = species.PageNumber,
                TotalPages = species.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetSpecies(int id)
        {
            try
            {
                SpeciesViewModel drug = _speciesService.GetById(id);
                return Ok(drug);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateSpecies(SpeciesViewModel model)
        {
            return Ok(_speciesService.Create(model));
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateSpecies(SpeciesViewModel model)
        {
            try
            {
                _speciesService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteSpecies(int id)
        {
            try
            {
                _speciesService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        [Authorize]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetSpeciesExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _speciesService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Виды животных.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
