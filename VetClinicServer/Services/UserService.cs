using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class UserService(UserRepository userRepository,
        PostRepository postRepository,
        ExcelConverter excelConverter,
        PasswordHasher passwordHasher)
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly PostRepository _postRepository = postRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;
        private readonly PasswordHasher _passwordHasher = passwordHasher;

        public int Create(UserViewModel model)
        {
            bool isUniqueUserName = _userRepository.GetByUserName(model.UserName) == null;
            if (!isUniqueUserName)
            {
                throw new InvalidDataException("Пользователь с таким логином существует");
            }
            Post post = _postRepository.GetById(model.Post.Id) ?? throw new NullReferenceException("Должность не найдена");
            if (string.IsNullOrEmpty(model.Password))
            {
                throw new NullReferenceException("Пароль не записан");
            }
            (string passwordHash, string saltHash) = _passwordHasher.Hash(model.Password);
            User user = new()
            {
                UserName = model.UserName,
                FIO = model.FIO,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PasswordHash = passwordHash,
                Salt = saltHash,
                Post = post
            };
            _userRepository.Create(user);
            _userRepository.Save();
            return user.Id;
        }

        public void Update(UserViewModel model)
        {
            User user = _userRepository.GetById(model.Id) ?? throw new NullReferenceException("Пользователь не найден");
            Post post = _postRepository.GetById(model.Post.Id) ?? throw new NullReferenceException("Должность не найдена");
            user.UserName = model.UserName;
            user.FIO = model.FIO;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.Post = post;
            if (!string.IsNullOrEmpty(model.Password))
            {
                (string passwordHash, string saltHash) = _passwordHasher.Hash(model.Password);
                user.PasswordHash = passwordHash;
                user.Salt = saltHash;
            }
            _userRepository.Update(user);
            _userRepository.Save();
        }

        public void Delete(int id)
        {
            User user = _userRepository.GetById(id) ?? throw new NullReferenceException("Пользователь не найден");
            _userRepository.Delete(user);
            _userRepository.Save();
        }

        public UserViewModel GetById(int id)
        {
            User user = _userRepository.GetById(id) ?? throw new NullReferenceException("Пользователь не найден");
            DisplayModel postDisplay = new() { Id = user.Post.Id, Name = user.Post.Name, Entity = "Post" };
            UserViewModel model = new()
            {
                Id = id,
                UserName = user.UserName,
                FIO = user.FIO,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Post = postDisplay
            };
            return model;
        }

        public async Task<PaginatedList<UserViewModel>> GetPaged(GetPagedUsersRequest request)
        {
            PaginatedList<User> users = await _userRepository.GetPaged(request);
            List<UserViewModel> userViewModels = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FIO = user.FIO,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Post = new DisplayModel() { Id = user.Post.Id, Name = user.Post.Name, Entity = "Post" }
            }).ToList();
            var pagedUsers = new PaginatedList<UserViewModel>(userViewModels, users.PageNumber, users.TotalPages);
            return pagedUsers;
        }

        public async Task<byte[]> ExportToExcel(GetUsersExcelRequest request)
        {
            GetPagedUsersRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                UserName = request.UserName,
                FIO = request.FIO,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Post = request.Post
            };
            PaginatedList<User> users = await _userRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (User user in users)
            {
                List<object> row = [user.UserName, user.FIO, user.PhoneNumber, user.Email, user.Post.Name];
                data.Add(row);
            }
            List<string> columns = ["Логин", "ФИО", "Номер телефона", "Электронная почта", "Должность"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Пользователи", data, columns);
            return excelBytes;
        }
    }
}
