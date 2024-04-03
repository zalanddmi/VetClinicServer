using Aspose.Cells;
using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests;
using VetClinicServer.Requests.Enums;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class DrugService(DrugRepository drugRepository, ExcelConverter excelConverter)
    {
        private readonly DrugRepository _drugRepository = drugRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public void Create(DrugViewModel model)
        {
            Drug drug = new()
            {
                Name = model.Name,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Description = model.Description,
            };
            _drugRepository.Create(drug);
            _drugRepository.Save();
        }

        public void Update(DrugViewModel model)
        {
            Drug drug = _drugRepository.GetById(model.Id) ?? throw new NullReferenceException("Лекарство не найдено");
            drug.Name = model.Name;
            drug.Cost = model.Cost;
            drug.Quantity = model.Quantity;
            drug.Description = model.Description;
            _drugRepository.Update(drug);
            _drugRepository.Save();
        }

        public void Delete(int id)
        {
            Drug drug = _drugRepository.GetById(id) ?? throw new NullReferenceException("Лекарство не найдено");
            _drugRepository.Delete(drug);
            _drugRepository.Save();
        }

        public DrugViewModel GetById(int id)
        {
            Drug drug = _drugRepository.GetById(id) ?? throw new NullReferenceException("Лекарство не найдено");
            DrugViewModel model = new()
            {
                Id = id,
                Name = drug.Name,
                Cost = drug.Cost,
                Quantity = drug.Quantity,
                Description = drug.Description
            };
            return model;
        }

        public async Task<PaginatedList<DrugViewModel>> GetPaged(GetPagedDrugsRequest request)
        {
            PaginatedList<Drug> drugs = await _drugRepository.GetPaged(request);
            List<DrugViewModel> drugViewModels = drugs.Select(drug => new DrugViewModel
            {
                Id = drug.Id,
                Name = drug.Name,
                Cost = drug.Cost,
                Quantity = drug.Quantity,
                Description = drug.Description
            }).ToList();
            var pagedDrugs = new PaginatedList<DrugViewModel>(drugViewModels, drugs.PageNumber, drugs.TotalPages);
            return pagedDrugs;
        }

        public async Task<byte[]> ExportToExcel(GetDrugsExcelRequest request)
        {
            GetPagedDrugsRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
                Cost = request.Cost,
                CostComparisonOperators = request.CostComparisonOperators,
                Quantity = request.Quantity,
                QuantityComparisonOperators = request.QuantityComparisonOperators
            };
            PaginatedList<Drug> drugs = await _drugRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Drug drug in drugs)
            {
                List<object> row = [drug.Name, drug.Cost, drug.Quantity, drug.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Название", "Стоимость", "Количество", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Лекарства", data, columns);
            return excelBytes;
        }
    }
}
