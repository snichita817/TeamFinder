using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamFinder.Models.Domain;

namespace TeamFinder.Data.Repositories
{
    public interface IOrganizerApplicationRepository
    {
        Task<IEnumerable<OrganizerApplication>> GetAllApplicationsAsync();
        Task<IEnumerable<OrganizerApplication>> GetUserApplications(string userId);
        Task<OrganizerApplication> GetApplicationByIdAsync(Guid id);
        Task AddApplicationAsync(OrganizerApplication application);
        Task UpdateApplicationAsync(OrganizerApplication application);
    }
}
