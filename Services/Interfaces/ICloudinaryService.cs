using GlassECommerce.DTOs;

namespace GlassECommerce.Services.Interfaces
{
    public interface ICloudinaryService
    {
        public PresignDTO GetPresignedUrl();
    }
}
