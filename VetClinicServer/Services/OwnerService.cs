using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class OwnerService(OwnerRepository ownerRepository, ExcelConverter excelConverter)
    {
        private readonly OwnerRepository _ownerRepository = ownerRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(OwnerViewModel model)
        {
            Owner owner = new()
            {
                FIO = model.FIO,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };
            _ownerRepository.Create(owner);
            _ownerRepository.Save();
            return owner.Id;
        }

        public void Update(OwnerViewModel model)
        {
            Owner owner = _ownerRepository.GetById(model.Id) ?? throw new NullReferenceException("Владелец не найден");
            owner.FIO = model.FIO;
            owner.PhoneNumber = model.PhoneNumber;
            owner.Email = model.Email;
            _ownerRepository.Update(owner);
            _ownerRepository.Save();
        }

        public void Delete(int id)
        {
            Owner owner = _ownerRepository.GetById(id) ?? throw new NullReferenceException("Владелец не найден");
            _ownerRepository.Delete(owner);
            _ownerRepository.Save();
        }

        public OwnerViewModel GetById(int id)
        {
            Owner owner = _ownerRepository.GetById(id) ?? throw new NullReferenceException("Владелец не найден");
            OwnerViewModel model = new()
            {
                Id = id,
                FIO = owner.FIO,
                PhoneNumber = owner.PhoneNumber,
                Email = owner.Email
            };
            return model;
        }

        public async Task<PaginatedList<OwnerViewModel>> GetPaged(GetPagedOwnersRequest request)
        {
            PaginatedList<Owner> owners = await _ownerRepository.GetPaged(request);
            List<OwnerViewModel> ownerViewModels = owners.Select(owner => new OwnerViewModel
            {
                Id = owner.Id,
                FIO = owner.FIO,
                PhoneNumber = owner.PhoneNumber,
                Email = owner.Email
            }).ToList();
            var pagedDrugs = new PaginatedList<OwnerViewModel>(ownerViewModels, owners.PageNumber, owners.TotalPages);
            return pagedDrugs;
        }

        public async Task<byte[]> ExportToExcel(GetOwnersExcelRequest request)
        {
            GetPagedOwnersRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                FIO = request.FIO,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
            PaginatedList<Owner> owners = await _ownerRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Owner owner in owners)
            {
                List<object> row = [owner.FIO, owner.PhoneNumber, owner.Email ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["ФИО", "Номер телефона", "Электронная почта"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Владельцы", data, columns);
            return excelBytes;
        }
    }
}
