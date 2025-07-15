using CareerCrafter.DTOs.EmployeeDTOs;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeReadDto>> GetAllEmployeesAsync();
        Task<EmployeeReadDto?> GetEmployeeByIdAsync(int id);
        Task<EmployeeReadDto> CreateEmployeeAsync(EmployeeCreateDto dto,int userId);
        Task<bool> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<EmployeeReadDto?> GetEmployeeByUserIdAsync(int userId);

    }
}
