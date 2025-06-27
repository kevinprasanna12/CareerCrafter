using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
    }
}
