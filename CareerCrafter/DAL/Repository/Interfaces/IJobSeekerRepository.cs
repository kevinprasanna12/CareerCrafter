using DAL.Models;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IJobSeekerRepository : IRepository<JobSeeker>
    {
        Task<JobSeeker> GetByEmailAsync(string email);
    }
}
