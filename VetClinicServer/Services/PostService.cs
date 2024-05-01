using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class PostService(PostRepository postRepository, RoleRepository roleRepository, ExcelConverter excelConverter)
    {
        private readonly PostRepository _postRepository = postRepository;
        private readonly RoleRepository _roleRepository = roleRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(PostViewModel model)
        {
            Role role = _roleRepository.GetById(model.Role.Id) ?? throw new NullReferenceException("Роль не найдена");
            Post post = new()
            {
                Name = model.Name,
                Role = role,
                Description = model.Description
            };
            _postRepository.Create(post);
            _postRepository.Save();
            return role.Id;
        }

        public void Update(PostViewModel model)
        {
            Post post = _postRepository.GetById(model.Id) ?? throw new NullReferenceException("Должность не найдена");
            Role role = _roleRepository.GetById(model.Role.Id) ?? throw new NullReferenceException("Роль не найдена");
            post.Name = model.Name;
            post.Role = role;
            post.Description = model.Description;
            _postRepository.Update(post);
            _postRepository.Save();
        }

        public void Delete(int id)
        {
            Post post = _postRepository.GetById(id) ?? throw new NullReferenceException("Должность не найдена");
            _postRepository.Delete(post);
            _postRepository.Save();
        }

        public PostViewModel GetById(int id)
        {
            Post post = _postRepository.GetById(id) ?? throw new NullReferenceException("Должность не найдена");
            DisplayModel roleDisplay = new() { Id = post.Role.Id, Name = post.Role.Name, Entity = "Role" };
            PostViewModel model = new()
            {
                Id = id,
                Name = post.Name,
                Role = roleDisplay,
                Description = post.Description
            };
            return model;
        }

        public async Task<PaginatedList<PostViewModel>> GetPaged(GetPagedPostsRequest request)
        {
            PaginatedList<Post> posts = await _postRepository.GetPaged(request);
            List<PostViewModel> postViewModels = posts.Select(post => new PostViewModel
            {
                Id = post.Id,
                Name = post.Name,
                Role = new DisplayModel() { Id = post.Role.Id, Name = post.Role.Name, Entity = "Role" },
                Description = post.Description
            }).ToList();
            var pagedPosts = new PaginatedList<PostViewModel>(postViewModels, posts.PageNumber, posts.TotalPages);
            return pagedPosts;
        }

        public async Task<byte[]> ExportToExcel(GetPostsExcelRequest request)
        {
            GetPagedPostsRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
                Role = request.Role
            };
            PaginatedList<Post> posts = await _postRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Post post in posts)
            {
                List<object> row = [post.Name, post.Role.Name, post.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Название", "Роль", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Должности", data, columns);
            return excelBytes;
        }
    }
}
