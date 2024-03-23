using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetClinicServer.Requests;
using VetClinicServer.Services;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(AccountService accountService) : Controller
    {
        private readonly AccountService _accountService = accountService;

        [HttpPost("login")]
        public IActionResult Login([FromQuery]LoginRequest request)
        {
            try
            {
                UserDTO userDTO = _accountService.Login(request);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        // [Authorize]
        public IActionResult Register([FromQuery]RegisterRequest request)
        {
            try
            {
                UserDTO userDTO = _accountService.Register(request);
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
