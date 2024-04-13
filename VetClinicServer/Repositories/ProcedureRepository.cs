using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class ProcedureRepository(Context context) : IRepository<Procedure>
    {
        private readonly Context _context = context;

        public void Create(Procedure entity)
        {
            _context.Procedures.Add(entity);
        }

        public void Delete(Procedure entity)
        {
            _context.Procedures.Remove(entity);
        }

        public IEnumerable<Procedure> GetAll()
        {
            return _context.Procedures;
        }

        public Procedure? GetById(int id)
        {
            return _context.Procedures.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Procedure entity)
        {
            _context.Procedures.Update(entity);
        }

        public async Task<PaginatedList<Procedure>> GetPaged(GetPagedProceduresRequest request)
        {
            IQueryable<Procedure> query = _context.Procedures;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Procedure>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Procedure> ApplySorting(IQueryable<Procedure> query, GetPagedProceduresRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Name.Contains(request.SearchString)
                || d.Cost.ToString().Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name.Contains(request.Name));
            }
            switch (request.CostComparisonOperators)
            {
                case ComparisonOperators.LessThan:
                    query = query.Where(d => d.Cost < request.Cost);
                    break;
                case ComparisonOperators.LessThanOrEqual:
                    query = query.Where(d => d.Cost <= request.Cost);
                    break;
                case ComparisonOperators.Equal:
                    query = query.Where(d => d.Cost == request.Cost);
                    break;
                case ComparisonOperators.NotEqual:
                    query = query.Where(d => d.Cost != request.Cost);
                    break;
                case ComparisonOperators.GreaterThanOrEqual:
                    query = query.Where(d => d.Cost >= request.Cost);
                    break;
                case ComparisonOperators.GreaterThan:
                    query = query.Where(d => d.Cost > request.Cost);
                    break;
            }
            return query;
        }

        private static IQueryable<Procedure> ApplyFilter(IQueryable<Procedure> query, GetPagedProceduresRequest request)
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
                    case "Cost":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Cost);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Cost);
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
