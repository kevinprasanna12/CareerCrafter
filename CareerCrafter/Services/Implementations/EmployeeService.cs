using DAL.Models;
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

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            return await _repository.AddAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            if (id != employee.EmployerId) return false;

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.UpdateAsync(employee);
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
