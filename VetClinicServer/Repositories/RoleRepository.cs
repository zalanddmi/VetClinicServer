using VetClinicServer.Data;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Utils;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Pages;

namespace VetClinicServer.Repositories
{
    public class RoleRepository(Context context): IRepository<Role>
    {
        private readonly Context _context = context;

        public void Create(Role entity)
        {
            _context.Roles.Add(entity);
        }

        public void Delete(Role entity)
        {
            _context.Roles.Remove(entity);
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles;
        }

        public Role? GetById(int id)
        {
            return _context.Roles.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Role entity)
        {
            _context.Roles.Update(entity);
        }

        public async Task<PaginatedList<Role>> GetPaged(GetPagedRolesRequest request)
        {
            IQueryable<Role> query = _context.Roles;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Role>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Role> ApplySorting(IQueryable<Role> query, GetPagedRolesRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Name.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name.Contains(request.Name));
            }
            return query;
        }

        private static IQueryable<Role> ApplyFilter(IQueryable<Role> query, GetPagedRolesRequest request)
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
                }
            }
            return query;
        }
    }
}
