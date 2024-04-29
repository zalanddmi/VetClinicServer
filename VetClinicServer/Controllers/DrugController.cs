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
    [Authorize]
    public class DrugController(DrugService drugService) : Controller
    {
        private readonly DrugService _drugService = drugService;

        [HttpGet]
        public async Task<IActionResult> GetPageDrugs([FromQuery]GetPagedDrugsRequest request)
        {
            PaginatedList<DrugViewModel> drugs = await _drugService.GetPaged(request);
            var list = new PaginatedListDTO<DrugViewModel>
            {
                Items = drugs,
                PageNumber = drugs.PageNumber,
                TotalPages = drugs.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetDrug(int id)
        {
            try
            {
                DrugViewModel drug = _drugService.GetById(id);
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
            return Ok(_drugService.Create(model));
        }

        [HttpPut]
        public IActionResult UpdateDrug(DrugViewModel model)
        {
            try
            {
                _drugService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteDrug(int id)
        {
            try
            {
                _drugService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetDrugsExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _drugService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Лекарства.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
