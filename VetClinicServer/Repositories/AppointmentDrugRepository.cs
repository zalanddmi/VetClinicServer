using VetClinicServer.Data;
using VetClinicServer.Interfaces;
using VetClinicServer.Models;

namespace VetClinicServer.Repositories
{
    public class AppointmentDrugRepository(Context context) : IRepository<AppointmentDrug>
    {
        private readonly Context _context = context;

        public void Create(AppointmentDrug entity)
        {
            _context.AppointmentDrugs.Add(entity);
        }

        public void Delete(AppointmentDrug entity)
        {
            _context.AppointmentDrugs.Remove(entity);
        }

        public IEnumerable<AppointmentDrug> GetAll()
        {
            return _context.AppointmentDrugs;
        }

        public IEnumerable<AppointmentDrug>? GetByAppointmentId(int appointmentId)
        {
            return _context.AppointmentDrugs.Where(d => d.AppointmentId == appointmentId);
        }

        public IEnumerable<AppointmentDrug>? GetByDrugId(int drugId)
        {
            return _context.AppointmentDrugs.Where(d => d.DrugId == drugId);
        }

        /// <summary>
        /// Не использовать
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppointmentDrug? GetById(int id)
        {
            return null;
        }

        public AppointmentDrug? GetByAppointmentDrugIds(int appointmentId, int drugId)
        {
            return _context.AppointmentDrugs.FirstOrDefault(d => d.AppointmentId == appointmentId &&  d.DrugId == drugId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(AppointmentDrug entity)
        {
            _context.AppointmentDrugs.Update(entity);
        }
    }
}
