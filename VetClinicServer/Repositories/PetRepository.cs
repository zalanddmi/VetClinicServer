using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class PetRepository(Context context) : IRepository<Pet>
    {
        private readonly Context _context = context;

        public void Create(Pet entity)
        {
            _context.Pets.Add(entity);
        }

        public void Delete(Pet entity)
        {
            _context.Pets.Remove(entity);
        }

        public IEnumerable<Pet> GetAll()
        {
            return _context.Pets;
        }

        public Pet? GetById(int id)
        {
            return _context.Pets.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Pet entity)
        {
            _context.Pets.Update(entity);
        }

        public async Task<PaginatedList<Pet>> GetPaged(GetPagedPetsRequest request)
        {
            IQueryable<Pet> query = _context.Pets;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Pet>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Pet> ApplySorting(IQueryable<Pet> query, GetPagedPetsRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Name.Contains(request.SearchString)
                || d.Species.Name.Contains(request.SearchString)
                || d.Breed.Contains(request.SearchString)
                || d.Owner.FIO.Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(d => d.Name.Contains(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Species))
            {
                query = query.Where(d => d.Species.Name.Contains(request.Species));
            }
            if (!string.IsNullOrEmpty(request.Breed))
            {
                query = query.Where(d => d.Breed.Contains(request.Breed));
            }
            if (!string.IsNullOrEmpty(request.Owner))
            {
                query = query.Where(d => d.Owner.FIO.Contains(request.Owner));
            }
            switch (request.DateOfBirthComparisonOperators)
            {
                case ComparisonOperators.LessThan:
                    query = query.Where(d => d.DateOfBirth < request.DateOfBirth);
                    break;
                case ComparisonOperators.LessThanOrEqual:
                    query = query.Where(d => d.DateOfBirth <= request.DateOfBirth);
                    break;
                case ComparisonOperators.Equal:
                    query = query.Where(d => d.DateOfBirth == request.DateOfBirth);
                    break;
                case ComparisonOperators.NotEqual:
                    query = query.Where(d => d.DateOfBirth != request.DateOfBirth);
                    break;
                case ComparisonOperators.GreaterThanOrEqual:
                    query = query.Where(d => d.DateOfBirth >= request.DateOfBirth);
                    break;
                case ComparisonOperators.GreaterThan:
                    query = query.Where(d => d.DateOfBirth > request.DateOfBirth);
                    break;
            }
            return query;
        }

        private static IQueryable<Pet> ApplyFilter(IQueryable<Pet> query, GetPagedPetsRequest request)
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
                    case "Species":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Species.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Species.Name);
                        }
                        break;
                    case "Breed":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Breed);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Breed);
                        }
                        break;
                    case "Owner":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Owner.FIO);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Owner.FIO);
                        }
                        break;
                    case "DateOfBirth":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.DateOfBirth);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.DateOfBirth);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
