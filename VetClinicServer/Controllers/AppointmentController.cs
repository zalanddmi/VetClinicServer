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
    public class AppointmentController(AppointmentService appointmentService) : Controller
    {
        private readonly AppointmentService _appointmentService = appointmentService;

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] GetPagedAppointmentsRequest request)
        {
            PaginatedList<AppointmentViewModel> appointments = await _appointmentService.GetPaged(request);
            var list = new PaginatedListDTO<AppointmentViewModel>
            {
                Items = appointments,
                PageNumber = appointments.PageNumber,
                TotalPages = appointments.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                AppointmentViewModel appointment = _appointmentService.GetById(id);
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(AppointmentViewModel model)
        {
            return Ok(_appointmentService.Create(model));
        }

        [HttpPut]
        public IActionResult Update(AppointmentViewModel model)
        {
            try
            {
                _appointmentService.Update(model);
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
                _appointmentService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetAppointmentsExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _appointmentService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Приемы.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
