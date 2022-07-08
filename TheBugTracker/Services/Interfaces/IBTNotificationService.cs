using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface IBTNotificationService
    {
        Task AddNotificationAsync(Notification notification);

        Task<List<Notification>> GetSentNotificationsAsync(string userId);

        Task<List<Notification>> GetReceivedNotificationsAsync(string userId);

        Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);

        Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members);

        Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject);
    }
}
