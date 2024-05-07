using GlassECommerce.Services.Models;

namespace GlassECommerce.Services.Interfaces
{
    public interface IEmailService
    {
        public bool SendEmail(Message message);
    }
}
