using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamFinder.Models.Domain;

namespace TeamFinder.Data.Repositories
{
    public class OrganizerApplicationRepository : IOrganizerApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public OrganizerApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrganizerApplication>> GetAllApplicationsAsync()
        {
            return await _context.OrganizerApplications
                                 .Include(app => app.User)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<OrganizerApplication>> GetUserApplications(string userId)
        {
            return await _context.OrganizerApplications
                                 .Include(app => app.User)
                                 .Where(app => app.User.Id == userId)
                                 .ToListAsync();
        }

        public async Task<OrganizerApplication> GetApplicationByIdAsync(Guid id)
        {
            var organizerApplications = await _context.OrganizerApplications
                                 .Include(app => app.User)
                                 .FirstOrDefaultAsync(app => app.Id == id);
            if(organizerApplications == null)
            {
                throw new ArgumentException("No application found with this id!");
            }

            return organizerApplications;
        }

        public async Task AddApplicationAsync(OrganizerApplication application)
        {
            await _context.OrganizerApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicationAsync(OrganizerApplication application)
        {
            _context.OrganizerApplications.Update(application);
            await _context.SaveChangesAsync();
        }
    }
}
