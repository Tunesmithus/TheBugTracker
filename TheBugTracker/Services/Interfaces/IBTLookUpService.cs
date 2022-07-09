using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface IBTLookUpService
    {
        Task<List<TicketPriority>> GetTicketPrioritiesAsync();
        Task<List<TicketStatus>> GetTicketStatusesAsync();
        Task<List<TicketType>> GetTicketTypesAsync();
        Task<List<ProjectPriority>> GetProjectPrioritiesAsync();
    }
}
