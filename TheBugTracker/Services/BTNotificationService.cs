using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBugTracker.Data;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class BTNotificationService : IBTNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IBTRolesService _rolesService;

        public BTNotificationService(ApplicationDbContext context, 
                                    IEmailSender emailSender, 
                                    IBTRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }

        //Create Notification
        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Get Notification
        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            try
            {
                List<Notification> receivedNotifications = await _context.Notifications
                                                                    .Include(n => n.Recipient)
                                                                    .Include(n => n.Sender)
                                                                    .Include(n => n.Ticket)
                                                                        .ThenInclude(t => t.Project)
                                                                    .Where(n => n.RecipientId == userId).ToListAsync();
                return receivedNotifications;

            }
            catch (Exception)
            {

                throw;
            } 
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            List<Notification> sentNotifications = await _context.Notifications
                                                        .Include(n => n.Sender)
                                                        .Include(n => n.Recipient)
                                                        .Include(n => n.Ticket)
                                                            .ThenInclude(t => t.Project)
                                                            .Where(n => n.SenderId == userId).ToListAsync();

            return sentNotifications;
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
           

            BTUser bTUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.RecipientId);

            if (bTUser != null)
            {
                string btUserEmail = bTUser.Email;
                string message = notification.Message;

                //Send Email
                try
                {
                    await _emailSender.SendEmailAsync(btUserEmail, emailSubject, message);
                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                return false;
            }
            
        }

        public async Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<BTUser> members = await _rolesService.GetUsersInRoleAsync(role, companyId);
                foreach (BTUser bTUser in members)
                {
                    notification.RecipientId = bTUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members)
        {
            try
            {
                foreach (BTUser bTUser in members)
                {
                    notification.RecipientId = bTUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
