using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;

namespace VetClinicServer.Repositories
{
    public class AppointmentRepository(Context context) : IRepository<Appointment>
    {
        private readonly Context _context = context;

        public void Create(Appointment entity)
        {
            _context.Appointments.Add(entity);
        }

        public void Delete(Appointment entity)
        {
            _context.Appointments.Remove(entity);
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments;
        }

        public Appointment? GetById(int id)
        {
            return _context.Appointments.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Appointment entity)
        {
            _context.Appointments.Update(entity);
        }

        public async Task<PaginatedList<Appointment>> GetPaged(GetPagedAppointmentsRequest request)
        {
            IQueryable<Appointment> query = _context.Appointments;

            query = ApplySorting(query, request);
            query = ApplyFilter(query, request);

            var list = await PaginatedList<Appointment>.CreateAsync(query, request.PageNumber, request.PageSize);
            return list;
        }

        private static IQueryable<Appointment> ApplySorting(IQueryable<Appointment> query, GetPagedAppointmentsRequest request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                query = query.Where(d => d.Procedure.Name.Contains(request.SearchString)
                || d.Pet.Name.Contains(request.SearchString)
                || d.User.FIO.Contains(request.SearchString)
                || d.TotalCost.ToString().Contains(request.SearchString));
            }
            if (!string.IsNullOrEmpty(request.Procedure))
            {
                query = query.Where(d => d.Procedure.Name.Contains(request.Procedure));
            }
            if (!string.IsNullOrEmpty(request.Pet))
            {
                query = query.Where(d => d.Pet.Name.Contains(request.Pet));
            }
            if (!string.IsNullOrEmpty(request.User))
            {
                query = query.Where(d => d.User.FIO.Contains(request.User));
            }
            switch (request.DateAppointmentComparisonOperators)
            {
                case ComparisonOperators.LessThan:
                    query = query.Where(d => d.DateAppointment < request.DateAppointment);
                    break;
                case ComparisonOperators.LessThanOrEqual:
                    query = query.Where(d => d.DateAppointment <= request.DateAppointment);
                    break;
                case ComparisonOperators.Equal:
                    query = query.Where(d => d.DateAppointment == request.DateAppointment);
                    break;
                case ComparisonOperators.NotEqual:
                    query = query.Where(d => d.DateAppointment != request.DateAppointment);
                    break;
                case ComparisonOperators.GreaterThanOrEqual:
                    query = query.Where(d => d.DateAppointment >= request.DateAppointment);
                    break;
                case ComparisonOperators.GreaterThan:
                    query = query.Where(d => d.DateAppointment > request.DateAppointment);
                    break;
            }
            if (request.Status != null)
            {
                query = query.Where(d => d.Status == request.Status);
            }
            if (request.DateCompleted != null)
            {
                switch (request.DateCompletedComparisonOperators)
                {
                    case ComparisonOperators.LessThan:
                        query = query.Where(d => d.DateCompleted < request.DateCompleted);
                        break;
                    case ComparisonOperators.LessThanOrEqual:
                        query = query.Where(d => d.DateCompleted <= request.DateCompleted);
                        break;
                    case ComparisonOperators.Equal:
                        query = query.Where(d => d.DateCompleted == request.DateCompleted);
                        break;
                    case ComparisonOperators.NotEqual:
                        query = query.Where(d => d.DateCompleted != request.DateCompleted);
                        break;
                    case ComparisonOperators.GreaterThanOrEqual:
                        query = query.Where(d => d.DateCompleted >= request.DateCompleted);
                        break;
                    case ComparisonOperators.GreaterThan:
                        query = query.Where(d => d.DateCompleted > request.DateCompleted);
                        break;
                }
            }
            switch (request.TotalCostComparisonOperators)
            {
                case ComparisonOperators.LessThan:
                    query = query.Where(d => d.TotalCost < request.TotalCost);
                    break;
                case ComparisonOperators.LessThanOrEqual:
                    query = query.Where(d => d.TotalCost <= request.TotalCost);
                    break;
                case ComparisonOperators.Equal:
                    query = query.Where(d => d.TotalCost == request.TotalCost);
                    break;
                case ComparisonOperators.NotEqual:
                    query = query.Where(d => d.TotalCost != request.TotalCost);
                    break;
                case ComparisonOperators.GreaterThanOrEqual:
                    query = query.Where(d => d.TotalCost >= request.TotalCost);
                    break;
                case ComparisonOperators.GreaterThan:
                    query = query.Where(d => d.TotalCost > request.TotalCost);
                    break;
            }
            return query;
        }

        private static IQueryable<Appointment> ApplyFilter(IQueryable<Appointment> query, GetPagedAppointmentsRequest request)
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
                    case "Procedure":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Procedure.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Procedure.Name);
                        }
                        break;
                    case "Pet":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.Pet.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.Pet.Name);
                        }
                        break;
                    case "User":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.User.FIO);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.User.FIO);
                        }
                        break;
                    case "DateAppointment":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.DateAppointment);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.DateAppointment);
                        }
                        break;
                    case "DateCompleted":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.DateCompleted);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.DateCompleted);
                        }
                        break;
                    case "TotalCost":
                        if (request.SortDirection == SortDirections.Ascending)
                        {
                            query = query.OrderBy(d => d.TotalCost);
                        }
                        else
                        {
                            query = query.OrderByDescending(d => d.TotalCost);
                        }
                        break;
                }
            }
            return query;
        }
    }
}
