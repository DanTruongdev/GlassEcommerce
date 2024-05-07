using System.IO.Compression;
using System.Text;

namespace GlassECommerce.Common
{
    public static class CompressionHelper
    {
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                {
                    gzipStream.Write(buffer, 0, buffer.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static string DecompressString(string compressedText)
        {
            byte[] compressedBytes = Convert.FromBase64String(compressedText);

            using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(gzipStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
