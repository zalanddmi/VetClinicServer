using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class SpeciesRepository(Context context) : IRepository<Species>
    {
        private readonly Context _context = context;

        public void Create(Species entity)
        {
            _context.Species.Add(entity);
        }

        public void Delete(Species entity)
        {
            _context.Species.Remove(entity);
        }

        public IEnumerable<Species> GetAll()
        {
            return _context.Species;
        }

        public Species? GetById(int id)
        {
            return _context.Species.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Species entity)
        {
            _context.Species.Update(entity);
        }

        public async Task<PaginatedList<Species>> GetPaged(GetPagedSpeciesRequest request)
        {
            IQueryable<Species> query = _context.Species;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Species>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Species> ApplySorting(IQueryable<Species> query, GetPagedSpeciesRequest request)
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

        private static IQueryable<Species> ApplyFilter(IQueryable<Species> query, GetPagedSpeciesRequest request)
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
