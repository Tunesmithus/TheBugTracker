using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Data;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public class BTLookUpService : IBTLookUpService
    {
        private readonly ApplicationDbContext _context;

        public BTLookUpService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectPriority>> GetProjectPrioritiesAsync()
        {
            try
            {
                var result = await _context.ProjectPriorities.ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        public async Task<List<TicketPriority>> GetTicketPrioritiesAsync()
        {
            try
            {
                return await _context.TicketPriorities.ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TicketStatus>> GetTicketStatusesAsync()
        {
            try
            {
                var result = await _context.TicketStatuses.ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<TicketType>> GetTicketTypesAsync()
        {
            try
            {
                var result = await _context.TicketTypes.ToListAsync();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
