using System;
using System.Linq;
using System.Collections.Immutable;
using VetClinicServer.Models.Enums;
using VetClinicServer.Repositories;

namespace VetClinicServer.Services
{
    public class ChartService(AppointmentRepository appointmentRepository)
    {
        private readonly AppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly ImmutableList<AppointmentStatuses> _appointmentStatuses = [AppointmentStatuses.Waiting, AppointmentStatuses.Completed, AppointmentStatuses.Cancelled];

        private readonly ImmutableList<string> _appointmentsStatusesName = ["Ожидание", "Завершен", "Отменен"];

        private readonly ImmutableList<string> _appointmentsStatusesColor = ["rgba(255, 255, 0, 1)", "rgba(0, 127, 0, 1)", "rgba(255, 0, 0, 1)"];

        public object GetChartData()
        {
            DateTime today = DateTime.Today;
            DateTime startOfMonth = new(today.Year, today.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var weeklyData = Enumerable.Range(0, (endOfMonth - startOfMonth).Days / 7 + 1)
                .Select(i => startOfMonth.AddDays(i * 7))
                .Select(startDate =>
                {
                    var endDate = startDate.AddDays(6);
                    if (endDate > endOfMonth)
                        endDate = endOfMonth;

                    var weekLabel = $"{startDate:dd.MM} - {endDate:dd.MM}";
                    var appointments = GenerateWeekData(startDate, endDate);
                    return new { weekLabel, appointments };
                })
                .ToArray();

            var labels = weeklyData.Select(data => data.weekLabel).ToArray();
            var datasets = _appointmentStatuses.Select((status, index) => new
            {
                label = _appointmentsStatusesName[index],
                data = weeklyData.Select(data =>
                {
                    var appointmentsData = data.appointments as IEnumerable<object>;
                    var appointment = appointmentsData.FirstOrDefault(app => (string)app.GetType().GetProperty("Status").GetValue(app) == status.ToString());
                    return appointment != null ? (int)appointment.GetType().GetProperty("Count").GetValue(appointment) : 0;
                }).ToArray(),
                backgroundColor = _appointmentsStatusesColor[index]
            }).ToArray();

            return new { labels, datasets };
        }

        private object GenerateWeekData(DateTime startDate, DateTime endDate)
        {
            var appointments = _appointmentRepository.GetAll()
                .Where(a => a.DateAppointment.Date >= startDate && a.DateAppointment.Date <= endDate)
                .GroupBy(a => a.Status)
                .Select(g => new
                {
                    Status = g.Key.ToString(),
                    Count = g.Count()
                });

            return appointments.ToArray();
        }
    }
}
