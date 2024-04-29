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
    public class OwnerController(OwnerService ownerService) : Controller
    {
        private readonly OwnerService _ownerService = ownerService;

        [HttpGet]
        public async Task<IActionResult> GetPageOwners([FromQuery] GetPagedOwnersRequest request)
        {
            PaginatedList<OwnerViewModel> owners = await _ownerService.GetPaged(request);
            var list = new PaginatedListDTO<OwnerViewModel>
            {
                Items = owners,
                PageNumber = owners.PageNumber,
                TotalPages = owners.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetOwner(int id)
        {
            try
            {
                OwnerViewModel owner = _ownerService.GetById(id);
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateOwner(OwnerViewModel model)
        {
            return Ok(_ownerService.Create(model));
        }

        [HttpPut]
        public IActionResult UpdateOwner(OwnerViewModel model)
        {
            try
            {
                _ownerService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult DeleteOwner(int id)
        {
            try
            {
                _ownerService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetOwnersExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _ownerService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Владельцы.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
