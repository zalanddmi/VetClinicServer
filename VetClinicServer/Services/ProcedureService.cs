using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class ProcedureService(ProcedureRepository procedureRepository, ExcelConverter excelConverter)
    {
        private readonly ProcedureRepository _procedureRepository = procedureRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(ProcedureViewModel model)
        {
            Procedure procedure = new()
            {
                Name = model.Name,
                Cost = model.Cost,
                Description = model.Description
            };
            _procedureRepository.Create(procedure);
            _procedureRepository.Save();
            return procedure.Id;
        }

        public void Update(ProcedureViewModel model)
        {
            Procedure procedure = _procedureRepository.GetById(model.Id) ?? throw new NullReferenceException("Процедура не найдена");
            procedure.Name = model.Name;
            procedure.Cost = model.Cost;
            procedure.Description = model.Description;
            _procedureRepository.Update(procedure);
            _procedureRepository.Save();
        }

        public void Delete(int id)
        {
            Procedure procedure = _procedureRepository.GetById(id) ?? throw new NullReferenceException("Процедура не найдена");
            _procedureRepository.Delete(procedure);
            _procedureRepository.Save();
        }

        public ProcedureViewModel GetById(int id)
        {
            Procedure procedure = _procedureRepository.GetById(id) ?? throw new NullReferenceException("Процедура не найдена");
            ProcedureViewModel model = new()
            {
                Id = id,
                Name = procedure.Name,
                Cost = procedure.Cost,
                Description = procedure.Description
            };
            return model;
        }

        public async Task<PaginatedList<ProcedureViewModel>> GetPaged(GetPagedProceduresRequest request)
        {
            PaginatedList<Procedure> procedures = await _procedureRepository.GetPaged(request);
            List<ProcedureViewModel> procedureViewModels = procedures.Select(procedure => new ProcedureViewModel
            {
                Id = procedure.Id,
                Name = procedure.Name,
                Cost = procedure.Cost,
                Description = procedure.Description
            }).ToList();
            var pagedProcedures = new PaginatedList<ProcedureViewModel>(procedureViewModels, procedures.PageNumber, procedures.TotalPages);
            return pagedProcedures;
        }

        public async Task<byte[]> ExportToExcel(GetProceduresExcelRequest request)
        {
            GetPagedProceduresRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
                Cost = request.Cost,
                CostComparisonOperators = request.CostComparisonOperators,
            };
            PaginatedList<Procedure> procedures = await _procedureRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Procedure procedure in procedures)
            {
                List<object> row = [procedure.Name, procedure.Cost, procedure.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Название", "Стоимость", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Процедуры", data, columns);
            return excelBytes;
        }
    }
}
