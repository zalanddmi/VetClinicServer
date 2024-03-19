using System.Linq;
using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class DrugRepository(Context context) : IRepository<Drug>
    {
        private readonly Context _context = context;

        public void Create(Drug entity)
        {
            _context.Drugs.Add(entity);
            Save();
        }

        public void Delete(Drug entity)
        {
            _context.Remove(entity);
            Save();
        }

        public IEnumerable<Drug> GetAll()
        {
            return _context.Drugs;
        }

        public Drug? GetById(int id)
        {
            return _context.Drugs.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Drug entity)
        {
            _context.Update(entity);
        }

        public async Task<PaginatedList<Drug>> GetPaged(GetPagedDrugsRequest request)
        {
            IQueryable<Drug> query = _context.Drugs;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);
            
            var list = await PaginatedList<Drug>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private IQueryable<Drug> ApplySorting(IQueryable<Drug> query, GetPagedDrugsRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Name.Contains(request.SearchString)
                || d.Cost.ToString().Contains(request.SearchString)
                || d.Quantity.ToString().Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name.Contains(d.Name));
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
            switch (request.QuantityComparisonOperators)
            {
                case ComparisonOperators.LessThan:
                    query = query.Where(d => d.Quantity < request.Quantity);
                    break;
                case ComparisonOperators.LessThanOrEqual:
                    query = query.Where(d => d.Quantity <= request.Quantity);
                    break;
                case ComparisonOperators.Equal:
                    query = query.Where(d => d.Quantity == request.Quantity);
                    break;
                case ComparisonOperators.NotEqual:
                    query = query.Where(d => d.Quantity != request.Quantity);
                    break;
                case ComparisonOperators.GreaterThanOrEqual:
                    query = query.Where(d => d.Quantity >= request.Quantity);
                    break;
                case ComparisonOperators.GreaterThan:
                    query = query.Where(d => d.Quantity > request.Quantity);
                    break;
            }
            return query;
        }

        private IQueryable<Drug> ApplyFilter(IQueryable<Drug> query, GetPagedDrugsRequest request)
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
                    case "Quantity":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Quantity);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Quantity);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
