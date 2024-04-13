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
    public class ProcedureController(ProcedureService procedureService) : Controller
    {
        private readonly ProcedureService _procedureService = procedureService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPageProcedures([FromQuery] GetPagedProceduresRequest request)
        {
            PaginatedList<ProcedureViewModel> procedures = await _procedureService.GetPaged(request);
            var list = new PaginatedListDTO<ProcedureViewModel>
            {
                Items = procedures,
                PageNumber = procedures.PageNumber,
                TotalPages = procedures.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetProcedure(int id)
        {
            try
            {
                ProcedureViewModel procedure = _procedureService.GetById(id);
                return Ok(procedure);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateProcedure(ProcedureViewModel model)
        {
            return Ok(_procedureService.Create(model));
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateProcedure(ProcedureViewModel model)
        {
            try
            {
                _procedureService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteProcedure(int id)
        {
            try
            {
                _procedureService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        [Authorize]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetProceduresExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _procedureService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Процедуры.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
