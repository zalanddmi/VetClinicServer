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
    public class PetController(PetService petService) : Controller
    {
        private readonly PetService _petService = petService;

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] GetPagedPetsRequest request)
        {
            PaginatedList<PetViewModel> pets = await _petService.GetPaged(request);
            var list = new PaginatedListDTO<PetViewModel>
            {
                Items = pets,
                PageNumber = pets.PageNumber,
                TotalPages = pets.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                PetViewModel owner = _petService.GetById(id);
                return Ok(owner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(PetViewModel model)
        {
            return Ok(_petService.Create(model));
        }

        [HttpPut]
        public IActionResult Update(PetViewModel model)
        {
            try
            {
                _petService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _petService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetPetsExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _petService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Животные.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
