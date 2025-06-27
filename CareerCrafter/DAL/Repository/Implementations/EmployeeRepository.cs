using DAL.Models;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly CareerCrafterDbContext _context;

        public EmployeeRepository(CareerCrafterDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.ContactEmail == email);
        }
    }
}
