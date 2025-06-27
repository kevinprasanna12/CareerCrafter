using DAL.Models;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ResumeRepository : Repository<Resume>, IResumeRepository
    {
        private readonly CareerCrafterDbContext _context;

        public ResumeRepository(CareerCrafterDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Resume> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await _context.Resumes.FirstOrDefaultAsync(r => r.JobSeekerId == jobSeekerId);
        }
    }
}
