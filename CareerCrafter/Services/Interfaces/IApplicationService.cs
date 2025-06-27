using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models;

namespace Services.Interfaces
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<Application> CreateApplicationAsync(Application application);
        Task<bool> UpdateApplicationAsync(int id, Application application);
        Task<bool> DeleteApplicationAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByJobSeekerAsync(string email);
        Task<(bool Success, string Message)> ApplyToJobAsync(string email, int jobListingId);
    }
}