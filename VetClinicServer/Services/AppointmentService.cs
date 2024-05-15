using VetClinicServer.Models;
using VetClinicServer.Models.Enums;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class AppointmentService(AppointmentRepository appointmentRepository,
        AppointmentDrugRepository appointmentDrugRepository,
        ProcedureRepository procedureRepository,
        PetRepository petRepository,
        UserRepository userRepository,
        DrugRepository drugRepository,
        ExcelConverter excelConverter)
    {
        private readonly AppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly AppointmentDrugRepository _appointmentDrugRepository = appointmentDrugRepository;
        private readonly ProcedureRepository _procedureRepository = procedureRepository;
        private readonly PetRepository _petRepository = petRepository;
        private readonly UserRepository _userRepository = userRepository;
        private readonly DrugRepository _drugRepository = drugRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(AppointmentViewModel model)
        {
            Procedure procedure = _procedureRepository.GetById(model.Procedure.Id) ?? throw new NullReferenceException("Процедура не найдена");
            Pet pet = _petRepository.GetById(model.Pet.Id) ?? throw new NullReferenceException("Животное не найдено");
            User user = _userRepository.GetById(model.Doctor.Id) ?? throw new NullReferenceException("Пользователь не найден");
            decimal totalCost = procedure.Cost;
            List<Drug> drugList = [];
            if (model.Drugs != null)
            {
                foreach (AppointmentDrugViewModel item in model.Drugs)
                {
                    Drug drug = _drugRepository.GetById(item.Drug.Id) ?? throw new NullReferenceException("Лекарство не найдено");
                    if (item.Quantity > drug.Quantity)
                    {
                        throw new ArgumentException("Количество требуемых лекарств превышает имеющиеся");
                    }
                    if (drugList.Any(d => d.Id == drug.Id))
                    {
                        throw new ArgumentException("Лекарства не должны повторяться");
                    }
                    drugList.Add(drug);
                    totalCost += drug.Cost * item.Quantity;
                    drug.Quantity -= item.Quantity;
                    _drugRepository.Update(drug);
                }
            }
            Appointment appointment = new()
            {
                Procedure = procedure,
                Pet = pet,
                User = user,
                Description = model.Description,
                DateAppointment = model.DateAppointment,
                Status = AppointmentStatuses.Waiting,
                TotalCost = totalCost
            };
            _appointmentRepository.Create(appointment);
            if (model.Drugs != null)
            {
                for (int i = 0; i < model.Drugs.Count; i++)
                {
                    AppointmentDrug appointmentDrug = new() { Appointment = appointment, Drug = drugList[i], Quantity = model.Drugs[i].Quantity };
                    _appointmentDrugRepository.Create(appointmentDrug);
                }
            }
            _appointmentDrugRepository.Save();
            return appointment.Id;
        }

        public void Update(AppointmentViewModel model)
        {
            Appointment appointment = _appointmentRepository.GetById(model.Id) ?? throw new NullReferenceException("Прием не найден");
            if (appointment.Status == AppointmentStatuses.Completed && model.Status == AppointmentStatuses.Cancelled)
            {
                throw new ArgumentException("Нельзя перевести статус Завершен в Отменен");
            }
            if (appointment.Status == AppointmentStatuses.Cancelled && model.Status == AppointmentStatuses.Completed)
            {
                throw new ArgumentException("Нельзя перевести статус Отменен в Завершен");
            }
            if (appointment.Status == AppointmentStatuses.Completed && model.Status == AppointmentStatuses.Waiting)
            {
                throw new ArgumentException("Нельзя перевести статус Завершен в Ожидание");
            }
            if (appointment.Status == AppointmentStatuses.Cancelled && model.Status == AppointmentStatuses.Waiting)
            {
                throw new ArgumentException("Нельзя перевести статус Отменен в Ожидание");
            }
            if (model.Status == AppointmentStatuses.Completed || model.Status == AppointmentStatuses.Cancelled)
            {
                appointment.Status = (AppointmentStatuses)model.Status;
                if (model.Status == AppointmentStatuses.Cancelled && (appointment.AppointmentDrugs != null || appointment.AppointmentDrugs.Count > 0))
                {
                    foreach (AppointmentDrug item in appointment.AppointmentDrugs)
                    {
                        Drug drug = _drugRepository.GetById(item.DrugId);
                        drug.Quantity += item.Quantity;
                        _drugRepository.Update(drug);
                    }
                }
                else
                {
                    appointment.DateCompleted = DateTime.UtcNow;
                }
                _appointmentRepository.Update(appointment);
                _appointmentRepository.Save();
                return;
            }
            Procedure procedure = _procedureRepository.GetById(model.Procedure.Id) ?? throw new NullReferenceException("Процедура не найдена");
            Pet pet = _petRepository.GetById(model.Pet.Id) ?? throw new NullReferenceException("Животное не найдено");
            User user = _userRepository.GetById(model.Doctor.Id) ?? throw new NullReferenceException("Пользователь не найден");
            decimal totalCost = procedure.Cost;
            List<Drug> drugList = [];
            if (appointment.AppointmentDrugs != null && appointment.AppointmentDrugs.Count > 0)
            {
                if (model.Drugs != null)
                {
                    foreach (AppointmentDrug item in appointment.AppointmentDrugs)
                    {
                        if (model.Drugs.Where(ad => ad.Drug.Id == item.DrugId) == null)
                        {
                            Drug drug = _drugRepository.GetById(item.Drug.Id);
                            drug.Quantity += item.Quantity;
                            _drugRepository.Update(drug);
                            _appointmentDrugRepository.Delete(item);
                            totalCost -= item.Quantity * drug.Cost;
                        }
                    }
                    foreach (AppointmentDrugViewModel item in model.Drugs)
                    {
                        Drug drug = _drugRepository.GetById(item.Drug.Id) ?? throw new NullReferenceException("Лекарство не найдено");
                        if (!appointment.AppointmentDrugs.Any(ad => ad.DrugId == drug.Id))
                        {
                            if (item.Quantity > drug.Quantity)
                            {
                                throw new ArgumentException("Количество требуемых лекарств превышает имеющиеся");
                            }
                            if (drugList.Any(d => d.Id == drug.Id))
                            {
                                throw new ArgumentException("Лекарства не должны повторяться");
                            }
                            drug.Quantity -= item.Quantity;
                            _drugRepository.Update(drug);
                            AppointmentDrug appointmentDrug = new() { Appointment = appointment, Drug = drug, Quantity = item.Quantity };
                            _appointmentDrugRepository.Create(appointmentDrug);
                            totalCost += item.Quantity * drug.Cost;
                        }
                        else
                        {
                            AppointmentDrug appointmentDrug = appointment.AppointmentDrugs.FirstOrDefault(ad => ad.DrugId == drug.Id);
                            if (item.Quantity > drug.Quantity + appointmentDrug.Quantity)
                            {
                                throw new ArgumentException("Количество требуемых лекарств превышает имеющиеся");
                            }
                            if (drugList.Any(d => d.Id == drug.Id))
                            {
                                throw new ArgumentException("Лекарства не должны повторяться");
                            }
                            drug.Quantity += appointmentDrug.Quantity - item.Quantity;
                            _drugRepository.Update(drug);
                            appointmentDrug.Quantity = item.Quantity;
                            _appointmentDrugRepository.Update(appointmentDrug);
                            totalCost += item.Quantity * drug.Cost;
                        }
                    }
                }
            }
            else
            {
                foreach (AppointmentDrugViewModel item in model.Drugs)
                {
                    Drug drug = _drugRepository.GetById(item.Drug.Id) ?? throw new NullReferenceException("Лекарство не найдено");
                    if (item.Quantity > drug.Quantity)
                    {
                        throw new ArgumentException("Количество требуемых лекарств превышает имеющиеся");
                    }
                    if (drugList.Any(d => d.Id == drug.Id))
                    {
                        throw new ArgumentException("Лекарства не должны повторяться");
                    }
                    drug.Quantity -= item.Quantity;
                    _drugRepository.Update(drug);
                    AppointmentDrug appointmentDrug = new() { Appointment = appointment, Drug = drug, Quantity = item.Quantity };
                    _appointmentDrugRepository.Create(appointmentDrug);
                    totalCost += item.Quantity * drug.Cost;
                }
            }
            appointment.Procedure = procedure;
            appointment.Pet = pet;
            appointment.User = user;
            appointment.Description = model.Description;
            appointment.DateAppointment = model.DateAppointment;
            appointment.Status = (AppointmentStatuses)model.Status;
            appointment.TotalCost = totalCost;
            _appointmentRepository.Update(appointment);
            _appointmentRepository.Save();
        }

        public void Delete(int id)
        {
            Appointment appointment = _appointmentRepository.GetById(id) ?? throw new NullReferenceException("Прием не найден");
            foreach (var item in appointment.AppointmentDrugs)
            {
                _appointmentDrugRepository.Delete(item);
            }
            _appointmentRepository.Delete(appointment);
            _appointmentRepository.Save();
        }

        public AppointmentViewModel GetById(int id)
        {
            Appointment appointment = _appointmentRepository.GetById(id) ?? throw new NullReferenceException("Прием не найден");
            DisplayModel procedureDisplay = new() { Id = appointment.ProcedureId, Name = appointment.Procedure.Name, Entity = "Procedure" };
            DisplayModel petDisplay = new() { Id = appointment.PetId, Name = appointment.Pet.Name, Entity = "Pet" };
            DisplayModel userDisplay = new() { Id = appointment.UserId, Name = appointment.User.FIO, Entity = "User" };
            List<AppointmentDrugViewModel> appointmentDrugs = [];
            foreach (var item in appointment.AppointmentDrugs)
            {
                appointmentDrugs.Add(new() { Drug = new DisplayModel() { Id = item.DrugId, Name = item.Drug.Name, Entity = "Drug" }, Quantity = item.Quantity });
            }
            AppointmentViewModel model = new()
            {
                Id = id,
                Procedure = procedureDisplay,
                Pet = petDisplay,
                Doctor = userDisplay,
                Drugs = appointmentDrugs,
                Description = appointment.Description,
                DateAppointment = appointment.DateAppointment,
                Status = appointment.Status,
                DateCompleted = appointment.DateCompleted.HasValue ? appointment.DateCompleted.Value.ToShortDateString() : null,
                TotalCost = appointment.TotalCost,
            };
            return model;
        }

        public async Task<PaginatedList<AppointmentViewModel>> GetPaged(GetPagedAppointmentsRequest request)
        {
            PaginatedList<Appointment> appointments = await _appointmentRepository.GetPaged(request);
            List<AppointmentViewModel> appointmentViewModels = [];
            foreach (var appointment in appointments)
            {
                DisplayModel procedureDisplay = new() { Id = appointment.ProcedureId, Name = appointment.Procedure.Name, Entity = "Procedure" };
                DisplayModel petDisplay = new() { Id = appointment.PetId, Name = appointment.Pet.Name, Entity = "Pet" };
                DisplayModel userDisplay = new() { Id = appointment.UserId, Name = appointment.User.FIO, Entity = "User" };
                List<AppointmentDrugViewModel> appointmentDrugs = [];
                foreach (var item in appointment.AppointmentDrugs)
                {
                    appointmentDrugs.Add(new() { Drug = new DisplayModel() { Id = item.DrugId, Name = item.Drug.Name, Entity = "Drug" }, Quantity = item.Quantity });
                }
                AppointmentViewModel model = new()
                {
                    Id = appointment.Id,
                    Procedure = procedureDisplay,
                    Pet = petDisplay,
                    Doctor = userDisplay,
                    Drugs = appointmentDrugs,
                    Description = appointment.Description,
                    DateAppointment = appointment.DateAppointment,
                    Status = appointment.Status,
                    DateCompleted = appointment.DateCompleted.HasValue ? appointment.DateCompleted.Value.ToShortDateString() : null,
                    TotalCost = appointment.TotalCost,
                };
                appointmentViewModels.Add(model);
            }
            var pagedPosts = new PaginatedList<AppointmentViewModel>(appointmentViewModels, appointments.PageNumber, appointments.TotalPages);
            return pagedPosts;
        }

        public async Task<byte[]> ExportToExcel(GetAppointmentsExcelRequest request)
        {
            GetPagedAppointmentsRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Procedure = request.Procedure,
                Pet = request.Pet,
                User = request.User,
                DateAppointment = request.DateAppointment,
                DateAppointmentComparisonOperators = request.DateAppointmentComparisonOperators,
                Status = request.Status,
                DateCompleted = request.DateCompleted,
                DateCompletedComparisonOperators = request.DateCompletedComparisonOperators,
                TotalCost = request.TotalCost,
                TotalCostComparisonOperators = request.TotalCostComparisonOperators
            };
            PaginatedList<Appointment> appointments = await _appointmentRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Appointment appointment in appointments)
            {
                string status = string.Empty;
                if (appointment.Status == AppointmentStatuses.Waiting)
                {
                    status = "Ожидание";
                }
                else if (appointment.Status == AppointmentStatuses.Completed)
                {
                    status = "Завершен";
                }
                else if (appointment.Status == AppointmentStatuses.Cancelled)
                {
                    status = "Отменен";
                }
                List<object> row = [appointment.Procedure.Name, appointment.Pet.Name, appointment.User.FIO,
                    appointment.DateAppointment.ToString(), status, appointment.DateCompleted.ToString() ?? string.Empty,
                    appointment.TotalCost, appointment.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Процедура", "Животное", "Врач", "Дата приема", "Статус", "Дата завершения", "Стоимость", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Приемы", data, columns);
            return excelBytes;
        }
    }
}
