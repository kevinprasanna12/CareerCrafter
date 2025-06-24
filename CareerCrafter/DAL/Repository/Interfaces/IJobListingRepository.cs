using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IJobListingRepository : IRepository<JobListing>
    {
        Task<IEnumerable<JobListing>> GetByEmployerIdAsync(int employerId);
        Task<IEnumerable<JobListing>> SearchAsync(string keyword, string location);
        Task<JobListing> GetWithApplicationsAsync(int jobListingId);
    }
}
