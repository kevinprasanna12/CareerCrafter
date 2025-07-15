using CareerCrafter.DTOs.EmployeeDTOs;
using DAL.Models;
using DAL.Repositories;
using DAL.Repository.Interfaces;
using Services.Interfaces;

namespace ServiceLayer.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<EmployeeReadDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.Select(e => new EmployeeReadDto
            {
                EmployerId = e.EmployerId,
                CompanyName = e.CompanyName,
                ContactEmail = e.ContactEmail
            });
        }

        public async Task<EmployeeReadDto?> GetEmployeeByIdAsync(int id)
        {
            var e = await _repository.GetByIdAsync(id);
            if (e == null) return null;

            return new EmployeeReadDto
            {
                EmployerId = e.EmployerId,
                CompanyName = e.CompanyName,
                ContactEmail = e.ContactEmail
            };
        }

        public async Task<EmployeeReadDto> CreateEmployeeAsync(EmployeeCreateDto dto, int userId)
        {
            var employee = new Employee
            {
                CompanyName = dto.CompanyName,
                ContactEmail = dto.ContactEmail,
                UserId = userId
            };

            var created = await _repository.AddAsync(employee);

            return new EmployeeReadDto
            {
                EmployerId = created.EmployerId,
                CompanyName = created.CompanyName,
                ContactEmail = created.ContactEmail
            };
        }

        public async Task<bool> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.CompanyName = dto.CompanyName;
            existing.ContactEmail = dto.ContactEmail;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<EmployeeReadDto?> GetEmployeeByUserIdAsync(int userId)
        {
            var employee = await _repository.GetByUserIdAsync(userId); 

            if (employee == null) return null;

            return new EmployeeReadDto
            {
                EmployerId = employee.EmployerId,
                CompanyName = employee.CompanyName,
                ContactEmail = employee.ContactEmail
            };
        }



    }
}
