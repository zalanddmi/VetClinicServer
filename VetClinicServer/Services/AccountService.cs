using VetClinicServer.Data;
using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class AccountService(UserRepository userRepository, PasswordHasher passwordHasher, JwtGenerator jwtGenerator, Context context)
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly JwtGenerator _jwtGenerator = jwtGenerator;
        // временно
        private readonly Context _context = context;

        public UserDTO Login(LoginRequest request)
        {
            User? user = _userRepository.GetByUserName(request.UserName) ?? throw new InvalidDataException("Неправильный логин");
            bool result = _passwordHasher.Verify(user.PasswordHash, user.Salt, request.Password);
            if (!result)
            {
                throw new InvalidDataException("Неправильный пароль");
            }
            string token = _jwtGenerator.GenerateToken(user);
            UserDTO userDTO = new()
            {
                FIO = user.FIO,
                Token = token
            };
            return userDTO;
        }

        public UserDTO Register(RegisterRequest request)
        {
            (string passwordHash, string saltHash) = _passwordHasher.Hash(request.Password);
            // временно
            var post = _context.Posts.Find(request.PostId) ?? throw new InvalidDataException("Должность не найдена");
            User user = new()
            {
                UserName = request.UserName,
                FIO = request.FIO,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PasswordHash = passwordHash,
                Salt = saltHash,
                Post = post
            };
            _userRepository.Create(user);
            _userRepository.Save();
            string token = _jwtGenerator.GenerateToken(user);
            UserDTO userDTO = new()
            {
                FIO = user.FIO,
                Token = token
            };
            return userDTO;
        }
    }
}
