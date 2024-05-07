using Microsoft.AspNetCore.Mvc;

namespace GlassECommerce.Services.Interfaces
{
    public interface INotificationService
    {
        public Task<IActionResult> GetAllNotifications();
        public Task<bool> AddNotification(string userId, string title, string content);
    }
}
