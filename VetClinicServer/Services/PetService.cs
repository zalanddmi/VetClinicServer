using Aspose.Cells.Charts;
using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class PetService(PetRepository petRepository,
        SpeciesRepository speciesRepository,
        OwnerRepository ownerRepository,
        ExcelConverter excelConverter)
    {
        private readonly PetRepository _petRepository = petRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;
        private readonly SpeciesRepository _speciesRepository = speciesRepository;
        private readonly OwnerRepository _ownerRepository = ownerRepository;

        public int Create(PetViewModel model)
        {
            Species species = _speciesRepository.GetById(model.Species.Id) ?? throw new NullReferenceException("Вид животного не найден");
            Owner owner = _ownerRepository.GetById(model.Owner.Id) ?? throw new NullReferenceException("Владелец не найден");
            Pet pet = new()
            {
                Name = model.Name,
                Species = species,
                Breed = model.Breed,
                Owner = owner,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description
            };
            _petRepository.Create(pet);
            _petRepository.Save();
            return pet.Id;
        }

        public void Update(PetViewModel model)
        {
            Pet pet = _petRepository.GetById(model.Id) ?? throw new NullReferenceException("Животное не найдено");
            Species species = _speciesRepository.GetById(model.Species.Id) ?? throw new NullReferenceException("Вид животного не найден");
            Owner owner = _ownerRepository.GetById(model.Owner.Id) ?? throw new NullReferenceException("Владелец не найден");
            pet.Name = model.Name;
            pet.Species = species;
            pet.Breed = model.Breed;
            pet.Owner = owner;
            pet.DateOfBirth = model.DateOfBirth;
            pet.Description = model.Description;
            _petRepository.Update(pet);
            _petRepository.Save();
        }

        public void Delete(int id)
        {
            Pet pet = _petRepository.GetById(id) ?? throw new NullReferenceException("Животное не найдено");
            _petRepository.Delete(pet);
            _petRepository.Save();
        }

        public PetViewModel GetById(int id)
        {
            Pet pet = _petRepository.GetById(id) ?? throw new NullReferenceException("Животное не найдено");
            DisplayModel speciesDisplay = new() { Id = pet.Species.Id, Name = pet.Species.Name, Entity = "Species" };
            DisplayModel ownerDisplay = new() { Id = pet.Owner.Id, Name = pet.Owner.FIO, Entity = "Owner" };
            PetViewModel model = new()
            {
                Id = id,
                Name = pet.Name,
                Species = speciesDisplay,
                Breed = pet.Breed,
                Owner = ownerDisplay,
                DateOfBirth = pet.DateOfBirth,
                Description = pet.Description
            };
            return model;
        }

        public async Task<PaginatedList<PetViewModel>> GetPaged(GetPagedPetsRequest request)
        {
            PaginatedList<Pet> pets = await _petRepository.GetPaged(request);
            List<PetViewModel> petViewModels = pets.Select(pet => new PetViewModel
            {
                Id = pet.Id,
                Name = pet.Name,
                Species = new DisplayModel() { Id = pet.Species.Id, Name= pet.Species.Name, Entity = "Species" },
                Breed = pet.Breed,
                Owner = new DisplayModel() { Id = pet.Owner.Id, Name = pet.Owner.FIO, Entity = "Owner" },
                DateOfBirth = pet.DateOfBirth,
                Description = pet.Description
            }).ToList();
            var pagedPets = new PaginatedList<PetViewModel>(petViewModels, pets.PageNumber, pets.TotalPages);
            return pagedPets;
        }

        public async Task<byte[]> ExportToExcel(GetPetsExcelRequest request)
        {
            GetPagedPetsRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
                Species = request.Species,
                Breed = request.Breed,
                Owner = request.Owner,
                DateOfBirth = request.DateOfBirth,
                DateOfBirthComparisonOperators = request.DateOfBirthComparisonOperators
            };
            PaginatedList<Pet> pets = await _petRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Pet pet in pets)
            {
                List<object> row = [pet.Name, pet.Species.Name, pet.Breed, pet.Owner.FIO, pet.DateOfBirth.ToShortDateString(), pet.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Кличка", "Вид", "Порода", "Владелец", "Дата рождения", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Животные", data, columns);
            return excelBytes;
        }
    }
}
