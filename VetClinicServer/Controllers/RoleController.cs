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
    public class RoleController(RoleService roleService) : Controller
    {
        private readonly RoleService _roleService = roleService;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPageRoles([FromQuery] GetPagedRolesRequest request)
        {
            PaginatedList<RoleViewModel> roles = await _roleService.GetPaged(request);
            var list = new PaginatedListDTO<RoleViewModel>
            {
                Items = roles,
                PageNumber = roles.PageNumber,
                TotalPages = roles.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetRole(int id)
        {
            try
            {
                RoleViewModel drug = _roleService.GetById(id);
                return Ok(drug);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateRole(RoleViewModel model)
        {
            return Ok(_roleService.Create(model));
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateRole(RoleViewModel model)
        {
            try
            {
                _roleService.Update(model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult DeleteRole(int id)
        {
            try
            {
                _roleService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        [Authorize]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetRolesExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _roleService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Роли.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
