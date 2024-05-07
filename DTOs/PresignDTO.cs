namespace GlassECommerce.DTOs
{
    public class PresignDTO
    {
        public PresignDTO(long timestamp, string signature, string key, string cloudName, string apiKey)
        {
            Timestamp = timestamp;
            Signature = signature;
            Key = key;
            CloudName = cloudName;
            ApiKey = apiKey;
        }

        public long Timestamp { get; set; }
        public string Signature { get; set; }
        public string Key { get; set; }
        public string CloudName { get; set; }
        public string ApiKey { get; set; }
    }
}
