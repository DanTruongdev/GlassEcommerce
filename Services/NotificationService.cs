using GlassECommerce.Data;
using GlassECommerce.Models;
using GlassECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Services
{
    public class NotificationService : ControllerBase, INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAuthenticationService _authService;

        public NotificationService(ApplicationDbContext dbContext, IAuthenticationService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<IActionResult> GetAllNotifications()
        {
            User userExist = await _authService.GetCurrentLoggedInUser();
            if (userExist == null) return Unauthorized();
            var notifications = userExist.Notifications?.OrderByDescending(n => n.CreationDate).ToList();
            if (!notifications.Any()) return Ok(new List<Notification>());
            return Ok(notifications.Select(n => new
            {
                NotificationId = n.NotificationId,
                User = userExist.ToString(),
                Title = n.Title,
                Content = n.Content,
                CreationDate = n.CreationDate
            }));

        }

        public async Task<bool> AddNotification(string userId, string title, string content)
        {
            try
            {
                bool userExist = await _dbContext.Users.AnyAsync(u => u.Id.Equals(userId));
                if (!userExist) return false;
                Notification newNotification = new Notification()
                {
                    UserId = userId,
                    Title = title,
                    Content = content,
                    CreationDate = DateTime.Now
                };
                await _dbContext.AddAsync(newNotification);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
