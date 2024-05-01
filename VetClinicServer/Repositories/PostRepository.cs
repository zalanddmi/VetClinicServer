using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class PostRepository(Context context) : IRepository<Post>
    {
        private readonly Context _context = context;

        public void Create(Post entity)
        {
            _context.Posts.Add(entity);
        }

        public void Delete(Post entity)
        {
            _context.Posts.Remove(entity);
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts;
        }

        public Post? GetById(int id)
        {
            return _context.Posts.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Post entity)
        {
            _context.Posts.Update(entity);
        }

        public async Task<PaginatedList<Post>> GetPaged(GetPagedPostsRequest request)
        {
            IQueryable<Post> query = _context.Posts;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Post>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Post> ApplySorting(IQueryable<Post> query, GetPagedPostsRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Name.Contains(request.SearchString)
                || d.Role.Name.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Role))
            {
                query = query.Where(d => d.Role.Name.Contains(request.Name));
            }
            return query;
        }

        private static IQueryable<Post> ApplyFilter(IQueryable<Post> query, GetPagedPostsRequest request)
        {
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                switch (request.OrderBy)
                {
                    case "Id":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Id);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Id);
                        }
                        break;
                    case "Name":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Name);
                        }
                        break;
                    case "Role":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Role.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Role.Name);
                        }
                        break;
                    case "Description":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Description);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Description);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
