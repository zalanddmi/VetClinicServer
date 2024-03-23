using Microsoft.IdentityModel.Tokens;
using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class UserRepository(Context context) : IRepository<User>
    {
        private readonly Context _context = context;

        public void Create(User entity)
        {
            _context.Users.Add(entity);
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public IEnumerable<User>? GetAll()
        {
            return _context.Users;
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User? GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }

        public async Task<PaginatedList<User>> GetPaged(GetPagedUsersRequest request)
        {
            IQueryable<User> query = _context.Users;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<User>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<User> ApplySorting(IQueryable<User> query, GetPagedUsersRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(u => u.FIO.Contains(request.SearchString)
                || u.PhoneNumber.Contains(request.SearchString) || u.Email.Contains(request.SearchString)
                || u.Post.Name.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                query = query.Where(u => u.UserName.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.FIO))
            {
                query = query.Where(u => u.FIO.Contains(request.FIO));
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                query = query.Where(u => u.PhoneNumber.Contains(request.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(u => u.Email.Contains(request.Email));
            }
            if (!string.IsNullOrEmpty(request.PostName))
            {
                query = query.Where(u => u.Post.Name.Contains(request.PostName));
            }
            return query;
        }

        private static IQueryable<User> ApplyFilter(IQueryable<User> query, GetPagedUsersRequest request)
        {
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                switch (request.OrderBy)
                {
                    case "Id":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(u => u.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.Id);
                        }
                        break;
                    case "FIO":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(u => u.FIO);
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.FIO);
                        }
                        break;
                    case "PhoneNumber":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(u => u.PhoneNumber);
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.PhoneNumber);
                        }
                        break;
                    case "Email":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(u => u.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.Email);
                        }
                        break;
                    case "PostName":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(u => u.Post.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(u => u.Post.Name);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
