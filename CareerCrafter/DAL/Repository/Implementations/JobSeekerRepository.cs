using DAL.Models;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class JobSeekerRepository : Repository<JobSeeker>, IJobSeekerRepository
    {
        private readonly CareerCrafterDbContext _context;

        public JobSeekerRepository(CareerCrafterDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<JobSeeker> GetByEmailAsync(string email)
        {
            return await _context.JobSeekers.FirstOrDefaultAsync(j => j.Email == email);
        }
    }
}
