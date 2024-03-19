using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class DrugService(DrugRepository repository)
    {
        private readonly DrugRepository _repository = repository;

        public void Create(DrugViewModel model)
        {
            Drug drug = new()
            {
                Name = model.Name,
                Cost = model.Cost,
                Quantity = model.Quantity,
                Description = model.Description,
            };
            _repository.Create(drug);
        }

        public void Update(DrugViewModel model)
        {
            Drug drug = _repository.GetById(model.Id) ?? throw new NullReferenceException("Лекарство не найдено");
            drug.Name = model.Name;
            drug.Cost = model.Cost;
            drug.Quantity = model.Quantity;
            drug.Description = model.Description;
            _repository.Update(drug);
        }

        public void Delete(int id)
        {
            Drug drug = _repository.GetById(id) ?? throw new NullReferenceException("Лекарство не найдено");
            _repository.Delete(drug);
        }

        public DrugViewModel GetById(int id)
        {
            Drug drug = _repository.GetById(id) ?? throw new NullReferenceException("Лекарство не найдено");
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
            PaginatedList<Drug> drugs = await _repository.GetPaged(request);
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
    }
}
