using CloudinaryDotNet;
using GlassECommerce.DTOs;
using GlassECommerce.Services.Interfaces;

namespace GlassECommerce.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IConfiguration _config;
        public CloudinaryService(IConfiguration config)
        {
            _config = config;

        }

        public PresignDTO GetPresignedUrl()
        {

            try
            {
                string cloudName = _config["Cloudinary:CloudName"];
                string apiKey = _config["Cloudinary:ApiKey"];
                string apiSecret = _config["Cloudinary:ApiSecret"];
                Account cloudinaryAccount = new Account(cloudName, apiKey, apiSecret);
                Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                string key = $"{Guid.NewGuid()}";
                SortedDictionary<string, object> parameters = new SortedDictionary<string, object>();
                parameters.Add("timestamp", timestamp);
                string signature = cloudinary.Api.SignParameters(parameters);
                return new PresignDTO(timestamp, signature, key, cloudName, apiKey);
            }
            catch
            {
                return null;
            }
        }



    }
}
