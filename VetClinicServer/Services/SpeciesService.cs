using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class SpeciesService(SpeciesRepository speciesRepository, ExcelConverter excelConverter)
    {
        private readonly SpeciesRepository _speciesRepository = speciesRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(SpeciesViewModel model)
        {
            Species species = new()
            {
                Name = model.Name
            };
            _speciesRepository.Create(species);
            _speciesRepository.Save();
            return species.Id;
        }

        public void Update(SpeciesViewModel model)
        {
            Species species = _speciesRepository.GetById(model.Id) ?? throw new NullReferenceException("Вид животного не найден");
            species.Name = model.Name;
            _speciesRepository.Update(species);
            _speciesRepository.Save();
        }

        public void Delete(int id)
        {
            Species species = _speciesRepository.GetById(id) ?? throw new NullReferenceException("Вид животного не найден");
            _speciesRepository.Delete(species);
            _speciesRepository.Save();
        }

        public SpeciesViewModel GetById(int id)
        {
            Species species = _speciesRepository.GetById(id) ?? throw new NullReferenceException("Вид животного не найден");
            SpeciesViewModel model = new()
            {
                Id = id,
                Name = species.Name
            };
            return model;
        }

        public async Task<PaginatedList<SpeciesViewModel>> GetPaged(GetPagedSpeciesRequest request)
        {
            PaginatedList<Species> roles = await _speciesRepository.GetPaged(request);
            List<SpeciesViewModel> roleViewModels = roles.Select(species => new SpeciesViewModel
            {
                Id = species.Id,
                Name = species.Name
            }).ToList();
            var pagedSpecies = new PaginatedList<SpeciesViewModel>(roleViewModels, roles.PageNumber, roles.TotalPages);
            return pagedSpecies;
        }

        public async Task<byte[]> ExportToExcel(GetSpeciesExcelRequest request)
        {
            GetPagedSpeciesRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
            };
            PaginatedList<Species> speciesList = await _speciesRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Species species in speciesList)
            {
                List<object> row = [species.Name];
                data.Add(row);
            }
            List<string> columns = ["Название"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Виды животных", data, columns);
            return excelBytes;
        }
    }
}
