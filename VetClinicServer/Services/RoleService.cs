using VetClinicServer.Models;
using VetClinicServer.Repositories;
using VetClinicServer.Requests.Excels;
using VetClinicServer.Requests.Pages;
using VetClinicServer.Utils;
using VetClinicServer.ViewModels;

namespace VetClinicServer.Services
{
    public class RoleService(RoleRepository roleRepository, ExcelConverter excelConverter)
    {
        private readonly RoleRepository _roleRepository = roleRepository;
        private readonly ExcelConverter _excelConverter = excelConverter;

        public int Create(RoleViewModel model)
        {
            Role role = new()
            {
                Name = model.Name,
                Description = model.Description,
            };
            _roleRepository.Create(role);
            _roleRepository.Save();
            return role.Id;
        }

        public void Update(RoleViewModel model)
        {
            Role role = _roleRepository.GetById(model.Id) ?? throw new NullReferenceException("Роль не найдена");
            role.Name = model.Name;
            role.Description = model.Description;
            _roleRepository.Update(role);
            _roleRepository.Save();
        }

        public void Delete(int id)
        {
            Role role = _roleRepository.GetById(id) ?? throw new NullReferenceException("Роль не найдена");
            _roleRepository.Delete(role);
            _roleRepository.Save();
        }

        public RoleViewModel GetById(int id) 
        {
            Role role = _roleRepository.GetById(id) ?? throw new NullReferenceException("Роль не найдена");
            RoleViewModel model = new()
            {
                Id = id,
                Name = role.Name,
                Description = role.Description
            };
            return model;
        }

        public async Task<PaginatedList<RoleViewModel>> GetPaged(GetPagedRolesRequest request)
        {
            PaginatedList<Role> roles = await _roleRepository.GetPaged(request);
            List<RoleViewModel> roleViewModels = roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            }).ToList();
            var pagedRoles = new PaginatedList<RoleViewModel>(roleViewModels, roles.PageNumber, roles.TotalPages);
            return pagedRoles;
        }

        public async Task<byte[]> ExportToExcel(GetRolesExcelRequest request)
        {
            GetPagedRolesRequest pagedRequest = new()
            {
                PageNumber = 1,
                PageSize = int.MaxValue,
                OrderBy = request.OrderBy,
                SortDirection = request.SortDirection,
                Name = request.Name,
            };
            PaginatedList<Role> roles = await _roleRepository.GetPaged(pagedRequest);
            List<List<object>> data = [];
            foreach (Role role in roles)
            {
                List<object> row = [role.Name, role.Description ?? string.Empty];
                data.Add(row);
            }
            List<string> columns = ["Название", "Описание"];
            byte[] excelBytes = _excelConverter.ExportToExcel("Роли", data, columns);
            return excelBytes;
        }
    }
}
