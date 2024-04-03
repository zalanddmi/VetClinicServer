using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetPageDrugs([FromQuery]GetPagedDrugsRequest request)
        {
            PaginatedList<DrugViewModel> drugs = await _service.GetPaged(request);
            var list = new PaginatedListDTO<DrugViewModel>
            {
                Items = drugs,
                PageNumber = drugs.PageNumber,
                TotalPages = drugs.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
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
        [Authorize]
        public IActionResult CreateDrug(DrugViewModel model)
        {
            _service.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateDrug(DrugViewModel model)
        {
            try
            {
                _service.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteDrug(int id)
        {
            try
            {
                _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        [Authorize]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetDrugsExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _service.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Лекарства.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
