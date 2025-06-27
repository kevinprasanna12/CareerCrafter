using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<IEnumerable<Application>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<IEnumerable<Application>> GetByJobListingIdAsync(int jobListingId);
    }
}
