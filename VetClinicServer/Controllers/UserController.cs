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
    public class UserController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] GetPagedUsersRequest request)
        {
            PaginatedList<UserViewModel> users = await _userService.GetPaged(request);
            var list = new PaginatedListDTO<UserViewModel>
            {
                Items = users,
                PageNumber = users.PageNumber,
                TotalPages = users.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                UserViewModel user = _userService.GetById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            return Ok(_userService.Create(model));
        }

        [HttpPut]
        public IActionResult Update(UserViewModel model)
        {
            try
            {
                _userService.Update(model);
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
                _userService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetUsersExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _userService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Пользователи.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
