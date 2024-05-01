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
    public class PostController(PostService postService) : Controller
    {
        private readonly PostService _postService = postService;

        [HttpGet]
        public async Task<IActionResult> GetPage([FromQuery] GetPagedPostsRequest request)
        {
            PaginatedList<PostViewModel> posts = await _postService.GetPaged(request);
            var list = new PaginatedListDTO<PostViewModel>
            {
                Items = posts,
                PageNumber = posts.PageNumber,
                TotalPages = posts.TotalPages
            };
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                PostViewModel post = _postService.GetById(id);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(PostViewModel model)
        {
            return Ok(_postService.Create(model));
        }

        [HttpPut]
        public IActionResult Update(PostViewModel model)
        {
            try
            {
                _postService.Update(model);
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
                _postService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("excel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] GetPostsExcelRequest request)
        {
            try
            {
                byte[] excelBytes = await _postService.ExportToExcel(request);
                return Ok(File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Должности.xlsx"));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
