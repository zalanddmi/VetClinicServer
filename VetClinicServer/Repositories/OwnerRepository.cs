using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class OwnerRepository(Context context) : IRepository<Owner>
    {
        private readonly Context _context = context;

        public void Create(Owner entity)
        {
            _context.Owners.Add(entity);
        }

        public void Delete(Owner entity)
        {
            _context.Owners.Remove(entity);
        }

        public IEnumerable<Owner> GetAll()
        {
            return _context.Owners;
        }

        public Owner? GetById(int id)
        {
            return _context.Owners.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Owner entity)
        {
            _context.Owners.Update(entity);
        }

        public async Task<PaginatedList<Owner>> GetPaged(GetPagedOwnersRequest request)
        {
            IQueryable<Owner> query = _context.Owners;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Owner>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Owner> ApplySorting(IQueryable<Owner> query, GetPagedOwnersRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.FIO.Contains(request.SearchString)
                || d.PhoneNumber.Contains(request.SearchString)
                || d.Email.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.FIO))
            {
                query = query.Where(d => d.FIO.Contains(request.FIO));
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                query = query.Where(d => d.PhoneNumber.Contains(request.PhoneNumber));
            }
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(d => d.Email.Contains(request.Email));
            }
            return query;
        }

        private static IQueryable<Owner> ApplyFilter(IQueryable<Owner> query, GetPagedOwnersRequest request)
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
                    case "FIO":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.FIO);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.FIO);
                        }
                        break;
                    case "PhoneNumber":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.PhoneNumber);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.PhoneNumber);
                        }
                        break;
                    case "Email":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Email);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Email);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
